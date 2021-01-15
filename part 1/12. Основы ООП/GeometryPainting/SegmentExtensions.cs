using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using GeometryTasks;
using System.Runtime.CompilerServices;

namespace GeometryPainting
{
    public class MyClass
    {
        public static Dictionary<Segment, Color> dict;
    }
    //Напишите здесь код, который заставит работать методы segment.GetColor и segment.SetColor
    public static class SegmentExtensions
    {
        public static void SetColor(this Segment s, Color color)
        {
            if (MyClass.dict == null)
                MyClass.dict = new Dictionary<Segment, Color>();
            MyClass.dict.Add(s, color);
        }

        public static Color GetColor(this Segment s)
        {
            if (MyClass.dict.ContainsKey(s))
                return MyClass.dict[s];
            else
                return Color.Black;
        }
    }
}
