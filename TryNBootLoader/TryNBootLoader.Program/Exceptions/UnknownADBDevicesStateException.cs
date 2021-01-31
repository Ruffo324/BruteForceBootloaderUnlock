using System;

namespace TryNBootLoader.Program.Exceptions
{
	public class UnknownADBDevicesStateException : Exception
	{
		public override string Message { get; }

		public UnknownADBDevicesStateException(string stateText)
		{
			Message = $"The device state '{stateText}' from the 'adb devices -l' call is unknown. Missing implementation?";
		}
	}
}
