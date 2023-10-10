namespace Hastane.Models
{
    public class Randevu
    {
        public int RandevuId { get; set; }
        public int DoktorID { get; set; }
        public DateTime Tarih { get; set; }
        public string AdSoyad { get; set; }
        public string Telefon { get; set; }
        public string TcKimlik { get; set; }
        public int DoktorSaatId { get; set; }

        public virtual Doktor? Doktor { get; set; }
       
    }
}
