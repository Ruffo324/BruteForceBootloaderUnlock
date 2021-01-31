using System;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Diagnostics;
using Serilog;

namespace TryNBootLoader.Program.Services
{
	internal class ProcessService // TODO [C.Groothoff]: Create interface after done!
	{
		private static readonly TimeSpan ProcessCancellationTimeoutDuration = TimeSpan.FromSeconds(1);

		private static CancellationToken CreateTimeoutToken()
			=> new CancellationTokenSource(ProcessCancellationTimeoutDuration).Token;

		/// <summary>
		/// Checks if the given process can be started.
		/// If that's not the case, the program isn't installed or added to the paths.
		/// </summary>
		/// <param name="processName">The process name to check!</param>
		/// <returns></returns>
		public async Task<bool> IsProcessStartAble(string processName)
		{
			if (processName.Contains(" "))
				throw new ArgumentException(
					$"Spaces means arguments, found some in '{processName}'! We just want the process name!");

			try
			{
				var process = ProcessX.StartAsync(processName);
				await process.ToTask(CreateTimeoutToken()).ConfigureAwait(false);
				return true;
			}
			catch (OperationCanceledException)
			{
				Log.Warning("Starting process {ProcessName} was canceled by timeout!", processName);
			}
			catch (Exception ex)
			{
				Log.Warning("Starting {ProcessName} failed. Reason: {ExceptionReason}!", processName, ex.Message);
			}

			return false;
		}
	}
}
