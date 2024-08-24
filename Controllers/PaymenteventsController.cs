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
    public class PaymenteventsController : Controller
    {
        private readonly ModelContext _context;

        public PaymenteventsController(ModelContext context)
        {
            _context = context;
        }

        // GET: Paymentevents
        public async Task<IActionResult> Index()
        {
            var modelContext = _context.Paymentevents.Include(p => p.Bank).Include(p => p.Reservation);
            return View(await modelContext.ToListAsync());
        }

        // GET: Paymentevents/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null || _context.Paymentevents == null)
            {
                return NotFound();
            }

            var paymentevent = await _context.Paymentevents
                .Include(p => p.Bank)
                .Include(p => p.Reservation)
                .FirstOrDefaultAsync(m => m.Paymentid == id);
            if (paymentevent == null)
            {
                return NotFound();
            }

            return View(paymentevent);
        }

        // GET: Paymentevents/Create
        public IActionResult Create()
        {
            ViewData["Bankid"] = new SelectList(_context.Banks, "Bankid", "Bankid");
            ViewData["Reservationid"] = new SelectList(_context.Reservationevents, "Reservationid", "Reservationid");
            return View();
        }

        // POST: Paymentevents/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Paymentid,Reservationid,Amountpaid,Paymentdate,Bankid")] Paymentevent paymentevent)
        {
            if (ModelState.IsValid)
            {
                _context.Add(paymentevent);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Bankid"] = new SelectList(_context.Banks, "Bankid", "Bankid", paymentevent.Bankid);
            ViewData["Reservationid"] = new SelectList(_context.Reservationevents, "Reservationid", "Reservationid", paymentevent.Reservationid);
            return View(paymentevent);
        }

        // GET: Paymentevents/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null || _context.Paymentevents == null)
            {
                return NotFound();
            }

            var paymentevent = await _context.Paymentevents.FindAsync(id);
            if (paymentevent == null)
            {
                return NotFound();
            }
            ViewData["Bankid"] = new SelectList(_context.Banks, "Bankid", "Bankid", paymentevent.Bankid);
            ViewData["Reservationid"] = new SelectList(_context.Reservationevents, "Reservationid", "Reservationid", paymentevent.Reservationid);
            return View(paymentevent);
        }

        // POST: Paymentevents/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Paymentid,Reservationid,Amountpaid,Paymentdate,Bankid")] Paymentevent paymentevent)
        {
            if (id != paymentevent.Paymentid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(paymentevent);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PaymenteventExists(paymentevent.Paymentid))
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
            ViewData["Bankid"] = new SelectList(_context.Banks, "Bankid", "Bankid", paymentevent.Bankid);
            ViewData["Reservationid"] = new SelectList(_context.Reservationevents, "Reservationid", "Reservationid", paymentevent.Reservationid);
            return View(paymentevent);
        }

        // GET: Paymentevents/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null || _context.Paymentevents == null)
            {
                return NotFound();
            }

            var paymentevent = await _context.Paymentevents
                .Include(p => p.Bank)
                .Include(p => p.Reservation)
                .FirstOrDefaultAsync(m => m.Paymentid == id);
            if (paymentevent == null)
            {
                return NotFound();
            }

            return View(paymentevent);
        }

        // POST: Paymentevents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.Paymentevents == null)
            {
                return Problem("Entity set 'ModelContext.Paymentevents'  is null.");
            }
            var paymentevent = await _context.Paymentevents.FindAsync(id);
            if (paymentevent != null)
            {
                _context.Paymentevents.Remove(paymentevent);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PaymenteventExists(decimal id)
        {
          return (_context.Paymentevents?.Any(e => e.Paymentid == id)).GetValueOrDefault();
        }
    }
}
