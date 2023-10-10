using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Hastane.Data;
using Hastane.Models;

namespace Hastane.Controllers
{
    public class İletisimController : Controller
    {
        private readonly ApplicationDbContext _context;

        public İletisimController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: İletisim
       
        public IActionResult Index()
        {
            ViewBag.İletisim = "active";

            return View();
        }

        // POST: İletisim/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index([Bind("İletisimId,AdSoyad,Mail,Mesaj,Tarih,OkunmaTarihi")] İletisim İletisim)
        {
            if (ModelState.IsValid)
            {
                İletisim.Tarih = DateTime.Now;
                _context.Add(İletisim);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(TesekkurMesajı));
            }
            return View(İletisim);
        }

        public IActionResult TesekkurMesajı()
        {
            return View();
        }


        private bool İletisimExists(int id)
        {
          return (_context.Mesajlar?.Any(e => e.İletisimId == id)).GetValueOrDefault();
        }
    }
}
