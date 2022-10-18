using System;
using System.Diagnostics;

namespace DecisionTree
{
    public static class Program
    {
        private static void Main(string[] args)
        {
            
            Console.WindowWidth = Console.LargestWindowWidth - 50;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Welcome to the decison tree calculator :)");
            Console.WriteLine("---------------------------------------");
            Console.ResetColor();
            string[] csvs = new string[7] { "10", "50", "7k", "10k", "25k", "50k", "100k" };
            Stopwatch sw;//para la toma del tiempo del algoritmo
            int a, c, l;
            for (int i = 0; i < 7; i++)
            {
                String file = csvs[i];
                a = 0;
                c = 0;
                l = 0;
                Console.WriteLine("\n---------------------------------------");
                Console.WriteLine("Cargango Dataset de: " + file + " datos");
                sw = new Stopwatch();
                sw.Start();
                l += 2;
                var data = CsvFileHandler.ImportFromCsvFile(file + "data.csv", ref a, ref c, ref l) ;  //se cargan los datos desde el CSV   
                Tree.Learn(data, "", ref a, ref c,ref l);  //Se entrena el árbol con los datos obtenidos en el csv              
                Console.WriteLine("Asignations: " + a);
                Console.WriteLine("Comparations: " + c);
                Console.WriteLine("Lineas ejecutadas: " + l);
                Console.WriteLine("Tomó: {0}ms", sw.Elapsed.TotalMilliseconds);
            }
            Console.ReadLine();

            //función que permite ver los árboles
            // Tree.Print(decisionTree.Root, decisionTree.Root.Name.ToUpper());
        }
    }
}