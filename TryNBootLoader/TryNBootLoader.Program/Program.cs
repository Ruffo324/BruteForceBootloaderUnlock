using System.Threading.Tasks;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;
using TryNBootLoader.Program.Constants;
using TryNBootLoader.Program.Enums;
using TryNBootLoader.Program.Exceptions;
using TryNBootLoader.Program.Extensions;

namespace TryNBootLoader.Program
{
	internal static class Program
	{
		private static async Task<int> Main(string[] args)
		{
			// TODO [C.Groothoff]: Remove file ProgramUglyAndWrong after working state!
			InitializeLogger();
			Log.Logger.PrintLine();
			Log.Information("Logger initialized..");

			var exitCode = await HandleExitCodes();

			if (exitCode <= 0)
			{
				return (int)exitCode;
			}

			Log.Logger.PrintLine();
			Log.Warning("Exited with exit code '{ExitCodeNr} ({ExitCodeStr})!", (int)exitCode, exitCode);

			return (int)exitCode;
		}


		/// <summary>
		/// Handles specified exceptions to produce the correct exit code. If no exception happens <see cref="ExitCode.Success"/> get's returned.
		/// </summary>
		private static async Task<ExitCode> HandleExitCodes()
		{
			try
			{
				await WorkerCalls();
			}
			catch (ProcessNotStartableException)
			{
				return ExitCode.RequiredProcessNotInstalled;
			}

			return ExitCode.Success;
		}

		private static async Task WorkerCalls()
		{
			var worker = new Worker();
			await worker.PreWork();
		}

		private static void InitializeLogger()
			=> Log.Logger = new LoggerConfiguration()
				.MinimumLevel.ControlledBy(BuildConstants.LoggingLevelSwitch)
				.WriteTo.Console(
					outputTemplate:
					"{Timestamp:yyyy-MM-dd HH:mm:ss.fff} | [{Level:u4}] | {Message:lj}{NewLine}{Exception}",
					theme: AnsiConsoleTheme.Code)
				.CreateLogger();
	}
}
