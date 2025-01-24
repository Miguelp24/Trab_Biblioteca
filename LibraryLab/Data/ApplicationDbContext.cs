using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using LibraryLab.Models;

namespace LibraryLab.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<LibraryLab.Models.Autor> Autor { get; set; }
        public DbSet<LibraryLab.Models.Genero> Genero { get; set; }
        public DbSet<LibraryLab.Models.Livro> Livro { get; set; }
        public DbSet<LibraryLab.Models.TabelaBiblio> TabelaBiblio { get; set; }
        public DbSet<LibraryLab.Models.Requisitar> Requisitar { get; set; }
        public DbSet<Utilizador> Utilizadores { get; set; }
    }
}
