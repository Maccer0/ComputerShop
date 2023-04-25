using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ComputerShop.Data;
using ComputerShop.Models;

namespace ComputerShop.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize]
	public class ProductTypeController : Controller
	{
		private ApplicationDbContext _db;

		public ProductTypeController(ApplicationDbContext db)
		{
			_db = db;
		}

		[AllowAnonymous]
		public IActionResult Index()
		{
			return View(_db.ProductTypes.ToList());
		}

		//CREATE PRODUCT TYPE
		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(ProductTypes productTypes)
		{
			if(ModelState.IsValid)
			{
				_db.ProductTypes.Add(productTypes);
				await _db.SaveChangesAsync();

				TempData["save"] = "Saved product type";

				return RedirectToAction(nameof(Index));
			}

			return View(productTypes);
		}

		//EDIT PRODUCT TYPE
		public IActionResult Edit(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var productType = _db.ProductTypes.Find(id);

			if(productType == null)
			{
				return NotFound();
			}

			return View(productType);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(ProductTypes productTypes)
		{
			if(ModelState.IsValid)
			{
				_db.Update(productTypes);

				await _db.SaveChangesAsync();

				return RedirectToAction(nameof(Index));
			}

			return View(productTypes);
		}
		//EDIT PRODUCT TYPE

		//DETAILS
		public IActionResult Details(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var productType = _db.ProductTypes.Find(id);

			if (productType == null)
			{
				return NotFound();
			}

			return View(productType);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Details(ProductTypes productTypes)
		{
			return RedirectToAction(nameof(Index));
		}
		//DETAILS

		//DELETE PRODUCT
		public IActionResult Delete(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var productType = _db.ProductTypes.Find(id);

			if(productType == null)
			{
				return NotFound();
			}

			return View(productType);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Delete(int? id, ProductTypes productTypes)
		{
			if (id == null)
			{
				return NotFound();
			}

			if (id != productTypes.Id)
			{
				return NotFound();
			}

			var productType = _db.ProductTypes.Find(id);
			if (productType == null)
			{
				return NotFound();
			}

			if(ModelState.IsValid)
			{
				_db.Remove(productType);

				await _db.SaveChangesAsync();

				return RedirectToAction(nameof(Index));
			}

			return View(productTypes);
		}
	}
}

