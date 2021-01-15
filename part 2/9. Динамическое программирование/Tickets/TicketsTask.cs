using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Tickets
{
    public class TicketsTask
    {
        private static Dictionary<Tuple<int, int>, BigInteger> dict = new Dictionary<Tuple<int, int>, BigInteger>();
        private static BigInteger CountIt(int halfLen, int sum)
        {
            if (dict.ContainsKey(new Tuple<int, int>(halfLen, sum)))
                return dict[new Tuple<int, int>(halfLen, sum)];

            if (sum < 0 || halfLen <= 0)
                return 0;

            if (halfLen == 1)
                return sum < 10 ? 1 : 0;

            BigInteger res = 0;
            for (int i = 0; i < 10; i++)
                res += CountIt(halfLen - 1, sum - i);

            dict.Add(new Tuple<int, int>(halfLen, sum), res);
            return res;
        }

        public static BigInteger Solve(int halfLen, int totalSum)
        {
            if (totalSum % 2 != 0)
                return 0;

            var temp = CountIt(halfLen, totalSum / 2);
            return temp * temp;
        }
    }
}
