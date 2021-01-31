using System;

namespace Ruffo324.Toolbox.Exceptions
{
	/// <summary>
	/// Thrown if the user decides to abort the whole process. 
	/// </summary>
	public class UserAbortedException : Exception
	{
		public override string Message { get; } = "User aborted.";
	}
}
