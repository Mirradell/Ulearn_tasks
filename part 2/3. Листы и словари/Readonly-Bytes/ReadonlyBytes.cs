using NUnit.Framework.Constraints;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Security.Cryptography.X509Certificates;

namespace hashes
{
    public static class HashDict
    {
        public  static Dictionary<byte[], int>       hashes = new Dictionary<byte[], int>();
        private static Dictionary<int, List<byte[]>> keys   = new Dictionary<int, List<byte[]>>();

        public static void Add(byte[] item, int value)
        {
            hashes.Add(item, value);
            if (keys.ContainsKey(item.Length))
                keys[item.Length].Add(item);
            else
                keys.Add(item.Length, new List<byte[]>() { item });
        }

        public static void HasKey(byte[] items, out int hash)
        {
            hash = GetHash(items);
        }

        public static int GetHash(byte[] items) 
        {
            var res = items.Length;

            if (items.Length > 0)
            {
                res += items[0] * 16777619; // длинное простое число
                res += items[items.Length - 1] * 16777619;
            }
            return res;
        }
    }

	public class ReadonlyBytes : IEnumerable<byte>
    {
        private readonly byte[] Items;
        public ReadonlyBytes(params byte[] items)
        {
            if (items == null)
                throw new ArgumentNullException();
            Items = items;
        }

        public byte this[int index]
        {
            get
            {
                if (index < 0 || index >= Length)
                    throw new IndexOutOfRangeException();

                return Items[index];
            }

            set
            {
                if (index < 0 || index >= Length)
                    throw new IndexOutOfRangeException();

                Items[index] = value;
            }
        }

        public int Length { get { return Items.Length; } }

        public virtual IEnumerator<byte> GetEnumerator()
        {
            return Items.ToList().GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }


        public override bool Equals(object obj)
        {
            if (!(obj is ReadonlyBytes) || (obj is ReadonlyBytesTests.DerivedFromReadonlyBytes) || obj == null) return false;
            
            var bytes = obj as ReadonlyBytes;
            if (bytes.Length != Length)
                return false;

            for (int i = 0; i < bytes.Length; i++)
                if (bytes[i] != Items[i])
                    return false;
            return true;
        }

        public override int GetHashCode()
        {
            int res = 0;
            HashDict.HasKey(Items, out res);
            return res;
        }

        public override string ToString()
        {
            string res = "";
            for (int i = 0; i < Length; i++)
                if (i == Length - 1)
                    res += Items[i];
                else
                    res += Items[i] + ", ";
            return "[" + res + "]";
        }
    }
}