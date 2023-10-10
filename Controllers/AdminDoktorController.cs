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
using Hastane.ViewModels;

namespace Hastane.Controllers
{
    //[Authorize(Roles = "Admin")]
    public class AdminDoktorController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public AdminDoktorController(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // GET: AdminDoktor
        public async Task<IActionResult> Index(string departmanAdi, string aramaMetni)
        {
            ViewBag.Doktor = "active";

            var kategoriler = _context.Departmanlar;
            // var kategoriAdlari = (from satir in _context.Kategoriler order by satir.Ad select satir.Ad);

            // tüm doktorları filtresiz getirecek sorgu
            var doktorlar = _context.Doktorlar.Select(k => k);

            // arama metni var ise where kriteri kullanir
            if (!string.IsNullOrEmpty(aramaMetni))
            {
                doktorlar = doktorlar.Where(doktor => doktor.AdSoyad!.Contains(aramaMetni));
            }

            // aramada departman secilmis ise where kriterine ek yapilir
            if (!string.IsNullOrEmpty(departmanAdi))
            {
                doktorlar = doktorlar.Where(doktor => doktor.Departman.DepartmanAdı == departmanAdi);
            }

            // view e teslim edilecek model hafizaya cikar.
            DoktorDepartmanFiltreViewModel modelim = new DoktorDepartmanFiltreViewModel()
            {
                Departmanlar = new SelectList(await kategoriler.ToListAsync(), "DepartmanAdı", "DepartmanAdı"),
                Doktorlar = await doktorlar.Include(d => d.Departman).ToListAsync()
            };


            return View(modelim);
         
        }

        

        // GET: AdminDoktor/Create
        public IActionResult Create()
        {
            ViewData["DepartmanId"] = new SelectList(_context.Departmanlar, "DepartmanId", "DepartmanAdı");
            return View();
        }

        // POST: AdminDoktor/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DoktorID,AdSoyad,DoktorFotograf,DoktorAcıklama,DepartmanId")] Doktor doktor , IFormFile DoktorFotograf)
        {
            if (ModelState.IsValid)
            {
                var photoName = Guid.NewGuid().ToString()
                + new Random().Next(0, 1000)
                + System.IO.Path.GetExtension(DoktorFotograf.FileName);

                using (var stream = new FileStream(
                           Path.Combine(_environment.WebRootPath, "img/", photoName),
                           FileMode.Create))
                {
                    await DoktorFotograf.CopyToAsync(stream);

                    doktor.DoktorFotograf = photoName;
                }



                _context.Add(doktor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DepartmanId"] = new SelectList(_context.Departmanlar, "DepartmanId", "DepartmanId", doktor.DepartmanId);
            return View(doktor);
        }

        // GET: AdminDoktor/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Doktorlar == null)
            {
                return NotFound();
            }

            var doktor = await _context.Doktorlar.FindAsync(id);
            if (doktor == null)
            {
                return NotFound();
            }
            ViewData["DepartmanId"] = new SelectList(_context.Departmanlar, "DepartmanId", "DepartmanAdı", doktor.DepartmanId);
            return View(doktor);
        }

        // POST: AdminDoktor/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DoktorID,AdSoyad,DoktorFotograf,DoktorAcıklama,DepartmanId")] Doktor doktor , IFormFile? file)
        {
            if (id != doktor.DoktorID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {

                    if (file != null)
                    {

                        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);


                        using (FileStream stream = new FileStream(Path.Combine(_environment.WebRootPath, "img/", fileName), FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }

                        string delete = Path.Combine(_environment.WebRootPath, "img/", doktor.DoktorFotograf);
                        System.IO.File.Delete(delete);


                        doktor.DoktorFotograf = fileName;
                    }


                    _context.Update(doktor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DoktorExists(doktor.DoktorID))
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
            ViewData["DepartmanId"] = new SelectList(_context.Departmanlar, "DepartmanId", "DepartmanId", doktor.DepartmanId);
            return View(doktor);
        }

        // GET: AdminDoktor/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Doktorlar == null)
            {
                return NotFound();
            }

            var doktor = await _context.Doktorlar
                .Include(d => d.Departman)
                .FirstOrDefaultAsync(m => m.DoktorID == id);
            if (doktor == null)
            {
                return NotFound();
            }

            return View(doktor);
        }

        // POST: AdminDoktor/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Doktorlar == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Doktorlar'  is null.");
            }
            var doktor = await _context.Doktorlar.FindAsync(id);
            if (doktor != null)
            {
                _context.Doktorlar.Remove(doktor);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DoktorExists(int id)
        {
          return (_context.Doktorlar?.Any(e => e.DoktorID == id)).GetValueOrDefault();
        }
    }
}
