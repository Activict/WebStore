using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebStore.Models.ViewModels.Cart
{
    public class CartVM
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Total { get { return Price * Quantity; } }
        public string Image { get; set; }
    }
}
