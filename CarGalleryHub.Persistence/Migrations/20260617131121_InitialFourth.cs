using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarGalleryHub.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialFourth : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PaymentStatus",
                table: "Payments",
                newName: "Status");

            migrationBuilder.AlterColumn<int>(
                name: "CarKM",
                table: "OrderItems",
                type: "int",
                maxLength: 2147483647,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldMaxLength: 2147483647);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Payments",
                newName: "PaymentStatus");

            migrationBuilder.AlterColumn<string>(
                name: "CarKM",
                table: "OrderItems",
                type: "nvarchar(max)",
                maxLength: 2147483647,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldMaxLength: 2147483647);
        }
    }
}
