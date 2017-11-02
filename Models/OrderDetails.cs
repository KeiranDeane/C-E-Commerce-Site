using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GradedUnitVersion5KeiranDeane.Models
{
    public class OrderDetail
    {
        public int OrderDetailId { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public virtual Product product { get; set; }
        public virtual CustomerOrder Order { get; set; }
    }
}