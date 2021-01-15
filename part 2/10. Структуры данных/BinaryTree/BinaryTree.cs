using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryTrees
{
    public class Node<T>
    {
        public T Value { get; }
        public Node<T> Left, Right, Parent;

        public Node(T value = default(T))
        {
            Value = value;
            Left = null;
            Right = null;
            Parent = null;
        }

    }
    public class BinaryTree<T> : IEnumerable<T>
        where T : IComparable
    {
        private Node<T> node;
        //private SortedSet<T> nodes = new SortedSet<T>();
        private SortedList<T, int> nodes = new SortedList<T, int>();
       // private HashSet<T> nodes = new HashSet<T>();

        public void Add(T key)
        {
              if (!nodes.ContainsKey(key))
                  nodes.Add(key, -1);
            else
                return;

            if (node == null)
                node = new Node<T>(key);
            //return

            var curNode = node;
            while (curNode != null)
                if (curNode.Value.CompareTo(key) == 0) return;
                else if (curNode.Value.CompareTo(key) > 0 && curNode.Left == null) curNode.Left = new Node<T>(key);
                else if (curNode.Value.CompareTo(key) < 0 && curNode.Right == null) curNode.Right = new Node<T>(key);
                else if (curNode.Value.CompareTo(key) > 0) curNode = curNode.Left;
                else if (curNode.Value.CompareTo(key) < 0) curNode = curNode.Right;
        }

        public T this[int i]
        {
            get
            {
                return nodes.Keys[i];
            }
        }

        public virtual IEnumerator<T> GetEnumerator()
        {
            /*
             
            var res = new List<T>();
            if (node != null)
            {
                 res.AddRange(Depth(node.Left));
                res.Add(node.Value);
                res.AddRange(Depth(node.Right));
            }
            return res;
            //*/
            return nodes.Keys.GetEnumerator();
        }

        public bool Contains(T key)
        {
            if (node == null)
                return false;

            if (node.Value.CompareTo(key) == 0)
                return true;

            var curNode = node;
            /*
             
            while (curNode != null)
                if (curNode.Value.CompareTo(key) == 0) return;
                else if (curNode.Value.CompareTo(key) > 0 && curNode.Left == null) curNode.Left = new Node<T>(key);
                else if (curNode.Value.CompareTo(key) < 0 && curNode.Right == null) curNode.Right = new Node<T>(key);
                else if (curNode.Value.CompareTo(key) > 0) curNode = curNode.Left;
                else if (curNode.Value.CompareTo(key) < 0) curNode = curNode.Right;
             //*/
            while (curNode != null)
                if (curNode.Value.CompareTo(key) == 0)
                    return true;
                else if (curNode.Value.CompareTo(key) > 0)
                    curNode = curNode.Left;
                else if (curNode.Value.CompareTo(key) < 0)
                    curNode = curNode.Right;

            return false;
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            //TODO!!!!!!!!!!!!!!!!
            return GetEnumerator();
        }

        public BinaryTree() { }
        private BinaryTree(Node<T> n){ node = n; }

        private List<T> Depth(Node<T> node)
        {
            var res = new List<T>();
            if (node != null)
            {
                 res.AddRange(Depth(node.Left));
                res.Add(node.Value);
                res.AddRange(Depth(node.Right));
            }
            return res;
        }
    }
}
