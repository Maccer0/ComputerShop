using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ComputerShop.Data;
using ComputerShop.Models;

namespace ComputerShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class TagController : Controller
    {
        ApplicationDbContext _db;

        public TagController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            return View(_db.Tags.ToList());
        }

        //CREATE TAG
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Tag tag)
        {
            if(ModelState.IsValid)
            {
                _db.Tags.Update(tag);

                await _db.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(tag);
        }
        //CREATE TAG

        //EDIT TAG
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var data = _db.Tags.Find(id);
            if (data == null)
            {
                return NotFound();
            }

            return View(data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Tag tag)
        {
            if(ModelState.IsValid)
            {
                _db.Tags.Update(tag);

                await _db.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(tag);
        }
        //EDIT TAG

        //DETAILS
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var data = _db.Tags.Find(id);

            if (data == null)
            {
                return NotFound();
            }

            return View(data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Details()
        {
            return RedirectToAction(nameof(Index));
        }
        //DETAILS

        //DELETE TAG
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var data = _db.Tags.Find(id);

            if(data == null)
            {
                return NotFound();
            }

            return View(data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id, Tag tag)
        {
            if (id == null)
            {
                return NotFound();
            }

            if(id != tag.Id)
            {
                return NotFound();
            }

            var data = _db.Tags.Find(id);

            if(data == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _db.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(tag);
        }
    }
}

