using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Diagnostics;
using Ruffo324.Toolbox.Extensions;
using Serilog;
using TryNBootLoader.Program.Attributes;

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

		/// <summary>
		/// // TODO [C.Groothoff]: summary.
		/// </summary>
		/// <param name="wrapperFunction">Just use <see cref="MethodBase.GetCurrentMethod()"/> as value.</param>
		/// <param name="beforeArguments">Runtime relevant argument parts that should be placed before the <see cref="ProcessFunctionSpecifiedInformation.ProcessArguments"/> part.</param>
		/// <param name="afterArguments">Runtime relevant argument parts that should be placed after the <see cref="ProcessFunctionSpecifiedInformation.ProcessArguments"/> part.</param>
		/// <param name="cancellationToken">Token to cancel the process call.</param>
		/// <returns>The result of the executed process.</returns>
		protected async Task<string[]> EvaluateWrapperFunction(MethodBase wrapperFunction,
			string beforeArguments = null, string afterArguments = null, CancellationToken cancellationToken = default)
		{
			// Check if the given function is from the class self. Others are not allowed.
			var derivedType = GetType();
			if (wrapperFunction.DeclaringType != derivedType)
				throw new InvalidOperationException(
					$"The function '{nameof(EvaluateWrapperFunction)}' should only called with functions from the class itself!"
					+ $"The class '{derivedType.FullName}' has no function named '{wrapperFunction.Name}'.");

			// Check if the given function also contains the needed attribute.
			var wrapperArguments = (ProcessFunctionSpecifiedInformation) wrapperFunction
					.GetCustomAttributes(typeof(ProcessFunctionSpecifiedInformation), true)
					.FirstOrDefault()
				?? throw new InvalidOperationException(
					$"Missing the '{nameof(ProcessFunctionSpecifiedInformation)}' attribute on the given function!");

			var processParts = new List<string> { ProcessName };
			processParts.AddIfNotNull(beforeArguments);
			processParts.Add(wrapperArguments.ProcessArguments);
			processParts.AddIfNotNull(afterArguments);
			var wholeProcessCall = string.Join(' ', processParts);

			Log.Verbose("'{WholeCommand}' is the evaluated process call for the function '{WrapperFunctionName}'",
				wholeProcessCall, wrapperFunction.GetType().FullName);

			return await ProcessX.StartAsync(wholeProcessCall).ToTask(cancellationToken);
		}
	}
}

namespace TryNBootLoader.Program.Attributes
{
	/// <summary>
	/// For functions that just wrapping an specified process argument combination,
	/// you can specify the process arguments.
	/// </summary>
	[AttributeUsage(AttributeTargets.Method)]

	// TODO [C.Groothoff]: Better name!
	// ProcessArgumentsWrapperFunction ?
	public class ProcessFunctionSpecifiedInformation : Attribute
	{
		public ProcessFunctionSpecifiedInformation(string processArguments)
		{
			ProcessArguments = processArguments;
		}

		public string ProcessArguments { get; }
	}
}
