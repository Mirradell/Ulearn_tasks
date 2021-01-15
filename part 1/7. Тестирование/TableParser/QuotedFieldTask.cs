using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace TableParser
{
    [TestFixture]
    public class QuotedFieldTaskTests
    {
        [TestCase("''", 0, "", 2)]
        [TestCase("'a'", 0, "a", 3)]
        [TestCase("''a", 0, "", 2)]
        [TestCase("';km lkmd'", 0, ";km lkmd", 10)]
        [TestCase("';km lkmd", 0, ";km lkmd", 9)]
        public void Test(string line, int startIndex, string expectedValue, int expectedLength)
        {
            var actualToken = QuotedFieldTask.ReadQuotedField(line, startIndex);
            Assert.AreEqual(actualToken, new Token(expectedValue, startIndex, expectedLength));
        }
    }

    class QuotedFieldTask
    {
        public static Token ReadQuotedField(string line, int startIndex)
        {
            var start = line[startIndex];
            var res = "";
            var length = 1;

            for (int i = startIndex + 1; i < line.Length; i++)
                if (line[i] == start)
                {
                    length++;
                    break;
                }
                else if (line[i] == '\\' && i + 1 < line.Length && (line[i + 1] == start || line[i + 1] == '\\'))
                {
                    res += line[i + 1];
                    i++;
                    length += 2;
                }
                else
                {
                    res += line[i];
                    length++;
                }

            return new Token(res, startIndex, length);
        }
    }
}
