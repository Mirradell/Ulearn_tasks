namespace Pluralize
{
	public static class PluralizeTask
	{
		public static string PluralizeRubles(int count)
		{
            count = count % 100;
            if (count % 10 == 0 || count >= 5 && count < 20 || count % 10 > 4)
                return "рублей";
            if (count % 10 == 1)
                return "рубль";
            return "рубля";
        }
	}
}