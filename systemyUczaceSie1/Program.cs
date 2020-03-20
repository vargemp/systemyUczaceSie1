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
            //var infoFunction = Helpers.InfoFunction(file.Columns, 1);
            //var infoFunction2 = file.GetColInfoFunction(1);

            //var gainOf1 = Helpers.GainFunction(file.Columns, 0);

            var lits = new List<int>() { 5,5 };
            Console.WriteLine("Info(T): " + file.GetColEntropy(3));
            Console.WriteLine("------------");
            for (int i = 0; i < 3; i++)
            {
                Console.WriteLine($"Info(a{i}, T): {file.GetColInfoFunction(i)}");
            }
            Console.WriteLine("------------");
            for (int i = 0; i < 3; i++)
            {
                Console.WriteLine($"Gain(a{i}, T): {file.GetColGain(i)}");
            }
            Console.WriteLine("------------");
            for (int i = 0; i < 3; i++)
            {
                Console.WriteLine($"SplitInfo(a{i}, T): {file.GetColSplitInfo(i)}");
            }
            Console.WriteLine("------------");
            for (int i = 0; i < 3; i++)
            {
                Console.WriteLine($"GainRatio(a{i}, T): {file.GetColGainRation(i)}");
            }


            Console.WriteLine("Hello World!");
            Console.ReadKey();
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
        
    }
}
