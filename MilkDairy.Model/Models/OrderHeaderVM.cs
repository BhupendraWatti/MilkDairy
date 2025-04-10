using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkDairy.Model.Models
{
    
    public class OrderHeaderVM
    {
        public OrderHeader orderHeader {  get; set; }
        public IEnumerable<OrderDetail> orderDetails { get; set; }
    }
}
