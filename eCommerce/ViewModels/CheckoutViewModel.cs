using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace eCommerce.ViewModels
{
    public class CheckoutViewModel
    {
        [Required(ErrorMessage = "Full Name is required.")]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Address is required.")]
        public string Address { get; set; }

        [Required(ErrorMessage = "City is required.")]
        public string City { get; set; }

        [Required(ErrorMessage = "State is required.")]
        public string State { get; set; }

        [Required(ErrorMessage = "Postal Code is required.")]
        [Display(Name = "Zip Code")]
        [RegularExpression(@"^[A-Za-z]\d[A-Za-z] \d[A-Za-z]\d$", ErrorMessage = "Please enter a valid format, e.g., N2L 3V9.")]
        public string ZipCode { get; set; }


        [Required(ErrorMessage = "Payment Method is required.")]
        [Display(Name = "Payment Method")]
        public string PaymentMethod { get; set; }

        [Required(ErrorMessage = "Card Number is required.")]
        [CreditCard(ErrorMessage = "Invalid Card Number.")]
        [Display(Name = "Card Number")]
        public string CardNumber { get; set; }

        [Required(ErrorMessage = "Card Expiry Date is required.")]
        [Display(Name = "Card Expiry")]
        [RegularExpression(@"^(0[1-9]|1[0-2])/\d{2}$", ErrorMessage = "Invalid Expiry Date format. Use MM/YY.")]
        public string CardExpiry { get; set; }

        [Required(ErrorMessage = "Card CVV is required.")]
        [Display(Name = "Card CVV")]
        [RegularExpression(@"^\d{3,4}$", ErrorMessage = "Invalid CVV format.")]
        public string CardCVV { get; set; }

        public IList<CartItemViewModel> CartItems { get; set; } = new List<CartItemViewModel>();

        public decimal Subtotal => CartItems.Sum(item => item.Total);
        public decimal Tax => Subtotal * 0.13m;
        public decimal TotalWithTax => Subtotal + Tax;
    }
}
