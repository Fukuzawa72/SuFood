using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SuFood.Models;

namespace SuFood.Controllers
{
    public class HelpChoiceController : Controller
    {
        private readonly SuFoodDBContext _context;

        public HelpChoiceController(SuFoodDBContext context)
        {
            _context = context;
        }

        // GET: SubscribeLists
        public async Task<IActionResult> Index()
        {
            return View();
        }
        public IActionResult HelpChoice()
        {
            return View();
        }

        // GET: SubscribeLists/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.SubscribeList == null)
            {
                return NotFound();
            }

            var subscribeList = await _context.SubscribeList
                .Include(s => s.Product)
                .FirstOrDefaultAsync(m => m.SubscribeId == id);
            if (subscribeList == null)
            {
                return NotFound();
            }

            return View(subscribeList);
        }

        // GET: SubscribeLists/Create
        public IActionResult Create()
        {
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductDescription");
            return View();
        }

        // POST: SubscribeLists/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SubscribeId,ProductId")] SubscribeList subscribeList)
        {
            if (ModelState.IsValid)
            {
                _context.Add(subscribeList);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductDescription", subscribeList.ProductId);
            return View(subscribeList);
        }

        // GET: SubscribeLists/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.SubscribeList == null)
            {
                return NotFound();
            }

            var subscribeList = await _context.SubscribeList.FindAsync(id);
            if (subscribeList == null)
            {
                return NotFound();
            }
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductDescription", subscribeList.ProductId);
            return View(subscribeList);
        }

        // POST: SubscribeLists/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SubscribeId,ProductId")] SubscribeList subscribeList)
        {
            if (id != subscribeList.SubscribeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(subscribeList);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SubscribeListExists(subscribeList.SubscribeId))
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
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductDescription", subscribeList.ProductId);
            return View(subscribeList);
        }

        // GET: SubscribeLists/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.SubscribeList == null)
            {
                return NotFound();
            }

            var subscribeList = await _context.SubscribeList
                .Include(s => s.Product)
                .FirstOrDefaultAsync(m => m.SubscribeId == id);
            if (subscribeList == null)
            {
                return NotFound();
            }

            return View(subscribeList);
        }

        // POST: SubscribeLists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.SubscribeList == null)
            {
                return Problem("Entity set 'SuFoodDBContext.SubscribeList'  is null.");
            }
            var subscribeList = await _context.SubscribeList.FindAsync(id);
            if (subscribeList != null)
            {
                _context.SubscribeList.Remove(subscribeList);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SubscribeListExists(int id)
        {
          return (_context.SubscribeList?.Any(e => e.SubscribeId == id)).GetValueOrDefault();
        }
    }
}
