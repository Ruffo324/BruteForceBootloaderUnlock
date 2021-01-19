using System;

namespace TryNBootLoader.Programm.Services
{
	/// <summary>
	/// Detects the used platform and handles the portable adb/fastboot and execution paths.
	/// </summary>
	internal class PlatformService : IPlatformService
	{
		public OperatingSystem OperatingSystem => Environment.OSVersion;
	}

	internal interface IPlatformService
	{
		public OperatingSystem OperatingSystem { get; }
	}
}
