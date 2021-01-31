using System.Threading.Tasks;
using Serilog;
using TryNBootLoader.Program.Exceptions;
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
		}

		/// <summary>
		/// Checking if all host requirements are given.
		/// Also gets the device IMEI automatically or from user. 
		/// </summary>
		public async Task PreWork()
		{
			// Ensures that the required android programs are correctly installed.
			Log.Information("Checking host requirements..");
			await CheckingHostRequirements(_adbProcessService).ConfigureAwait(false);
			await CheckingHostRequirements(_fastbootProcessService).ConfigureAwait(false);

			Log.Information("So far, everything seems ready to go! 🚀");
			Log.Logger.PrintLine();
			Log.Information("Welcome to \"{ProgramName}\", we get ur bootloader unlocked!", nameof(TryNBootLoader));

			// Now detect phone and try resolving the IMEI automatic.
			

		}


		private async Task CheckingHostRequirements(ProcessBasedService processServiceToCheck)
		{
			var processName = processServiceToCheck.ProcessName;
			if (!await processServiceToCheck.Startable.ConfigureAwait(false))
			{
				Log.Fatal(
					"Please ensure, that '{Process}' is installed to the system, and added correctly to the environment paths!",
					processName);

				throw new ProcessNotStartableException(processName);
			}

			Log.Information("'{Process}' is correctly installed on the system. ✅", processName);
		}
	}
}
