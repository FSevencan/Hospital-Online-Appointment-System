using Hastane.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.Design;

namespace Hastane.Controllers
{
	public class TıbbiBirimlerController : Controller
	{
		private readonly ApplicationDbContext _context;

		public TıbbiBirimlerController(ApplicationDbContext context)
		{
			_context = context;
		}


		public async Task<IActionResult> Index()
		{
			ViewBag.TıbbiBirimler = "active";
			var tıbbiBirimler = _context.Departmanlar;
			return View(await tıbbiBirimler.ToListAsync());
		}

		public ActionResult TıbbiBirimDetay(int id) 
		{ 
			var tıbbidt = _context.Departmanlar.Where(d => d.DepartmanId == id).ToList();

			 ViewData["Hekimlerimiz"] = _context.Doktorlar.Where(d => d.Departman.DepartmanId == id).ToList();

			return View(tıbbidt); 
		}

	}
}
