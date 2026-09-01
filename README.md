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
