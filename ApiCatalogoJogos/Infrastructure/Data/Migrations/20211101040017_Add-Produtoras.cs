using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ApiCatalogoJogos.Migrations
{
    public partial class AddProdutoras : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Produtora",
                table: "TB_JOGO");

            migrationBuilder.AddColumn<Guid>(
                name: "ProdutoraId",
                table: "TB_JOGO",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "TB_PRODUTORA",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ISOPais = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProdutoraMaeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_PRODUTORA", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TB_PRODUTORA_TB_PRODUTORA_ProdutoraMaeId",
                        column: x => x.ProdutoraMaeId,
                        principalTable: "TB_PRODUTORA",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_TB_JOGO_ProdutoraId",
                table: "TB_JOGO",
                column: "ProdutoraId");

            migrationBuilder.CreateIndex(
                name: "IX_TB_PRODUTORA_ProdutoraMaeId",
                table: "TB_PRODUTORA",
                column: "ProdutoraMaeId");

            migrationBuilder.AddForeignKey(
                name: "FK_TB_JOGO_TB_PRODUTORA_ProdutoraId",
                table: "TB_JOGO",
                column: "ProdutoraId",
                principalTable: "TB_PRODUTORA",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TB_JOGO_TB_PRODUTORA_ProdutoraId",
                table: "TB_JOGO");

            migrationBuilder.DropTable(
                name: "TB_PRODUTORA");

            migrationBuilder.DropIndex(
                name: "IX_TB_JOGO_ProdutoraId",
                table: "TB_JOGO");

            migrationBuilder.DropColumn(
                name: "ProdutoraId",
                table: "TB_JOGO");

            migrationBuilder.AddColumn<string>(
                name: "Produtora",
                table: "TB_JOGO",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
