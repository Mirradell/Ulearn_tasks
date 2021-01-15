using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PocketGoogle
{
    public class PocketGoogleClass
    {
        public string Doc;
        public Dictionary<string, HashSet<int>> Positions;

        public PocketGoogleClass(string doc, string key, int value) 
        {
            Doc = doc;
            Positions = new Dictionary<string, HashSet<int>>() { { key, new HashSet<int>() { value } } };
        }

        public void AddInPositions(string key, int value)
        {
            if (Positions.ContainsKey(key))
                Positions[key].Add(value);
            else
                Positions.Add(key, new HashSet<int>() { value });
        }
    }

    public class Indexer : IIndexer
    {
        private Dictionary<string, Dictionary<int, PocketGoogleClass>> dict = new Dictionary<string, Dictionary<int, PocketGoogleClass>>();

        public void Add(int id, string documentText)
        {
            //Add. Этот метод должен индексировать все слова в документе. Разделители слов: { ' ', '.', ',', '!', '?', ':', '-','\r','\n' }; Сложность – O(document.Length)
            var splittedText = documentText.Split(' ', '.', ',', '!', '?', ':', '-', '\r', '\n');
            var counter = 0;
            foreach (var s in splittedText)
            {
                if (dict.ContainsKey(s))
                {
                    if (!dict[s].ContainsKey(id))
                        dict[s].Add(id, new PocketGoogleClass(documentText, s, counter));
                    else
                        dict[s][id].AddInPositions(s, counter);
                }
                else
                    dict.Add(s, new Dictionary<int, PocketGoogleClass>() { { id, new PocketGoogleClass(documentText, s, counter) } });
                counter += s.Length + 1;
            }
        }

        public List<int> GetIds(string word)
        {
            //GetIds. Этот метод должен искать по слову все id документов, где оно встречается. Сложность — O(result), где result — размер ответа на запрос
            if (dict.ContainsKey(word))
            {
                var res = new List<int>();
                foreach (var pair in dict[word])
                    res.Add(pair.Key);
                return res;
            }
            else
                return new List<int>();
        }

        public List<int> GetPositions(int id, string word)
        {
            //GetPositions. Этот метод по слову и id документа должен искать все позиции, в которых слово начинается. Сложность — O(result)
            if (dict.ContainsKey(word))
            {
                if (dict[word].ContainsKey(id))
                    return dict[word][id].Positions[word].ToList();
            }
            return new List<int>();
        }

        public void Remove(int id)
        {
            // Remove. Этот метод должен удалять документ из индекса, после чего слова в нем искаться больше не должны. Сложность — O(document.Length)
            foreach (var key in dict.Keys)
            {
                if (dict[key].ContainsKey(id))
                    dict[key].Remove(id);
            }

        }
    }

    [TestFixture]
    public class MyTest
    {
        [Test]
        public void Test()
        {
            var MyIndexer = new Indexer();
            MyIndexer.Add(10, "g b c");
            MyIndexer.Add(1, "d c     a  f");
            MyIndexer.Add(2, "sdsba  -  b c b");

            var getIds_c = MyIndexer.GetIds("c");
            Assert.AreEqual(new int[] { 10, 1, 2 }, getIds_c, "Test GetIds symbol c");

            var getIds_a = MyIndexer.GetIds("a");
            Assert.AreEqual(new int[] { 1 }, getIds_a, "Test GetIds symbol a");

            var getIds_b = MyIndexer.GetIds("b");
            Assert.AreEqual(new int[] { 10, 2 }, getIds_b, "Test GetIds symbol b");

            var getIdsgetPosition = MyIndexer.GetPositions(10, "b");
            Assert.AreEqual(new int[] { 2 }, getIdsgetPosition, "Test GetPositions symbol b doc 10");

            getIdsgetPosition = MyIndexer.GetPositions(2, "b");
            Assert.AreEqual(new int[] { 10, 14 }, getIdsgetPosition, "Test GetPositions symbol b doc 2");

            MyIndexer.Remove(10);
            getIds_b = MyIndexer.GetIds("b");
            Assert.AreEqual(new int[] { 2 }, getIds_b, "Test GetIds symbol b after remove doc10");

            //getIds_g = MyIndexer.g
        }

        [Test]
        public void Test2()
        {
            var MyIndexer = new Indexer();
            MyIndexer.Add(2, "A C A CA A AC CA");
            var res = MyIndexer.GetPositions(2, "A");
            Assert.AreEqual(new int[] { 0, 4, 9 }, res, "WTF?!");
        }

        [Test]
        public void Test3()
        {
            var MyIndexer = new Indexer();
            MyIndexer.Add(2, "A C A C");
            MyIndexer.Add(10, "g b c");
            MyIndexer.Add(1, "d c     a  f");

            MyIndexer.Remove(10);
            MyIndexer.Add(10, "g");

            var getIdsgetPosition = MyIndexer.GetPositions(10, "g");
            Assert.AreEqual(new int[] { 0 }, getIdsgetPosition, "Test GetPositions symbol b doc 10");

        }
    }
}
