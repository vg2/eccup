using System;
using System.Collections.Generic;
using System.Text;

namespace BarelyCapable
{
    public class Input
    {
        public int Rows { get; set; }
        public int Cols { get; set; }
        public int ReservedSpaces { get; set; }
        public int UniqueShapes { get; set; }
        public List<Position> ReservedSpacePositions {get; set;}
        
    }

    public class Position
    {
        public int X { get; set; }
        public int Y { get; set; }
    } 


}
