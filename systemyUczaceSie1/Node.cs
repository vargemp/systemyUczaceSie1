using System.Collections.Generic;
using System;
using System.Linq;

namespace systemyUczaceSie1
{
    public class Node
    {
        public int Key { get; set; }
        public List<int> Values = new List<int>();

        public List<List<int>> SelectedRows = new List<List<int>>();
        public List<List<int>> Columns = new List<List<int>>();

        private int HighestGainRatioColumnNum;
        public List<int> HighestGainRatioColumn;
        public double HighestGainRatioValue;


        public Node(int key, List<int> row)
        {
            Key = key;
            Values.Add(row.Last());
            SelectedRows.Add(row);

            row.ForEach(colItem =>
            {
                Columns.Add(new List<int>() { colItem });
            });
        }

        public void AddRow(List<int> row)
        {
            Values.Add(row.Last());
            SelectedRows.Add(row);

            for (int i = 0; i < row.Count; i++)
            {
                Columns[i].Add(row[i]);
            }
        }

        public void CalcHighestGainRatio()
        {
            var bestGainRatio = 0.0;
            int bestGainRatioColIndex = 0;
            for (int i = 0; i < Columns.Count - 1; i++)
            {
                var columnGainRatio = Helpers.GainRatio(Columns, i);

                if (columnGainRatio > bestGainRatio)
                {
                    bestGainRatio = columnGainRatio;
                    bestGainRatioColIndex = i;
                }
            }
            HighestGainRatioColumnNum = bestGainRatioColIndex;
            HighestGainRatioColumn = Columns[HighestGainRatioColumnNum];
            HighestGainRatioValue = bestGainRatio;
        }

        public void DoTheMagic()
        {
            CalcHighestGainRatio();
            for (int i = 0; i < Columns.Count-1; i++)
            {
                var gainRatio = Helpers.GainRatio(Columns, i);
                var gain = Helpers.GainFunction(Columns, i);
                var entropy = Helpers.GetEntropy(Columns[i]);

                Console.WriteLine($"Colnum: {i}, Entropy:{entropy}, Gain: {gain}, GainRatio: {gainRatio}");
            }
            Console.WriteLine($"Highest gain ratio: {HighestGainRatioValue} on column: {HighestGainRatioColumnNum}");
        }
    }
}
