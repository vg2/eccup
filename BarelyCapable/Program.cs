using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace BarelyCapable
{
    class Program
    {


        static void Main(string[] args)
        {
            var root = JsonConvert.DeserializeObject<Root>(File.ReadAllText("shapes_file.json"));
            var input = ParseInput(File.ReadAllLines("map_3.input"), root.shapes);

            var grid = new int[input.Rows, input.Cols];

            foreach (var reservedSpacePosition in input.ReservedSpacePositions)
            {
                grid[reservedSpacePosition.X, reservedSpacePosition.Y] = 1;
            }

            int count = 0;
            var skippedShapes = new List<int>();
            foreach (var shape in input.AvailableShapes.OrderByDescending(s => s.capacity).ThenBy(s => s.bounding_box))
            {
                if (skippedShapes.Any(sid => sid == shape.shape_id)) { continue; }
                count++;
                var placed = PlaceShape(grid, shape);
                if (!placed)
                {
                    skippedShapes.Add(shape.shape_id);
                }
            }


            List<Shape> shapes = input.AvailableShapes.Where(x => x.Places != null).ToList();


            PrintOutputFile(shapes);
        }

        private static void PrintOutputFile(List<Shape> outputShapes)
        {
            StringBuilder stringBuilder = new StringBuilder();

            foreach (var shape in outputShapes)
            {
                stringBuilder.Append($"{shape.shape_id}");

                foreach (var coordinate in shape.Places)
                {
                    stringBuilder.Append($"|{coordinate.row},{coordinate.col}");
                }

                stringBuilder.AppendLine();
            }

            File.WriteAllText("output_file.txt", stringBuilder.ToString().TrimEnd());
        }

        private static bool IsBoxEmpty(int[,] grid, (int row, int col) cell, int size)
        {
            for (int r = 0; r < size; r++)
            {
                for (int c = 0; c < size; c++)
                {
                    if (grid[cell.row + r, cell.col + c] != 0)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public static bool PlaceShape(int[,] grid, Shape shape)
        {
            int count = 0;
            var used = new List<(int row, int col)>();

            while (shape.Places == null && count++ < 10000)
            {
                var start = FindEmptyCell(grid, used);

                used.Add(start);

                if (start.row != -1)
                {
                    foreach (var orientation in shape.orientations)
                    {
                        var places = CanPlace(orientation, grid, start);

                        if (places != null)
                        {
                            shape.Places = places;

                            foreach (var place in shape.Places)
                            {
                                grid[place.row, place.col] = 1;
                            }

                            return true;
                        }
                    }
                }
                else
                {
                    return false;
                }

            }

            return false;
        }

        public static List<(int row, int col)> CanPlace(Orientation orientation, int[,] grid, (int row, int col) cell)
        {
            bool canPlace = true;

            var result = new List<(int row, int col)>();

            foreach (var oCell in orientation.cells)
            {
                (int row, int col) current = (oCell[0] + cell.row, oCell[1] + cell.col);

                result.Add(current);

                if (current.row < grid.GetLength(0) && current.col < grid.GetLength(1))
                {
                    if (grid[current.row, current.col] == 0)
                    {
                        continue;
                    }
                }

                canPlace = false;

                break;
            }

            return canPlace ? result : null;
        }

        public static (int row, int col) FindEmptyCell(int[,] grid, List<(int row, int col)> used)
        {
            var last = used.Count > 0 ?  used.Last() : (row:0, col:0);

            for (int row = last.row; row < grid.GetLength(0); row++)
            {
                for (int col = last.col; col < grid.GetLength(1); col++)
                {
                    if (grid[row, col] == 0 && used.All(x => x.row != row && x.col != col))
                    {
                        return (row, col);
                    }
                }
            }

            return (-1, -1);
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
            public List<(int row, int col)> Places { get; set; }
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
            for (var i = 3; i < 3 + input.UniqueShapes; i++)
            {
                var shapeEntry = lines[i].Split(',');
                var shapeId = int.Parse(shapeEntry[0]);
                var shapeCount = int.Parse(shapeEntry[1]);

                for (var j = 0; j < shapeCount; j++)
                {
                    input.AvailableShapes.Add(JsonConvert.DeserializeObject<Shape>(JsonConvert.SerializeObject(shapes.Find(s => s.shape_id == shapeId))));
                }
            }

            input.ReservedSpacePositions = new List<Position>();
            var blockedCells = lines[lines.Length - 1].Split('|');
            foreach (var blockedCell in blockedCells)
            {
                var xy = blockedCell.Split(',');
                input.ReservedSpacePositions.Add(new Position
                {
                    X = int.Parse(xy[0]),
                    Y = int.Parse(xy[1])
                });
            }

            return input;
        }

    }
}
