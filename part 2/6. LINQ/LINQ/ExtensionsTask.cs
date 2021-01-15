using System;
using System.Collections.Generic;
using System.Linq;

namespace linq_slideviews
{
	public static class ExtensionsTask
	{
		/// <summary>
		/// Медиана списка из нечетного количества элементов — это серединный элемент списка после сортировки.
		/// Медиана списка из четного количества элементов — это среднее арифметическое 
        /// двух серединных элементов списка после сортировки.
		/// </summary>
		/// <exception cref="InvalidOperationException">Если последовательность не содержит элементов</exception>
		public static double Median(this IEnumerable<double> items)
		{
			var my_items = items.ToList();
			var count = my_items.Count();
			if (items == null || count == 0)
				throw new InvalidOperationException();

			my_items.Sort();
			if (count % 2 != 0)
				return my_items[count / 2];
			else
				return (my_items[count / 2] + my_items[(count - 1) / 2]) / 2.0;
		}

		/// <returns>
		/// Возвращает последовательность, состоящую из пар соседних элементов.
		/// Например, по последовательности {1,2,3} метод должен вернуть две пары: (1,2) и (2,3).
		/// </returns>
		public static IEnumerable<Tuple<T, T>> Bigrams<T>(this IEnumerable<T> items)
		{
			if (items == null || items.Count() < 1)
				throw new InvalidOperationException();

			T first = default(T);
			int i = -1;
			foreach(var item in items)
            {
				if (i != -1)
					yield return new Tuple<T, T>(first, item);
				else
					i = 0;
				first = item;
            }
		}
	}
}