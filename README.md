
# R.NET (Aura Ecosystem Edition)

A high-performance .NET and F# interoperability framework for the [R Statistical Computing Environment](https://www.r-project.org/). 

`R.NET` allows .NET applications, F# scripts, and microservices to run R code in-process, exchange data structures natively in memory, and execute statistical and machine learning algorithms without subprocess overhead.

---

## Features

- **In-Memory Data Exchange:** Move vectors, matrices, data frames, and lists between .NET and R without file I/O or JSON serialization costs.
- **F# Native Design:** idiomatic F# module wrappers for type-safe manipulation of R objects.
- **Multi-Platform:** Supports Windows, macOS, and Linux (requires an installed R environment).
- **Embedded Engine Execution:** Evaluate R expressions dynamically via an embedded R engine instance inside your .NET runtime.

---

## Architecture & Project Structure

```text
├── src/
│   ├── R.NET/                  # Core C# engine interop & native R API bindings
│   └── R.NET.FSharp/           # Idiomatic F# wrappers, active patterns, & extensions
├── tests/
│   ├── R.NET.Tests/            # Engine integration tests
│   └── R.NET.FSharp.Tests/     # F# interop tests
├── Documentation.shfbproj      # Sandcastle Help File Builder project
├── R.NET.sln                   # Visual Studio Solution
└── README.md

```

---

## Prerequisites

1. **R Runtime Environment:** Installed R engine (R $\ge$ 4.0 recommended).
* **Windows:** Standard R distribution installed or available in system `PATH`.
* **Linux:** `r-base` and `r-base-dev` packages installed.
* **macOS:** R installed via Homebrew (`brew install r`) or CRAN installer.


2. **.NET SDK:** .NET 8.0 SDK or higher.

---

## Quickstart & Code Examples

### F# Usage Example

```fsharp
open RDotNet
open RDotNet.FSharp

// Initialize R Engine engine instance
let engine = REngine.GetInstance()
engine.Initialize()

// Pass native F# arrays into R
let numericVector = engine.CreateNumericVector([| 1.0; 2.5; 3.8; 4.2 |])
engine.SetSymbol("x", numericVector)

// Evaluate R script and pull statistical summary back into .NET
let summary = engine.Evaluate("summary(x)")
printfn "%A" (summary.AsNumeric().ToArray())

// Clean up resources on shutdown
engine.Dispose()

```

### C# Usage Example

```csharp
using RDotNet;

REngine.SetEnvironmentVariables(); // Auto-detect R home path
using (var engine = REngine.GetInstance())
{
    // Evaluate math expressions
    NumericVector group1 = engine.CreateNumericVector(new[] { 30.0, 35.0, 40.0, 45.0 });
    engine.SetSymbol("group1", group1);

    GenericVector result = engine.Evaluate("t.test(group1)").AsList();
    double pValue = result["p.value"].AsNumeric().First();

    Console.WriteLine($"Calculated p-value: {pValue}");
}

```

---

## Building from Source

Clone the repository and build the solution via the .NET CLI:

```bash
git clone [https://github.com/auraecosystem/R.net.git](https://github.com/auraecosystem/R.net.git)
cd R.net

# Restore packages and build solution
dotnet restore
dotnet build --configuration Release

# Execute test suite (Requires local R environment)
dotnet test

```

---

## Generating API Documentation

The repository includes a Sandcastle Help File Builder (`Documentation.shfbproj`) configuration to build full offline API reference documentation:

```cmd
# Building via MSBuild on Windows
msbuild Documentation.shfbproj /p:Configuration=Release

```

Extracted `.chm` compiled HTML help files will be generated under the `.\Help\` directory.

---

## License

Distributed under the [MIT License](https://www.google.com/search?q=LICENSE).

```

```
