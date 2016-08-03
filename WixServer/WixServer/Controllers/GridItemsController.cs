using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using WixServer.Models;

namespace WixServer.Controllers
{
    public class GridItemsController : ApiController
    {
        private WixServerContext db = new WixServerContext();

        [Route("api/GridItems/{gridId}/{xCoord}/{yCoord}")]
        [ResponseType(typeof(GridItem))]
        public IHttpActionResult GetGridItem(int gridId, int xCoord, int yCoord)
        {
            var gridItem = db.GridItems.Where(x => x.GridId == gridId && x.xCoord == xCoord && x.yCoord == yCoord).FirstOrDefault();
            if (gridItem == null)
            {
                return NotFound();
            }

            return Ok(gridItem);
        }

        [Route("api/GridItems/{gridId}/{itemTypeId}/{xCoord}/{yCoord}/{name}")]
        [ResponseType(typeof(GridItem))]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult PostGridItem(int gridId, int itemTypeId, int xCoord, int yCoord, string name)
        {
            // Retrieve the max id
            //var id = db.GridItems.Max(x => x.Id) + 1;
            //var gridItemToAdd = db.GridItems.Where(x => x.Id == 10 && x.xCoord == xCoord && x.yCoord == yCoord && x.Name == name).FirstOrDefault();

            var gridItemToDelete = db.GridItems.Where(x => x.Id == gridId && x.xCoord == xCoord && x.yCoord == yCoord).FirstOrDefault();
            if (gridItemToDelete != null)
            {
                db.GridItems.Remove(gridItemToDelete);
                db.SaveChanges();
            }
            //if (gridItemToAdd == null)
            {
                GridItem gridItem = new GridItem
                {
                    //Id = id,
                    GridId = gridId,
                    ItemTypeId = itemTypeId,
                    xCoord = xCoord,
                    yCoord = yCoord,
                    Name = name
                };

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                db.GridItems.Add(gridItem);
                db.SaveChanges();
            }
            // return CreatedAtRoute("DefaultApi", new { id = gridItem.Id }, gridItem);
            return Ok();
        }

        // PUT: api/GridItems/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutGridItem(int id, GridItem gridItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != gridItem.Id)
            {
                return BadRequest();
            }

            db.Entry(gridItem).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GridItemExists(id))
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

        // DELETE: api/GridItems/5
        [Route("api/GridItems/{gridId}")]
        [ResponseType(typeof(GridItem))]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult DeleteGridItem(int gridId)
        {
            var gridItemsToDelete = db.GridItems.Where(x => x.GridId == gridId);
            if (gridItemsToDelete.Any())
            {

                db.GridItems.RemoveRange(gridItemsToDelete);
                db.SaveChanges();
            }

            db.SaveChanges();
            return Ok();
           
            //GridItem gridItem = db.GridItems.Find(gridId);
            //if (gridItem == null)
            //{
            //    return NotFound();
            //}

            //db.GridItems.Remove(gridItem);
            //db.SaveChanges();

            //return Ok(gridItem);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool GridItemExists(int id)
        {
            return db.GridItems.Count(e => e.Id == id) > 0;
        }

        // GET: api/GridItems
        public IQueryable<GridItem> GetGridItems()
        {
            return db.GridItems;
        }
    }
}