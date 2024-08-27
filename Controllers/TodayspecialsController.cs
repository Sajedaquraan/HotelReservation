using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HotelReservation.Models;

namespace HotelReservation.Controllers
{
    public class TodayspecialsController : Controller
    {
        private readonly ModelContext _context;
        private readonly IWebHostEnvironment _environment;

        public TodayspecialsController(ModelContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _environment= webHostEnvironment;
        }

        // GET: Todayspecials
        public async Task<IActionResult> Index()
        {
            var customers = _context.Customers.ToList();
            ViewBag.Customers = customers;
            var id = HttpContext.Session.GetInt32("AdminID");
            var users = _context.Customers.Where(x => x.Customerid == id).SingleOrDefault();
            ViewBag.name = users.Customername;
            ViewBag.image = users.Profileimage;
            ViewBag.email = users.Email;
            return _context.Todayspecials != null ? 
                          View(await _context.Todayspecials.ToListAsync()) :
                          Problem("Entity set 'ModelContext.Todayspecials'  is null.");
        }

        // GET: Todayspecials/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null || _context.Todayspecials == null)
            {
                return NotFound();
            }

            var todayspecial = await _context.Todayspecials
                .FirstOrDefaultAsync(m => m.Todayspecialid == id);
            if (todayspecial == null)
            {
                return NotFound();
            }

            return View(todayspecial);
        }

        // GET: Todayspecials/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Todayspecials/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Todayspecialid,Todayspecialname,Todayspecialimage,Todayspecialprice,Todayspecialcontant,ImageFile")] Todayspecial todayspecial)
        {
            if (ModelState.IsValid)
            {
                if (todayspecial.ImageFile != null)
                {
                    string wwwRootPath = _environment.WebRootPath;
                    string fileName = Guid.NewGuid().ToString()
                                      + "_"
                                      + todayspecial.ImageFile.FileName;

                    string path = Path.Combine(wwwRootPath + "/Images/", fileName);


                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await todayspecial.ImageFile.CopyToAsync(fileStream);
                    }

                    todayspecial.Todayspecialimage = fileName;
                }


                _context.Add(todayspecial);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(todayspecial);
        }

        // GET: Todayspecials/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null || _context.Todayspecials == null)
            {
                return NotFound();
            }

            var todayspecial = await _context.Todayspecials.FindAsync(id);
            if (todayspecial == null)
            {
                return NotFound();
            }
            return View(todayspecial);
        }

        // POST: Todayspecials/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Todayspecialid,Todayspecialname,Todayspecialimage,Todayspecialprice,Todayspecialcontant,ImageFile")] Todayspecial todayspecial)
        {
            if (id != todayspecial.Todayspecialid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (todayspecial.ImageFile != null)
                    {
                        string wwwRootPath = _environment.WebRootPath;
                        string fileName = Guid.NewGuid().ToString()
                                          + "_"
                                          + todayspecial.ImageFile.FileName;

                        string path = Path.Combine(wwwRootPath + "/Images/", fileName);


                        using (var fileStream = new FileStream(path, FileMode.Create))
                        {
                            await todayspecial.ImageFile.CopyToAsync(fileStream);
                        }

                        todayspecial.Todayspecialimage = fileName;
                    }

                    _context.Update(todayspecial);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TodayspecialExists(todayspecial.Todayspecialid))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(todayspecial);
        }

        // GET: Todayspecials/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null || _context.Todayspecials == null)
            {
                return NotFound();
            }

            var todayspecial = await _context.Todayspecials
                .FirstOrDefaultAsync(m => m.Todayspecialid == id);
            if (todayspecial == null)
            {
                return NotFound();
            }

            return View(todayspecial);
        }

        // POST: Todayspecials/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.Todayspecials == null)
            {
                return Problem("Entity set 'ModelContext.Todayspecials'  is null.");
            }
            var todayspecial = await _context.Todayspecials.FindAsync(id);
            if (todayspecial != null)
            {
                _context.Todayspecials.Remove(todayspecial);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TodayspecialExists(decimal id)
        {
          return (_context.Todayspecials?.Any(e => e.Todayspecialid == id)).GetValueOrDefault();
        }
    }
}
