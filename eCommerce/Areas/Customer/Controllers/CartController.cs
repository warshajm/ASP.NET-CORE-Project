using eCommerce.Data;
using eCommerce.Models;
using eCommerce.ViewModels;
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
        public async Task<IActionResult> AddToCart(int productId, int quantity, string returnUrl)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                TempData["ErrorMessage"] = "You need to be logged in to add items to the cart.";
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }

            var product = _context.Products.Find(productId);
            if (product == null || product.Stock < quantity)
            {
                TempData["ErrorMessage"] = "The product is out of stock or you requested more than available.";
                return Redirect(returnUrl);
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

            return Redirect(returnUrl);

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
                return RedirectToAction("Index", "Shop");
            }

            // check product stock
            foreach (var item in cart.CartItems)
            {
                if (item.Product.Stock < item.Quantity)
                {
                    TempData["ErrorMessage"] = $"Sorry, {item.Product.Name} is out of stock or insufficient quantity available.";
                    return RedirectToAction("Index", "Cart");
                }
            }

            var checkoutViewModel = new CheckoutViewModel
            {
                CartItems = cart.CartItems.Select(ci => new CartItemViewModel
                {
                    ProductId = ci.ProductId,
                    ProductName = ci.Product.Name,
                    Quantity = ci.Quantity,
                    Price = ci.Price
                }).ToList()
            };

            return View(checkoutViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Checkout(CheckoutViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                TempData["ErrorMessage"] = "You need to be logged in to complete the checkout.";
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }

            if (!ModelState.IsValid)
            {

                TempData["ErrorMessage"] = "Please correct the errors in the form.";
                return View(model);
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

            var order = new Order
            {
                CustomerId = user.Id,
                OrderDate = DateTime.Now,
                TotalAmount = cart.CartItems.Sum(ci => ci.Quantity * ci.Price) * 1.13m,
                FullName = model.FullName,
                Address = model.Address,
                City = model.City,
                State = model.State,
                ZipCode = model.ZipCode,
                PaymentMethod = model.PaymentMethod
            };

            _context.Orders.Add(order);

            await _context.SaveChangesAsync();

            var orderItems = cart.CartItems.Select(ci => new OrderItem
            {
                OrderId = order.Id,
                ProductId = ci.ProductId,
                Quantity = ci.Quantity,
                Price = ci.Price
            }).ToList();

            _context.OrderItems.AddRange(orderItems);
            _context.CartItems.RemoveRange(cart.CartItems);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Thank you for your purchase! Your order has been placed.";
            return RedirectToAction("Confirmation", new { orderId = order.Id });
        }

        public async Task<IActionResult> Confirmation(int orderId)
        {
            var order = await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product) 
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null)
            {
                return NotFound();
            }

            var viewModel = new ConfirmationViewModel
            {
                OrderId = order.Id,
                Name = order.Customer.FullName,  
                Email = order.Customer.Email,
                OrderDate = order.OrderDate,
                ShippingName = order.FullName,
                Address = order.Address,
                City = order.City,
                State = order.State,
                ZipCode = order.ZipCode,
                Payment = order.PaymentMethod,
                Items = order.OrderItems.Select(item => new OrderItemViewModel
                {
                    ProductName = item.Product.Name,
                    Quantity = item.Quantity,
                    Price = item.Price
                }).ToList()
            };

            return View(viewModel);
        }


    }
}
