using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Aseta.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class add_role : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "role_id",
                table: "AspNetUsers",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("b9c2d84a-9a7b-4f1e-8b5a-0e2c1d3f7a6b"));

            migrationBuilder.CreateTable(
                name: "InventoryUserRoles",
                columns: table => new
                {
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    inventory_id = table.Column<Guid>(type: "uuid", nullable: false),
                    role = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_inventory_user_roles", x => new { x.user_id, x.inventory_id, x.role });
                    table.ForeignKey(
                        name: "fk_inventory_user_roles_asp_net_users_user_id",
                        column: x => x.user_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_inventory_user_roles_inventories_inventory_id",
                        column: x => x.inventory_id,
                        principalTable: "Inventories",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "id", "concurrency_stamp", "name", "normalized_name" },
                values: new object[,]
                {
                    { new Guid("b9c2d84a-9a7b-4f1e-8b5a-0e2c1d3f7a6b"), null, "User", "USER" },
                    { new Guid("c100b9de-c285-4d0c-b3a0-58e3a7e6b80f"), null, "Admin", "ADMIN" }
                });

            migrationBuilder.CreateIndex(
                name: "ix_asp_net_users_role_id",
                table: "AspNetUsers",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "ix_inventory_user_roles_inventory_id",
                table: "InventoryUserRoles",
                column: "inventory_id");

            migrationBuilder.AddForeignKey(
                name: "fk_asp_net_users_roles_role_id",
                table: "AspNetUsers",
                column: "role_id",
                principalTable: "AspNetRoles",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_asp_net_users_roles_role_id",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "InventoryUserRoles");

            migrationBuilder.DropIndex(
                name: "ix_asp_net_users_role_id",
                table: "AspNetUsers");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "id",
                keyValue: new Guid("b9c2d84a-9a7b-4f1e-8b5a-0e2c1d3f7a6b"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "id",
                keyValue: new Guid("c100b9de-c285-4d0c-b3a0-58e3a7e6b80f"));

            migrationBuilder.DropColumn(
                name: "role_id",
                table: "AspNetUsers");
        }
    }
}
