using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dictionaries
{
    [Serializable]
    public class Word : IComparable<Word>
    {
        public string Key { get; set; }
        public List<string> ListValue { get; set; }

        public Word()
        {
            ListValue = new List<string>();
        }

        public Word(string key)
        {
            Key = key;
            ListValue = new List<string>();
        }

        public Word(string key, List<string> list)
        {
            Key = key;
            ListValue = list;
        }

        public void AddValue(string value)
        {
            ListValue.Add(value);
        }

        public void DelValue(string value)
        {
            ListValue.Remove(value);
        }

        public void DelAllValues()
        {
            ListValue.Clear();
        }

        public int GetSize()
        {
            return ListValue.Count();
        }

        public override string ToString()
        {
            string tmp = $"\t    {Key} — ";
            foreach (var item in ListValue)
            {
                tmp += item;
                if (item != ListValue.Last())
                {
                    tmp += ", ";
                }
            }
            return tmp;
        }

        public int CompareTo(Word other)
        {
            return this.Key.CompareTo(other.Key);
        }
    }
}
