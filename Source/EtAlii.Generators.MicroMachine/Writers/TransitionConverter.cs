// Copyright (c) Peter Vrenken. All rights reserved. See https://github.com/vrenken/EtAlii.Generators for more information and the license.

namespace EtAlii.Generators.MicroMachine
{
    using System.Collections.Generic;
    using System.Linq;
    using EtAlii.Generators.PlantUml;

    public class TransitionConverter
    {
        private readonly ParameterConverter _parameterConverter;

        public TransitionConverter(ParameterConverter parameterConverter)
        {
            _parameterConverter = parameterConverter;
        }

        public IEnumerable<Transition[]> ToTransitionsSetsPerTriggerAndUniqueParameters(Transition[] transitions, string trigger)
        {
            return transitions
                .Where(t => t.Trigger == trigger)
                .Select(t => new {Transition = t, ParametersAsKey = string.Join(", ", t.Parameters.Select(p => p.Type))})
                .GroupBy(item => item.ParametersAsKey)
                .Select(g => g.Select(t => t.Transition).ToArray())
                .Where(s => s.Any())
                .ToArray();
        }

        public string ToTransitionMethodName(Transition transition)
        {
            return transition.From == transition.To
                ? $"On{transition.To}Internal{transition.Trigger}Trigger"
                : $"On{transition.To}EnteredFrom{transition.Trigger}Trigger";
        }

        public string ToTriggerParameter(Transition transition)
        {
            return transition.Parameters.Any()
                ? ToTriggerMemberName(transition)
                : $"Trigger.{transition.Trigger}";
        }

        public string ToTriggerMemberName(Transition transition)
        {
            var parametersCombinedWithAnd = transition.Parameters.Any()
                ? $"With{string.Join("And", transition.Parameters.Select(_parameterConverter.ToParameterName))}"
                : string.Empty;
            return $"_{_parameterConverter.ToCamelCase(transition.Trigger)}{parametersCombinedWithAnd}Trigger";
        }
    }
}
