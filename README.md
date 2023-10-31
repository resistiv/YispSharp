# YispSharp
YispSharp is a C# implementation of a Lisp interpreter, created using [*Crafting Interpreters* by Robert Nystrom](https://craftinginterpreters.com/) as a guide. This project was developed primarily as an assignment for CS 403/503 - Programming Languages (Fall 2023) at the University of Alabama. The interpreter works on a dialect of Lisp created by our professor, Don Yessick, called Yessick's Lisp or Yisp for short.

The instructions provided for this project can be found [here](/Instructions.md), and some notes on the grammar used to build this interpreter can be found [here](/Grammar.md).

## Usage
When run without an argument, Y# operates as a <abbr title="read-eval-print loop">REPL</abbr> prompt which runs until it encounters an exit code. Otherwise, when given a Yisp source file, Y# will attempt to execute it and then exit.
```
YispSharp [Yisp script]
```

## Building
Building and running YispSharp requires the .NET 6.0 SDK. It can be installed via a package manager, such as ``apt`` or ``snap``, or from the [.NET download website](https://dotnet.microsoft.com/en-us/download/dotnet/6.0).
```
sudo apt-get install dotnet-sdk-6.0
```
On Windows, it can be installed via the Windows Package Manager, ``winget``, or from the aforementioned download site.
```
winget install Microsoft.DotNet.SDK.6
```
Once installed, clone the repository to a location of your choosing.
```
git clone https://github.com/resistiv/YispSharp.git
```
Navigate to the folder containing the file ``YispSharp.sln``, and run the following:
```
dotnet build
```
The resulting executables will be built to the ``bin`` subfolders of each project within the solution, from which they can be run.

## Testing
This project adapts several of [Robert Nystrom's Lox unit tests](https://github.com/munificent/craftinginterpreters/tree/master/test) to Yisp.

Tests can be built and run using the following command:
```
dotnet test
```
The number of successful and failed tests will be displayed.
