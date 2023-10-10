using Hastane.Data;
using Hastane.Models;
using Hastanee.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace Hastane.Controllers
{
	public class RandevuController : Controller
	{
		private readonly ApplicationDbContext _context;

		public RandevuController(ApplicationDbContext context)
		{
			_context = context;
		}

		public IActionResult Index()
		{
			var viewModel = new RandevuFormuViewModel
			{
				Departmanlar = _context.Departmanlar.ToList(),
				Doktorlar = _context.Doktorlar.ToList(),

			};
			return View(viewModel);
		}

		[HttpPost]
		public IActionResult Index(RandevuFormuViewModel viewModel)
		{

			if (ModelState.IsValid)
			{
				// Randevu kaydını yap
				var randevu = new Randevu
				{
					AdSoyad = viewModel.AdSoyad,
					Telefon = viewModel.Telefon,
					TcKimlik = viewModel.TcKimlik,
					DoktorSaatId = viewModel.SeciliSaatId,
					DoktorID = viewModel.SeciliDoktorId,
					Tarih = (DateTime)viewModel.SeciliTarih,

				};
				_context.Randevular.Add(randevu);
				_context.SaveChanges();

				// Başarılı mesajı göster

				TempData["Message"] = "Randevu kaydınız başarılı bir şekilde oluşturuldu!";

				// JSON formatında başarı cevabı gönder
				return Json(new { success = true });
			}
			else
			{
				// Hatalı model durumunda hata mesajını ViewBag veya ViewData ile geçir
				ViewBag.ErrorMessage = "Randevu kaydı yapılamadı, lütfen gerekli alanları doldurunuz.";
				return View(viewModel);
			}
		}

		public IActionResult GetDoktorlar(int departmanId)
		{
			var doktorlar = _context.Doktorlar.Where(d => d.DepartmanId == departmanId).ToList();
			return Json(doktorlar);
		}


        [HttpPost]
        public JsonResult GetDoktorTarihleri(int doktorId)
        {
            var doktorTarihleri = _context.RandevuTakvimleri
                                         .Where(s => s.DoktorId == doktorId)
                                         .Select(s => s.Tarih)
                                         .ToList();

            if (doktorTarihleri.Count == 0)
            {
                return Json(new { success = false, message = "Doktorun müsait olduğu bir tarih bulunamadı." });
            }
            else
            {
                return Json(new { success = true, tarihler = doktorTarihleri });
            }
        }

        [HttpPost]
        public JsonResult GetDoktorSaati(int doktorId, string tarih)
        {
            DateTime selectedDate = DateTime.ParseExact(tarih, "dd.MM.yyyy", CultureInfo.InvariantCulture);

            var doktorSaatler = _context.RandevuTakvimleri
                                        .SingleOrDefault(s => s.DoktorId == doktorId && s.Tarih == selectedDate);

           

            if (doktorSaatler == null)
            {
                var emptySaatler = Enumerable.Range(9, 8)
                                              .ToDictionary(saat => saat, saat => true);
                return Json(new { success = false, message = "*Tüm randevu saatleri dolu", saatler = emptySaatler, selectedTime = "" });
            }

            var saatler = new Dictionary<int, bool>
    {
        { 09, doktorSaatler.Saat9 && !CheckIfRandevuAlindi(doktorId, selectedDate, 9) },
        { 10, doktorSaatler.Saat10 && !CheckIfRandevuAlindi(doktorId, selectedDate, 10) },
        { 11, doktorSaatler.Saat11 && !CheckIfRandevuAlindi(doktorId, selectedDate, 11) },
        { 13, doktorSaatler.Saat13 && !CheckIfRandevuAlindi(doktorId, selectedDate, 13) },
        { 14, doktorSaatler.Saat14 && !CheckIfRandevuAlindi(doktorId, selectedDate, 14) },
        { 15, doktorSaatler.Saat15 && !CheckIfRandevuAlindi(doktorId, selectedDate, 15) },
        { 16, doktorSaatler.Saat16 && !CheckIfRandevuAlindi(doktorId, selectedDate, 16) }
    };

            return Json(new { success = true, saatler });
        }

        // Daha önce alınmış bir randevu olup olmadığını kontrol eden yardımcı bir fonksiyon

        private bool CheckIfRandevuAlindi(int doktorId, DateTime tarih, int saat)
        {
            return _context.Randevular
              .Any(r => r.DoktorID == doktorId && r.Tarih == tarih && r.DoktorSaatId == saat);
        }


    }
}
