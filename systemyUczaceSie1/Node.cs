using System.Collections.Generic;
using System;
using System.Linq;

namespace systemyUczaceSie1
{
    public class Node
    {
        public int Key { get; set; }
        public List<int> Values { get; set; } = new List<int>();

        public void AddValue(int val)
        {
            Values.Add(val);
        }

        public void DoTheMagic()
        {
            var entropy = Helpers.GetEntropy(Values);

            Console.WriteLine($"Key: {Key}, Values: {String.Join(',', Values)}, entropy: {entropy}");
        }
    }
}
