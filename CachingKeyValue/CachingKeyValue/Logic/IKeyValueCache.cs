using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CachingKeyValue.Logic
{
    public interface IKeyValueCache
    {
        object GetValue(string key);
        bool Add(string key, object value, double timeInSecs);
        bool Delete(string key);
    }
}
