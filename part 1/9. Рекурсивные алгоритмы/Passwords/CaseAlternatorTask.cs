using System;

using System.Collections.Generic;
using System.Linq;

namespace Passwords
{
    public class CaseAlternatorTask
    {
        //Тесты будут вызывать этот метод
        public static List<string> AlternateCharCases(string lowercaseWord)
        {
            var result = new List<string>();
            AlternateCharCases(lowercaseWord.ToCharArray(), 0, result);
            result = result.Distinct().ToList();
            result.Sort();
            return result;
        }

        public static string ToStr(char[] array)
        {
            string res = "";
            for (int i = 0; i < array.Length; i++)
                res += array[i];
            return res;
        }

        static void AlternateCharCases(char[] word, int startIndex, List<string> result)
        {
            if (startIndex == word.Length)
                result.Add(ToStr(word));
            else
            {
                if (char.IsLetter(word[startIndex]))
                {
                    word[startIndex] = char.ToLower(word[startIndex]);
                    AlternateCharCases(word, startIndex + 1, result);
                    word[startIndex] = char.ToUpper(word[startIndex]);
                    AlternateCharCases(word, startIndex + 1, result);
            }
                else
                    AlternateCharCases(word, startIndex + 1, result);
            }
        }
    }
}