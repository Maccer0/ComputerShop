using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ComputerShop.Models;
using ComputerShop.Utility;
using ComputerShop.Data;
using X.PagedList;


namespace ComputerShop.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private ApplicationDbContext _db;


        public HomeController(ILogger<HomeController> logger, ApplicationDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        [AllowAnonymous]
        public IActionResult Index(int? page)
        {
            return View(_db.Products.Include(c => c.ProductType).Include(c => c.Tag).ToList().ToPagedList(page??1, 6));
        }

        //get product details
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var products = _db.Products.Include(c => c.ProductType).Include(c => c.Tag).FirstOrDefault(c => c.Id == id);

            if(products == null)
            {
                return NotFound();
            }

            return View(products);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Details")]
        public IActionResult ProductDetail(int? id)
        {
            // cart products list
            List<Product>? products = new List<Product>();

            if (id == null)
            {
                return NotFound();
            }

            var data = _db.Products.Include(c => c.ProductType).Include(c => c.Tag).FirstOrDefault(c => c.Id == id);

            if (data == null)
            {
                return NotFound();
            }

            // get existing session data
            products = HttpContext.Session.Get<List<Product>>("products");

            if(products == null)
            {
                products = new List<Product>();
            }

            products.Add(data);
            HttpContext.Session.Set("products", products);

            return View(data);
        }

        [ActionName("Remove")]
        public IActionResult RemoveFromCart(int? id)
        {
            List<Product>? products = HttpContext.Session.Get<List<Product>>("products");

            if(products != null)
            {
                var product = products.FirstOrDefault(c => c.Id == id);

                if(product != null)
                {
                    products.Remove(product);

                    HttpContext.Session.Set("products", products);
                }
            }

            return RedirectToAction(nameof(Cart));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Remove(int? id)
        {
            List<Product>? products = HttpContext.Session.Get<List<Product>>("products");

            if(products != null)
            {
                var product = products.FirstOrDefault(c => c.Id == id);

                if(product != null)
                {
                    products.Remove(product);
                    HttpContext.Session.Set("products", products);
                }
            }

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Cart()
        {
            List<Product>? products = HttpContext.Session.Get<List<Product>>("products");

            if(products == null)
            {
                products = new List<Product>();
            }

            return View(products);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

