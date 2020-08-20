using System;
using System.Collections.Generic;
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


        public class Orientation
        {
            public int rotation { get; set; }
            public List<List<int>> cells { get; set; }
        }

        public class Shape
        {
            public int shape_id { get; set; }
            public int bounding_box { get; set; }
            public int capacity { get; set; }
            public List<Orientation> orientations { get; set; }
        }

        public class Root
        {
            public List<Shape> shapes { get; set; }
        }

    }
}
