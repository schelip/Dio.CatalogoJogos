using System;
using ApiCatalogoJogos.Business.Entities.Named;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiCatalogoJogos.Infrastructure.Data.Mappings
{
    public class UsuarioMapping : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.ToTable("TB_USUARIO");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id);
            builder.Property(p => p.Nome);
            builder.Property(p => p.Email);
            builder.Property(p => p.SenhaHash);
            builder.Property(p => p.Fundos);
            builder.Property(p => p.Permissao);
        }
    }
}
