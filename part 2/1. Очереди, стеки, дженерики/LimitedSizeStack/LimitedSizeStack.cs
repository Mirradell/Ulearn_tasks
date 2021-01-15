using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoApplication
{
    public class LimitedSizeStack<T>
    {
        private LinkedList<T> list;
        private int Size;

        public LimitedSizeStack(int limit)
        {
            list = new LinkedList<T>();
            Size = limit;
        }

        public void Push(T item)
        {
            list.AddFirst(item);
            if (list.Count > Size)
                list.RemoveLast();
        }

        public T Pop()
        {
            T node = list.First.Value;
            list.RemoveFirst();
            return node;
        }

        public int Count
        {
            get
            {
                return list.Count > Size ? Size : list.Count;
            }
        }
    }
}
