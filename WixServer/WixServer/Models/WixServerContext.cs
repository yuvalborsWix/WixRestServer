using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace WixServer.Models
{
    public class WixServerContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx
    
        public WixServerContext() : base("name=WixServerContext")
        {
        }

        public System.Data.Entity.DbSet<WixServer.Models.Grid> Grids { get; set; }
        public System.Data.Entity.DbSet<WixServer.Models.RestaurantOwner> RestaurantOwners { get; set; }
        public System.Data.Entity.DbSet<WixServer.Models.Table> Tables { get; set; }
        public System.Data.Entity.DbSet<WixServer.Models.GridItem> GridItems { get; set; }
        public System.Data.Entity.DbSet<WixServer.Models.GridItemType> GridItemTypes { get; set; }
    }
}
