using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace prog6212_st10440515_poe.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Claims_Lecturers_LecturerID1",
                table: "Claims");

            migrationBuilder.DropIndex(
                name: "IX_Claims_LecturerID1",
                table: "Claims");

            migrationBuilder.DropColumn(
                name: "LecturerID1",
                table: "Claims");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LecturerID1",
                table: "Claims",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Claims_LecturerID1",
                table: "Claims",
                column: "LecturerID1");

            migrationBuilder.AddForeignKey(
                name: "FK_Claims_Lecturers_LecturerID1",
                table: "Claims",
                column: "LecturerID1",
                principalTable: "Lecturers",
                principalColumn: "LecturerID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
