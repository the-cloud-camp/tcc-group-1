using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace bojpawnapi.Migrations
{
    /// <inheritdoc />
    public partial class interestrate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "InterestRate",
                table: "CollateralTxs",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InterestRate",
                table: "CollateralTxs");
        }
    }
}
