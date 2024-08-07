using eCommerce.Data;
using eCommerce.Models;
using eCommerce.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eCommerce.Controllers
{
    public class ShopController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ShopController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Shop/
        public async Task<IActionResult> Index(string selectedCategory)
        {
            IQueryable<Product> productsQuery = _context.Products.Include(p => p.Category);

            if (!string.IsNullOrEmpty(selectedCategory))
            {
                productsQuery = productsQuery.Where(p => p.CategoryId == selectedCategory);
            }

            var products = await productsQuery.ToListAsync();
            var categories = await _context.Categories.ToListAsync();

            var viewModel = new ShopViewModel
            {
                Products = products,
                Categories = new SelectList(categories, "Id", "Name"),
                SelectedCategory = selectedCategory
            };

            return View(viewModel);
        }

        // GET: Shop/Details/Id
        public async Task<IActionResult> Details(int id)
        {
            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            var model = new ProductViewModel
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Description = product.Description,
                Image = product.Image,
                Stock = product.Stock,
                CategoryId = product.CategoryId,
                CategoryName = product.Category.Name
            };

            return View(model);
        }

    }
}
