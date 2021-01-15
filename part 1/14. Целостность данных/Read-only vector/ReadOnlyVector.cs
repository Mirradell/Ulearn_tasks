using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace ReadOnlyVectorTask
{
    class ReadOnlyVector
    {
        public readonly double X;

        public readonly double Y;
        
        public ReadOnlyVector(double x, double y)
        {
            X = x;
            Y = y;
        }

        public ReadOnlyVector Add(ReadOnlyVector other)
        {
            return new ReadOnlyVector(X + other.X, Y + other.Y);
        }

        public ReadOnlyVector WithX(double y)
        {
            return new ReadOnlyVector(X, y);
        }
        public ReadOnlyVector WithY(double x)
        {
            return new ReadOnlyVector(x, Y);
        }
    } 
}
