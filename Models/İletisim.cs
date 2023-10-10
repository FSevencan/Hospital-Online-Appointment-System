namespace Hastane.Models
{
    public class İletisim
    {
        public int İletisimId { get; set; }
        public string AdSoyad { get; set; }
        public string Mail { get; set; }
        public string Mesaj { get; set; }
        public DateTime? Tarih { get; set; }
        public DateTime? OkunmaTarihi{ get; set; }
    }
}
