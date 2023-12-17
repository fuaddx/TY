using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Pustok2.Contexts;
using Pustok2.ViewModel.AuthorVM;

namespace Pustok2.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AuthorController : Controller
    {
        PustokDbContext _db { get; }

        public AuthorController(PustokDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _db.Author.Include(a => a.Blogs).Select(c => new AuthorListVm
            {
                Id = c.Id,
                Name = c.Name,
                Surname = c.Surname,
                IsDeleted = c.IsDeleted,
            }).ToListAsync());
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(AuthorCreateVm vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            if (await _db.Author.AnyAsync(x => x.Name == vm.Name))
            {
                ModelState.AddModelError("Name", vm.Name + " already exist");
                return View(vm);
            }
            await _db.Author.AddAsync(new Models.Author
            {
                Name = vm.Name,
                Surname = vm.Surname,
            });
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return BadRequest();
            var data = await _db.Author.FindAsync(id);
            if (data == null) return NotFound();
            _db.Author.Remove(data);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) return BadRequest();
            var data = await _db.Author.FindAsync(id);
            if (data == null) return NotFound();
            return View(new AuthorUpdateVm
            {
                Name = data.Name,
                Surname = data.Surname,
            });
        }
        [HttpPost]
        public async Task<IActionResult> Update(int? id, AuthorUpdateVm vm)
        {
            if (id == null || id <= 0) return BadRequest();
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            var data = await _db.Author.FindAsync(id);
            if (data == null) return NotFound();
            data.Name = vm.Name;
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Selection()
        {
            var authors = await _db.Author.ToListAsync();
            ViewBag.AuthorsList = new SelectList(authors, "Name", "Surname");
            return View();
        }
    }
}
