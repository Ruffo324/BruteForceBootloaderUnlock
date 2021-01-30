using System.Threading.Tasks;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;
using TryNBootLoader.Program.Constants;
using TryNBootLoader.Program.Extensions;

namespace TryNBootLoader.Program
{
	internal class Program
	{
		private static Task Main(string[] args)
		{
			// TODO [C.Groothoff]: Remove file ProgramUglyAndWrong after working state!
			InitializeLogger();
			Log.Logger.PrintLine();
			Log.Information("Welcome to \"{ProgrammName}\", we get ur bootloader unlocked!", nameof(TryNBootLoader));
			Log.Information("Logger initialized..");
			Log.Logger.PrintLine();

			return Task.CompletedTask;
		}


		private static void InitializeLogger()
		{
			Log.Logger = new LoggerConfiguration()
				.MinimumLevel.ControlledBy(BuildConstants.LoggingLevelSwitch)
				.WriteTo.Console(
					outputTemplate:
					"{Timestamp:yyyy-MM-dd HH:mm:ss.fff} | [{Level:u4}] | {Message:lj}{NewLine}{Exception}",
					theme: AnsiConsoleTheme.Code)
				.CreateLogger();
		}
	}
}
