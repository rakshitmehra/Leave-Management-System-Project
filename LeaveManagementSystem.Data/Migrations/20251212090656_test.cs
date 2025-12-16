using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LeaveManagementSystem.Data.Migrations
{
    /// <inheritdoc />
    public partial class test : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6d9ed3ff-bebb-42bc-ad07-0255bb0f7edb",
                column: "ConcurrencyStamp",
                value: "bfc01bbc-def3-40f6-aa2e-f77da4aed85f");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "cc4fcb01-de88-4c20-b4ac-8df5c2a65160",
                column: "ConcurrencyStamp",
                value: "abca1c3c-2d05-4e17-b83f-da3786a131dc");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e9f639de-624f-4a4e-b8bf-2381725462f1",
                column: "ConcurrencyStamp",
                value: "b9a07fcc-b548-49b4-a846-e54d31c5093d");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "408aa945-3d84-4421-8342-7269ec64d949",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "a9bf791c-08cb-4555-be39-30eff8198c03", "AQAAAAIAAYagAAAAEGcLknTTa5dnRchX2qQwOymSPMlH6EzAckVmZ25apAwo365bBMJBRYgW8LyxzFNfcg==", "8c7bb97f-dd64-43d1-893f-7dff2cc1bce3" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6d9ed3ff-bebb-42bc-ad07-0255bb0f7edb",
                column: "ConcurrencyStamp",
                value: "6361a817-c090-43ab-9557-901a8f53e05d");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "cc4fcb01-de88-4c20-b4ac-8df5c2a65160",
                column: "ConcurrencyStamp",
                value: "6a74172d-1199-4ef0-81a0-90f4737661c4");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e9f639de-624f-4a4e-b8bf-2381725462f1",
                column: "ConcurrencyStamp",
                value: "1452ab06-512f-432b-86ee-96e65169ac44");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "408aa945-3d84-4421-8342-7269ec64d949",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "d86cd1cf-1a83-41e2-a8a4-2e536caab012", "AQAAAAIAAYagAAAAEApL9uJoVI5qLxI282vlAEzSRdMe/EOfH1cSxbIPN+sTpLwq9dBJgx94dOYy1OJPpQ==", "7f245cd0-cc75-47be-811c-27d9588cb685" });
        }
    }
}
