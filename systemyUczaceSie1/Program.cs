using System;
using System.Collections.Generic;
using System.Linq;

namespace systemyUczaceSie1
{
    class Program
    {
        static void Main(string[] args)
        {
            MyFile file = new MyFile(InputFiles.gieldaLiczby);

            // liczenie wartosci wystapien atrybutu
            //List<Dictionary<int, int>> listOfOccursInCols = CountOccurencesInColumn(file.Columns);
            //Dictionary<int, Dictionary<int, int>> countedInLastCol = countInLastCol(file.Columns);
            //var lastColEntropy2 = GetEntropy(file.Columns.Last());
            InfoFunction(file.Columns, 1);

            Console.WriteLine("Hello World!");
        }

        static Dictionary<int, Dictionary<int, int>> CountInLastCol(List<List<int>> columns)
        {
            List<int> lastColumn = columns.Last();

            var columnsWithoutLast = columns.Take(columns.Count - 1);

            Dictionary<int, Dictionary<int, int>> dict = new Dictionary<int, Dictionary<int, int>>(); // dla jakiej liczby, ile bylo wystapien danej liczby w ostatniej kolumnie
            foreach (var column in columnsWithoutLast)
            {
                var distinctNumsInCol = column.Distinct();

                Dictionary<int, List<int>> indexList = new Dictionary<int, List<int>>(); // lista indexow dla kazdego unikalnego elementu, czyli np. indeksy 1, indeksy 0 itp

                for (int i = 0; i < column.Count; i++)
                {
                    if (indexList.ContainsKey(column[i]))
                    {
                        indexList[column[i]].Add(i);
                    }
                    else
                    {
                        indexList.Add(column[i], new List<int>() { i });
                    }
                }

                foreach (var uniqueNum in indexList)
                {
                    //var itemsOnUniqueNumIndexes = new List<int>();
                    var itemsOnUniqueNumIndexes = new Dictionary<int, int>(); // jaka liczba, ile razy

                    foreach (var index in uniqueNum.Value)
                    {
                        if (itemsOnUniqueNumIndexes.ContainsKey(lastColumn.ElementAt(index)))
                        {
                            itemsOnUniqueNumIndexes[lastColumn.ElementAt(index)] += 1;
                        }
                        else
                        {
                            itemsOnUniqueNumIndexes.Add(lastColumn.ElementAt(index), 1);
                        }
                        //itemsOnUniqueNumIndexes.Add(lastColumn.ElementAt(index));
                    }

                    if (dict.ContainsKey(uniqueNum.Key))
                    {
                        var existingValues = dict[uniqueNum.Key];

                        foreach(var newItem in itemsOnUniqueNumIndexes)
                        {
                            if (existingValues.ContainsKey(newItem.Key))
                            {
                                existingValues[newItem.Key] += newItem.Value;
                            }
                            else
                            {
                                existingValues.Append(newItem);
                            }
                        }

                        dict[uniqueNum.Key] = existingValues;
                    }
                    else
                    {
                        dict.Add(uniqueNum.Key, itemsOnUniqueNumIndexes);
                    }
                }
            }

            return dict;
        }

        static Dictionary<int, Dictionary<int, int>> CountInLastColForColumn(List<List<int>> columns, int colIndex)
        {
            List<int> lastColumn = columns.Last();

            if(colIndex > columns.Count()-2 || colIndex < 0)
            {
                throw new Exception("wrong column");
            }

            var selectedCol = columns[colIndex];

            Dictionary<int, Dictionary<int, int>> dict = new Dictionary<int, Dictionary<int, int>>(); // dla jakiej liczby, ile bylo wystapien danej liczby w ostatniej kolumnie

            var distinctNumsInCol = selectedCol.Distinct();

            Dictionary<int, List<int>> indexList = new Dictionary<int, List<int>>(); // lista indexow dla kazdego unikalnego elementu, czyli np. indeksy 1, indeksy 0 itp

            for (int i = 0; i < selectedCol.Count; i++)
            {
                if (indexList.ContainsKey(selectedCol[i]))
                {
                    indexList[selectedCol[i]].Add(i);
                }
                else
                {
                    indexList.Add(selectedCol[i], new List<int>() { i });
                }
            }

            foreach (var uniqueNum in indexList)
            {
                //var itemsOnUniqueNumIndexes = new List<int>();
                var itemsOnUniqueNumIndexes = new Dictionary<int, int>(); // jaka liczba, ile razy

                foreach (var index in uniqueNum.Value)
                {
                    if (itemsOnUniqueNumIndexes.ContainsKey(lastColumn.ElementAt(index)))
                    {
                        itemsOnUniqueNumIndexes[lastColumn.ElementAt(index)] += 1;
                    }
                    else
                    {
                        itemsOnUniqueNumIndexes.Add(lastColumn.ElementAt(index), 1);
                    }
                    //itemsOnUniqueNumIndexes.Add(lastColumn.ElementAt(index));
                }

                if (dict.ContainsKey(uniqueNum.Key))
                {
                    var existingValues = dict[uniqueNum.Key];

                    foreach (var newItem in itemsOnUniqueNumIndexes)
                    {
                        if (existingValues.ContainsKey(newItem.Key))
                        {
                            existingValues[newItem.Key] += newItem.Value;
                        }
                        else
                        {
                            existingValues.Append(newItem);
                        }
                    }

                    dict[uniqueNum.Key] = existingValues;
                }
                else
                {
                    dict.Add(uniqueNum.Key, itemsOnUniqueNumIndexes);
                }
            }
            return dict;
        }

        static List<Dictionary<int, int>> CountOccurencesInColumns(List<List<int>> columns)
        {
            List<Dictionary<int, int>> listOfOccursInCols = new List<Dictionary<int, int>>();
            foreach (var column in columns)
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
                listOfOccursInCols.Add(occurTimes);
            }
            return listOfOccursInCols;
        }
        
        static double GetEntropy(List<int> list)
        {
            var numOfOccursInCol = CountOccurencesInColumns(new List<List<int>>() { list }).First();
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

        static double InfoFunction(List<List<int>> matrix, int colIndex) // liczy funkcje informacji dla podanego atrybutu (kolumny)
        {
            var selectedCol = matrix[colIndex];
            var numOfOccursInCol = CountOccurencesInColumns(new List<List<int>>() { selectedCol }).First();
            var t = numOfOccursInCol.Values.Sum(); // liczba wszystkich elementów w wierszu
            double infoFunctionValue = 0;

            foreach (var Tx in numOfOccursInCol.Keys)
            {
                var lastColumnNumsForTxIndexes = new List<int>();
                for (int i = 0; i < matrix.Last().Count; i++)
                {
                    var lastCol = matrix.Last();
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
    }
}
