using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ElmonkezApp.Data;
using ElmonkezApp.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace ElmonkezApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ServicesController : Controller
    {
        private readonly ApplicationDbContext _context;
        [Obsolete]
        private IHostingEnvironment _he;

        [Obsolete]
        public ServicesController(ApplicationDbContext context, IHostingEnvironment he)
        {
            _context = context;
            _he = he;
        }

        // GET: Admin/Services
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Services.Include(s => s.Category);
            return View(await applicationDbContext.ToListAsync());
        }

        //Post Index action method
        [HttpPost]
        public IActionResult Index(int? lowAmount, int? largeAmount )
        {
            var services = _context.Services.Include(c => c.Category).Where(c=> c.Price >= lowAmount && c.Price <= largeAmount).ToList();
            return View(services);
            if (lowAmount == null || largeAmount == null)
            {
                services = _context.Services.Include(c => c.Category).ToList();
            }
        }

        // GET: Admin/Services/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var service = await _context.Services
                .Include(s => s.Category)
                .FirstOrDefaultAsync(m => m.ServiceId == id);
            if (service == null)
            {
                return NotFound();
            }

            return View(service);
        }

        // GET: Admin/Services/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName");
            return View();
        }

        // POST: Admin/Services/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Obsolete]
        public async Task<IActionResult> Create([Bind("ServiceId,ServiceName,Description,Details,Price,CategoryId,Image")] Service service, IFormFile image)
        {
            if (ModelState.IsValid)
            {
                var searchService = _context.Services.FirstOrDefault(s => s.ServiceName == service.ServiceName);
                if (searchService != null)
                {
                    ViewBag.message = "This Service Is already exist";
                    ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName", service.CategoryId);
                    return View(service);
                }

                if (image != null)
                {
                    var name = Path.Combine(_he.WebRootPath + "/Images", Path.GetFileName(image.FileName));
                    await image.CopyToAsync(new FileStream(name, FileMode.Create));
                    service.Image = "Images/" + image.FileName;
                }
                if (image == null)
                {
                    service.Image = "Img/Not.png";
                }


                _context.Add(service);
                await _context.SaveChangesAsync();
                TempData["save"] = "Category has been saved";
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName", service.CategoryId);
            return View(service);
        }

        // GET: Admin/Services/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var service = await _context.Services.FindAsync(id);
            if (service == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName", service.CategoryId);
            return View(service);
        }

        // POST: Admin/Services/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Obsolete]
        public async Task<IActionResult> Edit(int id, [Bind("ServiceId,ServiceName,Description,Details,Price,CategoryId,Image")] Service service, IFormFile image)
        {
            if (id != service.ServiceId)
            {
                return NotFound();
            }


            if (ModelState.IsValid)
            {

                if (image != null)
                {
                    var name = Path.Combine(_he.WebRootPath + "/Images", Path.GetFileName(image.FileName));
                    await image.CopyToAsync(new FileStream(name, FileMode.Create));
                    service.Image = "Images/" + image.FileName;
                }
                if (image == null)
                {
                    service.Image = "Img/Not.png";
                }


                _context.Update(service);
                await _context.SaveChangesAsync();
                TempData["Edit"] = "Category has been Update";
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName", service.CategoryId);
            return View(service);
        }

        // GET: Admin/Services/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var service = await _context.Services
                .Include(s => s.Category)
                .FirstOrDefaultAsync(m => m.ServiceId == id);
            if (service == null)
            {
                return NotFound();
            }

            return View(service);
        }

        // POST: Admin/Services/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var service = await _context.Services.FindAsync(id);
            _context.Services.Remove(service);
            await _context.SaveChangesAsync();
            TempData["del"] = "Category has been Deleted";
            return RedirectToAction(nameof(Index));
        }

        private bool ServiceExists(int id)
        {
            return _context.Services.Any(e => e.ServiceId == id);
        }
    }
}
