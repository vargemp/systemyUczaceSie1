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
        public static double GetEntropy(List<int> list) // Info(T)
        {
            var numOfOccursInCol = CountOccurencesInColumn(list);
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

        public static double InfoFunction(List<List<int>> colMatrix, int colIndex) // liczy funkcje informacji dla podanego atrybutu (kolumny) - Info(x,T)
        {
            var selectedCol = colMatrix[colIndex];
            var numOfOccursInCol = Helpers.CountOccurencesInColumn(selectedCol);
            var t = numOfOccursInCol.Values.Sum(); // liczba wszystkich elementów w wierszu
            double infoFunctionValue = 0;

            foreach (var Tx in numOfOccursInCol.Keys)
            {
                var lastColumnNumsForTxIndexes = new List<int>();
                for (int i = 0; i < colMatrix.Last().Count; i++)
                {
                    var lastCol = colMatrix.Last();
                    if (selectedCol[i] == Tx)
                    {
                        lastColumnNumsForTxIndexes.Add(lastCol[i]);
                    }
                }

                var entropy = GetEntropy(lastColumnNumsForTxIndexes);

                infoFunctionValue += ((double)numOfOccursInCol[Tx] / t) * entropy;
            }

            return infoFunctionValue;
        }

        public static double GainFunction(List<List<int>> colMatrix, int colNum)
        {
            var entropy = GetEntropy(colMatrix.Last()); // Info(T)
            var infoFunction = InfoFunction(colMatrix, colNum); // Info(X,T)

            var gain = entropy - infoFunction;

            return gain;
        }

        public static double SplitInfo(List<List<int>> colMatrix, int colNum) => GetEntropy(colMatrix[colNum]);

        public static double GainRatio(List<List<int>> colMatrix, int colNum)
        {
            var gain = GainFunction(colMatrix, colNum);
            var splitInfo = SplitInfo(colMatrix, colNum);

            return (double)gain / splitInfo;
        }
    }
}
