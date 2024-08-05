using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace eCommerce.Models
{
    public class ShopViewModel
    {
        public IEnumerable<Product> Products { get; set; }
        public SelectList Categories { get; set; }
        public string SelectedCategory { get; set; }
    }
}
