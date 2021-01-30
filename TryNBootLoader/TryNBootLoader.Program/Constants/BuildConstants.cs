using Serilog.Core;
using Serilog.Events;

namespace TryNBootLoader.Program.Constants
{
	internal class BuildConstants
	{
		public static readonly LoggingLevelSwitch LoggingLevelSwitch =
#if DEBUG
			new(LogEventLevel.Verbose);
#else
			new LoggingLevelSwitch(LogEventLevel.Information);
#endif
	}
}
