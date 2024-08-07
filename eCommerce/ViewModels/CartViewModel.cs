using System.Collections.Generic;
using System.Linq;

namespace eCommerce.ViewModels
{
    public class CartViewModel
    {
        public IList<CartItemViewModel> CartItems { get; set; } = new List<CartItemViewModel>();
        public decimal Subtotal => CartItems.Sum(item => item.Total);
        public decimal Tax => Subtotal * TaxRate;
        public decimal TotalWithTax => Subtotal + Tax;

        private const decimal TaxRate = 0.13m;
    }
}
