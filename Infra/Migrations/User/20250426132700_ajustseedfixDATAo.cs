using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infra.Migrations.User
{
    /// <inheritdoc />
    public partial class ajustseedfixDATAo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Usuario",
                keyColumn: "id",
                keyValue: new Guid("457f82ef-d838-48ec-8234-fa9df6249f0c"));

            migrationBuilder.DeleteData(
                table: "Usuario",
                keyColumn: "id",
                keyValue: new Guid("4bca7073-6a9e-4825-a218-59635e787148"));

            migrationBuilder.InsertData(
                table: "Usuario",
                columns: new[] { "id", "CPF", "Cargo", "CreatedAt", "DeletedAt", "email", "nome_completo", "nome_usuario", "Permissoes", "senha", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), "00000000000", 1, new DateTime(2025, 4, 25, 21, 0, 0, 0, DateTimeKind.Unspecified), null, "superadmin@exemplo.com", "Super Admin", "superadmin", 63, "$2a$06$qzBEXhEPIkjOSHZcY1u6Cu7gJ88U9.8tGtyS5g1RfKpqg4AWbsk6i", new DateTime(2025, 4, 25, 21, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("22222222-2222-2222-2222-222222222222"), "11111111111", 3, new DateTime(2025, 4, 25, 21, 0, 0, 0, DateTimeKind.Unspecified), null, "usuario@exemplo.com", "Usuário Comum", "usuario", 8, "$2a$06$qzBEXhEPIkjOSHZcY1u6Cu7gJ88U9.8tGtyS5g1RfKpqg4AWbsk6i", new DateTime(2025, 4, 25, 21, 0, 0, 0, DateTimeKind.Unspecified) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Usuario",
                keyColumn: "id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"));

            migrationBuilder.DeleteData(
                table: "Usuario",
                keyColumn: "id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"));

            migrationBuilder.InsertData(
                table: "Usuario",
                columns: new[] { "id", "CPF", "Cargo", "CreatedAt", "DeletedAt", "email", "nome_completo", "nome_usuario", "Permissoes", "senha", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("457f82ef-d838-48ec-8234-fa9df6249f0c"), "11111111111", 3, new DateTime(2025, 4, 26, 10, 21, 45, 615, DateTimeKind.Unspecified).AddTicks(88), null, "usuario@exemplo.com", "Usuário Comum", "usuario", 8, "$2a$06$qzBEXhEPIkjOSHZcY1u6Cu7gJ88U9.8tGtyS5g1RfKpqg4AWbsk6i", new DateTime(2025, 4, 26, 10, 21, 45, 615, DateTimeKind.Unspecified).AddTicks(95) },
                    { new Guid("4bca7073-6a9e-4825-a218-59635e787148"), "00000000000", 1, new DateTime(2025, 4, 26, 10, 21, 45, 613, DateTimeKind.Unspecified).AddTicks(4864), null, "superadmin@exemplo.com", "Super Admin", "superadmin", 63, "$2a$06$qzBEXhEPIkjOSHZcY1u6Cu7gJ88U9.8tGtyS5g1RfKpqg4AWbsk6i", new DateTime(2025, 4, 26, 10, 21, 45, 614, DateTimeKind.Unspecified).AddTicks(8969) }
                });
        }
    }
}
