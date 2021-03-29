# PlantUML diagram to Stateless C# state machine code

Usage:

1. Add the analyzer NuGet package to the target project:
   ```csproj
     <ItemGroup>
       <PackageReference Update="EtAlii.Generators.Stateless" Version="1.0.0" PrivateAssets="all" />
     </ItemGroup>
   ```

   Also add the Stateless NuGet package to the target project:
   ```csproj
     <ItemGroup>
       <PackageReference Include="stateless" Version="5.10.1" />
     </ItemGroup>
   ```

3. Come up with a fancy PlantUML state machine diagram:
   ```puml
   @startuml
   'stateless namespace My.ExampleNamespace
   'stateless class MyFancyStateMachineBase
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

      public class MyFancyStateMachine : MyFancyStateMachineBase
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
