using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WixServer.Models
{
    public class RestaurantOwner
    {
        public int Id { get; set; } // owner id
        public string Name { get; set; }
        public int RestaurantId { get; set; } // foreign key

        public string UserName { get; set; }
        public string Password { get; set; }
    }
}