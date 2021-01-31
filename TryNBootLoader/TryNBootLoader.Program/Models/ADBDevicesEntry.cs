using System;
using TryNBootLoader.Program.Enums;

namespace TryNBootLoader.Program.Models
{
	/// <summary>
	/// Entry of the "adb device" command.
	/// </summary>
	internal readonly struct ADBDevicesEntry : IEquatable<ADBDevicesEntry>
	{
		public readonly string SerialNumber;
		public readonly ADBDevicesDeviceState State;

		/// <summary>
		/// > "Tells you what the device is."
		/// > Quoted from https://developer.android.com/studio/command-line/adb
		/// </summary>
		public readonly string Description;

		public ADBDevicesEntry(string serialNumber, ADBDevicesDeviceState state, string description)
		{
			SerialNumber = serialNumber;
			State = state;
			Description = description;
		}

		public bool Equals(ADBDevicesEntry other)
			=> SerialNumber == other.SerialNumber && Description == other.Description && State == other.State;

		public override bool Equals(object obj)
			=> obj is ADBDevicesEntry other && Equals(other);

		public override int GetHashCode()
			=> HashCode.Combine(SerialNumber, Description, (int)State);

		public static bool operator ==(ADBDevicesEntry left, ADBDevicesEntry right)
			=> left.Equals(right);

		public static bool operator !=(ADBDevicesEntry left, ADBDevicesEntry right)
			=> !left.Equals(right);
	}
}
