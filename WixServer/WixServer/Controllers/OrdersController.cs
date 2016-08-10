using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Text;
using System.Web.Http;
using System.Web.Http.Cors;
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
       
        // GET: api/Orders/5
        [Route("api/GetAdminOrders/{orderDate}")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [ResponseType(typeof(List<OrderDto>))]
        public IHttpActionResult GetAdminOrders(string orderDate)
        {
           // Grid grid = db.Grids.Where(x => x.Date == gridId).FirstOrDefault();

            //if (grid == null)
            {
            //    return NotFound();
            }

            //var gridDate = grid.Date;

            // TODO: allow only one order from each customer?
            DateTime dtOrderDate = Convert.ToDateTime(orderDate);
            List<Order> orders = db.Orders.Where(x => DbFunctions.TruncateTime(x.FromTime).Value == dtOrderDate).ToList();

            List<OrderDto> ordersDtoList = new List<OrderDto>();

            if (orders == null)
            {
                return NotFound();
            }

            orders.ForEach(x =>
            {
                var orderDto = new OrderDto { Date = dtOrderDate, FromTime = x.FromTime, ToTime = x.ToTime, NumOfPeople = x.NumOfPeople, TableNumber = x.TableNumber };

                Customer customer = db.Customers.FirstOrDefault(y => y.Id == x.CustomerId);

                if (customer == null) return;

                orderDto.CustomerName = customer.Name;
                orderDto.CustomerPhone = customer.PhoneNumber;

                ordersDtoList.Add(orderDto);
            });

            return Ok(ordersDtoList);
        }

        [Route("api/Orders/{gridId}")]
        [ResponseType(typeof(List<Order>))]
        public IHttpActionResult GetOrdersByGridId(int gridId)
        {
            var orders = db.Orders.Where(x => x.GridId == gridId).ToList();
            
            if (orders == null)
            {
                return NotFound();
            }

            return Ok(orders);
        }

        // PUT: api/Orders/5
        [EnableCors(origins: "*", headers: "*", methods: "*")]
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
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [Route("api/Orders/CreateNewOrder")]
        //[Route("api/Orders/{gridID}/{tableNum}/{customerName}/{phoneNum}/{numOfPpl}/{reservationTime}")]
        [HttpPost]
        public IHttpActionResult PostOrder(Order data)
        {

            //(int gridID, int tableNum, String customerName, String phoneNum, int numOfPpl, DateTime reservationTime)
            //[Route("api/Orders/{gridID}/{tableNum}/{customerName}/{phoneNum}/{numOfPpl}/{reservationTime}")]
            // get customer by phone and name

            string customerName = data.CustomerInfo.ToString().Split('-')[0];
            string phoneNum = data.CustomerInfo.ToString().Split('-')[1];
            var customer = db.Customers.Where(x => x.Name == customerName && x.PhoneNumber == phoneNum).FirstOrDefault();

            if (customer == null)
            {
                customer = new Customer
                {
                    Name = customerName,
                    PhoneNumber = phoneNum
                };

                db.Customers.Add(customer);
                db.SaveChanges();
            }

            Order order = new Order
            {
                GridId = data.GridId,
                TableNumber = data.TableNumber,
                CustomerId = customer.Id,
                NumOfPeople = data.NumOfPeople,
                FromTime = Convert.ToDateTime(data.FromTime),
                ToTime = Convert.ToDateTime(data.ToTime)
            };

            db.Orders.Add(order);
            //db.Orders.Add(data);
            db.SaveChanges();

            return Ok();
        }


        /// <summary>
        /// Sends a mail with gmail account
        /// </summary>
        /// <param name="toAddress">The reciever</param>
        /// <param name="subject">subject</param>
        /// <param name="body">mail body</param>
        /// <returns></returns>
        private bool SendMail(string toAddress, string subject, string body)
        {
            try
            {
                SmtpClient client = new SmtpClient();
                client.Port = 587;
                client.Host = "smtp.gmail.com";
                client.EnableSsl = true;
                client.Timeout = 10000;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;

                // TODO: Configure the right user name and password
                var companyUserName = "xxx@gmail.com";
                var companyPassword = "Aa123456";

                var companyMailAddress = "xxx@gmail.com";

                client.Credentials = new NetworkCredential(companyUserName, companyPassword);

                MailMessage mailMessage = new MailMessage(companyMailAddress, toAddress, subject, body);
                mailMessage.BodyEncoding = Encoding.UTF8;
                mailMessage.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;

                client.Send(mailMessage);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
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

    public class OrderDto
    {
        public DateTime Date { get; set; }
        public int TableNumber { get; set; }
        public DateTime FromTime { get; set; }
        public DateTime ToTime { get; set; }
        public int NumOfPeople { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
    }
}