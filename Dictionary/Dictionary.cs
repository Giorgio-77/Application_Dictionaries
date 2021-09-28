using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dictionaries
{
    [Serializable]
    public class Dictionary
    {
        public string Name { get; set; }

        public string Path { get; set; }

        public List<Word> ListWord { get; set; }

        public Dictionary()
        {
            ListWord = new List<Word>();
        }

        public Dictionary(string name)
        {
            Name = name;
            Path = @"Datas\" + name.Replace(' ', '_').ToLower() + ".xml";
            ListWord = new List<Word>();
            if (!File.Exists(Path))
                File.Create(Path).Close();
        }

        public void AddWord(Word word)
        {
            ListWord.Add(word);
        }

        public void DellWord(int pos)
        {
            ListWord[pos].DelAllValues();
            ListWord.RemoveAt(pos);
        }

        public Word FindWord(string word)
        {
            return ListWord.Find(item => item.Key == word);
        }

        public void PrintGrouped()
        {
            Console.WriteLine($"\tDictionary {Name}\n" + "\t" + new string('-', 40));
            char tmp = '0';
            foreach (var item in ListWord)
            {
                if(item == ListWord.First() || tmp != item.Key[0])
                {
                    tmp = item.Key[0];
                    Console.WriteLine("\t  " + tmp.ToString().ToUpper());
                }

                Console.WriteLine(item.ToString());
            }
        }

        public void Print()
        {
            Console.WriteLine($"\tDictionary {Name}\n" + new string('-', 40));
            foreach (var item in ListWord)
            {
                Console.WriteLine(item.ToString());
            }
        }

        public void Sort()
        {
            ListWord.Sort();
        }



    }
}
