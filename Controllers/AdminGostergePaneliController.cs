using Hastane.Data;
using Hastane.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hastane.Controllers
{
    //[Authorize(Roles = "Admin")]
    public class AdminGostergePaneliController : Controller
    {
        private readonly ApplicationDbContext _context;
        CokluListeleme _ck = new CokluListeleme();

        public AdminGostergePaneliController(ApplicationDbContext context)
        {
            _context = context;
        }
      
        public IActionResult Index()
        {
            ViewBag.GostergePaneli = "active";

            _ck.Randevuler = _context.Randevular.ToList();
            _ck.Doktorlar = _context.Doktorlar.ToList();
            _ck.Departmanlar = _context.Departmanlar.ToList();
            _ck.RandevuTakvimleri = _context.RandevuTakvimleri.ToList();

            return View(_ck);
        }
    }
}
