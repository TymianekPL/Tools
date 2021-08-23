using System;
using System.Collections.Generic;
using Tools;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test_of_lib
{
    class Program
    {
        static void Main(string[] args)
        {
            Resource<int, string, string> resource = new Resource<int, string, string>();
            resource.Add(0, "Hello", "World");
            MessageBox.Show(resource.Find(0));
            MessageBox.Show(resource.FindName(0));
            Console.ReadLine();
        }
    }
}
