using System.Collections.Generic;

namespace TextAnalysis
{
    static class SentencesParserTask
    {
        public static List<List<string>> ParseSentences(string text)
        {
            var sentencesList = new List<List<string>>();
            foreach (var sentence in text.Split('.', ';', ':', '(', ')', '?', '!'))
            {
                var world = "";
                var sente = new List<string>();
                foreach (var letter in sentence)
                {
                    if (char.IsLetter(letter) || letter == '\'')
                        world += letter;
                    else if (world != "")
                    {
                        sente.Add(world.ToLower());
                        world = "";
                    }
                }

                if (world != "")
                {
                    sente.Add(world.ToLower());
                    world = "";
                }
                
                if (sente.Count > 0)
                    sentencesList.Add(sente);
            }
            return sentencesList;
        }
    }
}