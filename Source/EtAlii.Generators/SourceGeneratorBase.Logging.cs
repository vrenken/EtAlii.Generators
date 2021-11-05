// Copyright (c) Peter Vrenken. All rights reserved. See https://github.com/vrenken/EtAlii.Generators for more information and the license.
namespace EtAlii.Generators
{
    using Serilog;
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Reflection;
    using Serilog.Core;

    public partial class SourceGeneratorBase<T>
    {
        private ILogger _log;

        [SuppressMessage(
            category: "Sonar Code Smell",
            checkId: "S5332:Using http protocol is insecure. Use https instead",
            Justification = "Safe to do so: This code only gets enabled at the local development machine.")]
        [SuppressMessage(
            category: "Sonar Code Smell",
            checkId: "S1075:URIs should not be hardcoded",
            Justification = "We don't have access to a settings.json and the first attempt to create one from scratch exploded due to the limitations of Roslyn generator packages.")]
        [SuppressMessage(
            category: "Sonar Code Smell",
            checkId: "S4792:Make sure that this logger's configuration is safe",
            Justification = "Safe to do so: It's a hardcoded configuration, fully instantiated in code. We need to change (or even disable this), but not right now.")]
        private void SetupLogging()
        {
            var loggerConfiguration = new LoggerConfiguration();

            var executingAssemblyName = Assembly.GetCallingAssembly().GetName();

            loggerConfiguration
                .MinimumLevel.Verbose()
                .Enrich.FromLogContext()
                // These ones do not give elegant results during unit tests.
                // .Enrich.WithAssemblyName()
                // .Enrich.WithAssemblyVersion()
                // Let's do it ourselves.
                .Enrich.WithProperty("RootAssemblyName", executingAssemblyName.Name)
                .Enrich.WithProperty("RootAssemblyVersion", executingAssemblyName.Version)
                .Enrich.WithProperty("UniqueProcessId", ShortId.GetId()); // An int process ID is not enough

            // I know, ugly patch, but it works. And it's better than making all global generators try to phone home...
            if (Environment.MachineName == "FRACTAL")
            {
                loggerConfiguration.WriteTo.Seq("http://seq.avalon:5341");
                Log.Logger = loggerConfiguration
                    .CreateLogger();
            }
            else
            {
                Log.Logger = Logger.None;
            }

            _log = Log.Logger
                .ForContext("SourceContext", "SourceGenerator")
                .ForContext("CodeGeneration", ShortId.GetId());

            _log.Information("Logging setup finished");

            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
        }

        private void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            AppDomain.CurrentDomain.UnhandledException -= OnUnhandledException;

            _log.Fatal(e.ExceptionObject as Exception, "Unhandled fatal exception");
        }
    }
}
