using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EcoWatt.API.Migrations
{
    /// <inheritdoc />
    public partial class AddDescricaoToConsumoAgregado : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DESCRICAO",
                table: "ConsumosAgregados",
                type: "NVARCHAR2(2000)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DESCRICAO",
                table: "ConsumosAgregados");
        }
    }
}
