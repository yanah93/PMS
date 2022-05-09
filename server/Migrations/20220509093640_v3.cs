using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PMS.Migrations
{
    public partial class v3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employee_UserAccount_UserAccountId",
                table: "Employee");

            migrationBuilder.AlterColumn<int>(
                name: "UserAccountId",
                table: "Employee",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Employee_UserAccount_UserAccountId",
                table: "Employee",
                column: "UserAccountId",
                principalTable: "UserAccount",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employee_UserAccount_UserAccountId",
                table: "Employee");

            migrationBuilder.AlterColumn<int>(
                name: "UserAccountId",
                table: "Employee",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Employee_UserAccount_UserAccountId",
                table: "Employee",
                column: "UserAccountId",
                principalTable: "UserAccount",
                principalColumn: "Id");
        }
    }
}
