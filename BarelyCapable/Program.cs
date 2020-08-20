using System;
using System.IO;

namespace BarelyCapable
{
    class Program
    {
        static void Main(string[] args)
        {
            var test = File.ReadAllLines("shapes_file.json");

            Console.WriteLine("Hello World!");
        }
    }
}
