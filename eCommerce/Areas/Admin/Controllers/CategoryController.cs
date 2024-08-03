using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using eCommerce.Data;
using eCommerce.Models;
using Microsoft.AspNetCore.Authorization;

namespace eCommerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CategoryController> _logger;
        public CategoryController(ApplicationDbContext context, ILogger<CategoryController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Admin/Category
        public async Task<IActionResult> Index()
        {
            try
            {
                var categories = await _context.Categories.ToListAsync();
                return View(categories);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting categories.");
                return Problem("An error occurred while retrieving categories.");
            }
        }

        // GET: Admin/Category/Details/Id
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.Categories == null)
            {
                _logger.LogWarning("Category ID is null or Categories DbSet is null.");
                return NotFound();
            }

            var category = await _context.Categories.FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                _logger.LogWarning($"Category with ID {id} not found.");
                return NotFound();
            }

            return View(category);
        }

        // GET: Admin/Category/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Category/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Category category)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (CategoryExists(category.Id))
                    {
                        _logger.LogWarning($"Category with ID {category.Id} already exists.");
                        TempData["ErrorMessage"] = "Category ID already exists.";
                        return View(category);
                    }

                    if (await _context.Categories.AnyAsync(c => c.Name == category.Name))
                    {
                        _logger.LogWarning($"Category with Name {category.Name} already exists.");
                        TempData["ErrorMessage"] = "Category Name already exists.";
                        return View(category);
                    }

                    _context.Add(category);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Category created successfully!";
                    _logger.LogInformation($"Category with ID {category.Id} created successfully.");
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while creating the category.");
                    TempData["ErrorMessage"] = "An error occurred while creating the category.";
                    return Problem("An error occurred while creating the category.");
                }
            }
            return View(category);
        }

        // GET: Admin/Category/Edit/Id
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.Categories == null)
            {
                return NotFound();
            }

            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: Admin/Category/Edit/Id
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Name")] Category category)
        {
            if (id != category.Id)
            {
                _logger.LogWarning("Category ID mismatch.");
                TempData["ErrorMessage"] = "Category ID mismatch.";
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (await _context.Categories.AnyAsync(c => c.Name == category.Name && c.Id != category.Id))
                    {
                        _logger.LogWarning($"Another category with Name {category.Name} already exists.");
                        TempData["ErrorMessage"] = "Category Name already exists.";
                        return View(category);
                    }

                    _context.Update(category);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Category updated successfully!";
                    _logger.LogInformation($"Category with ID {category.Id} updated successfully.");
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    if (!CategoryExists(category.Id))
                    {
                        _logger.LogWarning($"Category with ID {category.Id} not found for update.");
                        TempData["ErrorMessage"] = "Category not found.";
                        return NotFound();
                    }
                    else
                    {
                        _logger.LogError(ex, "An error occurred while updating the category.");
                        TempData["ErrorMessage"] = "An error occurred while updating the category.";
                        throw;
                    }
                }
            }
            return View(category);
        }

        // GET: Admin/Category/Delete/Id
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.Categories == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Admin/Category/Delete/Id
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Categories == null)
            {
                _logger.LogWarning("Categories DbSet is null.");
                TempData["ErrorMessage"] = "An error occurred. Please try again later.";
                return Problem("Entity set 'ApplicationDbContext.Categories' is null.");
            }

            var category = await _context.Categories.FindAsync(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Category deleted successfully!";
                _logger.LogInformation($"Category with ID {id} deleted successfully.");
            }
            else
            {
                _logger.LogWarning($"Category with ID {id} not found for deletion.");
                TempData["ErrorMessage"] = "Category not found.";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(string id)
        {
          return (_context.Categories?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
