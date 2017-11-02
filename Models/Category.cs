using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GradedUnitVersion5KeiranDeane.Models
{
    public class Category
    { 
        [Key]
        [DisplayName("Catagory ID")]
        public int ID { get; set; }

        [DisplayName("Catagory")]
        public string Name { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}