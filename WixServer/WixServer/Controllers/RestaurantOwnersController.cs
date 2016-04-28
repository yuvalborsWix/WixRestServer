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
    public class RestaurantOwnersController : ApiController
    {
        private WixServerContext db = new WixServerContext();

        [ResponseType(typeof(RestaurantOwner))]
        public IHttpActionResult GetRestaurantOwner(string userName, string password)
        {
            RestaurantOwner restaurantOwner = db.RestaurantOwners.Where(x => x.UserName == userName && x.Password == password).FirstOrDefault();
            if (restaurantOwner == null)
            {
                return NotFound();
            }

            return Ok(restaurantOwner);
        }

        // POST: api/RestaurantOwners
        [ResponseType(typeof(RestaurantOwner))]
        public IHttpActionResult PostRestaurantOwner(string name, int restaurantId, string userName, string password)
        {
            var id = db.RestaurantOwners.Max(x => x.Id) + 1;

            var restaurantOwner = new RestaurantOwner
            {
                Id = id,
                Name = name,
                Password = password,
                RestaurantId = restaurantId,
                UserName = userName
            };

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.RestaurantOwners.Add(restaurantOwner);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = restaurantOwner.Id }, restaurantOwner);
        }

        // PUT: api/RestaurantOwners/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutRestaurantOwner(int id, RestaurantOwner restaurantOwner)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != restaurantOwner.Id)
            {
                return BadRequest();
            }

            db.Entry(restaurantOwner).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RestaurantOwnerExists(id))
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

        // DELETE: api/RestaurantOwners/5
        [ResponseType(typeof(RestaurantOwner))]
        public IHttpActionResult DeleteRestaurantOwner(int id)
        {
            RestaurantOwner restaurantOwner = db.RestaurantOwners.Find(id);
            if (restaurantOwner == null)
            {
                return NotFound();
            }

            db.RestaurantOwners.Remove(restaurantOwner);
            db.SaveChanges();

            return Ok(restaurantOwner);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool RestaurantOwnerExists(int id)
        {
            return db.RestaurantOwners.Count(e => e.Id == id) > 0;
        }

        // GET: api/RestaurantOwners
        public IQueryable<RestaurantOwner> GetRestaurantOwners()
        {
            return db.RestaurantOwners;
        }
    }
}