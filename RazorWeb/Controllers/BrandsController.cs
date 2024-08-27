using Microsoft.AspNetCore.Mvc;
using RazorWeb.Data;
using RazorWeb.Models;

namespace RazorWeb.Controllers
{
    public class BrandsController : Controller
    {
        private readonly AppDbContext _context;
        public BrandsController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            List<Brand> brands = _context.Brands.ToList();
            return View(brands);
        }
    }
}
