using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiskTree
{
    public class Tree
    {
        public string     Directory = "";
        public int        Spaces    = -1;
        public List<Tree> SubDirectory;

        public Tree(string dir = "", int space = -1)
        {
            Directory = dir;
            Spaces = space;
            SubDirectory = new List<Tree>();
        }

        public void Add(string name)
        {
            var splitted = name.Split('\\');
            
            if (splitted.Length <= 0)
                return;

            var lastNode = this;
            for (int i = 0; i < splitted.Length; i++)
            {
                var index = lastNode.SubDirectory.FindIndex(x => x.Directory == splitted[i]);
                if (index != -1)
                    lastNode = lastNode.SubDirectory[index];
                else
                {
                    lastNode.SubDirectory.Add(new Tree(splitted[i], lastNode.Spaces + 1));
                    lastNode = lastNode.SubDirectory[lastNode.SubDirectory.Count - 1];
                }
            }
        }

        private string GetSpaces(int count)
        {
            var res = "";
            for (int i = 0; i < count; i++)
                res += " ";
            return res;
        }

        public List<string> BeatifulPrint()
        {
            var res = new List<string>();
            var node = this;
            
            if (node.Spaces != -1)
            {
                res.Add(GetSpaces(node.Spaces) + node.Directory);
            }
            
            node.SubDirectory.OrderBy(e => e.Directory, StringComparer.Ordinal).ToList().ForEach(x => res.AddRange(x.BeatifulPrint()));
            return res;
        }
    }
    public class DiskTreeTask
    {
        public static List<string> Solve(List<String> treeString)
        {
            var tree = new Tree();
            foreach(var dir in treeString)
            {
                tree.Add(dir);
            }

            return tree.BeatifulPrint();
        }
    }
}
