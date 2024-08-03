using eCommerce.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eCommerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Dashboard
        public IActionResult Index()
        {
            return View();
        }

        // GET: Admin/Product
        public IActionResult Product()
        {
            return RedirectToAction("Index", "Product");
        }

        // GET: Admin/Category
        public IActionResult Category()
        {
            return RedirectToAction("Index", "Category");
        }
    }
}
