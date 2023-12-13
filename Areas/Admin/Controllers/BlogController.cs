using Microsoft.AspNetCore.Mvc;
using Pustok2.Contexts;
using Microsoft.EntityFrameworkCore;
using Pustok2.ViewModel.BlogVM;
using Microsoft.AspNetCore.Mvc.Rendering;
using Pustok2.Models;
using Pustok2.Helpers;

namespace Pustok2.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BlogController : Controller
    {
        PustokDbContext _db { get; }
        public BlogController(PustokDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {

            return View(_db.Blogs.Select(c => new BlogListVM
            {
                Id = c.Id,
                Title = c.Title,
                Description = c.Description,
                CreatedAt = c.CreatedAt,
                Author = c.Author,
                UptadedAt = c.UptadedAt,
                Tags = c.BlogTag.Select(i => i.Tag),
            }));
        }
        public IActionResult Create()
        {
            ViewBag.Author = _db.Author;
            ViewBag.Tags = new SelectList(_db.Tags.ToList(), "Id", "Title");
            return View();
        }
        

        [HttpPost]
        public async Task<IActionResult> Create(BloglistCreateVm vm)
        {
            if (await _db.Tags.Where(c => vm.TagsId.Contains(c.Id)).Select(c => c.Id).CountAsync() != vm.TagsId.Count())
            {
                ModelState.AddModelError("TagsId", "TagsId doesnt exist");
                ViewBag.Author = _db.Author;
                ViewBag.Tags = new SelectList(_db.Tags, "Id", "Title");
                return View(vm);
            }
            Blog prod = new Blog
            {
                Title = vm.Title,
                Description = vm.Description,
                AuthorId = vm.AuthorId,

                BlogTag = vm.TagsId.Select(id => new BlogTag
                {
                    TagId = id
                }).ToList()
            };
            await _db.Blogs.AddAsync(prod);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Cancel()
        {
            return RedirectToAction(nameof(Index));
        }
    }
}
