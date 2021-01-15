using System.Collections.Generic;
using NUnit.Framework;

namespace TableParser
{
    [TestFixture]
    public class FieldParserTaskTests
    {
        [Test]
        [TestCase("", new string[0])]
        [TestCase("test", new[] { "test" })]
        [TestCase("test test", new[] { "test", "test" })]
        [TestCase("test     test", new[] { "test", "test" })]
        [TestCase(" test ", new[] { "test" })]
        [TestCase("\'test test\'", new[] { "test test" })]
        [TestCase("\'test test", new[] { "test test" })]
        [TestCase("a\'test\'", new[] { "a", "test" })]
        [TestCase("a \'test\'", new[] { "a", "test" })]
        [TestCase("\'test\'a", new[] { "test", "a" })]
        [TestCase("\'\' test", new[] { "", "test" })]
        [TestCase("\'tes\"t\'", new[] { "tes\"t" })]
        [TestCase("\"tes\'t\"", new[] { "tes\'t" })]
        // [TestCase(@"""\""",         new[] { "\\" })]
        [TestCase(@"'te\'st'", new[] { "te\'st" })]
        [TestCase("\"test ", new[] { "test " })]
        [TestCase("\"\\\"\"", new[] { "\"" })]
        public static void Test(string input, string[] expectedResult)
        {
            var actualResult = FieldsParserTask.ParseLine(input);
            Assert.AreEqual(expectedResult.Length, actualResult.Count);
            for (int i = 0; i < expectedResult.Length; ++i)
            {
                Assert.AreEqual(expectedResult[i], actualResult[i].Value);
            }
        }

        public static void RunTests(string input, string[] expectedOutput)
        {
            Test(input, expectedOutput);
        }
    }

    public class FieldsParserTask
    {
        public static List<Token> ParseLine(string line)
        {
            var res = new List<Token>();
            int i = 0;
            while (i < line.Length)
            {
                Token token = new Token(line, 0, 0);
                if (line[i] == '\'' || line[i] == '\"')
                    token = ReadQuotedField(line, i);
                else if (line[i] != ' ')
                    token = ReadField(line, i);

                if (!token.Equals(new Token(line, 0, 0)))
                {
                    i += token.Length;
                    res.Add(token);
                }
                else
                    i++;

            }
            return res;
        }

        private static Token ReadField(string line, int startIndex)
        {
            var length = 0;
            for (int i = startIndex; i < line.Length; i++)
            {
                if (line[i] == ' ' || line[i] == '\'' || line[i] == '\"')
                    break;
                else
                    length++;
            }
            
            return new Token(line.Substring(startIndex, length), startIndex, length);
        }

        public static Token ReadQuotedField(string line, int startIndex)
        {
            var start = line[startIndex];
            var res = "";
            var length = 0;
            var shift = 1;
            var isEkranised = false;

            for (int i = startIndex + 1; i < line.Length; i++)
                if (line[i] == '\\')
                {
                    length++;
                    if (!isEkranised)
                        res += line[i];

                    isEkranised = !isEkranised;
                }
                else if (line[i] == start)
                    if (isEkranised)
                    {
                        length++;
                        res += line[i];
                        isEkranised = false;
                        res = res.Replace('\\'.ToString() + start, start.ToString());
                    }
                    else
                    {
                        shift++;
                        break;
                    }
                else
                {
                    length++;
                    res += line[i];
                }


            return new Token(res, startIndex, length + shift);
        }
    }
}