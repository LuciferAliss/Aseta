using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Aseta.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class update_connection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_inventory_user_roles_inventories_inventory_id",
                table: "InventoryUserRoles");

            migrationBuilder.AddForeignKey(
                name: "fk_inventory_user_roles_inventories_inventory_id",
                table: "InventoryUserRoles",
                column: "inventory_id",
                principalTable: "Inventories",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_inventory_user_roles_inventories_inventory_id",
                table: "InventoryUserRoles");

            migrationBuilder.AddForeignKey(
                name: "fk_inventory_user_roles_inventories_inventory_id",
                table: "InventoryUserRoles",
                column: "inventory_id",
                principalTable: "Inventories",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
