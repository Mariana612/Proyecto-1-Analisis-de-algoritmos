using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace DecisionTree
{
    public class Tree
    {
        public TreeNode Root { get; set; }

        
        //Editado 2/10/22
        //Esta es la funcion principal del aprendizaje del algoritmo
        public static TreeNode Learn(DataTable data, string edgeName, ref int a, ref int c,ref int l)
        {
            a++;
            l++;
            var root = GetRootNode(data, edgeName, ref a, ref c, ref l);

            foreach (var item in root.NodeAttribute.DifferentAttributeNames)
            {
                l += 3;//for, is leaf, if
                a++;//for each item 
                // if a leaf, leaf will be added in this method 
                a++;
                var isLeaf = CheckIfIsLeaf(root, data, item, ref a, ref c,ref l);

                // make a recursive call as long as the node is not a leaf
                c++;
                if (!isLeaf)
                {
                    a += 2;
                    l += 2;
                    var reducedTable = CreateSmallerTable(data, item, root.TableIndex, ref a, ref c,ref l);
                    root.ChildNodes.Add(Learn(reducedTable, item, ref a, ref c,ref l));
                }
            }
            l++;
            return root;
        }


        //editado 3/10/22
        //funcion que verifica si el nodo actual es una hoja
        private static bool CheckIfIsLeaf(TreeNode root, DataTable data, string attributeToCheck, ref int a, ref int c,ref int l)
        {
            a += 2;
            l += 2;
            var isLeaf = true;
            var allEndValues = new List<string>();

            // get all leaf values for the attribute in question
            a++;//i=0
            l++;//for false
            for (var i = 0; i < data.Rows.Count; i++)
            {
                c++;//i < data.Rows.Count true
                a++;//i++
                l++;//for true

                c++;
                l++;
                if (data.Rows[i][root.TableIndex].ToString().Equals(attributeToCheck))
                {
                    a++;
                    l++;
                    allEndValues.Add(data.Rows[i][data.Columns.Count - 1].ToString());
                }
            }
            c++;//i < data.Rows.Count false

            // check whether all elements of the list have the same value
            c += 2;
            l++;
            if (allEndValues.Count > 0 && allEndValues.Any(x => x != allEndValues[0]))
            {
                a++;
                l++;
                isLeaf = false;
            }

            // create leaf with value to display and edge to the leaf
            c++;
            l++;
            if (isLeaf)
            {
                a++;
                l++;
                root.ChildNodes.Add(new TreeNode(true, allEndValues[0], attributeToCheck));
            }
            l++;
            return isLeaf;
        }
        
        //editado 3/10/22
        //Esta funcion lo que hace es eliminar los datos que ya fueron agregados al árbol
        private static DataTable CreateSmallerTable(DataTable data, string edgePointingToNextNode, int rootTableIndex, ref int a, ref int c, ref int l)
        {
            a++;
            l++;
            var smallerData = new DataTable();

            // add column titles
            a++;//i=0
            l++;//for false
            for (var i = 0; i < data.Columns.Count; i++)
            {
                a++;//i++
                c++;//for cond true
                l += 2;//for true y smallerdata
                a++;
                smallerData.Columns.Add(data.Columns[i].ToString());
            }
            c++;//for cond false

            
            a++;//i=0
            l++;//for false
            // add rows which contain edgePointingToNextNode to new datatable
            for (var i = 0; i < data.Rows.Count; i++)
            {
                a++;//i++
                c++;//for cond true
                l += 2;//for true y el if 
                c++;//if comp
                if (data.Rows[i][rootTableIndex].ToString().Equals(edgePointingToNextNode))
                {
                    a++;
                    var row = new string[data.Columns.Count];
                    a++;//j=0
                    l += 2;//var y el for false
                    for (var j = 0; j < data.Columns.Count; j++)
                    {
                        a++;//j++
                        c++;//for cond true
                        a++;
                        l += 2;//for true y row[j]
                        row[j] = data.Rows[i][j].ToString();
                    }
                    c++;//for cond false
                    a++;
                    l++;
                    smallerData.Rows.Add(row);
                }
            }
            c++;//for cond false

            
            a++;
            l += 2;
            // remove column which was already used as node
            smallerData.Columns.Remove(smallerData.Columns[rootTableIndex]);

            return smallerData;
        }

        //Editado 3/10/22
        //Funcion que obtiene el nodo raiz
        private static TreeNode GetRootNode(DataTable data, string edge, ref int a, ref int c,ref int l)
        {
            a += 3;
            l += 4;//los 3 var y el for en false
            var attributes = new List<MyAttribute>();
            var highestInformationGainIndex = -1;
            var highestInformationGain = double.MinValue;

            // Get all names, amount of attributes and attributes for every column
            a += 2; //var=0,1++
            l++;//for false
            for (var i = 0; i < data.Columns.Count - 1; i++)
            {
                c++;//i<data.Columns.count -1 true
                a++;// differentAttributenames
                l += 2;
                var differentAttributenames = MyAttribute.GetDifferentAttributeNamesOfColumn(data, i, ref a, ref c,ref l);
                attributes.Add(new MyAttribute(data.Columns[i].ToString(), differentAttributenames));
            }
            c++;//i<data.Columns.count -1 false

            // Calculate Entropy (S)
            a++;
            l += 3;//var, el for false y el return 
            var tableEntropy = CalculateTableEntropy(data, ref a, ref c,ref l);
            l++;//for false
            for (var i = 0; i < attributes.Count; i++)
            {
                a++;//i++
                c++;//for conf true
                a++;
                l += 3;//for true,if y el atributes
                attributes[i].InformationGain = GetGainForAllAttributes(data, i, tableEntropy, ref a, ref c,ref l);

                c++;
                if (attributes[i].InformationGain > highestInformationGain)
                {
                    a += 2;
                    l += 2;
                    highestInformationGain = attributes[i].InformationGain;
                    highestInformationGainIndex = i;
                }
            }
            c++; //for cond false

            return new TreeNode(attributes[highestInformationGainIndex].Name, highestInformationGainIndex, attributes[highestInformationGainIndex], edge, ref a,ref l);
        }

        //Editado 3/10/22
        //Funcion que retorna la ganancia de los atributos 
        private static double GetGainForAllAttributes(DataTable data, int colIndex, double entropyOfDataset, ref int a, ref int c,ref int l)
        {
            a += 3;
            l += 6;//4 vars, el gain = y el return 
            var totalRows = data.Rows.Count;
            var amountForDifferentValue = GetAmountOfEdgesAndTotalPositivResults(data, colIndex, ref a, ref c,ref l);
            var stepsForCalculation = new List<double>();

            foreach (var item in amountForDifferentValue)
            {
                l++;               
                a++;//asignatin per item
                // helper for calculation

                a += 3;
                l += 2;//2 var
                var firstDivision = item[0, 1] / (double)item[0, 0];
                var secondDivision = (item[0, 0] - item[0, 1]) / (double)item[0, 0];

                // prevent dividedByZeroException
                c += 2;//if double comp
                a++; //only one asignement if T or F

                l++;//if
                if (firstDivision == 0 || secondDivision == 0)
                {
                    l++;
                    stepsForCalculation.Add(0.0);
                }
                else
                {
                    l += 2;
                    stepsForCalculation.Add(-firstDivision * Math.Log(firstDivision, 2) - secondDivision * Math.Log(secondDivision, 2));
                }
            }
            a += 2;
            var gain = stepsForCalculation.Select((t, i) => amountForDifferentValue[i][0, 0] / (double)totalRows * t).Sum();

            gain = entropyOfDataset - gain;

            return gain;
        }


        //Editado 3/10/22
        //Esta funcion es utilizado para calcular la entropia 
        private static double CalculateTableEntropy(DataTable data, ref int a, ref int c,ref int l)//receives ref c to send it to getAmountOfEdgesAndTotalPositionResult
        {
            a += 3;
            l += 4;
            var totalRows = data.Rows.Count;
            var amountForDifferentValue = GetAmountOfEdgesAndTotalPositivResults(data, data.Columns.Count - 1, ref a, ref c,ref l);

            var stepsForCalculation = amountForDifferentValue
                .Select(item => item[0, 0] / (double)totalRows)
                .Select(division => -division * Math.Log(division, 2))
                .ToList();

            return stepsForCalculation.Sum();
        }

        //Editado 3/10/22
        //Funcion que obtiene la cantidad de valores positivos y la cantidad de aristas
        private static List<int[,]> GetAmountOfEdgesAndTotalPositivResults(DataTable data, int indexOfColumnToCheck, ref int a, ref int c,ref int l)
        {
            a += 2;
            l += 2;
            var foundValues = new List<int[,]>();
            var knownValues = CountKnownValues(data, indexOfColumnToCheck, ref a, ref c,ref l);

            foreach (var item in knownValues)
            {
                l++;
                a += 2;
                l += 2;
                var amount = 0;
                var positiveAmount = 0;
                a++; //i=0
                l++;//for false
                for (var i = 0; i < data.Rows.Count; i++)
                {
                    l++;//for true
                    c++;//i < data.Rows.Count true
                    a++;//i++
                    l++;
                    if (data.Rows[i][indexOfColumnToCheck].ToString().Equals(item))
                    {
                        l += 2;//ammount y el if
                        a++;
                        amount++;

                        // Counts the positive cases and adds the sum later to the array for the calculation
                        c++;
                        if (data.Rows[i][data.Columns.Count - 1].ToString().Equals(data.Rows[0][data.Columns.Count - 1]))
                        {
                            a++;
                            l++;
                            positiveAmount++;
                        }
                    }
                }//i < data.Rows.Count false
                a++;
                int[,] array = { { amount, positiveAmount } };
                a++;
                foundValues.Add(array);
                l += 2;
            }
            l++;
            return foundValues;
        }

        //Editado 3/10/22
        //función que retorna la cantidad de valores conocidos
        private static IEnumerable<string> CountKnownValues(DataTable data, int indexOfColumnToCheck, ref int a, ref int c,ref int l)
        {
            a++;
            l++;
            var knownValues = new List<string>();

            // add the value of the first row to the list
            c++;
            l++;
            if (data.Rows.Count > 0)
            {
                a++;
                l++;
                knownValues.Add(data.Rows[0][indexOfColumnToCheck].ToString());
            }

            a++;//j=1
            l++;//for falso
            for (var j = 1; j < data.Rows.Count; j++)
            {
                l += 3;//var, if y el for true
                a++;//j++
                c++;//j<data.Rows.Count true
                a++;
                var newValue = knownValues.All(item => !data.Rows[j][indexOfColumnToCheck].ToString().Equals(item));
                c++;
                if (newValue)
                {
                    a++;
                    l++;
                    knownValues.Add(data.Rows[j][indexOfColumnToCheck].ToString());
                }
            }
            c++;//j<data.Rows.Count false
            l++;
            return knownValues;
        }
    }
}