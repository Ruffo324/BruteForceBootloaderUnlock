using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Ruffo324.Toolbox.Console;
using Serilog;
using TryNBootLoader.Program.Exceptions;
using TryNBootLoader.Program.Extensions;
using TryNBootLoader.Program.Models;
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
			var workDevice = DetermineWantedDevice();
			// TODO [C.Groothoff]: Next: IMEI!
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

		private async Task<ADBDevicesEntry> DetermineWantedDevice()
		{
			Log.Information("Checking which devices are available over adb..");
			ImmutableArray<ADBDevicesEntry> foundDevices = default;

			// Scanning and handling no devices found.
			while (foundDevices == default || foundDevices.Length == 0)
			{
				if (foundDevices != default)
				{
					const string adbDevicesCommand = "adb devices -l";
					Log.Warning("No devices found with '{DevicesCommand}'!", adbDevicesCommand);
					Log.Information("Please ensure that your device is started, and USB-Debugging is enabled!");

					// Users says when to retry, or to abort the whole process.
					await UserInput.ContinueOrAbort(continueKeyAction: "for retry").ConfigureAwait(true);
				}

				foundDevices = await _adbProcessService.GetConnectedDevices().ConfigureAwait(false);
			}

			// Just one device found? Nothing more to do then. 🥳
			if (foundDevices.Length == 1)
			{
				return foundDevices.First();
			}

			// Handle multiple devices found.
			Log.Warning("Detected '{DevicesCount}' devices. Please select the wanted device..", foundDevices.Length);

			return await UserInput.SelectItemOrAbort(foundDevices,
					d => new[] { d.SerialNumber, $"{d.State}", d.Description },
					new[] { "Serial number", "device state", "device description" })
				.ConfigureAwait(true);
		}
	}
}
