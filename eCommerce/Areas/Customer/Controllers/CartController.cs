using eCommerce.Data;
using eCommerce.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eCommerce.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize(Roles = "Customer")]
    public class CartController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _context;

        public CartController(UserManager<IdentityUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                TempData["ErrorMessage"] = "You need to be logged in to view your cart.";
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }

            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(c => c.CustomerId == user.Id);

            if (cart == null)
            {
                return View(new CartViewModel());
            }

            var cartViewModel = new CartViewModel
            {
                CartItems = cart.CartItems.Select(ci => new CartItemViewModel
                {
                    ProductId = ci.ProductId,
                    ProductName = ci.Product.Name,
                    Quantity = ci.Quantity,
                    Price = ci.Price,
                }).ToList()
            };

            return View(cartViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(int productId, int quantity)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                TempData["ErrorMessage"] = "You need to be logged in to add items to the cart.";
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }

            var product = _context.Products.Find(productId);
            if (product == null)
            {
                TempData["ErrorMessage"] = "The product you are trying to add does not exist.";
                return RedirectToAction("Index", "Shop");
            }

            var cart = _context.Carts.FirstOrDefault(c => c.CustomerId == user.Id);
            if (cart == null)
            {
                cart = new Cart
                {
                    CustomerId = user.Id
                };
                _context.Carts.Add(cart);
                await _context.SaveChangesAsync();
            }

            var cartItem = _context.CartItems
                .FirstOrDefault(ci => ci.CartId == cart.Id && ci.ProductId == productId);

            if (cartItem != null)
            {
                cartItem.Quantity += quantity;
                TempData["SuccessMessage"] = "Quantity updated for this item in your cart.";
            }
            else
            {
                cartItem = new CartItem
                {
                    CartId = cart.Id,
                    ProductId = productId,
                    Quantity = quantity,
                    Price = product.Price
                };
                _context.CartItems.Add(cartItem);
                TempData["SuccessMessage"] = "Product added to cart successfully!";
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "Shop", new { id = productId, area = "" });

        }

        [HttpPost]
        public async Task<IActionResult> Remove(int productId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                TempData["ErrorMessage"] = "You need to be logged in to remove items from the cart.";
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }

            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.CustomerId == user.Id);

            if (cart == null)
            {
                TempData["ErrorMessage"] = "Your cart could not be found.";
                return RedirectToAction("Index", "Shop");
            }

            var cartItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == productId);
            if (cartItem != null)
            {
                _context.CartItems.Remove(cartItem);
                await _context.SaveChangesAsync();
            }

            TempData["SuccessMessage"] = "Item removed from cart successfully!";
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Checkout()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                TempData["ErrorMessage"] = "You need to be logged in to proceed to checkout.";
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }

            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(c => c.CustomerId == user.Id);

            if (cart == null || !cart.CartItems.Any())
            {
                TempData["ErrorMessage"] = "Your cart is empty.";
                return RedirectToAction("Index");
            }

            var cartViewModel = new CartViewModel
            {
                CartItems = cart.CartItems.Select(ci => new CartItemViewModel
                {
                    ProductId = ci.ProductId,
                    ProductName = ci.Product.Name,
                    Quantity = ci.Quantity,
                    Price = ci.Price,
                }).ToList()
            };

            return View(cartViewModel);
        } 

    }
}
