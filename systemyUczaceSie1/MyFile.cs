﻿using System;
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

        double GetEntropy(int colIndex)
        {
            var desiredCol = Columns[colIndex];
            var numOfOccursInCol = Helpers.CountOccurencesInColumn(desiredCol);
            double entropyTotal = 0;

            foreach (var item in numOfOccursInCol)
            {
                double p = (double)item.Value / numOfOccursInCol.Values.Sum();
                var entropy = (p * Math.Log(p, 2));
                entropyTotal += entropy;
            }
            entropyTotal *= -1;
            return entropyTotal;
        }
    }
}
