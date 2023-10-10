namespace Hastane.Models
{
    public class RandevuTakvimi
    {
        public int RandevuTakvimiId { get; set; }
        public int DoktorId { get; set; }
     
        public DateTime Tarih { get; set; }
        public bool Saat9 { get; set; }
        public bool Saat10 { get; set; }
        public bool Saat11 { get; set; }
        public bool Saat13 { get; set; }
        public bool Saat14 { get; set; }
        public bool Saat15 { get; set; }
        public bool Saat16 { get; set; }

        public virtual Doktor? Doktor { get; set; }
      
    }
}
