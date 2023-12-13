using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Pustok2.Contexts;
using Pustok2.Models;
using Pustok2.ViewModel.TagVm;

namespace Pustok2.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TagController : Controller
    {
        PustokDbContext _pustokDbContext;

        public TagController(PustokDbContext pustokDbContext)
        {
            _pustokDbContext = pustokDbContext;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _pustokDbContext.Tags.Select(c=> new ListTagVm
            {
                Id = c.Id,
                Title = c.Title,
            }).ToListAsync());
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateTagVm vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            await _pustokDbContext.Tags.AddAsync(new Models.Tag
            {
                Title = vm.Title,
            });
            await _pustokDbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Update()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateTagVm updateTagVm)
        {
            Tag tag = new Tag()
            {
                Id = updateTagVm.Id,
                Title = updateTagVm.Title,
            };

            EntityEntry<Tag> entityEntry = _pustokDbContext.Tags.Update(tag);
            int result = await _pustokDbContext.SaveChangesAsync();

            return Redirect(nameof(Index));
        }
    }
}
