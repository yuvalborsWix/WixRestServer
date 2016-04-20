using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WixServer.Models
{
    public class GridItem
    {
        public int Id { get; set; }
        public int GridId { get; set; }
        public int ItemTypeId { get; set; }
        public int xCoord { get; set; }
        public int yCoord { get; set; }
    }
}