using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AcademicSupportQueueApi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AdicionaTipoSolicitacao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TipoSolicitacao",
                table: "Atendimentos",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TipoSolicitacao",
                table: "Atendimentos");
        }
    }
}
