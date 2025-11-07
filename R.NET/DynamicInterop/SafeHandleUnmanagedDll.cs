using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace RDotNet.DynamicInterop
{
    internal sealed class SafeHandleUnmanagedDll : SafeHandleZeroOrMinusOneIsInvalid
    {
        public SafeHandleUnmanagedDll(string dllName) : base(true)
        {
            if (dllName == null)
            {
                throw new ArgumentNullException("dllName", "The name of the library to load is a null reference");
            }

            if (dllName == string.Empty)
            {
                throw new ArgumentException("The name of the library to load is an empty string", "dllName");
            }

            handle = System.Runtime.InteropServices.NativeLibrary.Load(dllName);
        }

        public nint GetFunctionAddress(string lpProcName)
        {
            return System.Runtime.InteropServices.NativeLibrary.GetExport(handle, lpProcName);
        }


        protected override bool ReleaseHandle()
        {
            System.Runtime.InteropServices.NativeLibrary.Free(handle);
            return true;
        }
    }
}
