using Pustok2.Contexts;
using Pustok2.Models;
using Pustok2.ViewModel.ProductVM;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pustok2.Helpers;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Pustok2.ViewModel.ProductVM
{
    [Area("Admin")]
    public class ProductController : Controller
    {
       PustokDbContext _db { get; }
        IWebHostEnvironment _env { get; }

        public ProductController(PustokDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }

        public IActionResult Index()
        {

            return View(_db.Products.Select(p => new ProductListVM
            {
                Id = p.Id,
                Name = p.Name,
                CostPrice = p.CostPrice,
                Discount = p.Discount,
                Category = p.Category,
                ImageUrl = p.ImageUrl,
                IsDeleted = p.IsDeleted,
                Quantity = p.Quantity,
                SellPrice = p.SellPrice,
                Colors = p.ProductColors.Select(p => p.Color)
            }));
        }
        public IActionResult Create()
        {
            ViewBag.Categories = _db.Categories;
            ViewBag.Colors = new SelectList(_db.Color, "Id", "Name");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(ProductCreateVM vm)
        {
            if(vm.MainImage!= null)
            {
                if(!vm.MainImage.IsCorrectType())
                {
                    ModelState.AddModelError("MainImage", "Wrong fie type");
                }
                if(!vm.MainImage.IsValidSize(200))
                {
                    ModelState.AddModelError("MainImage", "Image must less than given kb");
                }
            }
            if (vm.HoverImage != null)
            {
                if (!vm.HoverImage.IsCorrectType())
                {
                    ModelState.AddModelError("HoverImage", "Wrong file type");
                }
                if (!vm.HoverImage.IsValidSize(200))
                {
                    ModelState.AddModelError("HoverImage", "Files length must be less than kb");
                }
            }
            if (vm.Images != null)
            {
                foreach (var img in vm.Images)
                {
                    if (!img.IsCorrectType())
                    {
                        ModelState.AddModelError("", "Wrong file type (" + img.FileName + ")");
                    }
                    if (!img.IsValidSize(600))
                    {
                        ModelState.AddModelError("", "Files length must be less than kb (" + img.FileName + ")");

                    }
                }
            }
            if (vm.CostPrice > vm.SellPrice)
            {
                ModelState.AddModelError("CostPrice", "Sell price must be bigger than cost price");
            }
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = _db.Categories;
                ViewBag.Colors = new SelectList(_db.Color, "Id", "Name");
                return View(vm);
            }
            if (!await _db.Categories.AnyAsync(c => c.Id == vm.CategoryId))
            {
                ModelState.AddModelError("CategoryId", "Category doesnt exist");
                ViewBag.Categories = _db.Categories;
                ViewBag.Colors = new SelectList(_db.Color, "Id", "Name");
                return View(vm);
            }
            if (await _db.Color.Where(c => vm.ColorIds.Contains(c.Id)).Select(c => c.Id).CountAsync() != vm.ColorIds.Count())
            {
                ModelState.AddModelError("ColorIds", "Color doesnt exist");
                ViewBag.Categories = _db.Categories;
                ViewBag.Colors = new SelectList(_db.Color, "Id", "Name");
                return View(vm);
            }
            Product prod = new Product
            {
                Name = vm.Name,
                About = vm.About,
                Quantity = vm.Quantity,
                Description = vm.Description,
                Discount = vm.Discount,
                ImageUrl = await vm.MainImage.SaveAsync(PathConstants.Product),
                CostPrice = vm.CostPrice,
                SellPrice = vm.SellPrice,
                UrlImage2= await vm.HoverImage.SaveAsync(PathConstants.Product),
                CategoryId = vm.CategoryId,
                ProductColors = vm.ColorIds.Select(id => new ProductColor
                {
                    ColorId = id,
                }).ToList(),
                ProductImages = vm.Images.Select(i => new ProductImages
                {
                    ImagePath = i.SaveAsync(PathConstants.Product).Result
                }).ToList()
            };
            await _db.Products.AddAsync(prod);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
