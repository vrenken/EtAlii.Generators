namespace EtAlii.Generators
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    //Enhanced version of enum.parse
    //As this does not use reflection for each call it should be slightly
    //faster than Enum.Parse. Still we should think about removing this if it ever leads to any problem.
    public static class EnumHelpers<TEnum>
    {
        private static readonly Dictionary<string, TEnum> _enumNameCache;

        static EnumHelpers()
        {
            _enumNameCache = Enum.GetNames(typeof(TEnum)).ToDictionary(x => x, x => (TEnum)Enum.Parse(typeof(TEnum), x), StringComparer.OrdinalIgnoreCase);
        }

        public static bool TryParse(string value, out TEnum result)
        {
            return _enumNameCache.TryGetValue(value, out result);
        }
    }
}
