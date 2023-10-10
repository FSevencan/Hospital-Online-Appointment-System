using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Hastane.Data;
using Hastane.Models;
using System.Reflection.Metadata;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace Hastane.Controllers
{
    //[Authorize(Roles = "Admin")]
    public class AdminDepartmanController : Controller
    {
        private readonly ApplicationDbContext _context;
		private readonly IWebHostEnvironment _environment;

		public AdminDepartmanController(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
			_environment = environment;
		}

        // GET: AdminDepartman
        public async Task<IActionResult> Index()
        {
            ViewBag.TıbbiBirim = "active";
            return _context.Departmanlar != null ? 
                          View(await _context.Departmanlar.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Departmanlar'  is null.");
        }

       

        // GET: AdminDepartman/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AdminDepartman/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DepartmanId,DepartmanAdı,DepartmanFotograf,DepartmanAcıklama")] Departman departman, IFormFile DepartmanFotograf)
        {
            if (ModelState.IsValid)
            {

			   var photoName = Guid.NewGuid().ToString()
			 + new Random().Next(0, 1000)
			 + System.IO.Path.GetExtension(DepartmanFotograf.FileName);

				using (var stream = new FileStream(
						   Path.Combine(_environment.WebRootPath, "img/", photoName),
						   FileMode.Create))
				{
					await DepartmanFotograf.CopyToAsync(stream);

					departman.DepartmanFotograf = photoName;
				}

				_context.Add(departman);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(departman);
        }

        // GET: AdminDepartman/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Departmanlar == null)
            {
                return NotFound();
            }

            var departman = await _context.Departmanlar.FindAsync(id);
            if (departman == null)
            {
                return NotFound();
            }
            return View(departman);
        }

        // POST: AdminDepartman/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DepartmanId,DepartmanAdı,DepartmanFotograf,DepartmanAcıklama")] Departman departman, IFormFile? file)
        {
            if (id != departman.DepartmanId)
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

                        string delete = Path.Combine(_environment.WebRootPath, "img/", departman.DepartmanFotograf);
                        System.IO.File.Delete(delete);


                        departman.DepartmanFotograf = fileName;
                    }


                    _context.Update(departman);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DepartmanExists(departman.DepartmanId))
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
            return View(departman);
        }

        // GET: AdminDepartman/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Departmanlar == null)
            {
                return NotFound();
            }

            var departman = await _context.Departmanlar
                .FirstOrDefaultAsync(m => m.DepartmanId == id);
            if (departman == null)
            {
                return NotFound();
            }

            return View(departman);
        }

        // POST: AdminDepartman/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Departmanlar == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Departmanlar'  is null.");
            }
            var departman = await _context.Departmanlar.FindAsync(id);
            if (departman != null)
            {
                _context.Departmanlar.Remove(departman);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DepartmanExists(int id)
        {
          return (_context.Departmanlar?.Any(e => e.DepartmanId == id)).GetValueOrDefault();
        }
    }
}
