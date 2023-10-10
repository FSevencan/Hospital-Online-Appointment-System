using Hastane.Models;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Hastanee.ViewModels
{
    public class RandevuFormuViewModel
    {
        public List<Departman> Departmanlar { get; set; } = new List<Departman>();
        public int SeciliKategoriId { get; set; }
        public List<Doktor>? Doktorlar { get; set; }
        public int SeciliDoktorId { get; set; }
        public List<RandevuTakvimi>? BosSaatler { get; set; }

        [Display(Name = "Tarih")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? SeciliTarih { get; set; }

        public string AdSoyad { get; set; }
        public string Telefon { get; set; }
        public string TcKimlik { get; set; }
        public int SeciliSaatId { get; set; }
    }
}
