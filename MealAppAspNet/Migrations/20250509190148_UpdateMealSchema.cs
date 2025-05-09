using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MealAppAspNet.Migrations
{
    /// <inheritdoc />
    public partial class UpdateMealSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Meals",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "ImageUrl",
                table: "Meals",
                newName: "Image");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Meals",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "Image",
                table: "Meals",
                newName: "ImageUrl");
        }
    }
}
