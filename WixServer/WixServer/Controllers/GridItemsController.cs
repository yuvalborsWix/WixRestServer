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
using WixServer.Models;

namespace WixServer.Controllers
{
    public class GridItemsController : ApiController
    {
        private WixServerContext db = new WixServerContext();

        // GET: api/GridItems/5
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

        // POST: api/GridItems
        [ResponseType(typeof(GridItem))]
        public IHttpActionResult PostGridItem(int gridId, int itemTypeId, int xCoord, int yCoord)
        {
            // Retrieve the max id
            var id = db.GridItems.Max(x => x.Id) + 1;

            GridItem gridItem = new GridItem
            {
                Id = id,
                GridId = gridId,
                ItemTypeId = itemTypeId,
                xCoord = xCoord,
                yCoord = yCoord
            };

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.GridItems.Add(gridItem);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = gridItem.Id }, gridItem);
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
        [ResponseType(typeof(GridItem))]
        public IHttpActionResult DeleteGridItem(int id)
        {
            GridItem gridItem = db.GridItems.Find(id);
            if (gridItem == null)
            {
                return NotFound();
            }

            db.GridItems.Remove(gridItem);
            db.SaveChanges();

            return Ok(gridItem);
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