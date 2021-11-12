using Dio.CatalogoJogos.Api.Business.Entities.Named;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dio.CatalogoJogos.Api.Infrastructure.Data.Mappings
{
    public class JogoMapping : IEntityTypeConfiguration<Jogo>
    {
        public void Configure(EntityTypeBuilder<Jogo> builder)
        {
            builder.ToTable("TB_JOGO");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id);
            builder.Property(p => p.Nome);
            builder.Property(p => p.Ano);
            builder.Property(p => p.Valor);
            builder.Property(p => p.ProdutoraId);
            builder.HasOne(p => p.Produtora)
                .WithMany()
                .IsRequired()
                .HasForeignKey(p => p.ProdutoraId);
        }
    }
}
