namespace EtAlii.Generators.MicroMachine
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using EtAlii.Generators.PlantUml;
    using Serilog;

    public class TriggerMethodWriter
    {
        private readonly ParameterConverter _parameterConverter;
        private readonly TransitionConverter _transitionConverter;
        private readonly IStateMachineLifetime _lifetime;
        private readonly StateFragmentHelper _stateFragmentHelper;
        private readonly MethodChainBuilder _methodChainBuilder;
        private readonly ILogger _log = Log.ForContext<TriggerMethodWriter>();

        public TriggerMethodWriter(
            ParameterConverter parameterConverter,
            TransitionConverter transitionConverter,
            IStateMachineLifetime lifetime,
            StateFragmentHelper stateFragmentHelper,
            MethodChainBuilder methodChainBuilder)
        {
            _transitionConverter = transitionConverter;
            _lifetime = lifetime;
            _stateFragmentHelper = stateFragmentHelper;
            _methodChainBuilder = methodChainBuilder;
            _parameterConverter = parameterConverter;
        }

        /// <summary>
        /// Write the trigger methods through which the individual triggers can be fired.
        /// There is some magic involved in creating duplicates for cases where both async
        /// and sync methods are needed.
        /// Also the sequence of parameter types are important as C# won't allow methods with the
        /// same name, parameters with the same type but other names.
        /// </summary>
        public void WriteTriggerMethods(WriteContext<StateMachine> context)
        {
            _log.Information("Writing trigger methods for {ClassName}", context.Instance.ClassName);

            context.Writer.WriteLine("// The methods below can be each called to fire a specific trigger");
            context.Writer.WriteLine("// and cause the state machine to transition to another state.");
            context.Writer.WriteLine();

            foreach (var trigger in context.Instance.AllTriggers)
            {
                var syncTransitions = _stateFragmentHelper.GetSyncTransitions(context.Instance.StateFragments);
                var syncTransitionSets = _transitionConverter.ToTransitionsSetsPerTriggerAndUniqueParameters(syncTransitions, trigger);
                var syncWrite = new Func<string, string, string, string, string, string>((triggerName, typedParameters, genericParameters, triggerParameter, namedParameters)
                    => $"public void {triggerName}({typedParameters}) => RunOrQueueTransition(new SyncTransition(() => {triggerName}Transition({namedParameters})));// {genericParameters}({triggerParameter}{namedParameters});");
                WriteTransitionMethod(context, syncTransitionSets);
                WriteTriggerMethods(context, syncTransitionSets, "sync", syncWrite);

                var asyncTransitions = _stateFragmentHelper.GetAsyncTransitions(context.Instance.StateFragments);
                var asyncTransitionSets = _transitionConverter.ToTransitionsSetsPerTriggerAndUniqueParameters(asyncTransitions, trigger);
                var asyncWrite = new Func<string, string, string, string, string, string>((triggerName, typedParameters, genericParameters, triggerParameter, namedParameters) => $"public Task {triggerName}Async({typedParameters}) => RunOrQueueTransitionAsync(new AsyncTransition(() => {triggerName}TransitionAsync({namedParameters})));// {genericParameters}({triggerParameter}{namedParameters});");
                WriteTransitionMethod(context, asyncTransitionSets);
                WriteTriggerMethods(context, asyncTransitionSets, "async", asyncWrite);
            }
        }

        public void WriteTriggerMethods(WriteContext<StateMachine> context, IEnumerable<Transition[]> transitionSets, string triggerType, Func<string, string, string, string, string, string> write)
        {
            foreach (var transitionSet in transitionSets)
            {
                var transition = transitionSet.First();

                _log
                    .ForContext("Transition", transition, true)
                    .Information("Writing trigger method for transition from {FromState} by {Trigger} to {ToState}", transition.From, transition.Trigger, transition.To);

                var parameters = transition.Parameters;
                var typedParameters = _parameterConverter.ToTypedNamedVariables(parameters);
                var genericParameters = _parameterConverter.ToGenericParameters(parameters);
                var namedParameters = parameters.Any() ? _parameterConverter.ToNamedVariables(parameters) : string.Empty;
                var triggerParameter = _transitionConverter.ToTriggerParameter(transition);

                WriteComment(context, transitionSet, $"Depending on the current state, call this method to trigger one of the {triggerType} transitions below:");
                context.Writer.WriteLine(write(transition.Trigger, typedParameters, genericParameters, triggerParameter, namedParameters));
                context.Writer.WriteLine();
            }
        }

        private void WriteTransitionMethod(WriteContext<StateMachine> context, Transition[][] transitionSet)
        {
            if (transitionSet.FirstOrDefault() is not { } transitionSetTransitions)
            {
                return;
            }

            if (transitionSetTransitions.FirstOrDefault() is not { } transition)
            {
                return;
            }

            _log
                .ForContext("Transition", transition, true)
                .Information("Writing transition method for transition from {FromState} by {Trigger} to {ToState}", transition.Trigger, transition.From, transition.To);

            var parameters = transition.Parameters;
            var typedParameters = _parameterConverter.ToTypedNamedVariables(parameters);
            var namedParameters = parameters.Any() ? _parameterConverter.ToNamedVariables(parameters) : string.Empty;

            context.Writer.WriteLine($"private {(transition.IsAsync ? "async Task" : "void")} {transition.Trigger}Transition{(transition.IsAsync ? "Async" : "")}({typedParameters})");
            context.Writer.WriteLine("{");
            context.Writer.Indent += 1;
            context.Writer.WriteLine("switch (_state)");
            context.Writer.WriteLine("{");
            context.Writer.Indent += 1;

            var transitions = context.Instance.AllTransitions
                .Where(t => t.Trigger == transition.Trigger)
                .ToArray();

            var writtenCases = new List<string>();

            foreach (var tr in transitions)
            {
                WriteTransitionStateCase(context, tr, namedParameters, transition.IsAsync, writtenCases);
            }

            context.Writer.WriteLine("default:");
            context.Writer.Indent += 1;
            context.Writer.WriteLine($"throw new NotSupportedException($\"Trigger {transition.Trigger} is not supported in state {{_state}}\");");
            context.Writer.Indent -= 1;
            context.Writer.Indent -= 1;
            context.Writer.WriteLine("}");
            context.Writer.Indent -= 1;
            context.Writer.WriteLine("}");
        }

        private void WriteTransitionStateCase(WriteContext<StateMachine> context, Transition transition, string namedParameters, bool isAsync, List<string> writtenCases)
        {
            _log
                .ForContext("Transition", transition, true)
                .Information("Writing transition state case for transition from {FromState} by {Trigger} to {ToState}", transition.From, transition.Trigger, transition.To);

            var methodChains = _methodChainBuilder.Build(context.Instance, transition);

            _log
                .ForContext("Transition", transition, true)
                .Information("Found {MethodChainCount} method chains alternatives for {Trigger} in {FromState}", methodChains.Length, transition.Trigger, transition.From);

            foreach(var methodChain in methodChains)
            {
                if (!writtenCases.Contains(methodChain.From))
                {
                    writtenCases.Add(methodChain.From);

                    _log
                        .ForContext("Transition", transition, true)
                        .Information("Writing method chain for transition from {FromState} by {Trigger} to {ToState}", methodChain.From, transition.Trigger, methodChain.To);

                    var triggerVariableName = $"trigger{methodChain.From}";
                    var triggerTypeName = $"{transition.Trigger}Trigger";

                    WriteCaseHeader(context, methodChain, triggerVariableName, triggerTypeName, namedParameters);

                    WriteExitCalls(context, isAsync, methodChain, triggerTypeName, triggerVariableName);
                    WriteEntryCalls(context, isAsync, methodChain, triggerVariableName, triggerTypeName);

                    WriteCaseTail(context, methodChain, isAsync, triggerVariableName);

                }
                else
                {
                    _log
                        .ForContext("Transition", transition, true)
                        .Information("Skipping method chain for transition from {FromState} by {Trigger} to {ToState}: Already written", methodChain.From, transition.Trigger, methodChain.To);
                }
            }
        }

        private void WriteCaseHeader(
            WriteContext<StateMachine> context,
            MethodChain methodChain,
            string triggerVariableName,
            string triggerTypeName,
            string namedParameters)
        {
            context.Writer.WriteLine($"case State.{methodChain.From}:");
            context.Writer.Indent += 1;
            context.Writer.WriteLine($"_state = State.{methodChain.To};");
            context.Writer.WriteLine($"Trigger {triggerVariableName} = new {triggerTypeName}({namedParameters});");

        }

        private void WriteCaseTail(WriteContext<StateMachine> context, MethodChain methodChain, bool isAsync, string triggerVariableName)
        {
            var toSuperState = _stateFragmentHelper
                .GetAllSuperStates(context.Instance.StateFragments)
                .SingleOrDefault(ss => ss.Name == methodChain.To);
            if (toSuperState != null)
            {
                var subTransitions = toSuperState.StateFragments
                    .OfType<Transition>()
                    .ToArray();

                var unnamedInboundTransition = subTransitions
                    .Where(t => !t.HasConcreteTriggerName)
                    .Where(t => t.Parameters.Length == 0)
                    .SingleOrDefault(t => t.From == _lifetime.BeginStateName);
                if (unnamedInboundTransition != null)
                {
                    var choice = context.Instance.GenerateTriggerChoices
                        ? $", _{_parameterConverter.ToCamelCase(unnamedInboundTransition.To)}Choices"
                        : "";

                    var writeAsync = _stateFragmentHelper.HasOnlyAsyncInboundTransitions(context.Instance, unnamedInboundTransition.To);

                    string prefix, postFix;
                    if (isAsync)
                    {
                        prefix = writeAsync ? "await " : "";
                        postFix = writeAsync ? ".ConfigureAwait(false)" : "";
                    }
                    else
                    {
                        prefix = writeAsync ? "Task.Run(() => " : "";
                        postFix = writeAsync ? ").Wait()" : "";
                    }

                    context.Writer.WriteLine("// We also need to activate the first substate.");
                    context.Writer.WriteLine($"_state = State.{unnamedInboundTransition.To};");
                    context.Writer.WriteLine($"{prefix}On{unnamedInboundTransition.To}Entered({triggerVariableName}{choice}){postFix};");
                }
            }

            context.Writer.WriteLine("break;");
            context.Writer.Indent -= 1;
        }
        private void WriteEntryCalls(WriteContext<StateMachine> context, bool isAsync, MethodChain methodChain, string triggerVariableName, string triggerTypeName)
        {
            foreach (var call in methodChain.EntryCalls)
            {
                var writeAsync = _stateFragmentHelper.HasOnlyAsyncInboundTransitions(context.Instance, call.State);

                string prefix, postFix;
                if (isAsync)
                {
                    prefix = writeAsync ? "await " : "";
                    postFix = writeAsync ? ".ConfigureAwait(false)" : "";
                }
                else
                {
                    prefix = writeAsync ? "Task.Run(() => " : "";
                    postFix = writeAsync ? ").Wait()" : "";
                }

                var choice = context.Instance.GenerateTriggerChoices
                    ? $", _{_parameterConverter.ToCamelCase(call.State)}Choices"
                    : "";
                context.Writer.WriteLine($"{prefix}On{call.State}Entered({triggerVariableName}{choice}){postFix};");
                if (!call.IsSuperState)
                {
                    context.Writer.WriteLine($"{prefix}On{call.State}Entered(({triggerTypeName}){triggerVariableName}{choice}){postFix};");
                }
            }
        }

        private void WriteExitCalls(WriteContext<StateMachine> context, bool isAsync, MethodChain methodChain, string triggerTypeName, string triggerVariableName)
        {
            foreach (var call in methodChain.ExitCalls)
            {
                var writeAsync = _stateFragmentHelper.HasOnlyAsyncOutboundTransitions(context.Instance, call.State);

                string prefix, postFix;
                if (isAsync)
                {
                    prefix = writeAsync ? "await " : "";
                    postFix = writeAsync ? ".ConfigureAwait(false)" : "";
                }
                else
                {
                    prefix = writeAsync ? "Task.Run(() => " : "";
                    postFix = writeAsync ? ").Wait()" : "";
                }

                if (!call.IsSuperState)
                {
                    context.Writer.WriteLine($"{prefix}On{call.State}Exited(({triggerTypeName}){triggerVariableName}){postFix};");
                }

                context.Writer.WriteLine($"{prefix}On{call.State}Exited({triggerVariableName}){postFix};");
            }
        }


        private void WriteComment(WriteContext<StateMachine> context, Transition[] transitionSet, string message)
        {
            context.Writer.WriteLine($"/// <summary>");
            context.Writer.WriteLine($"/// {message}<br/>");
            foreach (var transition in transitionSet)
            {
                context.Writer.WriteLine($"/// {transition.From} --&gt; {transition.To} : {transition.Trigger}<br/>");
            }
            context.Writer.WriteLine($"/// </summary>");
        }
    }
}
