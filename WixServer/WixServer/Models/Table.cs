using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WixServer.Models
{
    public class Table
    {
        public int Id { get; set; }
        public int GridId { get; set; }
        public int TableNumber { get; set; }
        public int Capacity { get; set; } // max people
        public bool IsSmokingAllowed { get; set; }
        public int xCoord { get; set; }
        public int yCoord { get; set; }
        public int xLength { get; set; }
        public int yLength { get; set; }
    }
}