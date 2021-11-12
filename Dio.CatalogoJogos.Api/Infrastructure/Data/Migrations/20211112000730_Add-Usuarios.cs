using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Dio.CatalogoJogos.Api.Migrations
{
    public partial class AddUsuarios : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "Valor",
                table: "TB_JOGO",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.CreateTable(
                name: "TB_USUARIO",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SenhaHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Fundos = table.Column<float>(type: "real", nullable: false),
                    Permissao = table.Column<int>(type: "int", nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_USUARIO", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TB_USUARIOJOGO",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UsuarioId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    JogoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_USUARIOJOGO", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TB_USUARIOJOGO_TB_JOGO_JogoId",
                        column: x => x.JogoId,
                        principalTable: "TB_JOGO",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TB_USUARIOJOGO_TB_USUARIO_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "TB_USUARIO",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TB_USUARIOJOGO_JogoId",
                table: "TB_USUARIOJOGO",
                column: "JogoId");

            migrationBuilder.CreateIndex(
                name: "IX_TB_USUARIOJOGO_UsuarioId",
                table: "TB_USUARIOJOGO",
                column: "UsuarioId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TB_USUARIOJOGO");

            migrationBuilder.DropTable(
                name: "TB_USUARIO");

            migrationBuilder.DropColumn(
                name: "Valor",
                table: "TB_JOGO");
        }
    }
}
