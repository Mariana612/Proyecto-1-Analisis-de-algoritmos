using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace DecisionTree
{
    public class MyAttribute
    {
        public MyAttribute(string name, List<string> differentAttributenames)
        {
            Name = name;
            DifferentAttributeNames = differentAttributenames;
        }

        public string Name { get; }

        public List<string> DifferentAttributeNames { get; }

        public double InformationGain { get; set; }

        //Editado el 3/10/22
        //Esta funcion obtiene los nombres de los atributos de cada columna
        public static List<string> GetDifferentAttributeNamesOfColumn(DataTable data, int columnIndex, ref int a, ref int c, ref int l)
        {
            a++;
            l +=3;// var, return y el for cuando es true
            var differentAttributes = new List<string>();
            a += 2;//i=0,1++
            for (var i = 0; i < data.Rows.Count; i++)
            {
                c++;//cuando i<data.row.count sea false
                a++;
                l += 2;//var y el if
                var found = differentAttributes.Any(t => t.ToUpper().Equals(data.Rows[i][columnIndex].ToString().ToUpper()));

                c++;
                if (!found)
                {
                    a++;
                    l++;
                    differentAttributes.Add(data.Rows[i][columnIndex].ToString());
                }
            }
            c++;//cuando i<data.row.count sea false

            return differentAttributes;
        }
    }
}