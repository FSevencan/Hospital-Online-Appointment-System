namespace Hastane.Models
{
    public class CokluListeleme
    {
        public IEnumerable<Randevu> Randevuler { get; set; }
        public IEnumerable<Doktor> Doktorlar { get; set; }
        public IEnumerable<Departman> Departmanlar { get; set; }
        public IEnumerable<RandevuTakvimi> RandevuTakvimleri { get; set; }
    }
}
