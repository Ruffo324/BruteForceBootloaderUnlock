using System;

namespace TryNBootLoader.Program.Exceptions
{
	public class ProcessNotStartableException : Exception
	{
		public override string Message { get; }

		public ProcessNotStartableException(string processName)
		{
			Message = $"Unable to start process '{processName}'!";
		}
	}
}
