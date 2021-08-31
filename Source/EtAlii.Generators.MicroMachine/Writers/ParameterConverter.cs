namespace EtAlii.Generators.MicroMachine
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using EtAlii.Generators.PlantUml;

    public class ParameterConverter
    {
        public string ToParameterName(Parameter parameter) => parameter.HasName ? ToPascalCase(parameter.Name) : ToCamelCase(parameter.Type);

        public string ToGenericParameters(Parameter[] parameters)
        {
            return parameters.Any()
                ? $"<{string.Join(", ", parameters.Select(p => p.Type))}>"
                : string.Empty;
        }

        public string ToTypedNamedVariables(Parameter[] parameters)
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

        public string[] ToProperties(Parameter[] parameters)
        {
            var result = new List<string>();
            for (var i = 0; i < parameters.Length; i++)
            {
                var type = parameters[i].Type;
                var name = parameters[i].HasName ? ToPascalCase(parameters[i].Name) : $"@{ToPascalCase(parameters[i].Type)}{i}";
                result.Add($"public {type} {name} {{ get; private set; }}");
            }

            return result.ToArray();
        }

        public string[] ToPropertyAssignments(Parameter[] parameters)
        {
            var result = new List<string>();
            for (var i = 0; i < parameters.Length; i++)
            {
                var propertyName = parameters[i].HasName ? ToPascalCase(parameters[i].Name) : $"@{ToPascalCase(parameters[i].Type)}{i}";
                var variableName = parameters[i].HasName ? parameters[i].Name : $"@{ToCamelCase(parameters[i].Type)}{i}";

                result.Add($"this.{propertyName} = {variableName};");
            }
            return result.ToArray();
        }

        public string ToNamedVariables(Parameter[] parameters)
        {
            var result = new List<string>();
            for (var i = 0; i < parameters.Length; i++)
            {
                var name = parameters[i].HasName ? parameters[i].Name : $"@{ToCamelCase(parameters[i].Type)}{i}";
                result.Add($"{name}");
            }
            return string.Join(", ", result);
        }

        public string ToPascalCase(string s)
        {
            var span = new Span<char>(s.ToCharArray());
            span[0] = char.ToUpper(span[0]);
            return span.ToString();
        }

        public string ToCamelCase(string s)
        {
            var span = new Span<char>(s.ToCharArray());
            span[0] = char.ToLower(span[0]);
            return span.ToString();
        }
    }
}
