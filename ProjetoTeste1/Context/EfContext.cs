using Microsoft.EntityFrameworkCore;
using ProjetoTeste.Models;
using System.Reflection;
using System.Data.SQLite;

namespace ProjetoTeste.CamadaNegocio
{
    public class EfContext : DbContext
    {
        public DbSet<LoginModel> Login { get; set; }
        public DbSet<EmpresaModel> Empresa { get; set; }
        public DbSet<AtividadeModel> Atividades { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=DB_TESTE.db", options =>
           {
               options.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName);
           });
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Map table names
            modelBuilder.Entity<LoginModel>().ToTable("Login");
            modelBuilder.Entity<LoginModel>(entity =>
           {
               entity.HasKey(e => e.Id);
               entity.HasIndex(e => e.Email).IsUnique();
               entity.HasIndex(e => e.Cnpj).IsUnique();
               entity.HasIndex(e => e.Usuario);
           });

            modelBuilder.Entity<EmpresaModel>(entity =>
            {
                entity.OwnsOne(e => e.Billing);

                entity.HasMany(e => e.AtividadePrincipal)
                    .WithMany(a => a.EmpresaPrincipal);

                entity.HasMany(e => e.AtividadesSecundarias)
                   .WithMany(a => a.EmpresaSecundario);

                entity.Navigation( e => e.AtividadePrincipal ).AutoInclude();

                entity.Navigation(e => e.AtividadesSecundarias).AutoInclude();

                entity.Navigation(e => e.Qsa).AutoInclude();
            });
            base.OnModelCreating(modelBuilder);
        }
    }
}
