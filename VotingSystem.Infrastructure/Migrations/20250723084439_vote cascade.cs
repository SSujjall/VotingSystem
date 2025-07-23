using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace VotingSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class votecascade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Votes_PollOptions_PollOptionId",
                table: "Votes");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Votes_PollOptions_PollOptionId",
                table: "Votes",
                column: "PollOptionId",
                principalTable: "PollOptions",
                principalColumn: "PollOptionId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Votes_PollOptions_PollOptionId",
                table: "Votes");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Votes_PollOptions_PollOptionId",
                table: "Votes",
                column: "PollOptionId",
                principalTable: "PollOptions",
                principalColumn: "PollOptionId");
        }
    }
}
