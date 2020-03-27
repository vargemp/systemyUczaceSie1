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
            
            //List<Dictionary<int, int>> listOfOccursInCols = CountOccurencesInColumn(file.Columns);
            //Dictionary<int, Dictionary<int, int>> countedInLastCol = countInLastCol(file.Columns);
            var bestAttr = file.HighestGainRatioColumn;
            var lastCol = file.Columns.Last();
            List<Node> nodes = new List<Node>();

            for (int i = 0; i < bestAttr.Count; i++)
            {
                var value = bestAttr[i];

                var row = file.Rows[i];
                if (nodes.Any(n => n.Key == value))
                {
                    var node = nodes.Single(n => n.Key == value);
                    node.AddRow(row);
                }
                else
                {
                    nodes.Add(new Node(value, row, null));
                }

            }

            foreach (var node in nodes)
            {
                var nodeX = MakeTree(node);
            }

            nodes.ForEach(x => x.CalcHighestGainRatio());
            double highestGainRatioSoFar = nodes.Max(x => x.HighestGainRatioValue);

            Console.WriteLine($"Starting nodes: {nodes.Count}, best gain ratio so far: {highestGainRatioSoFar}");

            while (highestGainRatioSoFar > 0)
            {
                var nodesToIterate = nodes.Where(n => n.Values.Distinct().Count() > 1).ToList();
                foreach (var node in nodesToIterate)
                {
                    var bestAttribute = node.HighestGainRatioColumn;
                    var lastColumn = node.Columns.Last();

                    

                    List<Node> nextNodes = new List<Node>();

                    for (int i = 0; i < bestAttribute.Count; i++)
                    {
                        var value = bestAttribute[i];

                        var row = node.SelectedRows[i];
                        if (nextNodes.Any(n => n.Key == value))
                        {
                            var node2 = nextNodes.Single(n => n.Key == value);
                            node2.AddRow(row);
                        }
                        else
                        {
                            nextNodes.Add(new Node(value, row, node));
                        }
                    }

                    nextNodes.ForEach(x => x.CalcHighestGainRatio());
                    nodes = nextNodes;
                    highestGainRatioSoFar = nodes.Max(x => x.HighestGainRatioValue);
                    Console.WriteLine($"Next iter nodes: {nodes.Count}, best gain ratio so far: {highestGainRatioSoFar}");
                };
                
            }

            
                
            // zakonczenie gdy max GainRatio = 0
            Console.WriteLine("End!");
            Console.ReadKey();
        }

        static Node MakeTree(Node parent)
        {
            parent.CalcHighestGainRatio();
            var shouldStop = parent.HighestGainRatioValue > 0 ? false : true;
            if (!shouldStop)
            {
                var groupedRows = parent.SelectedRows.GroupBy(col => col[col.Count - 1]);  // rows grouped by last col
                if (groupedRows.Count() > 1)
                {
                    foreach (var group in groupedRows)
                    {
                        var key = group.Key;
                        var rows = new List<List<int>>();
                        foreach (var row in group)
                        {
                            rows.Add(row);
                        }
                        if (parent.LeftChild is null)
                        {
                            parent.LeftChild = MakeTree(new Node(key, rows, parent));
                        }
                        else
                        {
                            parent.RightChild = MakeTree(new Node(key, rows, parent));
                        }
                    }
                }

                parent.LeftChild = new Node();
            }
            

            return parent;
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
