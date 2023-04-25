using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ComputerShop.Data;
using ComputerShop.Models;
using ComputerShop.Utility;

namespace ComputerShop.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class OrderController : Controller
    {
        private ApplicationDbContext _db;

        public OrderController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Checkout()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Checkout(Order order)
        {
            List<Product>? products = HttpContext.Session.Get<List<Product>>("products");

            if(products != null)
            {
                foreach (var product in products)
                {
                    OrderDetails orderDetails = new OrderDetails();
                    orderDetails.ProductId = product.Id;
                    order.Details.Add(orderDetails);
                }
            }

            order.OrderNo = GetOrderNo();

            if(ModelState.IsValid)
            {
                _db.Orders.Add(order);
                await _db.SaveChangesAsync();
                HttpContext.Session.Set("products", new List<Product>());
            }

            return RedirectToAction("Index","Home");
        }

        public String GetOrderNo()
        {
            int rowCount = _db.Orders.ToList().Count() + 1;
            string s = rowCount.ToString("000");
            return s;
        }
    }
}

