using System;
using System.Collections.Generic;

namespace TodoApplication
{
    public class WhatUndo<TItem>
    {
        public string Action { get; private set; }
        public int Index     { get; private set; }
        public TItem Item    { get; private set; }

        public WhatUndo(string action, TItem item, int index)
        {
            Action = action;
            Item = item;
            Index = index;
        }
    }

    public class ListModel<TItem>
    {
        public List<TItem> Items { get; }
        public int Limit;
        private LimitedSizeStack<WhatUndo<TItem>> history;

        public ListModel(int limit)
        {
            Items = new List<TItem>();
            Limit = limit;
            history = new LimitedSizeStack<WhatUndo<TItem>>(limit);
        }

        public void AddItem(TItem item)
        {
            Items.Add(item);
            history.Push(new WhatUndo<TItem>("add", item, 0));
        }

        public void RemoveItem(int index)
        {
            history.Push(new WhatUndo<TItem>("remove", Items[index], index));
            Items.RemoveAt(index);
        }

        public bool CanUndo()
        {
            return history.Count > 0;
        }

        public void Undo()
        {
            if (!CanUndo()) return;
            var whatUndo = history.Pop();
            if (whatUndo.Action == "add")
                Items.RemoveAt(Items.Count - whatUndo.Index - 1);
            else if (whatUndo.Action == "remove")
                //Items.Add(whatUndo.Item);
                Items.Insert(whatUndo.Index, whatUndo.Item);
        }
    }
}
