using Dio.CatalogoJogos.Api.Business.Entities;
using Dio.CatalogoJogos.Api.Business.Entities.Composites;
using Dio.CatalogoJogos.Api.Business.Entities.Named;
using Dio.CatalogoJogos.Api.Infrastructure.Data.Mappings;
using Dio.CatalogoJogos.Api.Infrastructure.Data.Mappings.Composites;
using Microsoft.EntityFrameworkCore;

namespace Dio.CatalogoJogos.Api.Data.Infrastructure
{
    public class CatalogoJogosDbContext : DbContext
    {
        public virtual DbSet<Jogo> Jogos { get; set; }
        public virtual DbSet<Produtora> Produtoras { get; set; }
        public virtual DbSet<Usuario> Usuarios { get; set; }
        public virtual DbSet<UsuarioJogo> UsuarioJogos { get; set; }

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
