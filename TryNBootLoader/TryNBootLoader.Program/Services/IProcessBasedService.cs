using System.Threading.Tasks;

namespace TryNBootLoader.Program.Services
{
	/// <summary>
	/// Services for e.g. "adb" or "fastboot".
	/// </summary>
	internal abstract class ProcessBasedService
	{
		private protected readonly ProcessService ProcessService;

		protected ProcessBasedService(ProcessService processService)
		{
			ProcessService = processService;
		}

		public abstract string ProcessName { get; }
		public Task<bool> Startable => ProcessService.IsProcessStartAble(ProcessName);
	}
}
