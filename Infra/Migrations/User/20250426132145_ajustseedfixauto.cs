using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infra.Migrations.User
{
    /// <inheritdoc />
    public partial class ajustseedfixauto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:pgcrypto", ",,");

            migrationBuilder.CreateTable(
                name: "Usuario",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    nome_completo = table.Column<string>(type: "text", nullable: false),
                    nome_usuario = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    senha = table.Column<string>(type: "text", nullable: true),
                    email = table.Column<string>(type: "text", nullable: true),
                    CPF = table.Column<string>(type: "text", nullable: true),
                    Cargo = table.Column<int>(type: "integer", nullable: false),
                    Permissoes = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuario", x => x.id);
                });

            migrationBuilder.InsertData(
                table: "Usuario",
                columns: new[] { "id", "CPF", "Cargo", "CreatedAt", "DeletedAt", "email", "nome_completo", "nome_usuario", "Permissoes", "senha", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("457f82ef-d838-48ec-8234-fa9df6249f0c"), "11111111111", 3, new DateTime(2025, 4, 26, 10, 21, 45, 615, DateTimeKind.Unspecified).AddTicks(88), null, "usuario@exemplo.com", "Usuário Comum", "usuario", 8, "$2a$06$qzBEXhEPIkjOSHZcY1u6Cu7gJ88U9.8tGtyS5g1RfKpqg4AWbsk6i", new DateTime(2025, 4, 26, 10, 21, 45, 615, DateTimeKind.Unspecified).AddTicks(95) },
                    { new Guid("4bca7073-6a9e-4825-a218-59635e787148"), "00000000000", 1, new DateTime(2025, 4, 26, 10, 21, 45, 613, DateTimeKind.Unspecified).AddTicks(4864), null, "superadmin@exemplo.com", "Super Admin", "superadmin", 63, "$2a$06$qzBEXhEPIkjOSHZcY1u6Cu7gJ88U9.8tGtyS5g1RfKpqg4AWbsk6i", new DateTime(2025, 4, 26, 10, 21, 45, 614, DateTimeKind.Unspecified).AddTicks(8969) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Usuario");
        }
    }
}
