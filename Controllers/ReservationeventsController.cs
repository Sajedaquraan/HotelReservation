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
    public class ReservationeventsController : Controller
    {
        private readonly ModelContext _context;

        public ReservationeventsController(ModelContext context)
        {
            _context = context;
        }

        // GET: Reservationevents
        public async Task<IActionResult> Index()
        {
            var modelContext = _context.Reservationevents.Include(r => r.Customer).Include(r => r.Event);
            return View(await modelContext.ToListAsync());
        }

        // GET: Reservationevents/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null || _context.Reservationevents == null)
            {
                return NotFound();
            }

            var reservationevent = await _context.Reservationevents
                .Include(r => r.Customer)
                .Include(r => r.Event)
                .FirstOrDefaultAsync(m => m.Reservationid == id);
            if (reservationevent == null)
            {
                return NotFound();
            }

            return View(reservationevent);
        }

        // GET: Reservationevents/Create
        public IActionResult Create()
        {
            ViewData["Customerid"] = new SelectList(_context.Customers, "Customerid", "Customerid");
            ViewData["Eventid"] = new SelectList(_context.Events, "Eventid", "Eventid");
            return View();
        }

        // POST: Reservationevents/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Reservationid,Customerid,Eventid,Reservationdate,Checkindate,Checkoutdate,Paymentstatus")] Reservationevent reservationevent)
        {
            if (ModelState.IsValid)
            {
                _context.Add(reservationevent);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Customerid"] = new SelectList(_context.Customers, "Customerid", "Customerid", reservationevent.Customerid);
            ViewData["Eventid"] = new SelectList(_context.Events, "Eventid", "Eventid", reservationevent.Eventid);
            return View(reservationevent);
        }

        // GET: Reservationevents/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null || _context.Reservationevents == null)
            {
                return NotFound();
            }

            var reservationevent = await _context.Reservationevents.FindAsync(id);
            if (reservationevent == null)
            {
                return NotFound();
            }
            ViewData["Customerid"] = new SelectList(_context.Customers, "Customerid", "Customerid", reservationevent.Customerid);
            ViewData["Eventid"] = new SelectList(_context.Events, "Eventid", "Eventid", reservationevent.Eventid);
            return View(reservationevent);
        }

        // POST: Reservationevents/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Reservationid,Customerid,Eventid,Reservationdate,Checkindate,Checkoutdate,Paymentstatus")] Reservationevent reservationevent)
        {
            if (id != reservationevent.Reservationid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reservationevent);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReservationeventExists(reservationevent.Reservationid))
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
            ViewData["Customerid"] = new SelectList(_context.Customers, "Customerid", "Customerid", reservationevent.Customerid);
            ViewData["Eventid"] = new SelectList(_context.Events, "Eventid", "Eventid", reservationevent.Eventid);
            return View(reservationevent);
        }

        // GET: Reservationevents/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null || _context.Reservationevents == null)
            {
                return NotFound();
            }

            var reservationevent = await _context.Reservationevents
                .Include(r => r.Customer)
                .Include(r => r.Event)
                .FirstOrDefaultAsync(m => m.Reservationid == id);
            if (reservationevent == null)
            {
                return NotFound();
            }

            return View(reservationevent);
        }

        // POST: Reservationevents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.Reservationevents == null)
            {
                return Problem("Entity set 'ModelContext.Reservationevents'  is null.");
            }
            var reservationevent = await _context.Reservationevents.FindAsync(id);
            if (reservationevent != null)
            {
                _context.Reservationevents.Remove(reservationevent);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReservationeventExists(decimal id)
        {
          return (_context.Reservationevents?.Any(e => e.Reservationid == id)).GetValueOrDefault();
        }
    }
}
