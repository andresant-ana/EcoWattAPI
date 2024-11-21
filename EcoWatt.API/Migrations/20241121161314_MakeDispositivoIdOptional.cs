using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EcoWatt.API.Migrations
{
    /// <inheritdoc />
    public partial class MakeDispositivoIdOptional : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ConsumosAgregados_Dispositivos_DispositivoId",
                table: "ConsumosAgregados");

            migrationBuilder.AlterColumn<int>(
                name: "DispositivoId",
                table: "ConsumosAgregados",
                type: "NUMBER(10)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "NUMBER(10)");

            migrationBuilder.AddForeignKey(
                name: "FK_ConsumosAgregados_Dispositivos_DispositivoId",
                table: "ConsumosAgregados",
                column: "DispositivoId",
                principalTable: "Dispositivos",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ConsumosAgregados_Dispositivos_DispositivoId",
                table: "ConsumosAgregados");

            migrationBuilder.AlterColumn<int>(
                name: "DispositivoId",
                table: "ConsumosAgregados",
                type: "NUMBER(10)",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "NUMBER(10)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ConsumosAgregados_Dispositivos_DispositivoId",
                table: "ConsumosAgregados",
                column: "DispositivoId",
                principalTable: "Dispositivos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
