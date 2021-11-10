using ApiCatalogoJogos.Business.Entities.Composites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiCatalogoJogos.Infrastructure.Data.Mappings.Composites
{
    public class UsuarioJogoMapping : IEntityTypeConfiguration<UsuarioJogo>
    {
        public void Configure(EntityTypeBuilder<UsuarioJogo> builder)
        {
            builder.ToTable("TB_USUARIOJOGO");
            builder.HasKey(p => p.Id);
            builder.HasOne(p => p.Usuario)
                .WithMany(u => u.UsuarioJogos)
                .HasForeignKey(p => p.UsuarioId);
            builder.HasOne(p => p.Jogo)
                .WithMany(j => j.UsuarioJogos)
                .HasForeignKey(p => p.JogoId);
        }
    }
}
