using Hastane.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Hastane.Data
{
	public class ApplicationDbContext : IdentityDbContext
	{
		public DbSet<Doktor> Doktorlar { get; set; }
		public DbSet<Randevu> Randevular { get; set; }
		public DbSet<RandevuTakvimi> RandevuTakvimleri { get; set; }
		public DbSet<Departman> Departmanlar { get; set; }
		public DbSet<İletisim> Mesajlar { get; set; }
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
			: base(options)
		{
		}
	}
}