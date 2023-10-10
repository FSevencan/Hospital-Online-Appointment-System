using Hastane.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Drawing;

namespace Hastane.ViewModels
{
    public class DoktorDepartmanFiltreViewModel
    {
        public List<Doktor>? Doktorlar{ get; set; }
        public List<Randevu>? Randevular { get; set; }
        public List<RandevuTakvimi>? RandevuTakvimleri { get; set; }
        public SelectList? Departmanlar { get; set; }
        public string? DepartmanAdi { get; set; }
        public string? AramaMetni { get; set; }
    }
}
