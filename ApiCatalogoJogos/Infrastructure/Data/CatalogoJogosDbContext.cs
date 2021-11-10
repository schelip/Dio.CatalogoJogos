using ApiCatalogoJogos.Business.Entities;
using ApiCatalogoJogos.Business.Entities.Composites;
using ApiCatalogoJogos.Business.Entities.Named;
using ApiCatalogoJogos.Infrastructure.Data.Mappings;
using ApiCatalogoJogos.Infrastructure.Data.Mappings.Composites;
using Microsoft.EntityFrameworkCore;

namespace ApiCatalogoJogos.Data.Infrastructure
{
    public class CatalogoJogosDbContext : DbContext
    {
        public DbSet<Jogo> Jogos { get; set; }
        public DbSet<Produtora> Produtoras { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<UsuarioJogo> UsuarioJogos { get; set; }

        public CatalogoJogosDbContext(DbContextOptions<CatalogoJogosDbContext> options)
            : base(options)
        {}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new JogoMapping());
            modelBuilder.ApplyConfiguration(new ProdutoraMapping());
            modelBuilder.ApplyConfiguration(new UsuarioMapping());
            modelBuilder.ApplyConfiguration(new UsuarioJogoMapping());
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<T> GetDbSet<T>() where T: EntityBase
        {
            var properties = GetType().GetProperties();
            var targetType = typeof(DbSet<>).MakeGenericType(typeof(T));
            foreach (var property in properties)
            {
                if (property.PropertyType == targetType)
                    return (DbSet<T>)property.GetValue(this);
            }
            return null;
        }
    }
}
