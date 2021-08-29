namespace EtAlii.Generators.MicroMachine
{
    public class MethodCall
    {
        public string MethodName { get; }
        public string Cast { get; }
        public bool IsAsync { get; }

        public MethodCall(string methodName, string cast, bool isAsync)
        {
            MethodName = methodName;
            IsAsync = isAsync;
            Cast = cast ?? string.Empty;
        }
    }
}
