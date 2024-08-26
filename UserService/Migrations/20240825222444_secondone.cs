using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace UserService.Migrations
{
    /// <inheritdoc />
    public partial class secondone : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0f832d97-9c8c-4f63-9e3d-c02054d3d0c9");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4acb18fd-32ca-4b4e-8429-7c378a499a69");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c9aa9279-e8e7-4577-9ccc-7e82fafe3fac");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "0f832d97-9c8c-4f63-9e3d-c02054d3d0c9", null, "seller", "seller" },
                    { "4acb18fd-32ca-4b4e-8429-7c378a499a69", null, "client", "client" },
                    { "c9aa9279-e8e7-4577-9ccc-7e82fafe3fac", null, "Admin", "admin" }
                });
        }
    }
}
