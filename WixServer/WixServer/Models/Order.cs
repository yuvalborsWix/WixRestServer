using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WixServer.Models
{
    public class Order
    {
        public int Id { get; set; } // order id
        public int GridId { get; set; }
        public int TableNumber { get; set; }
        public int CustomerId { get; set; }
        public int NumOfPeople { get; set; }
        public DateTime FromTime { get; set; }
        public DateTime ToTime { get; set; }
        [NotMapped]
        public String CustomerInfo { get; set; }
    }
}