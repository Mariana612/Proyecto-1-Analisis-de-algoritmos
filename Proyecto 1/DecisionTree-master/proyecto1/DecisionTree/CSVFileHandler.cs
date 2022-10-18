using System;
using System.Data;
using System.IO;

namespace DecisionTree
{
    public static class CsvFileHandler
    {
        //Editado 3/10/22
        //Esta funcion lee el archivo y extrae los datos para agregarlos al DataTable
        public static DataTable ImportFromCsvFile(string filePath, ref int a, ref int c, ref int l)
        {
            a += 2;
            l += 4;//var, el try y el return
            var rows = 0;
            var data = new DataTable();

            try
            {
                a++;
                l += 3;//using el var y el if
                using (var reader = new StreamReader(File.OpenRead(filePath)))
                {
                    l += 2;//try y el using
                    while (!reader.EndOfStream)
                    {
                        c++;//while comp true
                        a += 2;
                        l += 5;// while, las dos var, los dos if 
                        var line = reader.ReadLine();
                        var values = line.Substring(0, line.Length - 1).Split(',');

                        foreach (var item in values)
                        {
                            l += 3;//forach and ifs 
                            a++;//one asignation for each item

                            c += 2; //if double comp
                            if (string.IsNullOrEmpty(item) || string.IsNullOrWhiteSpace(item))
                            {
                                l++;
                                throw new Exception("Value can't be empty");
                            }
                            c++;
                            if (rows == 0)
                            {
                                a++;
                                l++;
                                data.Columns.Add(item);
                            }
                        }
                        c++;
                        if (rows > 0)
                        {
                            a++;
                            l++;
                            data.Rows.Add(values);
                        }
                        a++;
                        rows++;

                        c++;
                        if (values.Length != data.Columns.Count)
                        {
                            l++;
                            throw new Exception("Row is shorter or longer than title row");
                        }
                    }
                }
                a++;
                var differentValuesOfLastColumn = MyAttribute.GetDifferentAttributeNamesOfColumn(data, data.Columns.Count - 1, ref a, ref c,ref l);

                c++;
                if (differentValuesOfLastColumn.Count > 2)
                {
                    l++;
                    throw new Exception("The last column is the result column and can contain only 2 different values");
                }
            }
            catch (Exception ex)
            {
                l += 3;//catch, display y data
                DisplayErrorMessage(ex.Message);
                a++;
                data = null;
            }

            // if no rows are entered or data == null, return null
            return data?.Rows.Count > 0 ? data : null;
        }

        //no fué editada
        //Esta funcion solo muestra un error
        private static void DisplayErrorMessage(string errorMessage)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"\n{errorMessage}\n");
            Console.ResetColor();
        }
    }
}