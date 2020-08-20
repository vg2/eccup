using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace BarelyCapable
{
    class Program
    {
        static void Main(string[] args)
        {
            var root = JsonConvert.DeserializeObject<Root>(File.ReadAllText("shapes_file.json"));

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

        public class Input
        {
            public int Rows { get; set; }
            public int Cols { get; set; }
            public int ReservedSpaces { get; set; }
            public int UniqueShapes { get; set; }
            public List<Shape> AvailableShapes { get; set; }
            public List<Position> ReservedSpacePositions { get; set; }

        }

        public class Position
        {
            public int X { get; set; }
            public int Y { get; set; }
        }

    }
}
