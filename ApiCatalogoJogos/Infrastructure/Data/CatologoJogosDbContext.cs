using ApiCatalogoJogos.Business.Entities;
using ApiCatalogoJogos.Infrastructure.Data.Mappings;
using Microsoft.EntityFrameworkCore;

namespace ApiCatalogoJogos.Data.Infrastructure
{
    public class CatalogoJogosDbContext : DbContext
    {
        public DbSet<Jogo> Jogos { get; set; }

        public CatalogoJogosDbContext(DbContextOptions<CatalogoJogosDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new JogoMapping());
            base.OnModelCreating(modelBuilder);
        }
    }
}
