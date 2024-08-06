using System.ComponentModel.DataAnnotations;

namespace eCommerce.Models
{
    public class CheckoutViewModel
    {
        [Required]
        public string ShippingAddress { get; set; }

        [Required]
        public string ShippingCity { get; set; }

        [Required]
        public string ShippingState { get; set; }

        [Required]
        public string ShippingZipCode { get; set; }

        [Required]
        public string PaymentMethod { get; set; }

        public decimal TotalAmount { get; set; }
    }
}
