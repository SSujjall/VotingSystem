using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace VotingSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class PollOptionImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "84ed24ea-4706-4e88-9009-ce8c6d33a98d");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "73a40f4a-ef97-4f95-99f2-159dc396fa66", "957a411a-20b2-434b-a65c-2a1b083b0e2e" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "73a40f4a-ef97-4f95-99f2-159dc396fa66");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "957a411a-20b2-434b-a65c-2a1b083b0e2e");

            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                table: "PollOptions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "86eddc41-acd9-4ead-9e7b-6da0ee5b18e5", "86eddc41-acd9-4ead-9e7b-6da0ee5b18e5", "Superadmin", "SUPERADMIN" },
                    { "8d18c6b9-067a-4283-a83d-39e76b391029", "8d18c6b9-067a-4283-a83d-39e76b391029", "Admin", "ADMIN" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Discriminator", "Email", "EmailConfirmed", "FullName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "36270d33-93ec-4565-93fb-46b10fe67f85", 0, "4cc13ef6-7934-4bb0-9093-fab8e5b551ce", "User", "superadmin@voting.com", true, "Superadmin", false, null, "SUPERADMIN@VOTING.COM", "SUPERADMIN", "AQAAAAIAAYagAAAAEGAFioJjchoMSlUYcwqqHtn5MBcn/Ccff0XJ9+vpUy7l527zANcNwIboZwxrOnlRQQ==", null, false, "5da3252c-8a1c-43ef-87e0-b183fefaf2ed", false, "superadmin" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "86eddc41-acd9-4ead-9e7b-6da0ee5b18e5", "36270d33-93ec-4565-93fb-46b10fe67f85" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8d18c6b9-067a-4283-a83d-39e76b391029");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "86eddc41-acd9-4ead-9e7b-6da0ee5b18e5", "36270d33-93ec-4565-93fb-46b10fe67f85" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "86eddc41-acd9-4ead-9e7b-6da0ee5b18e5");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "36270d33-93ec-4565-93fb-46b10fe67f85");

            migrationBuilder.DropColumn(
                name: "ImagePath",
                table: "PollOptions");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "73a40f4a-ef97-4f95-99f2-159dc396fa66", "73a40f4a-ef97-4f95-99f2-159dc396fa66", "Superadmin", "SUPERADMIN" },
                    { "84ed24ea-4706-4e88-9009-ce8c6d33a98d", "84ed24ea-4706-4e88-9009-ce8c6d33a98d", "Admin", "ADMIN" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Discriminator", "Email", "EmailConfirmed", "FullName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "957a411a-20b2-434b-a65c-2a1b083b0e2e", 0, "6ab2dae2-dd44-4324-bfa8-3cfb7373d07c", "User", "superadmin@voting.com", true, "Superadmin", false, null, "SUPERADMIN@VOTING.COM", "SUPERADMIN", "AQAAAAIAAYagAAAAEAYmkZAsmtOxfGN4uNuYAkJ8M/bDDOfI8EC3BoLsoLzcjcPzR2WndVssAQBWhefJ7w==", null, false, "eb769766-af96-4235-886f-424e1a6dd0c1", false, "superadmin" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "73a40f4a-ef97-4f95-99f2-159dc396fa66", "957a411a-20b2-434b-a65c-2a1b083b0e2e" });
        }
    }
}
