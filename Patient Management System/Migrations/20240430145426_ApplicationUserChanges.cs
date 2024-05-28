using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Patient_Management_System.Migrations
{
    /// <inheritdoc />
    public partial class ApplicationUserChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "EmergencyDetails",
                newName: "EmergencyName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EmergencyName",
                table: "EmergencyDetails",
                newName: "Name");
        }
    }
}
