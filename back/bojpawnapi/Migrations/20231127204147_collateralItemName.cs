using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace bojpawnapi.Migrations
{
    /// <inheritdoc />
    public partial class collateralItemName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CollateralItemName",
                table: "CollateralTxDetails",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CollateralItemName",
                table: "CollateralTxDetails");
        }
    }
}
