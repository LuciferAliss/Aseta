using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Aseta.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class change_customidpart_on_customidrulen : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "custom_id_parts",
                table: "Inventories",
                newName: "custom_id_rules");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "custom_id_rules",
                table: "Inventories",
                newName: "custom_id_parts");
        }
    }
}
