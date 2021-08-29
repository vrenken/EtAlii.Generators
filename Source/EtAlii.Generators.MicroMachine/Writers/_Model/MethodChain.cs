namespace EtAlii.Generators.MicroMachine
{
    using System.Collections.Generic;

    public class MethodChain
    {
        public MethodCall[] Calls { get; private set; }


        public static MethodChain Create(string fromState, string trigger, string toState)
        {
            var calls = new List<MethodCall>();

            return new MethodChain { Calls = calls.ToArray() };
        }
    }
}
