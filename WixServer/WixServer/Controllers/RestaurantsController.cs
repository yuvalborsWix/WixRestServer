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
    public class RestaurantsController : ApiController
    {
        private WixServerContext db = new WixServerContext();

        // GET: api/Restaurants/5
        [ResponseType(typeof(Restaurant))]
        public IHttpActionResult GetRestaurant(string name)
        {
            Restaurant restaurant = db.Restaurants.Where(x => x.Name == name).FirstOrDefault();
            if (restaurant == null)
            {
                return NotFound();
            }

            return Ok(restaurant);
        }

        // POST: api/Restaurants
        [ResponseType(typeof(Restaurant))]
        public IHttpActionResult PostRestaurant(string name)
        {
            if (db.Restaurants.Where(x => x.Name == name).FirstOrDefault() != null)
            {
                var resp = new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    ReasonPhrase = "A restaurant with that name already exists"
                };

                throw new HttpResponseException(resp);
            }

            var id = db.Restaurants.Max(x => x.Id) + 1;

            Restaurant restaurant = new Restaurant { Id = id, Name = name };

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Restaurants.Add(restaurant);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = restaurant.Id }, restaurant);
        }

        // PUT: api/Restaurants/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutRestaurant(int id, Restaurant restaurant)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != restaurant.Id)
            {
                return BadRequest();
            }

            db.Entry(restaurant).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RestaurantExists(id))
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

        // DELETE: api/Restaurants/5
        [ResponseType(typeof(Restaurant))]
        public IHttpActionResult DeleteRestaurant(int id)
        {
            Restaurant restaurant = db.Restaurants.Find(id);
            if (restaurant == null)
            {
                return NotFound();
            }

            db.Restaurants.Remove(restaurant);
            db.SaveChanges();

            return Ok(restaurant);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        // GET: api/Restaurants
        public IQueryable<Restaurant> GetRestaurants()
        {
            return db.Restaurants;
        }

        private bool RestaurantExists(int id)
        {
            return db.Restaurants.Count(e => e.Id == id) > 0;
        }
    }
}