using Microsoft.EntityFrameworkCore;
using ProjetoTeste.Models;
using System.Reflection;
using System.Data.SQLite;

namespace ProjetoTeste.CamadaNegocio
{
    public class EfContext : DbContext
    {
        public DbSet<LoginModel> LOGIN { get; set; }
        public DbSet<EmpresaModel> EMPRESA { get; set; }

        protected override void OnConfiguring( DbContextOptionsBuilder optionsBuilder )
        {
            optionsBuilder.UseSqlite( "Filename=DB_TESTE.db", options =>
            {
                options.MigrationsAssembly( Assembly.GetExecutingAssembly().FullName );
            } );
            base.OnConfiguring( optionsBuilder );
        }
        protected override void OnModelCreating( ModelBuilder modelBuilder )
        {
            // Map table names
            modelBuilder.Entity<LoginModel>().ToTable( "LOGIN" );
            modelBuilder.Entity<LoginModel>( entity =>
            {
                entity.HasKey( e => e.Id );
                entity.HasIndex( e => e.Email ).IsUnique();
                entity.HasIndex( e => e.Cnpj ).IsUnique();
                entity.HasIndex( e => e.Usuario );
            } );

            modelBuilder.Entity<EmpresaModel>( entity => {
                entity.OwnsOne( e => e.Billing );

                entity.Navigation( e => e.AtividadePrincipal ).AutoInclude();

                entity.Navigation( e => e.AtividadesSecundarias ).AutoInclude();

                entity.Navigation( e => e.Qsa ).AutoInclude();

                entity.HasMany( e => e.AtividadePrincipal )
                    .WithMany( a => a.EmpresaPrincipal );

                entity.HasMany( e => e.AtividadesSecundarias )
                    .WithMany( a => a.EmpresaSecundario );
             } );
            base.OnModelCreating( modelBuilder );
        }
    }
}
