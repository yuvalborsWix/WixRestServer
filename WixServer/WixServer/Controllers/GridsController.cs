namespace WixServer.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Entity;
    using System.Linq;
    using System.Web.Http;
    using System.Web.Http.Description;
    using Dtos;
    using Models;
    using System.Data.Entity.Infrastructure;
    using System.Net;
    using Cluster;
    using System.Data.Entity.Core.Objects;
    using System.Web.Http.Cors;
    public class GridsController : ApiController
    {
        private WixServerContext db = new WixServerContext();

        // GET: api/Grids
        public IQueryable<Grid> GetGrids()
        {
            return db.Grids;
        }

        [Route("api/Grids/{restaurantId}/{date}/{fromTime}/{toTime}")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [ResponseType(typeof(GridDto))]
        public IHttpActionResult GetGrid(int restaurantId, string date, string fromTime, string toTime)
        {
            // TODO: change the signature of this mathod to be meaningful
            //userName = "sergey@gmail.com";
            //password = "Aa123456";

            //var restaurantOwner = db.RestaurantOwners.Where(x => x.UserName == userName && x.Password == password).FirstOrDefault();

            //if (restaurantOwner == null)
            //{
            //    return NotFound();
            //}

            //var restaurantId = restaurantOwner.RestaurantId;

            var today = Convert.ToDateTime(date);

            var grid = db.Grids.Where(x => DbFunctions.TruncateTime(x.Date).Value == today && x.RestaurantId == restaurantId).FirstOrDefault();
            //var grid = db.Grids.Where(x => x.RestaurantId == restaurantId).FirstOrDefault();

            if (grid == null)
            {
                return NotFound();
            }

            var tables = db.Tables.Where(x => x.GridId == grid.Id).ToList();

            var gridItems = db.GridItems.Where(x => x.GridId == grid.Id).ToList();
            //************************************************
            DateTime startDinner = Convert.ToDateTime(date + " " + fromTime.Replace('-', ':'));
            DateTime endDinner = Convert.ToDateTime(date + " " + toTime.Replace('-', ':'));
            //check if the table is reserve between times or start/end dinner is part of other dinner
            var orders = db.Orders.Where(x => x.GridId == grid.Id && ((x.FromTime <= startDinner && x.ToTime > startDinner) ||
                                                                    (x.FromTime < endDinner && x.ToTime >= endDinner) ||
                                                                    (x.FromTime >= startDinner && x.ToTime <= endDinner))).ToList();


            GridDto gridDto = new GridDto();

            gridDto.simpleItems = gridItems;

            gridDto.XLen = grid.XLen;
            gridDto.YLen = grid.YLen;
            gridDto.Id = grid.Id;

            gridDto.Items = new List<ItemDto>();

            tables.ForEach(table =>
            {
                var tableDto = new GridTableDto
                {
                    X = table.xCoord,
                    Y = table.yCoord,
                    MaxCapacity = table.Capacity,
                    SmokingAllowed = table.IsSmokingAllowed,
                    TableNumber = table.TableNumber,
                    XLen = table.xLength,
                    YLen = table.yLength
                };

                tableDto.Taken = orders.Where(x => x.TableNumber == tableDto.TableNumber).FirstOrDefault() != null;

                gridDto.Items.Add(tableDto);
            });

            return Ok(gridDto);
        }

        // POST: api/Grids

        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [Route("api/Grids/{restaurantId}/{date}/{gridType}/{name}/{isDefault}/{xlen}/{ylen}")]
        [ResponseType(typeof(int))]
        public IHttpActionResult PostGrid(int restaurantId, string date, int gridType, string name, bool isDefault, int xlen, int ylen)
        {
            //var gridToDelete = db.Grids.Where(x => x.RestaurantId == 10).FirstOrDefault();
            //if (gridToDelete != null)
            //{
            //    db.Grids.Remove(gridToDelete);
            //    db.SaveChanges();
            //}
            DateTime dateGrid = Convert.ToDateTime(date);
            var gridToUpdate = db.Grids.Where(x => x.RestaurantId == 10 && DateTime.Compare(x.Date, dateGrid) == 0).FirstOrDefault();
            var id = 0;
            if (gridToUpdate ==null)
            {
                // Retrieve the max id
                id = db.Grids.Max(x => x.Id) + 1;

                // To transfer the parameters to a grid
                //Grid grid = new Grid
                //{
                //    Id = id,
                //    RestaurantId = restaurantId,
                //    Date = DateTime.FromBinary(date),
                //    GridType = gridType,
                //    Name = name,
                //    IsDefault = isDefault,
                //    XLen = xlen,
                //    YLen = ylen
                //};

                Grid grid = new Grid
                {
                    Id = id,
                    RestaurantId = restaurantId,
                    Date = Convert.ToDateTime(date),
                    GridType = gridType,
                    Name = name,
                    IsDefault = isDefault,
                    XLen = xlen,
                    YLen = ylen
                };

                try
                {
                    db.Grids.Add(grid);

                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    //if (!GridExists(id))
                    //{
                    //    return NotFound();
                    //}
                    //else
                    //{
                    //    throw;
                    //}
                }
            }
            else
            {
                gridToUpdate.XLen = xlen;
                gridToUpdate.YLen = ylen;
                db.SaveChanges();
                id = gridToUpdate.Id;
            }
            return Ok(id);
        }

        // DELETE: api/Grids/5
        [ResponseType(typeof(Grid))]
        public IHttpActionResult DeleteGrid(int id)
        {
            Grid grid = db.Grids.Find(id);
            if (grid == null)
            {
                return NotFound();
            }

            db.Grids.Remove(grid);
            db.SaveChanges();

            return Ok(grid);
        }

        [ResponseType(typeof(void))]
        public IHttpActionResult PutGrid(int id, Grid grid)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != grid.Id)
            {
                return BadRequest();
            }

            db.Entry(grid).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GridExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool GridExists(int id)
        {
            return db.Grids.Count(e => e.Id == id) > 0;
        }

        [Route("api/Grids/GetGridCluster/{restaurantId}/{date}")]
        [ResponseType(typeof(String))]
        public IHttpActionResult GetGridCluster(int restaurantId, string date)
        {
            var today = Convert.ToDateTime(date);

            Grid grid = db.Grids.Where(x => DbFunctions.TruncateTime(x.Date).Value == today && x.RestaurantId == restaurantId).FirstOrDefault();
            //var grid = db.Grids.Where(x => x.RestaurantId == restaurantId).FirstOrDefault();

            if (grid == null)
            {
                return NotFound();
            }

            var cluster = KMeans.GetKmeansForGrid(grid.Id);
            String result = "";

            for (int i = 0; i < cluster.Length; i++)
            {
                // get table details
                Table table = db.Tables.Where(x => x.GridId == grid.Id && x.TableNumber == i).FirstOrDefault();
                result += "Table " + (i+1) + "("+table.xCoord+ "," + table.yCoord+") " + "is in cluster " + (cluster[i] + 1) + "<br>";
            }

            return Ok(result);
        }
    }
}