using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tools.Window.Components
{
    internal interface IComponent
    {

    }

    public class ComponentCollection : IEnumerable
    {
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public ComponentCollectionEnum GetEnumerator()
        {
            return new ComponentCollectionEnum(list);
        }
        internal Component[] list = { };

        public bool Contains(Component component)
        {
            foreach (Component item in list)
            {
                if (item.Equals(component))
                {
                    return true;
                }
            }
            return false;
        }

        public void Remove(Component component)
        {
            foreach (Component res in list)
            {
                if (res.Equals(component))
                {
                    int toRemove = 0;
                    foreach (Component item in list)
                    {
                        if (item.Equals(component))
                        {
                            break;
                        }
                        toRemove++;
                    }
                    list = list.Where((source, index) => index != toRemove).ToArray();
                    return;
                }
            }
            throw new ArgumentNullException(nameof(component));
        }

        public void Add(Component key)
        {
            foreach (Component res in list)
            {
                if (res.Equals(key))
                    throw new ArgumentException("Key already in list!", nameof(key));
            }
            Array.Resize(ref list, list.Length + 1);
            list[list.Length - 1] = key;
        }

        public Component this[int key]
        {
            get
            {
                return list[key];
            }
            set
            {
                list[key] = value;
            }
        }
    }
    public class ComponentCollectionEnum : IEnumerator
    {
        public Component[] list;

        // Enumerators are positioned before the first element
        // until the first MoveNext() call.
        int position = -1;

        public ComponentCollectionEnum(Component[] list)
        {
            this.list = list;
        }

        public bool MoveNext()
        {
            position++;
            return (position < list.Length);
        }

        public void Reset()
        {
            position = -1;
        }

        object IEnumerator.Current
        {
            get
            {
                return Current;
            }
        }

        public Component Current
        {
            get
            {
                try
                {
                    return list[position];
                }
                catch (IndexOutOfRangeException)
                {
                    throw new InvalidOperationException();
                }
            }
        }
    }

    public abstract class Component : IComponent 
    {
        public ComponentCollection components = new ComponentCollection();

        internal abstract IntPtr Implement(IntPtr parent);

        public abstract string Name { get; internal set; }
    }
}
