using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dictionaries
{
    [Serializable]
    public class LogString
    {
        public DateTime DateStamp { get; set; }
        public string Key { get; set; }

        public string Dictionary { get; set; }

        public LogString()
        {

        }

        public LogString(DateTime dateTime, string key, string dictionary)
            => (DateStamp, Key, Dictionary) = (dateTime, key, dictionary);

        public override string ToString()
        {
            return $"\t{DateStamp} [Dictionary: {Dictionary}] => {Key}";
        }
    }
}
