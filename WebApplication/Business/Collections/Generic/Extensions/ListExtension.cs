using System;
using System.Collections.Generic;

namespace WebApplication.Business.Collections.Generic.Extensions
{
	public static class ListExtension
	{
		#region Methods

		public static void Add<T>(this IList<T> list, IEnumerable<T> items)
		{
			if(list == null)
				throw new ArgumentNullException(nameof(list));

			if(items == null)
				throw new ArgumentNullException(nameof(items));

			foreach(var item in items)
			{
				list.Add(item);
			}
		}

		#endregion
	}
}