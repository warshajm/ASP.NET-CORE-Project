using System.Collections.Generic;

namespace eCommerce.Models
{
    public class CartViewModel
    {
        public IList<CartItemViewModel> CartItems { get; set; } = new List<CartItemViewModel>();
        public decimal TotalPrice => CartItems.Sum(item => item.Total);
    }
}
