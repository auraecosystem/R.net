using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace RDotNet.DynamicInterop
{
    /// <summary>
    /// Code from DynamicInterop
    /// A proxy for unmanaged dynamic link library (DLL).
    /// </summary>  
    public class UnmanagedDll : IDisposable
    {
        private SafeHandleUnmanagedDll _handle;
        public UnmanagedDll(string dllName)
        {
            if (dllName == null)
            {
                throw new ArgumentNullException("dllName", "The name of the library to load is a null reference");
            }

            if (dllName == string.Empty)
            {
                throw new ArgumentException("The name of the library to load is an empty string", "dllName");
            }

            _handle = new SafeHandleUnmanagedDll(dllName);
            
            Filename = dllName;
        }

        public string Filename { get; private set; }


        public string GetAnsiString(string symbolName)
        {
            var addr = checkedGetSymbolHandle(symbolName);
            return Marshal.PtrToStringAnsi(addr);
        }
        public nint DangerousGetHandle(string entryPoint)
        {
            if (string.IsNullOrEmpty(entryPoint))
            {
                throw new ArgumentNullException("The entry point cannot be null or an empty string", "entryPoint");
            }
            return GetFunctionAddress(entryPoint);
        }

        public TDelegate GetFunction<TDelegate>()
           where TDelegate : class
        {
            return GetFunction<TDelegate>(typeof(TDelegate).Name);
        }

        /// <summary>
        /// Creates the delegate function for the specified function defined in the DLL.
        /// </summary>
        /// <typeparam name="TDelegate">The type of delegate.</typeparam>
        /// <param name="entryPoint">The name of the function exported by the DLL</param>
        /// <returns>The delegate.</returns>
        public TDelegate GetFunction<TDelegate>(string entryPoint)
           where TDelegate : class
        {
            if (string.IsNullOrEmpty(entryPoint))
                throw new ArgumentNullException("entryPoint", "Native function name cannot be null or empty");
            lock (this)
            {
                Type delegateType = typeof(TDelegate);
                if (delegateFunctionPointers.ContainsKey(entryPoint))
                    return (TDelegate)delegateFunctionPointers[entryPoint];
                if (!delegateType.IsSubclassOf(typeof(Delegate)))
                {
                    throw new InvalidCastException();
                }
                nint function = GetFunctionAddress(entryPoint);
                if (function == nint.Zero)
                {
                    throwEntryPointNotFound(entryPoint);
                }
                var dFunc = Marshal.GetDelegateForFunctionPointer(function, delegateType) as TDelegate;
                delegateFunctionPointers[entryPoint] = dFunc;
                return dFunc;
            }
        }


        public nint GetFunctionAddress(string lpProcName)
        {
            return _handle.GetFunctionAddress(lpProcName);
        }
        public void WriteInt32(string symbolName, int value)
        {
            var addr = checkedGetSymbolHandle(symbolName);
            Marshal.WriteInt32(addr, value);
        }
        public void WriteIntPtr(string symbolName, nint value)
        {
            var addr = checkedGetSymbolHandle(symbolName);
            Marshal.WriteIntPtr(addr, value);
        }
        protected virtual void Dispose(bool disposing)
        {
            _handle.Dispose();
        }
        public void Dispose()
        {
            Dispose(true);
        }

        public int GetInt32(string symbolName)
        {
            var addr = checkedGetSymbolHandle(symbolName);
            return Marshal.ReadInt32(addr);
        }

        private void throwEntryPointNotFound(string entryPoint)
        {
            throw new EntryPointNotFoundException(string.Format("Function {0} not found in native library {1}", entryPoint, Filename));
        }

        nint checkedGetSymbolHandle(string symbolName)
        {
            var addr = GetFunctionAddress(symbolName);
            if (nint.Zero == addr)
                throw new ArgumentException(string.Format("Could not retrieve a pointer for the symbol '{0}' in file '{1}'", symbolName, Filename));
            return addr;
        }


        private ConcurrentDictionary<string, object> delegateFunctionPointers = new ConcurrentDictionary<string, object>();


    }
}
