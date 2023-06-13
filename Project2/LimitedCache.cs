using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace Project2
{
    internal class LimitedCache<Tkey, TValue>
    {
        private Dictionary<Tkey, TValue> dictionary;
        private int maxCapacity;
        private LinkedList<Tkey> list;
        public LimitedCache()
        {
            this.dictionary = new Dictionary<Tkey, TValue>();
            this.maxCapacity = 31;
            this.list = new LinkedList<Tkey>();
        }

        public void Add(Tkey key, TValue value)
        {
            if (this.dictionary.ContainsKey(key))
            {
                this.list.Remove(key);
                this.list.AddFirst(key);
            }
            else
            {
                if (dictionary.Count == maxCapacity)
                {
                    dictionary.Remove(this.list.Last!.Value);
                    this.list.RemoveLast();
                }

                this.dictionary.Add(key, value);
                this.list.AddFirst(key);
            }
        }

        public bool TryGetValue(Tkey key, out TValue value)
        {
            bool result = dictionary.TryGetValue(key, out value);
            if (result)
            {
                this.list.Remove(key);
                this.list.AddFirst(key);
            }
            return result;
        }
    }
}

