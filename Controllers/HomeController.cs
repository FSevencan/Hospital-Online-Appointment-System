using Hastane.Data;
using Hastane.Models;
using Hastanee.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Globalization;

namespace Hastane.Controllers
{
	public class HomeController : Controller
	{
		private readonly ApplicationDbContext _context;

		public HomeController(ApplicationDbContext context)
		{
			_context = context;
		}


		public IActionResult Index()
		{
			ViewBag.AnaSayfa = "active";
			var viewModel = new RandevuFormuViewModel
			{
				Departmanlar = _context.Departmanlar.ToList(),
				Doktorlar = _context.Doktorlar.ToList(),

			};
			return View(viewModel);
		}

		

		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}