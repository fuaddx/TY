using Pustok2.Contexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pustok2.ViewModel.CategoryVM;
using Pustok2.ViewModel.CommonVM;
using Pustok2.ViewModel.ProductVM;

namespace Pustok2.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        PustokDbContext _db { get; }

        public CategoryController(PustokDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            int take = 4;
            var items = _db.Categories.Take(take).Select(p => new CategoryListItemVM
            {
                Id = p.Id,
                Name = p.Name,
            });
            int count = await _db.Categories.CountAsync();
            PaginatonVM<IEnumerable<CategoryListItemVM>> pag = new(count, 1, (int)Math.Ceiling((decimal)count / take), items);
            return View(await _db.Categories.Select(c => new CategoryListItemVM { Id = c.Id, Name = c.Name }).ToListAsync());
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CategoryCreateVM vm)
        {
            if (!ModelState.IsValid) 
            { 
                return View(vm);
            }
            if (await _db.Categories.AnyAsync(x=>x.Name == vm.Name))
            {
                ModelState.AddModelError("Name", vm.Name + " already exist");
                return View(vm);
            }
            await _db.Categories.AddAsync(new Models.Category {
                Name = vm.Name,
            });
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> ProductPagination(int page = 1, int count = 8)
        {
            var datas = await _db.Products.Where(p => !p.IsDeleted).Take(count).ToListAsync();
            return PartialView("_ProductPaginationPartial",datas);
        }
    }
}
