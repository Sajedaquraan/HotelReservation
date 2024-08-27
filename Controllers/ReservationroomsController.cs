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
    public class ReservationroomsController : Controller
    {
        private readonly ModelContext _context;

        public ReservationroomsController(ModelContext context)
        {
            _context = context;
        }

        // GET: Reservationrooms
        public async Task<IActionResult> Index()
        {
            var customers = _context.Customers.ToList();
            ViewBag.Customers = customers;
            var id = HttpContext.Session.GetInt32("AdminID");
            var users = _context.Customers.Where(x => x.Customerid == id).SingleOrDefault();
            ViewBag.name = users.Customername;
            ViewBag.image = users.Profileimage;
            ViewBag.email = users.Email;
            var modelContext = _context.Reservationrooms.Include(r => r.Customer).Include(r => r.Room);
            return View(await modelContext.ToListAsync());
        }


        [HttpPost]
        public IActionResult Index(DateTime? startDate, DateTime? endDate)
        {
            var modelContext = _context.Reservationrooms.Include(p => p.Customer).Include(p => p.Room).ToList();

            if (startDate == null && endDate == null)
            {
                //ViewBag.TotalPrice = modelContext.Sum(x => x.Product.Price * x.Quantity);
                return View(modelContext);
            }
            else if (startDate != null && endDate == null) //1/4/2024 
            {
                modelContext = modelContext.Where(x => x.Checkindate.Value.Date >= startDate).ToList();
                //ViewBag.TotalPrice = modelContext.Sum(x => x.Product.Price * x.Quantity);
                return View(modelContext);
            }

            else if (startDate == null && endDate != null) //1/5/2024
            {
                modelContext = modelContext.Where(x => x.Checkoutdate.Value.Date <= endDate).ToList();
                //ViewBag.TotalPrice = modelContext.Sum(x => x.Product.Price * x.Quantity);
                return View(modelContext);
            }
            else // 1/4/2024 - 1/5/2024 
            {
                modelContext = modelContext.Where(x => x.Checkindate.Value.Date >= startDate &&
                x.Checkoutdate.Value.Date <= endDate).ToList();
                //ViewBag.TotalPrice = modelContext.Sum(x => x.Product.Price * x.Quantity);
                return View(modelContext);

            }
        }



        // GET: Reservationrooms/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null || _context.Reservationrooms == null)
            {
                return NotFound();
            }

            var reservationroom = await _context.Reservationrooms
                .Include(r => r.Customer)
                .Include(r => r.Room)
                .FirstOrDefaultAsync(m => m.Reservationid == id);
            if (reservationroom == null)
            {
                return NotFound();
            }

            return View(reservationroom);
        }

        // GET: Reservationrooms/Create
        public IActionResult Create()
        {
            ViewData["Customerid"] = new SelectList(_context.Customers, "Customerid", "Customerid");
            ViewData["Roomid"] = new SelectList(_context.Rooms, "Roomid", "Roomid");
            return View();
        }

        // POST: Reservationrooms/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Reservationid,Customerid,Roomid,Reservationdate,Checkindate,Checkoutdate,Paymentstatus")] Reservationroom reservationroom)
        {
            if (ModelState.IsValid)
            {
                _context.Add(reservationroom);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Customerid"] = new SelectList(_context.Customers, "Customerid", "Customerid", reservationroom.Customerid);
            ViewData["Roomid"] = new SelectList(_context.Rooms, "Roomid", "Roomid", reservationroom.Roomid);
            return View(reservationroom);
        }

        // GET: Reservationrooms/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null || _context.Reservationrooms == null)
            {
                return NotFound();
            }

            var reservationroom = await _context.Reservationrooms.FindAsync(id);
            if (reservationroom == null)
            {
                return NotFound();
            }
            ViewData["Customerid"] = new SelectList(_context.Customers, "Customerid", "Customerid", reservationroom.Customerid);
            ViewData["Roomid"] = new SelectList(_context.Rooms, "Roomid", "Roomid", reservationroom.Roomid);
            return View(reservationroom);
        }

        // POST: Reservationrooms/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Reservationid,Customerid,Roomid,Reservationdate,Checkindate,Checkoutdate,Paymentstatus")] Reservationroom reservationroom)
        {
            if (id != reservationroom.Reservationid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reservationroom);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReservationroomExists(reservationroom.Reservationid))
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
            ViewData["Customerid"] = new SelectList(_context.Customers, "Customerid", "Customerid", reservationroom.Customerid);
            ViewData["Roomid"] = new SelectList(_context.Rooms, "Roomid", "Roomid", reservationroom.Roomid);
            return View(reservationroom);
        }

        // GET: Reservationrooms/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null || _context.Reservationrooms == null)
            {
                return NotFound();
            }

            var reservationroom = await _context.Reservationrooms
                .Include(r => r.Customer)
                .Include(r => r.Room)
                .FirstOrDefaultAsync(m => m.Reservationid == id);
            if (reservationroom == null)
            {
                return NotFound();
            }

            return View(reservationroom);
        }

        // POST: Reservationrooms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.Reservationrooms == null)
            {
                return Problem("Entity set 'ModelContext.Reservationrooms'  is null.");
            }
            var reservationroom = await _context.Reservationrooms.FindAsync(id);
            if (reservationroom != null)
            {
                _context.Reservationrooms.Remove(reservationroom);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReservationroomExists(decimal id)
        {
          return (_context.Reservationrooms?.Any(e => e.Reservationid == id)).GetValueOrDefault();
        }
    }
}
