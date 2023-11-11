using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookingSystem.Migrations
{
    public partial class renamescheduletable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_ClassSchedules_ScheduleId",
                table: "Bookings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ClassSchedules",
                table: "ClassSchedules");

            migrationBuilder.RenameTable(
                name: "ClassSchedules",
                newName: "Schedules");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Schedules",
                table: "Schedules",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Schedules_ScheduleId",
                table: "Bookings",
                column: "ScheduleId",
                principalTable: "Schedules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Schedules_ScheduleId",
                table: "Bookings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Schedules",
                table: "Schedules");

            migrationBuilder.RenameTable(
                name: "Schedules",
                newName: "ClassSchedules");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ClassSchedules",
                table: "ClassSchedules",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_ClassSchedules_ScheduleId",
                table: "Bookings",
                column: "ScheduleId",
                principalTable: "ClassSchedules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
