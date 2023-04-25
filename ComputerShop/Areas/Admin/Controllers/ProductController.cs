using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ComputerShop.Data;
using ComputerShop.Models;
using IWebHostEnvironment = Microsoft.AspNetCore.Hosting.IWebHostEnvironment;

namespace ComputerShop.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize]
	public class ProductController : Controller
	{
		ApplicationDbContext _db;

		IWebHostEnvironment _dir;

		public ProductController(ApplicationDbContext db, IWebHostEnvironment dir)
		{
			_db = db;
			_dir = dir;
		}

		public IActionResult Index()
		{
			var products = _db.Products.Include(c => c.ProductType).Include(c => c.Tag).ToList();

			return View(products);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Index(decimal lowAmount, decimal largeAmount)
		{
			var products = _db.Products.Include(c => c.ProductType).Include(c => c.Tag).Where(c => c.Price >= lowAmount && c.Price <= largeAmount).ToList();

			return View(products);
		}

		//CREATE PRODUCT
		public IActionResult Create()
		{
			ViewData["productTypeId"] = new SelectList(_db.ProductTypes.ToList(), "Id", "ProductType");
			ViewData["TagId"] = new SelectList(_db.Tags.ToList(), "Id", "TagName");

			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(Product product, IFormFile image)
		{

			if(ModelState.IsValid)
			{
				var searchProduct = _db.Products.FirstOrDefault(c => c.Name == product.Name);

				if(searchProduct != null)
				{
					ViewBag.message = "This product already exists";

					ViewData["productTypeId"] = new SelectList(_db.ProductTypes.ToList(), "Id", "ProductType");
					ViewData["TagId"] = new SelectList(_db.Tags.ToList(), "Id", "TagName");

					return View();
				}

				if(image != null)
				{
					var name = Path.Combine(_dir.WebRootPath + "/images", Path.GetFileName(image.FileName));
					await image.CopyToAsync(new FileStream(name, FileMode.Create));

					product.Image = "images/" + image.FileName;
				}

				if(image == null)
				{
					product.Image = "images/default_product.png";
				}

				_db.Products.Add(product);
				await _db.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}
			ViewData["productTypeId"] = new SelectList(_db.ProductTypes.ToList(), "Id", "ProductType");
			ViewData["TagId"] = new SelectList(_db.Tags.ToList(), "Id", "TagName");
			return View(product);
		}
		//CREATE PRODUCT

		//EDIT PRODUCT
		public IActionResult Edit(int? id)
		{
			ViewData["productTypeId"] = new SelectList(_db.ProductTypes.ToList(), "Id", "ProductType");
			ViewData["TagId"] = new SelectList(_db.Tags.ToList(), "Id", "TagName");
			if (id == null)
			{
				return NotFound();
			}

			var product = _db.Products.Include(c => c.ProductType).Include(c => c.Tag).FirstOrDefault(c => c.Id == id);

			if(product == null)
			{
				return NotFound();
			}

			product.Image = _dir.WebRootPath + "/" + product.Image;


            return View(product);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(Product product, IFormFile image)
		{
			if (ModelState.IsValid)
			{

				if (image != null)
				{
					var name = Path.Combine(_dir.WebRootPath + "/images", Path.GetFileName(image.FileName));
					await image.CopyToAsync(new FileStream(name, FileMode.Create));

					product.Image = "images/" + image.FileName;
				}

				if (image == null)
				{
					product.Image = "images/default_product.png";
				}

				_db.Products.Update(product);
				await _db.SaveChangesAsync();

				return RedirectToAction(nameof(Index));
			}

            ViewData["productTypeId"] = new SelectList(_db.ProductTypes.ToList(), "Id", "ProductType");
            ViewData["TagId"] = new SelectList(_db.Tags.ToList(), "Id", "TagName");
			return View(product);
        }
		//EDIT PRODUCT

		//PRODUCT DETAILS
		public IActionResult Details(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var product = _db.Products.Include(c => c.ProductType).Include(c => c.Tag).FirstOrDefault(c => c.Id == id);

			if (product == null)
			{
				return NotFound();
			}

			return View(product);
		}
		//PRODUCT DETAILS

		//DELETE PRODUCT
		public IActionResult Delete(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var product = _db.Products.Include(c => c.ProductType).Include(c => c.Tag).Where(c => c.Id == id).FirstOrDefault();

			if (product == null)
			{
				return NotFound();
			}

			return View(product);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[ActionName("Delete")]
		public async Task<IActionResult> DeleteConfirm(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var product = _db.Products.FirstOrDefault(c => c.Id == id);

			if(product == null)
			{
				return NotFound();
			}

			_db.Products.Remove(product);
			await _db.SaveChangesAsync();

			return RedirectToAction(nameof(Index));
		}
		//DELETE PRODUCT
	}
}