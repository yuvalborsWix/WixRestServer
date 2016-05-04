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
    public class OrdersController : ApiController
    {
        // TODO: move to properties file or make const class
        const int RESERVATION_TIME_IN_MINUTES = 180;

        private WixServerContext db = new WixServerContext();

        // GET: api/Orders
        public IQueryable<Order> GetOrders()
        {
            return db.Orders;
        }

        // GET: api/Orders/5
        [Route("api/Orders/{customerId}")]
        [ResponseType(typeof(Order))]
        public IHttpActionResult GetOrder(int customerId)
        {
            // TODO: allow only one order from each customer?
            Order order = db.Orders.Where(x => x.CustomerId == customerId).FirstOrDefault();
            
            if (order == null)
            {
                return NotFound();
            }

            return Ok(order);
        }

        // PUT: api/Orders/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutOrder(int id, Order order)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != order.Id)
            {
                return BadRequest();
            }

            db.Entry(order).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(id))
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

        // for problems with the dateTime web api caster
        //http://stackoverflow.com/questions/5523870/pass-a-datetime-from-javascript-to-c-sharp-controller
        [Route("api/Orders/{gridID}/{tableNum}/{customerID}/{numOfPpl}/{reservationTime}")]
        public IHttpActionResult PostOrder(int gridID, int tableNum, int customerID, int numOfPpl, DateTime reservationTime)
        {
            // TODO: allow only one order from each customer?
            if (db.Orders.Where(x => x.CustomerId == customerID).FirstOrDefault() != null)
            {
                return BadRequest("Same person cannot reserve twice!");
            }

            Order order = new Order
            {
                GridId = gridID,
                TableNumber = tableNum,
                CustomerId = customerID,
                NumOfPeople = numOfPpl,
                FromTime = reservationTime,
                ToTime = reservationTime.AddMinutes(RESERVATION_TIME_IN_MINUTES)
            };
            
            db.Orders.Add(order);
            db.SaveChanges();

            return Ok();
        }

        [ResponseType(typeof(void))]
        [Route("api/Orders/Update/{gridID}/{tableNum}/{customerID}/{numOfPpl}/{reservationTime}")]
        // Gettig order by customer id and he may change anything else.
        public IHttpActionResult UpdateOrder(int gridID, int tableNum, int customerID, int numOfPpl, DateTime reservationTime)
        {
            // TODO: allow only one order from each customer?
            Order order = db.Orders.Where(x => x.CustomerId == customerID).FirstOrDefault();

            if (order == null)
            {
                return NotFound();
            }

            order.GridId = gridID;
            order.TableNumber = tableNum;
            order.NumOfPeople = numOfPpl;
            order.FromTime = reservationTime;
            order.ToTime = reservationTime.AddMinutes(RESERVATION_TIME_IN_MINUTES);

            db.Orders.Add(order);
            db.SaveChanges();

            return StatusCode(HttpStatusCode.NoContent);
            //return Ok();
        }

        // DELETE: api/Orders/5
        [ResponseType(typeof(Order))]
        public IHttpActionResult DeleteOrder(int id)
        {
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return NotFound();
            }

            db.Orders.Remove(order);
            db.SaveChanges();

            return Ok(order);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool OrderExists(int id)
        {
            return db.Orders.Count(e => e.Id == id) > 0;
        }
    }
}