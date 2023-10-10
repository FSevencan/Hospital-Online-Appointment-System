namespace Hastane.Models
{
    public class Doktor
    {
        public int DoktorID { get; set; }
        public string AdSoyad { get; set; }
        public string? DoktorFotograf { get; set; }
        public string DoktorAcıklama { get; set; }
       
       
        public List<Randevu>? Randevular { get; set; }

        public int DepartmanId { get; set; }
        public virtual Departman? Departman { get; set; }

    }
}
