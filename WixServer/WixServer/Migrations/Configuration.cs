namespace WixServer.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<WixServer.Models.WixServerContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(WixServer.Models.WixServerContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            context.RestaurantOwners.AddOrUpdate(x => x.Id,
                new Models.RestaurantOwner() { Id = 1, RestaurantId = 1, Name = "Sergey", UserName = "sergey@gmail.com", Password = "Aa123456" }
            );

            context.Orders.AddOrUpdate(x => x.Id,
                new Models.Order() { Id = 1, CustomerId = 1, FromTime = new DateTime(2016, 5, 28), ToTime = new DateTime(2016, 5, 28), GridId = 1, NumOfPeople = 5, TableNumber = 1 },
                new Models.Order() { Id = 2, CustomerId = 2, FromTime = new DateTime(2016, 4, 28), ToTime = new DateTime(2016, 4, 28), GridId = 1, NumOfPeople = 4, TableNumber = 3 }
            );

            context.Customers.AddOrUpdate(x => x.Id,
                new Models.Customer() { Id = 1, Name = "yuval", PhoneNumber = "098789" },
                new Models.Customer() { Id = 2, Name = "daniel", PhoneNumber = "755775" }
            );

            context.Grids.AddOrUpdate(x => x.Id,
                new Models.Grid() { Id = 1, RestaurantId = 1, GridType = 1, Date = DateTime.Now, Name = "cool_grid", IsDefault = true }
            );

            context.Tables.AddOrUpdate(x => x.Id,
                new Models.Table() { Id = 1, GridId = 1, TableNumber = 1, IsSmokingAllowed = true, Capacity = 8, xCoord = 0, yCoord = 0, xLength = 5, yLength = 5 },
                new Models.Table() { Id = 2, GridId = 1, TableNumber = 2, IsSmokingAllowed = true, Capacity = 6, xCoord = 6, yCoord = 6, xLength = 2, yLength = 2 }
            );

            context.GridItemTypes.AddOrUpdate(x => x.Id,
                new Models.GridItemType() { Id = 1, Name = "Mazgan", xLength = 4, yLength = 4 }
            );

            context.GridItems.AddOrUpdate(x => x.Id,
                new Models.GridItem() { Id = 1, GridId = 1, ItemTypeId = 1, xCoord = 8, yCoord = 6 },
                new Models.GridItem() { Id = 2, GridId = 1, ItemTypeId = 1, xCoord = 2, yCoord = 4 }
            );
        }
    }
}
