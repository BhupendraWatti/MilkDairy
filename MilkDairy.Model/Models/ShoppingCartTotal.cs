using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkDairy.Model.Models
{
    public class ShoppingCartTotal 
    {

        [Required]
        public IEnumerable<ShoppingCart> shoppingCartList { get; set; }
        public OrderHeader OrderHeader { get; set; }
    }
}
