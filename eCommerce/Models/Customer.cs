using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eCommerce.Models
{
    public class Customer
    {
        [Key]
        public string CustomerId { get; set; } // Primary Key, matches IdentityUser.Id

        [ForeignKey("CustomerId")]
        public IdentityUser User { get; set; }

        // Additional customer properties
        [Required]
        [StringLength(100)]
        public string FullName { get; set; }

        [StringLength(250)]
        public string Address { get; set; }

        [StringLength(20)]
        public string PhoneNumber { get; set; }

        public ICollection<Cart> Carts { get; set; }
        public ICollection<Order> Orders { get; set; }


    }
}
