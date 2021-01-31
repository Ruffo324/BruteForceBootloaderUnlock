namespace TryNBootLoader.Program.Services
{
	internal class FastbootProcessService : ProcessBasedService
	{
		public FastbootProcessService(ProcessService processService) : base(processService)
		{
		}

		public override string ProcessName => "fastboot";
	}
}
