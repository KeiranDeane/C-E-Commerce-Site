using GradedUnitVersion5KeiranDeane.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GradedUnitVersion5KeiranDeane.ViewModels
{
    public class ShoppingCartViewModel
    {
        public List<Cart> CartItems { get; set; }
        public decimal CartTotal { get; set; }
    }
}