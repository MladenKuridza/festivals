using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Mladen_Kuridza.Models
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {

        public DbSet<Festival> Festivali { get; set; }
        public DbSet<Mesto> Mesta { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Festival>().HasData(
                new Festival { Id = 1, Naziv = "Exit", Cena = 11000, Godina = 1997, MestoId = 1 },
                new Festival { Id = 2, Naziv = "Sziget", Cena = 1100, Godina = 2000, MestoId = 2 },
                new Festival { Id = 3, Naziv = "Sea Dance", Cena = 5600, Godina = 2015, MestoId = 3 },
                new Festival { Id = 4, Naziv = "Festival lampiona", Cena = 15600, Godina = 1972, MestoId = 1 },
                new Festival { Id = 5, Naziv = "Grozdjebal", Cena = 1450, Godina = 1965, MestoId = 1 }

            );

            modelBuilder.Entity<Mesto>().HasData(
                new Mesto { Id = 1, Naziv = "Novi Sad", Kod = 21000 },
                new Mesto { Id = 2, Naziv = "Budimpesta", Kod = 15612 },
                new Mesto { Id = 3, Naziv = "Budva", Kod = 12345 }

            );

            base.OnModelCreating(modelBuilder);
        }
    }
}
