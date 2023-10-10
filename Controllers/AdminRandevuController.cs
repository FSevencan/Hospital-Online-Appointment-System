using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Hastane.Data;
using Hastane.Models;
using Hastane.ViewModels;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace Hastane.Controllers
{
    //[Authorize(Roles = "Admin")]
    public class AdminRandevuController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminRandevuController(ApplicationDbContext context)
        {
            _context = context;
        }


		public async Task<IActionResult> Index(string departmanAdi, string aramaMetni)
		{
			ViewBag.Randevu = "active";

			// veri tabanından tüm departman bilgilerini getirecek sorgu.
			var kategoriler = _context.Departmanlar;

			var doktorlar = _context.Doktorlar;
			// tüm doktorları filtresiz getirecek sorgu

			// randevuları bugünün tarihinden sonrakilere filtreleyecek sorgu
            // OrderBy ile önce tarih sonra ThenBy ile en yakın saati siraladık
			var randevular = _context.Randevular
								.Where(randevu => randevu.Tarih >= DateTime.Today).OrderBy(r => r.Tarih)
	                            .ThenBy(r => r.DoktorSaatId)
								.Select(r => r);

			// arama metni var ise where kriteri kullanılır
			if (!string.IsNullOrEmpty(aramaMetni))
			{
				randevular = randevular.Where(randevu => randevu.Doktor.AdSoyad.Contains(aramaMetni));
			}

			// aramada departman secilmis ise where kriterine ek yapilir
			if (!string.IsNullOrEmpty(departmanAdi))
			{
				randevular = randevular.Where(randevu => randevu.Doktor.Departman.DepartmanAdı == departmanAdi);
			}

			// view e teslim edilecek model hafizaya cikar.
			DoktorDepartmanFiltreViewModel modelim = new DoktorDepartmanFiltreViewModel()
			{
				Randevular = await randevular.Include(r => r.Doktor).Include(d => d.Doktor.Departman).ToListAsync(),
				Departmanlar = new SelectList(await kategoriler.ToListAsync(), "DepartmanAdı", "DepartmanAdı"),
				Doktorlar = await doktorlar.Include(d => d.Departman).ToListAsync()
			};

			return View(modelim);
		}





		public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Randevular == null)
            {
                return NotFound();
            }

            var randevu = await _context.Randevular
                .Include(r => r.Doktor)
                .FirstOrDefaultAsync(m => m.RandevuId == id);
            if (randevu == null)
            {
                return NotFound();
            }

            return View(randevu);
        }

        // POST: AdminRandevu/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Randevular == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Randevular'  is null.");
            }
            var randevu = await _context.Randevular.FindAsync(id);
            if (randevu != null)
            {
                _context.Randevular.Remove(randevu);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RandevuExists(int id)
        {
          return (_context.Randevular?.Any(e => e.RandevuId == id)).GetValueOrDefault();
        }
    }
}
