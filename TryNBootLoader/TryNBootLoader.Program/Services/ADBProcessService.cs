namespace TryNBootLoader.Program.Services
{
	internal class ADBProcessService : ProcessBasedService
	{
		public ADBProcessService(ProcessService processService) : base(processService)
		{
		}

		public override string ProcessName => "adb";
	}
}
