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
    public class AdminRandevuTakvimiController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminRandevuTakvimiController(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<IActionResult> Index(string departmanAdi, string aramaMetni)
        {
            ViewBag.RandevuTakvimi = "active";


            var kategoriler = _context.Departmanlar;


            var doktorlar = _context.Doktorlar;


            var randevutakvimi = _context.RandevuTakvimleri
                                .Where(randevu => randevu.Tarih >= DateTime.Today).OrderBy(r => r.Doktor.Departman.DepartmanAdı)
                                .ThenBy(r => r.Doktor.AdSoyad).ThenBy(r => r.Tarih)
                                .Select(r => r);


            if (!string.IsNullOrEmpty(aramaMetni))
            {
                randevutakvimi = randevutakvimi.Where(randevu => randevu.Doktor.AdSoyad.Contains(aramaMetni));
            }

            if (!string.IsNullOrEmpty(departmanAdi))
            {
                randevutakvimi = randevutakvimi.Where(randevu => randevu.Doktor.Departman.DepartmanAdı == departmanAdi);
            }


            DoktorDepartmanFiltreViewModel modelim = new DoktorDepartmanFiltreViewModel()
            {
                RandevuTakvimleri = await randevutakvimi.Include(r => r.Doktor).Include(d => d.Doktor.Departman).ToListAsync(),
                Departmanlar = new SelectList(await kategoriler.ToListAsync(), "DepartmanAdı", "DepartmanAdı"),
                Doktorlar = await doktorlar.Include(d => d.Departman).ToListAsync()
            };

            return View(modelim);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewData["DepartmanId"] = new SelectList(_context.Departmanlar, "DepartmanId", "DepartmanAdı");
            ViewData["DoktorId"] = Enumerable.Empty<SelectListItem>();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RandevuTakvimiId,DoktorId,Tarih,Saat9,Saat10,Saat11,Saat13,Saat14,Saat15,Saat16")] RandevuTakvimi randevuTakvimi)
        {
            if (ModelState.IsValid)
            {
                _context.Add(randevuTakvimi);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DoktorId"] = new SelectList(_context.Doktorlar, "DoktorID", "DoktorID", randevuTakvimi.DoktorId);
            return View(randevuTakvimi);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.RandevuTakvimleri == null)
            {
                return NotFound();
            }

            var randevuTakvimi = await _context.RandevuTakvimleri.FindAsync(id);
            if (randevuTakvimi == null)
            {
                return NotFound();
            }
            ViewData["DoktorId"] = new SelectList(_context.Doktorlar, "DoktorID", "AdSoyad", randevuTakvimi.DoktorId);
            return View(randevuTakvimi);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RandevuTakvimiId,DoktorId,Tarih,Saat9,Saat10,Saat11,Saat13,Saat14,Saat15,Saat16")] RandevuTakvimi randevuTakvimi)
        {
            if (id != randevuTakvimi.RandevuTakvimiId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(randevuTakvimi);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RandevuTakvimiExists(randevuTakvimi.RandevuTakvimiId))
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
            ViewData["DoktorId"] = new SelectList(_context.Doktorlar, "DoktorID", "AdSoyad", randevuTakvimi.DoktorId);
            return View(randevuTakvimi);
        }


        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.RandevuTakvimleri == null)
            {
                return NotFound();
            }

            var randevuTakvimi = await _context.RandevuTakvimleri
                .Include(r => r.Doktor)
                .FirstOrDefaultAsync(m => m.RandevuTakvimiId == id);
            if (randevuTakvimi == null)
            {
                return NotFound();
            }

            return View(randevuTakvimi);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.RandevuTakvimleri == null)
            {
                return Problem("Entity set 'ApplicationDbContext.RandevuTakvimleri'  is null.");
            }
            var randevuTakvimi = await _context.RandevuTakvimleri.FindAsync(id);
            if (randevuTakvimi != null)
            {
                _context.RandevuTakvimleri.Remove(randevuTakvimi);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RandevuTakvimiExists(int id)
        {
            return (_context.RandevuTakvimleri?.Any(e => e.RandevuTakvimiId == id)).GetValueOrDefault();
        }


        public JsonResult DepartmanaGoreDoktorGetir(int departmanId)
        {
            var doktorlar = _context.Doktorlar.Where(d => d.DepartmanId == departmanId).Select(d => new { d.DoktorID, d.AdSoyad }).ToList();
            return Json(doktorlar);
        }

    }
}
