using System;
using System.Collections.Generic;
using Aseta.Domain.Entities.Inventories.CustomField;
using Aseta.Domain.Entities.Inventories.CustomId;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Aseta.Infrastructure.Database.Migrations;

/// <inheritdoc />
public partial class InitialCreate : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Categories",
            columns: table => new
            {
                id = table.Column<Guid>(type: "uuid", nullable: false),
                name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
            },
            constraints: table => table.PrimaryKey("pk_categories", x => x.id));

        migrationBuilder.CreateTable(
            name: "Tags",
            columns: table => new
            {
                id = table.Column<Guid>(type: "uuid", nullable: false),
                name = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: false)
            },
            constraints: table => table.PrimaryKey("pk_tags", x => x.id));

        migrationBuilder.CreateTable(
            name: "Users",
            columns: table => new
            {
                id = table.Column<Guid>(type: "uuid", nullable: false),
                user_name = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                password_hash = table.Column<string>(type: "text", nullable: false),
                role = table.Column<string>(type: "text", nullable: false),
                is_locked = table.Column<bool>(type: "boolean", nullable: false),
                created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
            },
            constraints: table => table.PrimaryKey("pk_users", x => x.id));

        migrationBuilder.CreateTable(
            name: "Inventories",
            columns: table => new
            {
                id = table.Column<Guid>(type: "uuid", nullable: false),
                name = table.Column<string>(type: "character varying(36)", maxLength: 36, nullable: false),
                description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                image_url = table.Column<string>(type: "text", nullable: false),
                is_public = table.Column<bool>(type: "boolean", nullable: false),
                custom_fields = table.Column<ICollection<CustomFieldDefinition>>(type: "jsonb", nullable: false),
                items_count = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                category_id = table.Column<Guid>(type: "uuid", nullable: false),
                custom_id_rules = table.Column<ICollection<CustomIdRuleBase>>(type: "jsonb", nullable: false),
                created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                creator_id = table.Column<Guid>(type: "uuid", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_inventories", x => x.id);
                table.ForeignKey(
                    name: "fk_inventories_categories_category_id",
                    column: x => x.category_id,
                    principalTable: "Categories",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "fk_inventories_users_creator_id",
                    column: x => x.creator_id,
                    principalTable: "Users",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateTable(
            name: "Comments",
            columns: table => new
            {
                id = table.Column<Guid>(type: "uuid", nullable: false),
                content = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                user_id = table.Column<Guid>(type: "uuid", nullable: false),
                inventory_id = table.Column<Guid>(type: "uuid", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_comments", x => x.id);
                table.ForeignKey(
                    name: "fk_comments_inventories_inventory_id",
                    column: x => x.inventory_id,
                    principalTable: "Inventories",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "fk_comments_users_user_id",
                    column: x => x.user_id,
                    principalTable: "Users",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateTable(
            name: "inventory_tag",
            columns: table => new
            {
                inventories_id = table.Column<Guid>(type: "uuid", nullable: false),
                tags_id = table.Column<Guid>(type: "uuid", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_inventory_tag", x => new { x.inventories_id, x.tags_id });
                table.ForeignKey(
                    name: "fk_inventory_tag_inventories_inventories_id",
                    column: x => x.inventories_id,
                    principalTable: "Inventories",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "fk_inventory_tag_tags_tags_id",
                    column: x => x.tags_id,
                    principalTable: "Tags",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "InventoryUserRoles",
            columns: table => new
            {
                user_id = table.Column<Guid>(type: "uuid", nullable: false),
                inventory_id = table.Column<Guid>(type: "uuid", nullable: false),
                role = table.Column<string>(type: "text", nullable: false),
                id = table.Column<Guid>(type: "uuid", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_inventory_user_roles", x => new { x.user_id, x.inventory_id });
                table.ForeignKey(
                    name: "fk_inventory_user_roles_inventories_inventory_id",
                    column: x => x.inventory_id,
                    principalTable: "Inventories",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "fk_inventory_user_roles_users_user_id",
                    column: x => x.user_id,
                    principalTable: "Users",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateTable(
            name: "Items",
            columns: table => new
            {
                id = table.Column<Guid>(type: "uuid", nullable: false),
                custom_id = table.Column<string>(type: "text", nullable: false),
                inventory_id = table.Column<Guid>(type: "uuid", nullable: false),
                custom_field_values = table.Column<ICollection<CustomFieldValue>>(type: "jsonb", nullable: false),
                creator_id = table.Column<Guid>(type: "uuid", nullable: false),
                updater_id = table.Column<Guid>(type: "uuid", nullable: false),
                updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_items", x => x.id);
                table.ForeignKey(
                    name: "fk_items_inventories_inventory_id",
                    column: x => x.inventory_id,
                    principalTable: "Inventories",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "fk_items_users_creator_id",
                    column: x => x.creator_id,
                    principalTable: "Users",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "fk_items_users_updater_id",
                    column: x => x.updater_id,
                    principalTable: "Users",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateTable(
            name: "Likes",
            columns: table => new
            {
                id = table.Column<Guid>(type: "uuid", nullable: false),
                comment_id = table.Column<Guid>(type: "uuid", nullable: false),
                user_id = table.Column<Guid>(type: "uuid", nullable: false),
                created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_likes", x => x.id);
                table.ForeignKey(
                    name: "fk_likes_comments_comment_id",
                    column: x => x.comment_id,
                    principalTable: "Comments",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "fk_likes_users_user_id",
                    column: x => x.user_id,
                    principalTable: "Users",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateIndex(
            name: "ix_categories_name",
            table: "Categories",
            column: "name",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "ix_comments_inventory_id",
            table: "Comments",
            column: "inventory_id");

        migrationBuilder.CreateIndex(
            name: "ix_comments_user_id",
            table: "Comments",
            column: "user_id");

        migrationBuilder.CreateIndex(
            name: "ix_inventories_category_id",
            table: "Inventories",
            column: "category_id");

        migrationBuilder.CreateIndex(
            name: "ix_inventories_creator_id",
            table: "Inventories",
            column: "creator_id");

        migrationBuilder.CreateIndex(
            name: "ix_inventory_tag_tags_id",
            table: "inventory_tag",
            column: "tags_id");

        migrationBuilder.CreateIndex(
            name: "ix_inventory_user_roles_inventory_id",
            table: "InventoryUserRoles",
            column: "inventory_id");

        migrationBuilder.CreateIndex(
            name: "ix_items_creator_id",
            table: "Items",
            column: "creator_id");

        migrationBuilder.CreateIndex(
            name: "ix_items_inventory_id_custom_id",
            table: "Items",
            columns: ["inventory_id", "custom_id"],
            unique: true);

        migrationBuilder.CreateIndex(
            name: "ix_items_updater_id",
            table: "Items",
            column: "updater_id");

        migrationBuilder.CreateIndex(
            name: "ix_likes_comment_id",
            table: "Likes",
            column: "comment_id");

        migrationBuilder.CreateIndex(
            name: "ix_likes_user_id_comment_id",
            table: "Likes",
            columns: ["user_id", "comment_id"],
            unique: true);

        migrationBuilder.CreateIndex(
            name: "ix_tags_name",
            table: "Tags",
            column: "name",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "ix_users_email",
            table: "Users",
            column: "email",
            unique: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "inventory_tag");

        migrationBuilder.DropTable(
            name: "InventoryUserRoles");

        migrationBuilder.DropTable(
            name: "Items");

        migrationBuilder.DropTable(
            name: "Likes");

        migrationBuilder.DropTable(
            name: "Tags");

        migrationBuilder.DropTable(
            name: "Comments");

        migrationBuilder.DropTable(
            name: "Inventories");

        migrationBuilder.DropTable(
            name: "Categories");

        migrationBuilder.DropTable(
            name: "Users");
    }
}
