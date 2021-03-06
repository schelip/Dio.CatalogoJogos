// <auto-generated />
using System;
using Dio.CatalogoJogos.Api.Data.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Dio.CatalogoJogos.Api.Migrations
{
    [DbContext(typeof(CatalogoJogosDbContext))]
    [Migration("20211115220402_Add-Usuarios")]
    partial class AddUsuarios
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.11")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Dio.CatalogoJogos.Api.Business.Entities.Composites.UsuarioJogo", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("JogoId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("UsuarioId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("JogoId");

                    b.HasIndex("UsuarioId");

                    b.ToTable("TB_USUARIOJOGO");
                });

            modelBuilder.Entity("Dio.CatalogoJogos.Api.Business.Entities.Named.Jogo", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Ano")
                        .HasColumnType("int");

                    b.Property<string>("Nome")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("ProdutoraId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<float>("Valor")
                        .HasColumnType("real");

                    b.HasKey("Id");

                    b.HasIndex("ProdutoraId");

                    b.ToTable("TB_JOGO");
                });

            modelBuilder.Entity("Dio.CatalogoJogos.Api.Business.Entities.Named.Produtora", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ISOPais")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Nome")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("ProdutoraMaeId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("ProdutoraMaeId");

                    b.ToTable("TB_PRODUTORA");
                });

            modelBuilder.Entity("Dio.CatalogoJogos.Api.Business.Entities.Named.Usuario", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<float>("Fundos")
                        .HasColumnType("real");

                    b.Property<string>("Nome")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Permissao")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SenhaHash")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("TB_USUARIO");
                });

            modelBuilder.Entity("Dio.CatalogoJogos.Api.Business.Entities.Composites.UsuarioJogo", b =>
                {
                    b.HasOne("Dio.CatalogoJogos.Api.Business.Entities.Named.Jogo", "Jogo")
                        .WithMany("UsuarioJogos")
                        .HasForeignKey("JogoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Dio.CatalogoJogos.Api.Business.Entities.Named.Usuario", "Usuario")
                        .WithMany("UsuarioJogos")
                        .HasForeignKey("UsuarioId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Jogo");

                    b.Navigation("Usuario");
                });

            modelBuilder.Entity("Dio.CatalogoJogos.Api.Business.Entities.Named.Jogo", b =>
                {
                    b.HasOne("Dio.CatalogoJogos.Api.Business.Entities.Named.Produtora", "Produtora")
                        .WithMany()
                        .HasForeignKey("ProdutoraId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Produtora");
                });

            modelBuilder.Entity("Dio.CatalogoJogos.Api.Business.Entities.Named.Produtora", b =>
                {
                    b.HasOne("Dio.CatalogoJogos.Api.Business.Entities.Named.Produtora", "ProdutoraMae")
                        .WithMany()
                        .HasForeignKey("ProdutoraMaeId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.Navigation("ProdutoraMae");
                });

            modelBuilder.Entity("Dio.CatalogoJogos.Api.Business.Entities.Named.Jogo", b =>
                {
                    b.Navigation("UsuarioJogos");
                });

            modelBuilder.Entity("Dio.CatalogoJogos.Api.Business.Entities.Named.Usuario", b =>
                {
                    b.Navigation("UsuarioJogos");
                });
#pragma warning restore 612, 618
        }
    }
}
