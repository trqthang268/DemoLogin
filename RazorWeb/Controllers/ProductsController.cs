using Microsoft.AspNetCore.Mvc;
using RazorWeb.Data;
using RazorWeb.Models;

namespace RazorWeb.Controllers
{
    public class ProductsController : Controller
    {
        private readonly AppDbContext _context;
        public ProductsController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index(string search = "" , string SortColumn = "ProductID"
            , string IconClass = "fa-sort-asc", int page = 1)
        {
            // Search
            List<Product> products = _context.Products.Where(row => 
            row.ProductName.Contains(search)).ToList();
            ViewBag.Search = search;

            //Sort
            ViewBag.SortColumn = SortColumn;
            ViewBag.IconClass = IconClass;
            if(SortColumn == "ProductID")
            {
                if(IconClass == "fa-sort-asc")
                {
                    products = products.OrderBy(row => row.ProductID).ToList();
                }
                else
                {
                    products = products.OrderByDescending(row => row.ProductID).ToList();
                }
            }
            else if (SortColumn == "ProductName")
            {
                if (IconClass == "fa-sort-asc")
                {
                    products = products.OrderBy(row =>
                    row.ProductName).ToList();
                }
                else
                {
                    products = products.OrderByDescending(row =>
                    row.ProductName).ToList();
                }
            }
            else if(SortColumn == "Price")
            {
                if (IconClass == "fa-sort-asc")
                {
                    products = products.OrderBy(row =>
                    row.Price).ToList();
                }
                else
                {
                    products = products.OrderByDescending(row =>
                    row.Price).ToList();
                }
            }

            //Paging
            int NoOfRecordPerPage = 5;
            int NoOfPages = Convert.ToInt32(Math.Ceiling
                (Convert.ToDouble(products.Count) / Convert.ToDouble(NoOfRecordPerPage)));
            int NoOfRecordToSkip = (page - 1) * NoOfRecordPerPage;
            ViewBag.Page = page;
            ViewBag.NoOfPages = NoOfPages;
            products = products.Skip(NoOfRecordToSkip).Take(NoOfRecordPerPage).ToList();

            return View(products);
        }

        public IActionResult Detail(long id)
        {
            Product pro = _context.Products.Where(
                row => row.ProductID == id).FirstOrDefault();
            return View(pro);
        }

        public IActionResult Create()
        {
            ViewBag.Categories = _context.Categories.ToList();
            ViewBag.Brands = _context.Brands.ToList();
            return View();
        }

        [HttpPost]
        public IActionResult Create(Product p)
        {
            _context.Products.Add(p);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Edit(long id)
        {
            Product product = _context.Products.Where(
                row => row.ProductID == id).FirstOrDefault();
            ViewBag.Categories = _context.Categories.ToList();
            ViewBag.Brands = _context.Brands.ToList();
            return View(product);
        }

        [HttpPost]
        public IActionResult Edit(Product pro)
        {
            Product product = _context.Products.Where(row => row.ProductID == pro.ProductID).FirstOrDefault();

            //Update
            product.ProductName = pro.ProductName;
            product.Price = pro.Price;
            product.DateOfPurchase = pro.DateOfPurchase;
            product.AvailabilityStatus = pro.AvailabilityStatus;
            product.CategoryID = pro.CategoryID;
            product.BrandID = pro.BrandID;
            product.Active = pro.Active;
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Delete(long id)
        {
            Product product = _context.Products.Where(row => row.ProductID == id).FirstOrDefault();
            return View(product);
        }

        [HttpPost]
        public IActionResult Delete(long id, Product p)
        {
            Product product = _context.Products.Where(row => row.ProductID == id).FirstOrDefault();
            _context.Products.Remove(product);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
