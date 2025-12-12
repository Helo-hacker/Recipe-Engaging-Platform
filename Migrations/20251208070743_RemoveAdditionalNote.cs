using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RecipeSharingApp.Migrations
{
    /// <inheritdoc />
    public partial class RemoveAdditionalNote : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdditionalNote",
                table: "RecipeProcesses");

            migrationBuilder.AlterColumn<string>(
                name: "ThumbnailPath",
                table: "Recipes",
                type: "nvarchar(260)",
                maxLength: 260,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(260)",
                oldMaxLength: 260);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ThumbnailPath",
                table: "Recipes",
                type: "nvarchar(260)",
                maxLength: 260,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(260)",
                oldMaxLength: 260,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AdditionalNote",
                table: "RecipeProcesses",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");
        }
    }
}
