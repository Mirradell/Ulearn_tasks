using System.Collections.Generic;
using System.Linq;

namespace TextAnalysis
{
    static class TextGeneratorTask
    {
        public static string ContinuePhrase(Dictionary<string, string> nextWords, string phraseBeginning, int wordsCount)
        {
            var ending = phraseBeginning;
            var beginning = phraseBeginning.Split(' ').ToList();

            for (int i = 0; i < wordsCount; i++)
            {
                var gramma = beginning[beginning.Count - 1];
                if (beginning.Count > 1)
                    gramma = beginning[beginning.Count - 2] + " " + gramma;
                    
                if (nextWords.ContainsKey(gramma))
                {
                    ending += " " + nextWords[gramma];
                    beginning.Add(nextWords[gramma]);
                }
                else
                {
                    gramma = beginning[beginning.Count - 1];
                    if (nextWords.ContainsKey(gramma))
                    {
                        ending += " " + nextWords[gramma];
                        beginning.Add(nextWords[gramma]);
                    }
                    else
                        return ending;
                }
            }

            return ending;
        }
    }
}
/*Описание алгоритма:
На вход алгоритму передается словарь nextWords, полученный в предыдущей задаче, одно или несколько 
первых слов фразы phraseBeginning и wordsCount — количество слов, которые нужно дописать к phraseBeginning.
Словарь nextWords в качестве ключей содержит либо отдельные слова, либо пары слов, соединённые через пробел. 
По ключу key содержится слово, которым нужно продолжать фразы, заканчивающиеся на key.
Алгоритм должен работать следующим образом:
Итоговая фраза должна начинаться с phraseBeginning.
К ней дописывается wordsCount слов таким образом:
    a. Если фраза содержит как минимум два слова и в словаре есть ключ, состоящий из двух последних слов фразы, то продолжать нужно словом, из словаря по этому ключу.
    b. Иначе, если в словаре есть ключ, состоящий из одного последнего слова фразы, то продолжать нужно словом, хранящемся в словаре по этому ключу.
    c. Иначе, нужно досрочно закончить генерирование фразы и вернуть сгенерированный на данный момент результат.
Проверяющая система сначала запустит эталонный способ разделения исходного текста на предложения и слова, потом эталонный способ построения 
словаря наиболее частотных продолжений из предыдущей задачи, а затем вызовет реализованный вами метод.
В случае ошибки вы увидите исходный текст, на котором запускался процесс тестирования.
Если запустить проект на выполнение, он предложит ввести начало фразы и сгенерирует продолжение. 
Позапускайте алгоритм на разных текстах и разных фразах. Результат может быть интересным!
*/