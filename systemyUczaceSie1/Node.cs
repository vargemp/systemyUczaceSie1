using System.Collections.Generic;
using System;
using System.Linq;

namespace systemyUczaceSie1
{
    public class Node
    {
        public Guid Id;
        public int Key { get; set; }
        public List<int> Values = new List<int>();

        public List<List<int>> SelectedRows = new List<List<int>>();
        public List<List<int>> Columns = new List<List<int>>();

        private int HighestGainRatioColumnNum;
        public List<int> HighestGainRatioColumn;
        public double HighestGainRatioValue;

        public Node ParentNode;

        public Node()
        {
        }

        public Node(int key, List<int> row, Node parentNode)
        {
            Id = Guid.NewGuid();
            Key = key;
            Values.Add(row.Last());
            SelectedRows.Add(row);
            ParentNode = parentNode;

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
    }
}
