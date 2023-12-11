using Microsoft.AspNetCore.Mvc;
using Pustok2.Contexts;
using Microsoft.EntityFrameworkCore;
using Pustok2.ViewModel.BlogVM;
using Microsoft.AspNetCore.Mvc.Rendering;
using Pustok2.Models;

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
        public async Task<IActionResult> Index()
        {
            //await _db.Blogs.Include(o => o.Tags).ToListAsync();
            IEnumerable<BlogListVM> blogs = await _db.Blogs.Select(c => new BlogListVM
            {
                Id = c.Id,
                Title = c.Title,
                Description = c.Description,
                CreatedAt = c.CreatedAt,
                Author = c.Author,
                UptadedAt = c.UptadedAt,
            }).ToListAsync();

            return View(blogs);
        }

        public IActionResult Create()
        {
            ViewBag.Author = _db.Author;
            ViewBag.Tags = _db.Tags.ToList();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult>Create(BloglistCreateVm vm)
        {
            if (!ModelState.IsValid)
            {   
                return View();
            }
            if (await _db.Blogs.AnyAsync(x => x.Title== vm.Title))
            {
                ModelState.AddModelError("Title", vm.Title + " already exist");
                return View(vm);
            }
            

            Blog new_blog = new Blog
            {
                Title = vm.Title,
                Description = vm.Description,
                AuthorId = vm.AuthorId,
               
            };

            await _db.Blogs.AddAsync(new_blog);

            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        
    }
}
