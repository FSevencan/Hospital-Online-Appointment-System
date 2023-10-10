using Hastane.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hastane.Controllers
{
	public class DoktorController : Controller
	{
		private readonly ApplicationDbContext _context;

		public DoktorController(ApplicationDbContext context)
		{
			_context = context;
		}

        public async Task<IActionResult> Index(string filter = "all")
        {
            ViewBag.Doktor = "active";

            // Departmanları veritabanından çek
            var departmanlar = await _context.Departmanlar.ToListAsync();
            ViewBag.Departmanlar = departmanlar;

            // Doktorları veritabanından çek
            var doktorlar = await _context.Doktorlar.Include(d => d.Departman).ToListAsync();

            // Filtreleme yap
            if (filter != "all")
            {
                doktorlar = doktorlar.Where(d => d.Departman?.DepartmanAdı == filter).ToList();
            }

            ViewBag.Filter = filter;

            return View(doktorlar);
        }

        public async Task<IActionResult> DoktorDetay(int id)
        {
            var doktordt = await _context.Doktorlar.Include(d => d.Departman).Where(d=> d.DoktorID==id).ToListAsync();

            return View(doktordt);      
        }

    }
}
