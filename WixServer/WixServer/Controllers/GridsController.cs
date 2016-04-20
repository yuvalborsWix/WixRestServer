using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using WixServer.Dtos;
using WixServer.Models;

namespace WixServer.Controllers
{
    public class GridsController : ApiController
    {
        private WixServerContext db = new WixServerContext();

        // GET: api/Grids
        public IQueryable<Grid> GetGrids()
        {
            return db.Grids;
        }

        //// GET: api/Grids/5
        //[ResponseType(typeof(Grid))]
        //public IHttpActionResult GetGrid(int id)
        //{
        //    Grid grid = db.Grids.Find(id);
        //    if (grid == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(grid);
        //}

        [ResponseType(typeof(GridDto))]
        //public IHttpActionResult GetGrid(string ownerUserName, string ownerPassword)
        public IHttpActionResult GetGrid(int id)
        {

            string ownerUserName = "sergey@gmail.com";
            string ownerPassword = "Aa123456";

            var restaurantOwner = db.RestaurantOwners.Where(x => x.UserName == ownerUserName && x.Password == ownerPassword).FirstOrDefault();

            if (restaurantOwner == null)
            {
                return NotFound();
            }

            var restaurantId = restaurantOwner.RestaurantId;

            var today = DateTime.Now.Date;

            var grid = db.Grids.Where(x => DbFunctions.TruncateTime(x.Date).Value == today && x.RestaurantId == restaurantId).FirstOrDefault();

            if (grid == null)
            {
                return NotFound();
            }

            var tables = db.Tables.Where(x => x.GridId == grid.Id).ToList();

            var gridItems = db.GridItems.Where(x => x.GridId == grid.Id).ToList();

            GridDto gridDto = new GridDto();
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

                gridDto.Items.Add(tableDto);
            });

            gridItems.ForEach(gridItem => 
            {
                var gridItemDto = new GridItemDto
                {
                    X = gridItem.xCoord,
                    Y = gridItem.yCoord,
                };

                var gridItemType = db.GridItemTypes.Where(x => gridItem.ItemTypeId == x.Id).FirstOrDefault();

                if (gridItemType == null) return;

                gridItemDto.Name = gridItemType.Name;
                gridItemDto.XLen = gridItemType.xLength;
                gridItemDto.YLen = gridItemType.yLength;

                gridDto.Items.Add(gridItemDto);
            });

            return Ok(gridDto);
        }

        // PUT: api/Grids/5
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

        // POST: api/Grids
        [ResponseType(typeof(Grid))]
        public IHttpActionResult PostGrid(Grid grid)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Grids.Add(grid);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = grid.Id }, grid);
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
    }
}