using System;
using System.Collections.Generic;
using System.Linq;

namespace systemyUczaceSie1
{
    class Program
    {
        static void Main(string[] args)
        {
            var textFile = "test.txt";
            MyFile file = new MyFile(textFile);

            // liczenie wartosci wystapien atrybutu
            List<Dictionary<int, int>> listOfOccursInCols = countOccurencesInColumn(file.Columns);
            countInLastCol(file.Columns);
            Console.WriteLine("Hello World!");
        }

        static void countInLastCol(List<List<int>> columns)
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
        }

        static List<Dictionary<int, int>> countOccurencesInColumn(List<List<int>> columns)
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
    }
}
