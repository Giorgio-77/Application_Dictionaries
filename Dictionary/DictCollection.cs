using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dictionaries
{
    [Serializable]
    public class DictCollection
    {
        public string Path { get; set; } = @"Datas\dict_list.dat";
        public List<Dictionary> DictList { get; set; }

        public DictCollection()
        {
            DictList = new List<Dictionary>();
        }

        public void Add(Dictionary dictionary)
        {
            DictList.Add(dictionary);
        }

        public int Size {
            get 
            {
                return DictList.Count();
            }
        }

        public Dictionary this[int pos]
        {
            get => DictList[pos];
        }
    }
}
