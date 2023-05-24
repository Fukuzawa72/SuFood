using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SuFood.Models;

namespace SuFood.Areas.BackStage.Controllers
{
    [Area("BackStage")]
    public class AnnouncementsController : Controller
    {
        private readonly SuFoodDBContext _context;

        public AnnouncementsController(SuFoodDBContext context)
        {
            _context = context;
        }

        // GET: BackStage/Announcements
        public async Task<IActionResult> Index()
        {
            var suFoodDBContext = _context.Announcement.Include(a => a.Account);
            return View(await suFoodDBContext.ToListAsync());
        }

        // GET: BackStage/Announcements/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Announcement == null)
            {
                return NotFound();
            }

            var announcement = await _context.Announcement
                .Include(a => a.Account)
                .FirstOrDefaultAsync(m => m.AnnouncementId == id);
            if (announcement == null)
            {
                return NotFound();
            }

            return View(announcement);
        }

        // GET: BackStage/Announcements/Create
        public IActionResult Create()
        {
            ViewData["AccountId"] = new SelectList(_context.Account, "AccountId", "Account1");
            return View();
        }

        // POST: BackStage/Announcements/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AnnouncementId,AnnouncementContent,AnnouncementStatus,AnnouncementStartDate,AnnouncementEndDate,AnnouncementImage,AccountId")] Announcement announcement)
        {
            if (ModelState.IsValid)
            {
                _context.Add(announcement);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AccountId"] = new SelectList(_context.Account, "AccountId", "Account1", announcement.AccountId);
            return View(announcement);
        }

        // GET: BackStage/Announcements/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Announcement == null)
            {
                return NotFound();
            }

            var announcement = await _context.Announcement.FindAsync(id);
            if (announcement == null)
            {
                return NotFound();
            }
            ViewData["AccountId"] = new SelectList(_context.Account, "AccountId", "Account1", announcement.AccountId);
            return View(announcement);
        }

        // POST: BackStage/Announcements/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AnnouncementId,AnnouncementContent,AnnouncementStatus,AnnouncementStartDate,AnnouncementEndDate,AnnouncementImage,AccountId")] Announcement announcement)
        {
            if (id != announcement.AnnouncementId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(announcement);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AnnouncementExists(announcement.AnnouncementId))
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
            ViewData["AccountId"] = new SelectList(_context.Account, "AccountId", "Account1", announcement.AccountId);
            return View(announcement);
        }

        // GET: BackStage/Announcements/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Announcement == null)
            {
                return NotFound();
            }

            var announcement = await _context.Announcement
                .Include(a => a.Account)
                .FirstOrDefaultAsync(m => m.AnnouncementId == id);
            if (announcement == null)
            {
                return NotFound();
            }

            return View(announcement);
        }

        // POST: BackStage/Announcements/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Announcement == null)
            {
                return Problem("Entity set 'SuFoodDBContext.Announcement'  is null.");
            }
            var announcement = await _context.Announcement.FindAsync(id);
            if (announcement != null)
            {
                _context.Announcement.Remove(announcement);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AnnouncementExists(int id)
        {
          return (_context.Announcement?.Any(e => e.AnnouncementId == id)).GetValueOrDefault();
        }
    }
}
