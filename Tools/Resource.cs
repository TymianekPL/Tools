using System.Diagnostics;

namespace Tools
{
    [DebuggerDisplay("Count = {Count}")]
    [Serializable]
    [System.Runtime.InteropServices.ComVisible(false)]
    internal class Resource<Tkey, TValue>
    {
        public struct Res<Key, Value>
        {
            public Key key;
            public Value value;
        }

        internal Res<Tkey, TValue>[] list = { };

        public TValue this[Tkey key]
        {
            get
            {
                foreach (Res<Tkey, TValue> res in list)
                {
                    if (res.key.Equals(key))
                    {
                        return res.value;
                    }
                }
                return default;
            }
            set => Add(key, value);
        }

        public bool Contains(Tkey key)
        {
            foreach (Res<Tkey, TValue> item in list)
            {
                if (item.key.Equals(key))
                {
                    return true;
                }
            }
            return false;
        }

        public void Remove(Tkey key)
        {
            foreach (Res<Tkey, TValue> res in list)
            {
                if (res.key.Equals(key))
                {
                    int toRemove = 0;
                    foreach (Res<Tkey, TValue> item in list)
                    {
                        if (item.key.Equals(key))
                        {
                            break;
                        }
                        toRemove++;
                    }
                    list = list.Where((source, index) => index != toRemove).ToArray();
                    return;
                }
            }
            throw new ArgumentNullException(nameof(key));
        }

        public void Add(Tkey key, TValue value)
        {
            foreach (Res<Tkey, TValue> res in list)
            {
                if (res.key.Equals(key))
                {
                    throw new ArgumentException("Key already in list!", nameof(key));
                }
            }
            Array.Resize(ref list, list.Length + 1);
            Res<Tkey, TValue> r = new()
            {
                key = key,
                value = value
            };
            list[list.Length] = r;
        }
    }
}
