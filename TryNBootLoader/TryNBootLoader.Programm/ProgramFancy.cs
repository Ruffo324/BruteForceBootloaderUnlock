namespace TryNBootLoader.Programm
{
    /*internal class ProgramFancy
    {
        /*private static Task Main(string[] args)
        {
            InitializeLogger();
            Log.Information("Logger initialized..");
            Log.Information("Initializing dependency injection..");
            using var kernel = InitializeDependencyInjection();

            return Task.CompletedTask;
        }#1#

        private static IKernel InitializeDependencyInjection()
        {
            var kernel = new StandardKernel();


            kernel.Bind<IPlatformService>().To<PlatformService>().InSingletonScope();

            return kernel;
        }

        private static void InitializeLogger()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.ControlledBy(BuildConstants.LoggingLevelSwitch)
                .WriteTo.Console(theme: AnsiConsoleTheme.Code)
                .CreateLogger();
        }
    }*/
}