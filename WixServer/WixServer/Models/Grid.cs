using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WixServer.Models
{
    public class Grid
    {
        public int Id { get; set; }
        public int RestaurantId { get; set; } // foreign key
        public DateTime Date { get; set; }
        public int GridType { get; set; } // foreign key

        public string Name { get; set; }
        public bool IsDefault{ get; set; }
    }
}