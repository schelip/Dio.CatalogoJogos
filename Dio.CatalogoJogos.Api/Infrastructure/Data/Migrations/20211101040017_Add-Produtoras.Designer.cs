﻿// <auto-generated />
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
    [Migration("20211101040017_Add-Produtoras")]
    partial class AddProdutoras
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.11")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Dio.CatalogoJogos.Api.Business.Entities.Jogo", b =>
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

                    b.HasKey("Id");

                    b.HasIndex("ProdutoraId");

                    b.ToTable("TB_JOGO");
                });

            modelBuilder.Entity("Dio.CatalogoJogos.Api.Business.Entities.Produtora", b =>
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

            modelBuilder.Entity("Dio.CatalogoJogos.Api.Business.Entities.Jogo", b =>
                {
                    b.HasOne("Dio.CatalogoJogos.Api.Business.Entities.Produtora", "Produtora")
                        .WithMany()
                        .HasForeignKey("ProdutoraId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Produtora");
                });

            modelBuilder.Entity("Dio.CatalogoJogos.Api.Business.Entities.Produtora", b =>
                {
                    b.HasOne("Dio.CatalogoJogos.Api.Business.Entities.Produtora", "ProdutoraMae")
                        .WithMany()
                        .HasForeignKey("ProdutoraMaeId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.Navigation("ProdutoraMae");
                });
#pragma warning restore 612, 618
        }
    }
}