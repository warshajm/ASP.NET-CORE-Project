using eCommerce.Data;
using eCommerce.Models;
using eCommerce.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace eCommerce.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            // fetch a random product
            var featuredProducts = await _context.Products
                .Include(p => p.Category) // eagerly load the Category
                .OrderBy(p => Guid.NewGuid()) // shuffle the products
                .Take(3) // select 3 products
                .ToListAsync();

            var viewModels = featuredProducts.Select(p => new ProductViewModel
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                Description = p.Description,
                Image = p.Image,
                Stock = p.Stock,
                CategoryId = p.CategoryId,
                CategoryName = p.Category.Name
            }).ToList();

            return View(viewModels);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
