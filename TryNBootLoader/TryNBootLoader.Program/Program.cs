using System;
using System.Threading.Tasks;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;
using TryNBootLoader.Program.Constants;
using TryNBootLoader.Program.Extensions;
using TryNBootLoader.Program.Services;

namespace TryNBootLoader.Program
{
	internal class Worker
	{
		private readonly ADBProcessService _adbProcessService;
		private readonly FastbootProcessService _fastbootProcessService;

		public Worker()
		{
			var processService = new ProcessService();
			Log.Information("{Service} initialized..", nameof(ProcessService));

			_adbProcessService = new ADBProcessService(processService);
			Log.Information("{Service} initialized..", nameof(ADBProcessService));

			_fastbootProcessService = new FastbootProcessService(processService);
			Log.Information("{Service} initialized..", nameof(FastbootProcessService));


			PreWork().Start();
		}

		/// <summary>
		/// Checking if all host requirements are given. Also gets the device IMEI automatically or from user. 
		/// </summary>
		/// <returns></returns>
		private async Task PreWork()
		{
			// Ensures that the required android programs are correctly installed.
			Log.Information("Checking host requirements..");
			await CheckingHostRequirements(_adbProcessService);
			await CheckingHostRequirements(_fastbootProcessService);

			Log.Information("So far, everything seems ready to go! 🚀");
			Log.Logger.PrintLine();

			// Try resolving the IMEI automatic.
			Log.Information("! 🚀");
		}

		private async Task CheckingHostRequirements(ProcessBasedService processServiceToCheck)
		{
			var processName = processServiceToCheck.ProcessName;
			if (!await processServiceToCheck.Startable)
			{
				Log.Fatal(
					"Please ensure, that '{Process}' is installed to the system, and added correctly to the environment paths!",
					processName);

				throw new InvalidOperationException($"Unable to start process {processName}!");
			}

			Log.Information("'{Process}' is correctly installed on the system. ✅", processName);
		}
	}

	internal class Program
	{
		private static Task Main(string[] args)
		{
			// TODO [C.Groothoff]: Remove file ProgramUglyAndWrong after working state!
			InitializeLogger();
			Log.Logger.PrintLine();
			Log.Information("Logger initialized..");

			Log.Logger.PrintLine();
			Log.Information("Welcome to \"{ProgramName}\", we get ur bootloader unlocked!", nameof(TryNBootLoader));


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
