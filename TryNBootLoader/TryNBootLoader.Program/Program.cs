using System.Threading.Tasks;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;
using TryNBootLoader.Program.Constants;
using TryNBootLoader.Program.Extensions;

namespace TryNBootLoader.Program
{
	internal static class Program
	{
		private static async Task Main(string[] args)
		{
			// TODO [C.Groothoff]: Remove file ProgramUglyAndWrong after working state!
			InitializeLogger();
			Log.Logger.PrintLine();
			Log.Information("Logger initialized..");

			var worker = new Worker();
			await worker.PreWork();
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
