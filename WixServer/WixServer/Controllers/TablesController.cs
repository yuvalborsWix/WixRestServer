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
    public class TablesController : ApiController
    {
        private WixServerContext db = new WixServerContext();

        [Route("api/Tables/{gridId}/{xCoord}/{yCoord}")]
        [ResponseType(typeof(Table))]
        public IHttpActionResult GetTable(int gridId, int xCoord, int yCoord)
        {
            Table table = db.Tables.Where(x => x.GridId == gridId && x.xCoord == xCoord && x.yCoord == yCoord).FirstOrDefault();

            if (table == null)
            {
                return NotFound();
            }

            return Ok(table);
        }

        [Route("api/Tables/{gridId}/{tableNumber}/{capacity}/{isSmokingAllowed}/{xCoord}/{yCoord}/{xLength}/{yLength}")]
        [ResponseType(typeof(Table))]
        public IHttpActionResult PostTable(int gridId, int tableNumber, int capacity, bool isSmokingAllowed, int xCoord, int yCoord, int xLength, int yLength)
        {
            // Retrieve the max id
            var id = db.Tables.Max(x => x.Id) + 1;

            var table = new Table
            {
                Id = id,
                Capacity = capacity,
                GridId = gridId,
                IsSmokingAllowed = isSmokingAllowed,
                TableNumber = tableNumber,
                xCoord = xCoord,
                yCoord = yCoord,
                xLength = xLength,
                yLength = yLength
            };

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Tables.Add(table);
            db.SaveChanges();

            return Ok();
        }

        // PUT: api/Tables/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutTable(int id, Table table)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != table.Id)
            {
                return BadRequest();
            }

            db.Entry(table).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TableExists(id))
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

        // DELETE: api/Tables/5
        [ResponseType(typeof(Table))]
        public IHttpActionResult DeleteTable(int id)
        {
            Table table = db.Tables.Find(id);
            if (table == null)
            {
                return NotFound();
            }

            db.Tables.Remove(table);
            db.SaveChanges();

            return Ok(table);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TableExists(int id)
        {
            return db.Tables.Count(e => e.Id == id) > 0;
        }

        // GET: api/Tables
        public IQueryable<Table> GetTables()
        {
            return db.Tables;
        }
    }
}