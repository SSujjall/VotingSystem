using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace VotingSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RoleUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ccde8bfa-a615-4637-afe9-930b7d238b4a");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "e67087f0-823d-49f7-a154-57f6bff52b0c", "38c4e0f7-3cb5-45eb-9d37-a8dcb62ba035" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e67087f0-823d-49f7-a154-57f6bff52b0c");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "38c4e0f7-3cb5-45eb-9d37-a8dcb62ba035");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "6f5638dd-b5a2-4f59-8320-cbd32ba2dfa8", "6f5638dd-b5a2-4f59-8320-cbd32ba2dfa8", "Admin", "ADMIN" },
                    { "d671dd32-7f37-4834-83d4-86ba2f684ba5", "d671dd32-7f37-4834-83d4-86ba2f684ba5", "Superadmin", "SUPERADMIN" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Discriminator", "Email", "EmailConfirmed", "FullName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "ea49a16d-f1c1-4300-a858-bee1d6ec3e83", 0, "39cfd89b-26dc-4972-b503-1c473494bedd", "User", "superadmin@voting.com", true, "Superadmin", false, null, "SUPERADMIN@VOTING.COM", "SUPERADMIN", "AQAAAAIAAYagAAAAEFcekGE4P017Ogmu2qDb5OSPs8A5v08u2+1Epiu0XOTMZApMia+VFrjB8/z/CyuWbQ==", null, false, "9989cf87-ed94-4ac3-abe2-78127806a8f2", false, "superadmin" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "d671dd32-7f37-4834-83d4-86ba2f684ba5", "ea49a16d-f1c1-4300-a858-bee1d6ec3e83" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6f5638dd-b5a2-4f59-8320-cbd32ba2dfa8");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "d671dd32-7f37-4834-83d4-86ba2f684ba5", "ea49a16d-f1c1-4300-a858-bee1d6ec3e83" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d671dd32-7f37-4834-83d4-86ba2f684ba5");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "ea49a16d-f1c1-4300-a858-bee1d6ec3e83");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "ccde8bfa-a615-4637-afe9-930b7d238b4a", "ccde8bfa-a615-4637-afe9-930b7d238b4a", "Admin", "ADMIN" },
                    { "e67087f0-823d-49f7-a154-57f6bff52b0c", "e67087f0-823d-49f7-a154-57f6bff52b0c", "Superadmin", "SUPERADMIN" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Discriminator", "Email", "EmailConfirmed", "FullName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "38c4e0f7-3cb5-45eb-9d37-a8dcb62ba035", 0, "62d3a089-b1cd-4302-8154-ff28932b9809", "User", "superadmin@voting.com", true, "Superadmin", false, null, "SUPERADMIN@VOTING.COM", "SUPERADMIN", "AQAAAAIAAYagAAAAEP241yQjDA2uKtUpawUkHVlGXnZTAyGirWeVcTYgnsL3rPu2lHMDuwk5EGaaG/rUtg==", null, false, "996a4cd0-597b-4ef8-a595-8b180dae1a39", false, "superadmin" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "e67087f0-823d-49f7-a154-57f6bff52b0c", "38c4e0f7-3cb5-45eb-9d37-a8dcb62ba035" });
        }
    }
}
