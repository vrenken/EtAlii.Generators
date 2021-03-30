# PlantUML diagram to Stateless C# state machine code

## How to start:
Usage:

1. Add the analyzer NuGet package to the target project:
   ```csproj
    <ItemGroup>
        <PackageReference Update="EtAlii.Generators.Stateless" Version="1.0.0" PrivateAssets="all" />
    </ItemGroup>
   ```

2. Also add the Stateless NuGet package to the target project:
   ```csproj
     <ItemGroup>
       <PackageReference Include="stateless" Version="5.10.1" />
     </ItemGroup>
   ```

3. Come up with a fancy PlantUML state machine diagram, for example:

    ![SimpleStateMachine.puml](http://www.plantuml.com/plantuml/proxy?cache=no&src=https://raw.githubusercontent.com/vrenken/EtAlii.Generators/main/Source/EtAlii.Generators.Stateless.Tests/SimpleStateMachine.puml)

4. Put the PlantUML diagram in a file, for example `SimpleStateMachine.puml` as specified below:
   ```puml
    @startuml
    'stateless namespace EtAlii.Generators.Stateless.Tests
    'stateless class SimpleStateMachineBase

    [*] -> State1 << async (string name) >> : Start
    State1 : this is a string
    State1 : this is another string
    State1 -> State2 << async >> : Continue
    State2 -down-> [*] : Stop
    State2 -> State2 << (string name) >> : Check
    State2 -up-> State3 << async >> : Continue
    State3 -up-> State4 : Continue
    @enduml
   ```
   Make sure to notice the stateless parameters added at the start of the file. They help the code generation process understand which class/namespace to generate.


5. Reference the file from the project (.csproj) as a `StatelessFile` entry:
   ```csproj
     <ItemGroup>
       <Stateless Include="MyFancyStateMachine.puml" />
     </ItemGroup>
   ```
6. Compile the project - Antlr is used to parse the puml file and instruct the Roslyn generator to create C# state machine code according to the states, triggers and transitions defined in the diagram.


7. Add a class file to implement the needed state machine behavior.
   ```cs
    namespace EtAlii.Generators.Stateless.Tests
    {
        using System;
        using System.Collections.Generic;

        public class SimpleStateMachine : SimpleStateMachineBase
        {
            public List<string> Actions { get; } = new List<string>();

            protected override void OnState1Entered() => Actions.Add("State 1 entered");

            protected override void OnState1Exited() => Actions.Add("State 1 exited");
        }
    }
   ```
8. Create and trigger the state machine from somewhere in your code, for example as done in the Main method below.
   ```cs
    namespace EtAlii.Generators.Stateless.Tests
    {
        using System;

        public static class Program
        {
            public static void Main()
            {
                var stateMachine = new SimpleStateMachine();
                stateMachine.Start();
                Console.WriteLine(stateMachine.Actions[0]);
                stateMachine.Continue();
            }
        }
    }
   ```
9. Enjoy the magic that code-generation can bring to your project.


10. Star this project if you like it :-)

## features:


<table width="100%">
    <thead>
        <td><b>Stateless</b></td>
        <td><b>PlantUML</b></td>
    </thead>
    <tr valign="top">
        <td>
        Supported: <br/>
        - State/Trigger enum creation<br/>
        - State machine instantiation<br/>
        - OnEntry/OnExit configuration and method mapping<br/>
        - Synchronous / Asynchronous transitions<br/>
        - Transition method parameterization<br/>
        Unsupported: <br/>
        - Guards <br/>
        </td>
        <td>
        Supported: <br/>
        - Named states<br/>
        - Named triggers/transitions<br/>
        - Internal transitions<br/>
        - Composite/nested/sub states<br/>
        - Notes + titles<br/>
        - Begin/End<br/>
        Unsupported: <br/>
        - Fork<br/>
        - History<br/>
        - Concurrent state<br/>
        - Conditional/choice<br/>
        </td>
    </tr>
</table>

## Advanced features:

1. Synchronous / Asynchronous transitions:
   The code generation can be signalled to create asynchronous methods by providing stereotype details on the needed transitions:
   ```puml

   ```
   Please take notice:
    - The `OnEntry` method will also be made asynchronous when all _inbound_ transitions are tagged as being asynchronous.
    - The `OnExit` method will also be made asynchronous when all _outbound_ transitions are tagged as being asynchronous.
    - When triggers are to be used in both synchronous and asynchronous two different methods will be generated: One for synchronous invocation and one for asynchronous invocation.
      It is up to the developer to call the right one.
