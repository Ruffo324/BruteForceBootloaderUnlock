using System;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using TryNBootLoader.Program.Attributes;
using TryNBootLoader.Program.Enums;
using TryNBootLoader.Program.Exceptions;
using TryNBootLoader.Program.Models;

namespace TryNBootLoader.Program.Services
{
	internal class ADBProcessService : ProcessBasedService
	{
		public ADBProcessService(ProcessService processService) : base(processService)
		{
			// TODO [C.Groothoff]: HANDLE "* adb.exe: device unauthorized."
		}

		public override string ProcessName => "adb";

		[ProcessFunctionSpecifiedInformation("devices -l")]
		public async Task<ImmutableArray<ADBDevicesEntry>> GetConnectedDevices()
			=> (await EvaluateWrapperFunction(MethodBase.GetCurrentMethod()))
				.Select(line => line.Split(" ", StringSplitOptions.RemoveEmptyEntries))
				.Where(line => line.Length >= 3)
				.SkipWhile(line => ResolveStateFromText(line[1], false) == ADBDevicesDeviceState.Unknown)
				.Select(line => new ADBDevicesEntry(line[0], ResolveStateFromText(line[1]), string.Join(' ', line[2..])))
				.ToImmutableArray();

		private ADBDevicesDeviceState ResolveStateFromText(string textState, bool throwException = true)
			=> textState.ToLower() switch {
				"offline" => ADBDevicesDeviceState.Offline,
				"device" => ADBDevicesDeviceState.Device,
				"unauthorized" => ADBDevicesDeviceState.Unauthorized,
				_ => throwException
					? throw new UnknownADBDevicesStateException(textState)
					: ADBDevicesDeviceState.Unknown
			};

		/**
		// TODO [C.Groothoff]:	Remove cheatsheet
		* Phone Info
		 
			adb get-statе (print device state)
			adb get-serialno (get the serialNumber number)
			adb shell dumpsys iphonesybinfo (get the IMEI)
			adb shell netstat (list TCP connectivity)
			adb shell pwd (print current working directory)
			adb shell dumpsys battery (battery status)
			adb shell pm list features (list phone features)
			adb shell service list (list all services)
			adb shell dumpsys activity <package>/<activity> (activity info)
			adb shell ps (print process status)
			adb shell wm size (displays the current screen resolution)
			dumpsys window windows | grep -E 'mCurrentFocus|mFocusedApp' (print current app's opened activity)
		*/
	}

	// TODO [C.Groothoff]:  adb -s 544b4d4146423498 shell
}

namespace TryNBootLoader.Program.Models
{
}

namespace TryNBootLoader.Program.Enums
{
}
