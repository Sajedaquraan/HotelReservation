using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HotelReservation.Models;
using Microsoft.Extensions.Hosting;

namespace HotelReservation.Controllers
{
    public class HotelsController : Controller
    {
        private readonly ModelContext _context;
        private readonly IWebHostEnvironment _environment;

        public HotelsController(ModelContext context,IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _environment = webHostEnvironment;
        }

        // GET: Hotels
        public async Task<IActionResult> Index()
        {
            var customers = _context.Customers.ToList();
            ViewBag.Customers = customers;
            var id = HttpContext.Session.GetInt32("AdminID");
            var users = _context.Customers.Where(x => x.Customerid == id).SingleOrDefault();
            ViewBag.name = users.Customername;
            ViewBag.image = users.Profileimage;
            ViewBag.email = users.Email;
            return _context.Hotels != null ? 
                          View(await _context.Hotels.ToListAsync()) :
                          Problem("Entity set 'ModelContext.Hotels'  is null.");
        }

        // GET: Hotels/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null || _context.Hotels == null)
            {
                return NotFound();
            }

            var hotel = await _context.Hotels
                .FirstOrDefaultAsync(m => m.Hotelid == id);
            if (hotel == null)
            {
                return NotFound();
            }

            return View(hotel);
        }

        // GET: Hotels/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Hotels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Hotelid,Hotelname,Location,Description,Imagepath,ImageFile")] Hotel hotel)
        {
            if (ModelState.IsValid)
            {
                if (hotel.ImageFile != null)
                {
                    string wwwRootPath = _environment.WebRootPath;
                    string fileName = Guid.NewGuid().ToString()
                                      + "_"
                                      + hotel.ImageFile.FileName;

                    string path = Path.Combine(wwwRootPath + "/Images/", fileName);


                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await hotel.ImageFile.CopyToAsync(fileStream);
                    }

                    hotel.Imagepath = fileName;
                }

                _context.Add(hotel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(hotel);
        }

        // GET: Hotels/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null || _context.Hotels == null)
            {
                return NotFound();
            }

            var hotel = await _context.Hotels.FindAsync(id);
            if (hotel == null)
            {
                return NotFound();
            }
            return View(hotel);
        }

        // POST: Hotels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Hotelid,Hotelname,Location,Description,Imagepath,ImageFile")] Hotel hotel)
        {
            if (id != hotel.Hotelid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (hotel.ImageFile != null)
                    {
                        string wwwRootPath = _environment.WebRootPath;
                        string fileName = Guid.NewGuid().ToString()
                                          + "_"
                                          + hotel.ImageFile.FileName;
                        string path = Path.Combine(wwwRootPath + "/Images/", fileName);


                        using (var fileStream = new FileStream(path, FileMode.Create))
                        {
                            await hotel.ImageFile.CopyToAsync(fileStream);
                        }

                        hotel.Imagepath = fileName;
                    }

                    _context.Update(hotel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HotelExists(hotel.Hotelid))
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
            return View(hotel);
        }

        // GET: Hotels/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null || _context.Hotels == null)
            {
                return NotFound();
            }

            var hotel = await _context.Hotels
                .FirstOrDefaultAsync(m => m.Hotelid == id);
            if (hotel == null)
            {
                return NotFound();
            }

            return View(hotel);
        }

        // POST: Hotels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.Hotels == null)
            {
                return Problem("Entity set 'ModelContext.Hotels'  is null.");
            }
            var hotel = await _context.Hotels.FindAsync(id);
            if (hotel != null)
            {
                _context.Hotels.Remove(hotel);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HotelExists(decimal id)
        {
          return (_context.Hotels?.Any(e => e.Hotelid == id)).GetValueOrDefault();
        }
    }
}
