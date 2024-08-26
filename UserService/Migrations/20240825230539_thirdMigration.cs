using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace UserService.Migrations
{
    /// <inheritdoc />
    public partial class thirdMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "336c4630-9363-40f3-88cd-0d1db7fb564c");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e59377bf-e456-42c0-b1c3-fefed0f131f9");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f5ba9c5f-de16-43ce-9dc4-6d476256fbbc");

            migrationBuilder.RenameColumn(
                name: "token",
                table: "_refreshTokens",
                newName: "Token");

            migrationBuilder.RenameColumn(
                name: "jwtId",
                table: "_refreshTokens",
                newName: "JwtId");

            migrationBuilder.RenameColumn(
                name: "isUsed",
                table: "_refreshTokens",
                newName: "IsUsed");

            migrationBuilder.RenameColumn(
                name: "isRevorked",
                table: "_refreshTokens",
                newName: "IsRevorked");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "209ae945-b251-4817-bcea-ee0b0256db33", null, "seller", "seller" },
                    { "5286423f-56a3-4193-9150-2354794299cf", null, "client", "client" },
                    { "e7a43f75-0ce0-4143-b521-9fc4b8630d2a", null, "Admin", "admin" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "209ae945-b251-4817-bcea-ee0b0256db33");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5286423f-56a3-4193-9150-2354794299cf");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e7a43f75-0ce0-4143-b521-9fc4b8630d2a");

            migrationBuilder.RenameColumn(
                name: "Token",
                table: "_refreshTokens",
                newName: "token");

            migrationBuilder.RenameColumn(
                name: "JwtId",
                table: "_refreshTokens",
                newName: "jwtId");

            migrationBuilder.RenameColumn(
                name: "IsUsed",
                table: "_refreshTokens",
                newName: "isUsed");

            migrationBuilder.RenameColumn(
                name: "IsRevorked",
                table: "_refreshTokens",
                newName: "isRevorked");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "336c4630-9363-40f3-88cd-0d1db7fb564c", null, "Admin", "admin" },
                    { "e59377bf-e456-42c0-b1c3-fefed0f131f9", null, "client", "client" },
                    { "f5ba9c5f-de16-43ce-9dc4-6d476256fbbc", null, "seller", "seller" }
                });
        }
    }
}
