using System;
using System.Collections.Generic;
using System.Linq;

namespace linq_slideviews
{
	public class ParsingTask
	{
		/// <param name="lines">все строки файла, которые нужно распарсить. Первая строка заголовочная.</param>
		/// <returns>Словарь: ключ — идентификатор слайда, значение — информация о слайде</returns>
		/// <remarks>Метод должен пропускать некорректные строки, игнорируя их</remarks>
		public static IDictionary<int, SlideRecord> ParseSlideRecords(IEnumerable<string> lines)
		{
			var dict = new Dictionary<int, SlideRecord>();
			var enumerator = lines.GetEnumerator();

			if (enumerator == null)
				throw new ArgumentOutOfRangeException();

			enumerator.MoveNext();
			if (enumerator.Current.StartsWith("Slide")) //"SlideId;SlideType;UnitTitle"
				lines.All(x =>
				{
					var str = x.Split(';');
					int id;
					if (str.Length == 3 && int.TryParse(str[0], out id))
					{
						SlideType type = SlideType.Exercise;
						if (str[1].ToLower() == "exercise")
							type = SlideType.Exercise;
						else if (str[1].ToLower() == "quiz")
							type = SlideType.Quiz;
						else if (str[1].ToLower() == "theory")
							type = SlideType.Theory;
						else
							return true;

						dict.Add(id, new SlideRecord(id, type, str[2]));
					}
					return true;
				});
			return dict;
		}


		/// <param name="lines">все строки файла, которые нужно распарсить. Первая строка — заголовочная.</param>
		/// <param name="slides">Словарь информации о слайдах по идентификатору слайда. 
		/// Такой словарь можно получить методом ParseSlideRecords</param>
		/// <returns>Список информации о посещениях</returns>
		/// <exception cref="FormatException">Если среди строк есть некорректные</exception>
		public static IEnumerable<VisitRecord> ParseVisitRecords(IEnumerable<string> lines, IDictionary<int, SlideRecord> slides)
		{
			return lines.Skip(1).Select(x =>
			{ //UserId;SlideId;Date;Time
				var str = x.Split(';');
				int userId, slideId;
				DateTime date;
				if (str.Length != 4 ||
					!int.TryParse(str[0], out userId) ||
					!int.TryParse(str[1], out slideId) ||
					!DateTime.TryParse(str[2] + " " + str[3], out date) ||
				   	!slides.ContainsKey(slideId))
					throw new FormatException("Wrong line [" + x + "]");

				return new VisitRecord(userId, slideId, date, slides[slideId].SlideType);
			}).ToArray();
		}
	}
}