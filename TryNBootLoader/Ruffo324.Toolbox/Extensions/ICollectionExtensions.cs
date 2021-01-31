// TODO [C.Groothoff]: Move to own repo, and include as submodule! 

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Ruffo324.Toolbox.Exceptions;
using Serilog;

// TODO [C.Groothoff]: Split project "Toolbox" and "Toolbox.LazyDeveloper" or "Toolbox.CodeLess".
namespace Ruffo324.Toolbox.Extensions
{
	public static class ICollectionExtensions
	{
		/// <summary>
		/// Adds <paramref name="value"/> only to <paramref name="enumerable"/>,
		/// if the <paramref name="condition"/> results in true.
		/// </summary>
		public static void AddOnCondition<T>(this ICollection<T> enumerable, T value, Func<bool> condition)
			=> enumerable.AddOnCondition(value, condition());

		/// <summary>
		/// Adds <paramref name="value"/> only to <paramref name="enumerable"/>,
		/// if the <paramref name="condition"/> is true.
		/// </summary>
		public static void AddOnCondition<T>(this ICollection<T> enumerable, T value, bool condition)
		{
			if (condition)
			{
				enumerable.Add(value);
			}
		}

		/// <summary>
		/// Adds <paramref name="value"/> only to <paramref name="enumerable"/>,
		/// if <paramref name="value"/> is not null.
		/// </summary>
		public static void AddIfNotNull<T>(this ICollection<T> enumerable, T? value)
			=> enumerable.AddOnCondition(value!, value is not null); // TODO [C.Groothoff]: If this crashes, use the code below.

		/*{
			if (value is not null)
			{
				enumerable.Add(value);
			}
		}*/
	}
}

namespace Ruffo324.Toolbox.Console
{
	public static class UserInput
	{
		/// <summary>
		/// Waits until the user made the decision continue or abort the whole process.
		/// </summary>
		/// <param name="exitKey">Default <see cref="ConsoleKey.E"/> for "exit".</param>
		/// <param name="continueKey">Default "any key". Set it to allow only the given key for continue.</param>
		/// <param name="continueKeyAction">No logic impact, just for the message. Example alternative: "for retry.", "to stop", "to discard." etc..</param>
		/// <exception cref="UserAbortedException">Thrown if the user decides to abort the whole process.</exception>
		/// <returns>Completed <see cref="Task"/> if the user continues.</returns>
		public static Task ContinueOrAbort(ConsoleKey exitKey = ConsoleKey.E, ConsoleKey continueKey = ConsoleKey.NoName, string continueKeyAction = "to continue")
			=> Task.Run(() => {
				var anyKeyContinue = continueKey == ConsoleKey.NoName;
				var continueKeyString = anyKeyContinue ? "any key" : $"{continueKey}";

				Log.Information("Press '{ExitKey}' to abort the process, or '{ContinueKey}' {ContinueAction}..", exitKey, continueKeyString, continueKeyAction);
				ConsoleKey userKey;
				do
				{
					userKey = System.Console.ReadKey().Key;
					if (userKey == exitKey)
					{
						throw new UserAbortedException();
					}
				} while (!anyKeyContinue && userKey != continueKey);

				return Task.CompletedTask;
			});

		/// <summary>
		/// Creates a numbered table from the passed <paramref name="dataSet"/>, and lets the user select an entry.
		/// </summary>
		/// <typeparam name="T">Type of the entries, and as so also the return value.</typeparam>
		/// <param name="dataSet">The pile from which the user should choose one.</param>
		/// <param name="toCellStringsConverter">Function to convert <typeparamref name="T"/> to single text cells for the correct table alignment.</param>
		/// <param name="headerCells">Column captions. Must be equal to the length of the <paramref name="toCellStringsConverter"/> result.</param>
		/// <param name="exitKey">Default <see cref="ConsoleKey.E"/> for "exit".</param>
		/// <exception cref="UserAbortedException">Thrown if the user decides to abort the whole process.</exception>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if the length of <paramref name="headerCells"/> is not equal to the length of the <paramref name="toCellStringsConverter"/> result.</exception>
		/// <returns>The selected entry from the passed <paramref name="dataSet"/>.</returns>
		public static Task<T> SelectItemOrAbort<T>(ImmutableArray<T> dataSet, Func<T, string[]> toCellStringsConverter, string[] headerCells, ConsoleKey exitKey = ConsoleKey.E)
			=> Task.Run(() => {
				// TODO [C.Groothoff]: TODO: Table format!
				var cellDataString = dataSet.Select(toCellStringsConverter);

				// if (headerCells.Length !=)
				Log.Information("Enter the number of the wanted row, or press '{ExitKey}' to abort the whole process..", exitKey);
				Log.Information("Submit with {Enter}..", ConsoleKey.Enter);

				while (true)
				{
					// Throw on exit.
					var userInput = System.Console.ReadLine();
					if (userInput == exitKey.ToString())
					{
						throw new UserAbortedException();
					}

					// Repeat on invalid input.
					if (!int.TryParse(userInput, out var selectedEntry))
					{
						Log.Warning("'{NoNumber}' is not an valid number. Try again!", userInput);
						continue;
					}

					// Number in range? -> User selected something valid, yay!
					if (selectedEntry >= 0 && selectedEntry <= dataSet.Length)
					{
						return dataSet[selectedEntry];
					}

					// Number was not in range? Message for user to retry.
					Log.Warning("The number '{OutOfRangeNumber}' is not available for selection. Try again!", selectedEntry);
				}
			});
	}
}
