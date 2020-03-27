using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace systemyUczaceSie1
{
    public class MyFile
    {
        public string FileName { get; set; }
        public List<List<int>> Rows { get; set; }
        public List<List<int>> Columns { get; set; }
        public MyFile(string fileName)
        {
            FileName = fileName;
            Rows = new List<List<int>>();
            Columns = new List<List<int>>();

            foreach(var row in File.ReadAllLines(FileName))
            {
                List<int> rowItems = new List<int>();
                foreach(var item in row.Split(','))
                {
                    rowItems.Add(Int32.Parse(item));
                }
                Rows.Add(rowItems);
            }

            Columns = Rows.SelectMany(inner => inner.Select((item, index) => new { item, index }))
                .GroupBy(i => i.index, i => i.item)
                .Select(g => g.ToList())
                .ToList();

        }

        public int HighestGainRatioColumnNum => GetHighestGainRatioColumnNum();
        public List<int> HighestGainRatioColumn => Columns[HighestGainRatioColumnNum];
        public double GetColEntropy(int colIndex) => Helpers.GetEntropy(Columns[colIndex]);
        public double GetColInfoFunction(int colIndex) => Helpers.InfoFunction(Columns, colIndex);
        public double GetRowEntropy(int rowIndex) => Helpers.GetEntropy(Rows[rowIndex]);
        public double GetColGain(int colIndex) => Helpers.GainFunction(Columns, colIndex);
        public double GetColSplitInfo(int colIndex) => Helpers.SplitInfo(Columns, colIndex);
        public double GetColGainRation(int colIndex) => Helpers.GainRatio(Columns, colIndex);

        public void Print()
        {
            Console.WriteLine("------------");
            for (int i = 0; i < Columns.Count-1; i++)
            {
                Console.WriteLine($"Info(a{i}, T): {GetColInfoFunction(i)}");
            }
            Console.WriteLine("------------");
            for (int i = 0; i < Columns.Count - 1; i++)
            {
                Console.WriteLine($"Gain(a{i}, T): {GetColGain(i)}");
            }
            Console.WriteLine("------------");
            for (int i = 0; i < Columns.Count - 1; i++)
            {
                Console.WriteLine($"SplitInfo(a{i}, T): {GetColSplitInfo(i)}");
            }
            Console.WriteLine("------------");
            for (int i = 0; i < Columns.Count - 1; i++)
            {
                Console.WriteLine($"GainRatio(a{i}, T): {GetColGainRation(i)}");
            }
            Console.WriteLine("------------");
        }
        private int GetHighestGainRatioColumnNum()
        {
            double bestGainRatio = 0.0;
            int bestGainRatioColIndex = 0;
            for (int i = 0; i < Columns.Count-1; i++)
            {
                var columnGainRatio = Helpers.GainRatio(Columns, i);

                if (columnGainRatio > bestGainRatio)
                {
                    bestGainRatio = columnGainRatio;
                    bestGainRatioColIndex = i;
                }
            }
            return bestGainRatioColIndex;
        }


    }
}
