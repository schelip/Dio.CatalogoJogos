using Dio.CatalogoJogos.Api.Business.Entities.Named;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dio.CatalogoJogos.Api.Infrastructure.Data.Mappings
{
    public class ProdutoraMapping : IEntityTypeConfiguration<Produtora>
    {
        public void Configure(EntityTypeBuilder<Produtora> builder)
        {
            builder.ToTable("TB_PRODUTORA");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id);
            builder.Property(p => p.Nome);
            builder.Property(p => p.ISOPais);
            builder.HasOne(p => p.ProdutoraMae)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
