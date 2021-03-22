# EtAlii.Generators

## Introduction
Code generation is fun and when done right extremely helpful. This project aims to use a combination of [ANTLR](https://github.com/antlr/antlr4) and [Roslyn source generators](https://github.com/dotnet/roslyn/blob/main/docs/features/source-generators.cookbook.md) to simplify usage of some of the more mainstream frameworks/libraries.
The first attempt is to use PlantUML to visually design [Stateless](https://github.com/dotnet-state-machine/stateless) state machines.

## Project status

[![Build](https://github.com/vrenken/EtAlii.Generators/actions/workflows/build.yml/badge.svg?branch=main)](https://github.com/vrenken/EtAlii.Generators/actions/workflows/build.yml)
[![Analysis](https://github.com/vrenken/EtAlii.Generators/actions/workflows/analysis.yml/badge.svg)](https://github.com/vrenken/EtAlii.Generators/actions/workflows/analysis.yml)
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;![GitHub](https://img.shields.io/github/license/vrenken/EtAlii.Generators)

[![Bugs](https://sonarcloud.io/api/project_badges/measure?project=EtAlii.Generators&metric=bugs)](https://sonarcloud.io/dashboard?id=EtAlii.Generators)
[![Code Smells](https://sonarcloud.io/api/project_badges/measure?project=EtAlii.Generators&metric=code_smells)](https://sonarcloud.io/dashboard?id=EtAlii.Generators)
[![Coverage](https://sonarcloud.io/api/project_badges/measure?project=EtAlii.Generators&metric=coverage)](https://sonarcloud.io/dashboard?id=EtAlii.Generators)
[![Duplicated Lines (%)](https://sonarcloud.io/api/project_badges/measure?project=EtAlii.Generators&metric=duplicated_lines_density)](https://sonarcloud.io/dashboard?id=EtAlii.Generators)

[![Lines of Code](https://sonarcloud.io/api/project_badges/measure?project=EtAlii.Generators&metric=ncloc)](https://sonarcloud.io/dashboard?id=EtAlii.Generators)
[![Technical Debt](https://sonarcloud.io/api/project_badges/measure?project=EtAlii.Generators&metric=sqale_index)](https://sonarcloud.io/dashboard?id=EtAlii.Generators)
[![Vulnerabilities](https://sonarcloud.io/api/project_badges/measure?project=EtAlii.Generators&metric=vulnerabilities)](https://sonarcloud.io/dashboard?id=EtAlii.Generators)

[![Maintainability Rating](https://sonarcloud.io/api/project_badges/measure?project=EtAlii.Generators&metric=sqale_rating)](https://sonarcloud.io/dashboard?id=EtAlii.Generators)
[![Reliability Rating](https://sonarcloud.io/api/project_badges/measure?project=EtAlii.Generators&metric=reliability_rating)](https://sonarcloud.io/dashboard?id=EtAlii.Generators)
[![Security Rating](https://sonarcloud.io/api/project_badges/measure?project=EtAlii.Generators&metric=security_rating)](https://sonarcloud.io/dashboard?id=EtAlii.Generators)
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=EtAlii.Generators&metric=alert_status)](https://sonarcloud.io/dashboard?id=EtAlii.Generators)

## Examples

### Behavior trees
![Nuget](https://img.shields.io/nuget/v/EtAlii.Generators.Behavior?url=https://www.nuget.org/packages/EtAlii.Generators.Behavior/)
![Nuget](https://img.shields.io/nuget/dt/EtAlii.Generators.Behavior?url=https://www.nuget.org/packages/EtAlii.Generators.Behavior/)

Tbd.
### EtAlii.Generators.Stateless - PlantUML state diagrams to Stateless state machines
![Nuget](https://img.shields.io/nuget/v/EtAlii.Generators.Stateless?url=https://www.nuget.org/packages/EtAlii.Generators.Stateless/)
![Nuget](https://img.shields.io/nuget/dt/EtAlii.Generators.Stateless?url=https://www.nuget.org/packages/EtAlii.Generators.Stateless/)


Usage:

1. Add the package to the target project:
   ```csproj
     <ItemGroup>
       <PackageReference Update="EtAlii.Generators.Stateless" Version="1.0.0" PrivateAssets="all" />
     </ItemGroup>
   ```
3. Come up with a fancy PlantUML state machine diagram:
   ```puml
   @startuml
   'stateless namespace My.ExampleNamespace
   'stateless class MyFancyStateMachine
   'stateless generate partial
   [*] --> State1 : Start
   State1 --> [*]
   State1 : this is a string
   State1 : this is another string
   State1 -> State2 : Continue
   State2 --> [*]
   State2 -> State3 : DoState3
   @enduml
   ```
3. Put the PlantUML diagram in a file and reference it in the project as a `Stateless` entry:
   ```csproj
     <ItemGroup>
       <Stateless Include="MyFancyStateMachine.puml" />
     </ItemGroup>
   ```
4. Compile the project - the Roslyn generator now uses the diagram to create C# state machine code.
5. Add a class file to implement the needed state machine behavior.
   ```cs
   namespace My.ExampleNamespace
   {
      using System;

      public partial class MyFancyStateMachine
      {
         public override void OnState1Entered() => Console.WriteLine("Entered State 1");
         public override void OnState1Exited() => Console.WriteLine("Exited State 1");

         public override void OnState2Entered() => Console.WriteLine("Entered State 2");
         public override void OnState2Exited() => Console.WriteLine("Exited State 2");
      }
   }
   ```
6. Create and trigger the state machine from somewhere in your code.
   ```cs
   namespace My.ExampleNamespace
   {
      using System;

      public static void Main()
      {
         var stateMachine = new MyFancyStateMachine();
         stateMachine.Start();
         stateMachine.Continue();
      }
   }
   ```
