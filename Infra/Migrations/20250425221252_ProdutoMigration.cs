using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infra.Migrations
{
    /// <inheritdoc />
    public partial class ProdutoMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CaminhoArquivo",
                table: "Produto");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Produto",
                type: "timestamp",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "Produto",
                type: "timestamp",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Produto",
                type: "timestamp",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "ImagemProdutoSet",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Caminho = table.Column<string>(type: "text", nullable: false),
                    ProdutoId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImagemProdutoSet", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ImagemProdutoSet_Produto_ProdutoId",
                        column: x => x.ProdutoId,
                        principalTable: "Produto",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ImagemProdutoSet_ProdutoId",
                table: "ImagemProdutoSet",
                column: "ProdutoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ImagemProdutoSet");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Produto");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Produto");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Produto");

            migrationBuilder.AddColumn<string>(
                name: "CaminhoArquivo",
                table: "Produto",
                type: "text",
                nullable: true);
        }
    }
}
