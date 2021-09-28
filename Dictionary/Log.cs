using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Dictionaries
{
    [Serializable]
    public class Log
    {
        public string Path { get; set; } = @"Datas\logs.xml";

        public List<LogString> List { get; set; }

        public Log()
        {
            List = new List<LogString>();
        }

        public void Add(LogString str)
        {
            List.Add(str);
        }

        public void ClearList()
        {
            List.Clear();
        }

        public void Print()
        {
            Console.WriteLine("\tІсторія пошуку\n\t" + new string('-', 30));
            foreach (var item in List)
            {
                Console.WriteLine(item.ToString());
            }
            Console.WriteLine();
        }
    }
}
