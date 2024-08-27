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
    public class ExpenseamountsController : Controller
    {
        private readonly ModelContext _context;

        public ExpenseamountsController(ModelContext context)
        {
            _context = context;
        }

        // GET: Expenseamounts
        public async Task<IActionResult> Index()
        {
            var id = HttpContext.Session.GetInt32("AdminID");
            var users = _context.Customers.Where(x => x.Customerid == id).SingleOrDefault();
            ViewBag.name = users.Customername;
            ViewBag.image = users.Profileimage;
            ViewBag.email = users.Email;
            return _context.Expenseamounts != null ? 
                          View(await _context.Expenseamounts.ToListAsync()) :
                          Problem("Entity set 'ModelContext.Expenseamounts'  is null.");
        }

        // GET: Expenseamounts/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null || _context.Expenseamounts == null)
            {
                return NotFound();
            }

            var expenseamount = await _context.Expenseamounts
                .FirstOrDefaultAsync(m => m.Expenseid == id);
            if (expenseamount == null)
            {
                return NotFound();
            }

            return View(expenseamount);
        }

        // GET: Expenseamounts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Expenseamounts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Expenseid,Expensetype,Amount,Expensedate")] Expenseamount expenseamount)
        {
            if (ModelState.IsValid)
            {
                _context.Add(expenseamount);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(expenseamount);
        }

        // GET: Expenseamounts/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null || _context.Expenseamounts == null)
            {
                return NotFound();
            }

            var expenseamount = await _context.Expenseamounts.FindAsync(id);
            if (expenseamount == null)
            {
                return NotFound();
            }
            return View(expenseamount);
        }

        // POST: Expenseamounts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Expenseid,Expensetype,Amount,Expensedate")] Expenseamount expenseamount)
        {
            if (id != expenseamount.Expenseid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(expenseamount);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExpenseamountExists(expenseamount.Expenseid))
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
            return View(expenseamount);
        }

        // GET: Expenseamounts/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null || _context.Expenseamounts == null)
            {
                return NotFound();
            }

            var expenseamount = await _context.Expenseamounts
                .FirstOrDefaultAsync(m => m.Expenseid == id);
            if (expenseamount == null)
            {
                return NotFound();
            }

            return View(expenseamount);
        }

        // POST: Expenseamounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.Expenseamounts == null)
            {
                return Problem("Entity set 'ModelContext.Expenseamounts'  is null.");
            }
            var expenseamount = await _context.Expenseamounts.FindAsync(id);
            if (expenseamount != null)
            {
                _context.Expenseamounts.Remove(expenseamount);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ExpenseamountExists(decimal id)
        {
          return (_context.Expenseamounts?.Any(e => e.Expenseid == id)).GetValueOrDefault();
        }
    }
}
