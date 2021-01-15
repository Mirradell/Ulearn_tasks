using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace hashes
{
	public class Elems<T>
    {
		public T[] elems = new T[0];
    }

	public class GhostsTask : 
		IFactory<Document>, IFactory<Vector>, IFactory<Segment>, IFactory<Cat>, IFactory<Robot>, 
		IMagic
	{
		static byte[] content = new byte[1] { 1 };
		Elems<Vector>   vecs = new Elems<Vector>   { elems = new Vector[1]   { new Vector(0, 0) } };
		Elems<Cat>	    cats = new Elems<Cat>	   { elems = new Cat[1]		 { new Cat("001", "002", DateTime.Today) } };
		Elems<Document> docs = new Elems<Document> { elems = new Document[1] { new Document("001", Encoding.UTF8, content) } };
		Elems<Segment>  segs = new Elems<Segment>  { elems = new Segment[1]  { new Segment(new Vector(0, 0), new Vector(1, 2)) } };
		Elems<Robot>	robs = new Elems<Robot>	   { elems = new Robot[1]	 { new Robot("001", 200) } };

		public void DoMagic()
		{
			if(vecs != null)
				vecs.elems[0] = vecs.elems[0].Add(new Vector(1, 0));

			if (cats != null)
				cats.elems[0].Rename("002");

			if (docs != null)
				//				docs.elems[0] = new Document(docs.elems[0].Title, Encoding.ASCII, Encoding.Convert(Encoding.ASCII, Encoding.UTF8, content));
				content[0] = 3;

			if (segs != null)
				segs.elems[0] = new Segment(segs.elems[0].Start.Add(new Vector(0, 1)), segs.elems[0].End);

			if (robs != null)
				Robot.BatteryCapacity = 2;
		}

        public override int GetHashCode()
        {
			return base.GetHashCode();
        }

        Document IFactory<Document>.Create()
		{
			content[0] = 1;
			return docs.elems[0];
		}
		Cat IFactory<Cat>.Create()
		{
			return cats.elems[0];
		}
		Robot IFactory<Robot>.Create()
		{
			Robot.BatteryCapacity = 200;
			return robs.elems[0];
		}

		Vector IFactory<Vector>.Create()
		{
			return vecs.elems[0];
		}

		Segment IFactory<Segment>.Create()
		{
			return segs.elems[0];
		}
	}
}