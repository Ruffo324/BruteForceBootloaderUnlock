using System;
using TryNBootLoader.Program.Exceptions;

namespace TryNBootLoader.Program.Enums
{
	/// <summary>
	/// The possible states in the "adb devices -l" result. 
	/// </summary>
	internal enum ADBDevicesDeviceState
	{
		/// <summary>
		///  The device is not connected to adb or is not responding.
		/// </summary>
		Offline,

		/// <summary>
		/// The device is now connected to the adb server.
		/// Note that this state does not imply that the Android system is fully booted and operational because the device connects
		/// to adb while the system is still booting. However, after boot-up, this is the normal operational state of an device.
		/// </summary>
		Device,

		/// <summary>
		/// There is no device connected.
		/// </summary>
		[Obsolete("For more information see "
			+ "https://stackoverflow.com/questions/42804554/when-should-i-see-no-device-output-when-running-adb-devices")]
		NoDevice,

		/// <summary>
		/// Debugging through this computer is not allowed. Allow in popup or reconnect phone to fix this. 
		/// </summary>
		Unauthorized,
		
		/// <summary>
		/// Used for filtering all stuff above the "adb devices -l" result.
		/// Otherwise unknown states are thrown with the <see cref="UnknownADBDevicesStateException"/>
		/// </summary>
		Unknown
	}
}
