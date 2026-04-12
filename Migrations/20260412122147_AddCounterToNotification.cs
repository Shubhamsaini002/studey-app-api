using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace studyapp.Migrations
{
    public partial class AddCounterToNotification : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Counter",
                table: "Notification",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Counter",
                table: "Notification");
        }
    }
}
