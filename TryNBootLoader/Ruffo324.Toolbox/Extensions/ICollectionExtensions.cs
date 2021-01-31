// TODO [C.Groothoff]: Move to own repo, and include as submodule! 

using System;
using System.Collections.Generic;

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
