using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HealthApi.Migrations
{
    /// <inheritdoc />
    public partial class AddServiceNameIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_HealthRecords_ServiceName",
                table: "HealthRecords",
                column: "ServiceName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_HealthRecords_ServiceName",
                table: "HealthRecords");
        }
    }
}
