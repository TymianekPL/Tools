using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Tools
{
    [DebuggerDisplay("Count = {Count}")]
    [Serializable]
    [System.Runtime.InteropServices.ComVisible(false)]
    public class Resource<TKey, TName, TValue>
    {
        public struct Entry
        {
            public TKey Key;
            public TName Name; 
            public TValue Value; 
        }

        public Entry[] items = new Entry[0];
        

        public void Add(TKey key, TName name, TValue value)
        {
            if(Find(key) != null)
                throw new ArgumentException("Item already in array!", nameof(key));
            Array.Resize(ref items, items.Length + 1);
            items[items.Length - 1] = new Entry
            {
                Key = key,
                Name = name,
                Value = value
            };
        }


        public void Remove(TKey item)
        {
            items[Array.IndexOf(items, item)] = items[items.Length - 1];
            Array.Resize(ref items, items.Length -1);
        }

        public TValue Find(TKey key)
        {
            foreach (var item in items)
            {
                if(item.Key.Equals(key))
                    return item.Value;
            }

            return default;
        }


        public TName FindName(TKey key)
        {
            foreach (var item in items)
            {
                if(item.Key.Equals(key))
                    return item.Name;
            }

            return default;
        }

        public Entry[] GetItems()
        {
            return items;
        }
    }
}
