using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RazorWeb.Data;
using RazorWeb.Models;

namespace RazorWeb.Controllers
{
    public class ProductsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment environment;
        public ProductsController(AppDbContext context, IWebHostEnvironment environment)
        {
            this.environment = environment;
            _context = context;
        }
        // GET: Products
        public async Task<IActionResult> Index()
        {
            var products = await _context.Products.Include(p => p.Brand).Include(p => p.Category).ToListAsync();
            return View(products);
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.ProductID == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            ViewData["BrandID"] = new SelectList(_context.Brands, "BrandID", "BrandName");
            ViewData["CategoryID"] = new SelectList(_context.Categories, "CategoryID", "CategoryName");
            return View();
        }

        // POST: Products/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductDto productDto)
        {
            if (productDto.ImageFile == null)
            {
                ModelState.AddModelError("ImageFile", "The Image file is required");
            }
            if (ModelState.IsValid)
            {

                string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                newFileName += Path.GetExtension(productDto.ImageFile!.FileName);

                string imageFullPath = environment.WebRootPath + "/products/" + newFileName;
                using (var stream = System.IO.File.Create(imageFullPath))
                {
                    productDto.ImageFile.CopyTo(stream);
                }

                var product = new Product
                {
                    ProductName = productDto.ProductName,
                    Description = productDto.Description,
                    Price = productDto.Price,
                    CategoryID = productDto.CategoryID,
                    BrandID = productDto.BrandID,
                    Active = productDto.Active,
                    CreateAt = DateTime.Now,
                    ImageFileName = newFileName
                };

                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Products");
            }
            ViewData["BrandID"] = new SelectList(_context.Brands, "BrandID", "BrandName", productDto.BrandID);
            ViewData["CategoryID"] = new SelectList(_context.Categories, "CategoryID", "CategoryName", productDto.CategoryID);
            return View(productDto);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return RedirectToAction("Index", "Products");
            }

            //create productDto from product
            var productDto = new ProductDto
            {
                ProductName = product.ProductName,
                Description = product.Description,
                Price = product.Price,
                CategoryID = product.CategoryID,
                BrandID = product.BrandID,
                Active = product.Active
            };

            ViewData["BrandID"] = new SelectList(_context.Brands, "BrandID", "BrandName", productDto.BrandID);
            ViewData["CategoryID"] = new SelectList(_context.Categories, "CategoryID", "CategoryName", productDto.CategoryID);
            ViewData["ProductId"] = product.ProductID;
            ViewData["ImageFileName"] = product.ImageFileName;
            ViewData["CreateAt"] = product.CreateAt.ToString("MM/dd/yyyy");
            return View(productDto);
        }

        // POST: Products/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProductDto productDto)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return RedirectToAction("Index", "Products");
            }
            if (!ModelState.IsValid)
            {
                ViewData["BrandID"] = new SelectList(_context.Brands, "BrandID", "BrandName", productDto.BrandID);
                ViewData["CategoryID"] = new SelectList(_context.Categories, "CategoryID", "CategoryName", productDto.CategoryID);
                ViewData["ProductId"] = product.ProductID;
                ViewData["ImageFileName"] = product.ImageFileName;
                ViewData["CreateAt"] = product.CreateAt.ToString("MM/dd/yyyy");
                return View(productDto);
            }

            //update the image file if we have a new image file
            string newFileName = product.ImageFileName;
            if (productDto.ImageFile != null)
            {
                newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                newFileName += Path.GetExtension(productDto.ImageFile!.FileName);

                string imageFullPath = environment.WebRootPath + "/products/" + newFileName;
                using (var stream = System.IO.File.Create(imageFullPath))
                {
                    productDto.ImageFile.CopyTo(stream);
                }

                //delete the old image
                string oldImageFullPath = environment.WebRootPath + "/products/" + product.ImageFileName;
                System.IO.File.Delete(oldImageFullPath);
            }

            //update the product in the database
            product.ProductName = productDto.ProductName;
            product.Description = productDto.Description;
            product.Price = productDto.Price;
            product.CategoryID = productDto.CategoryID;
            product.BrandID = productDto.BrandID;
            product.Active = productDto.Active;
            product.ImageFileName = newFileName;

            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Products");
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.ProductID == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.ProductID == id);
        }
    }
}
