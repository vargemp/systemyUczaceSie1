using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace systemyUczaceSie1
{
    public static class Helpers
    {
        public static Dictionary<int, int> CountOccurencesInColumn(List<int> column)
        {
            Dictionary<int, int> occurTimes = new Dictionary<int, int>();

            foreach (var item in column)
            {
                if (occurTimes.Keys.Contains(item))
                {
                    occurTimes[item] += 1;
                }
                else
                {
                    occurTimes.Add(item, 1);
                }
            }
            
            return occurTimes;
        }
    }
}
