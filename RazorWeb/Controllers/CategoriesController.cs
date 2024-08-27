using Microsoft.AspNetCore.Mvc;
using RazorWeb.Data;
using RazorWeb.Models;


namespace RazorWeb.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly AppDbContext _context;
        public CategoriesController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            List<Category> categories = _context.Categories.ToList();
            return View(categories);
        }
    }
}
