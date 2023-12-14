using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace bojpawnapi.Migrations
{
    /// <inheritdoc />
    public partial class secodtmigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PrevCollateralId",
                table: "CollateralTxs",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PrevCollateralId",
                table: "CollateralTxs");
        }
    }
}
