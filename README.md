# EtAlii.Generators

[![Build](https://github.com/vrenken/EtAlii.Generators/actions/workflows/build.yml/badge.svg?branch=main)](https://github.com/vrenken/EtAlii.Generators/actions/workflows/build.yml)
[![Analysis](https://github.com/vrenken/EtAlii.Generators/actions/workflows/analysis.yml/badge.svg)](https://github.com/vrenken/EtAlii.Generators/actions/workflows/analysis.yml)

A set of Roslyn generators to simplify usage of some of the more mainstream frameworks/libraries.

As today's code generators mature they can bring a lot of value to the complex world of software development.
In this solution we'll try to gather a few nifty code generators with which speed up the usage of well-known open source projects.

Below is a small grasp on what is possible.

## PlantUML state diagrams to Stateless state machines
_(EtAlii.Generators.Stateless)_

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
