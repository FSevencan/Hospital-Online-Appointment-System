using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Hastane.Data;
using Hastane.Models;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace Hastane.Controllers
{
    //[Authorize(Roles = "Admin")]
    public class AdminİletisimController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminİletisimController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Adminİletisim
        public async Task<IActionResult> Index()
        {
              return _context.Mesajlar != null ? 
                          View(await _context.Mesajlar.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Mesajlar'  is null.");
        }

        // GET: Adminİletisim/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Mesajlar == null)
            {
                return NotFound();
            }

            

            var İletisim = await _context.Mesajlar
                .FirstOrDefaultAsync(m => m.İletisimId == id);
            if (İletisim == null)
            {
                return NotFound();
            }

            İletisim.OkunmaTarihi = DateTime.Now;
            _context.SaveChanges();

            return View(İletisim);
        }

       
        

        // GET: Adminİletisim/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Mesajlar == null)
            {
                return NotFound();
            }

            var İletisim = await _context.Mesajlar
                .FirstOrDefaultAsync(m => m.İletisimId == id);
            if (İletisim == null)
            {
                return NotFound();
            }

            return View(İletisim);
        }

        // POST: Adminİletisim/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Mesajlar == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Mesajlar'  is null.");
            }
            var İletisim = await _context.Mesajlar.FindAsync(id);
            if (İletisim != null)
            {
                _context.Mesajlar.Remove(İletisim);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool İletisimExists(int id)
        {
          return (_context.Mesajlar?.Any(e => e.İletisimId == id)).GetValueOrDefault();
        }
    }
}
