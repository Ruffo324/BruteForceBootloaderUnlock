using System;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Diagnostics;
using Serilog;

namespace TryNBootLoader.Program.Services
{
	internal class ProcessService // TODO [C.Groothoff]: Create interface after done!
	{
		private static readonly TimeSpan _processCancellationTimeoutDuration = TimeSpan.FromSeconds(1);

		private static CancellationToken CreateTimeoutToken()
			=> new CancellationTokenSource(_processCancellationTimeoutDuration).Token;

		/// <summary>
		/// Checks if the given process can be started.
		/// If that's not the case, the program isn't installed or added to the paths.
		/// </summary>
		/// <param name="processName">The process name to check!</param>
		/// <returns></returns>
		public async Task<bool> IsProcessStartAble(string processName)
		{
			if (processName.Contains(" "))
			{
				throw new ArgumentException(
					$"Spaces means arguments, found some in '{processName}'! We just want the process name!");
			}

			try
			{
				var timeoutToken = CreateTimeoutToken();
				var processCall = $"{processName} --version"; // TODO [C.Groothoff]: Reusabler way of the "--version" implementation.   
				_ = await ProcessX.StartAsync(processCall).ToTask(timeoutToken);

				return true;
			}
			catch (OperationCanceledException)
			{
				Log.Warning("Starting process '{ProcessName}' was canceled by timeout!", processName);
			}
			catch (Exception ex)
			{
				Log.Warning("Starting '{ProcessName}' failed. Reason: '{ExceptionReason}!'", processName, ex.Message.Replace(Environment.NewLine, string.Empty));
			}

			return false;
		}
	}
}
