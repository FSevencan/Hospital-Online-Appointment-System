using Microsoft.AspNetCore.Mvc;

namespace Hastane.Controllers
{
	public class HakkımızdaController : Controller
	{
		public IActionResult Index()
		{
			ViewBag.Hakkimizda = "active";
			return View();
		}
	}
}
