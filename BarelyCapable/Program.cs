using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace BarelyCapable
{
    class Program
    {
        static void Main(string[] args)
        {
            var root = JsonConvert.DeserializeObject<Root>(File.ReadAllText("shapes_file.json"));
            var input = ParseInput(File.ReadAllLines("map_1.input"), root.shapes);
            Console.WriteLine("Hello World!");
        }


        public class Orientation
        {
            public int rotation { get; set; }
            public List<List<int>> cells { get; set; }
            public List<Position> Positions => cells?.Select(cells => new Position { X = cells[0], Y = cells[1] }).ToList();
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

        static Input ParseInput(string[] lines, List<Shape> shapes)
        {
            var input = new Input();
            var dimensions = lines[0].Split(',');
            input.Rows = int.Parse(dimensions[0]);
            input.Cols = int.Parse(dimensions[1]);

            var uniqueShapes = lines[1];
            input.UniqueShapes = int.Parse(uniqueShapes);
            var reservedCellsCount = lines[2];
            input.ReservedSpaces = int.Parse(reservedCellsCount);

            input.AvailableShapes = new List<Shape>();
            for(var i = 3; i < 3 + input.UniqueShapes; i++)
            {
                var shapeEntry = lines[i].Split(',');
                var shapeId = int.Parse(shapeEntry[0]);
                var shapeCount = int.Parse(shapeEntry[1]);
                
                for (var j = 0; j < shapeCount; j++)
                {
                    input.AvailableShapes.Add(shapes.Find(s => s.shape_id == shapeId));
                }
            }

            input.ReservedSpacePositions = new List<Position>();
            var blockedCells = lines[lines.Length - 1].Split('|');
            foreach (var blockedCell in blockedCells)
            {
                var xy = blockedCell.Split(',');
                input.ReservedSpacePositions.Add(new Position {
                    X = int.Parse(xy[0]),
                    Y = int.Parse(xy[1])
                });
            }

            return input;
        }

    }
}
