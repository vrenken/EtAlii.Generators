﻿namespace EtAlii.Generators.Stateless
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public partial class SourceGenerator
    {
        private StateTransition[][] ToTransitionsSetsWithUniqueParameters(StateTransition[] transitions)
        {
            return transitions
                .Select(t => new { Transition = t, ParametersAsKey = string.Join(", ", t.Parameters.Select(p => p.Type)) })
                .GroupBy(item => item.ParametersAsKey)
                .Select(g => g.Select(t => t.Transition).ToArray())
                .Where(s => s.Any())
                .ToArray();
        }
        private string ToTransitionMethodName(StateTransition transition)
        {
            return transition.From == transition.To
                ? $"On{transition.To}Internal{transition.Trigger}Trigger"
                : $"On{transition.To}EnteredFrom{transition.Trigger}Trigger";
        }
        private string ToTriggerParameter(StateTransition transition)
        {
            return transition.Parameters.Any()
                ? ToTriggerMemberName(transition)
                : $"Trigger.{transition.Trigger}";
        }

        private string ToTriggerMemberName(StateTransition transition)
        {
            var parametersCombinedWithAnd = transition.Parameters.Any()
                ? $"With{string.Join("And", transition.Parameters.Select(p => p.HasName ? ToPascalCase(p.Name) : ToCamelCase(p.Type)))}"
                : string.Empty;
            return $"_{ToCamelCase(transition.Trigger)}{parametersCombinedWithAnd}Trigger";
        }

        private string ToGenericParameters(Parameter[] parameters)
        {
            return parameters.Any()
                ? $"<{string.Join(", ", parameters.Select(p => p.Type))}>"
                : string.Empty;
        }

        private string ToTypedNamedVariables(Parameter[] parameters)
        {
            var result = new List<string>();
            for (var i = 0; i < parameters.Length; i++)
            {
                var type = parameters[i].Type;
                var name = parameters[i].HasName ? parameters[i].Name : $"@{ToCamelCase(parameters[i].Type)}{i}";
                result.Add($"{type} {name}");
            }

            return string.Join(", ", result);
        }

        private string ToNamedVariables(Parameter[] parameters)
        {
            var result = new List<string>();
            for (var i = 0; i < parameters.Length; i++)
            {
                var name = parameters[i].HasName ? parameters[i].Name : $"@{ToCamelCase(parameters[i].Type)}{i}";
                result.Add($"{name}");
            }
            return string.Join(", ", result);
        }

        private string ToPascalCase(string s)
        {
            var span = new Span<char>(s.ToCharArray());
            span[0] = char.ToUpper(span[0]);
            return span.ToString();
        }

        private string ToCamelCase(string s)
        {
            var span = new Span<char>(s.ToCharArray());
            span[0] = char.ToLower(span[0]);
            return span.ToString();
        }
    }
}
