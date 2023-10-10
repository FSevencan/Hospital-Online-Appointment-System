namespace Hastane.Models
{
    public class Departman
    {
        public int DepartmanId { get; set; }
        public string DepartmanAdı { get; set; }
        public string? DepartmanFotograf { get; set; }
        public string DepartmanAcıklama { get; set; }

        public virtual ICollection<Doktor> Doktorlar { get; set; }
    }
}
