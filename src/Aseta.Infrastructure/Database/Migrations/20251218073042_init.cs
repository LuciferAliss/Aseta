using System;
using System.Collections.Generic;
using Aseta.Domain.Entities.Inventories.CustomField;
using Aseta.Domain.Entities.Inventories.CustomId;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Aseta.Infrastructure.Database.Migrations;

/// <inheritdoc />
public partial class Init : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "categories",
            columns: table => new
            {
                id = table.Column<Guid>(type: "uuid", nullable: false),
                name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
            },
            constraints: table => table.PrimaryKey("pk_categories", x => x.id));

        migrationBuilder.CreateTable(
            name: "tags",
            columns: table => new
            {
                id = table.Column<Guid>(type: "uuid", nullable: false),
                name = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: false)
            },
            constraints: table => table.PrimaryKey("pk_tags", x => x.id));

        migrationBuilder.CreateTable(
            name: "users",
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
            name: "inventories",
            columns: table => new
            {
                id = table.Column<Guid>(type: "uuid", nullable: false),
                name = table.Column<string>(type: "character varying(36)", maxLength: 36, nullable: false),
                description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                image_url = table.Column<string>(type: "text", nullable: false),
                is_public = table.Column<bool>(type: "boolean", nullable: false),
                custom_fields = table.Column<List<CustomFieldDefinition>>(type: "jsonb", nullable: false),
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
                    principalTable: "categories",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "fk_inventories_users_creator_id",
                    column: x => x.creator_id,
                    principalTable: "users",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateTable(
            name: "user_sessions",
            columns: table => new
            {
                id = table.Column<Guid>(type: "uuid", nullable: false),
                token = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                user_id = table.Column<Guid>(type: "uuid", nullable: false),
                expires_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                is_revoked = table.Column<bool>(type: "boolean", nullable: false),
                device_id = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                device_name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_user_sessions", x => x.id);
                table.ForeignKey(
                    name: "fk_user_sessions_users_user_id",
                    column: x => x.user_id,
                    principalTable: "users",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "comments",
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
                    principalTable: "inventories",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "fk_comments_users_user_id",
                    column: x => x.user_id,
                    principalTable: "users",
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
                    principalTable: "inventories",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "fk_inventory_tag_tags_tags_id",
                    column: x => x.tags_id,
                    principalTable: "tags",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "inventory_user_roles",
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
                    principalTable: "inventories",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "fk_inventory_user_roles_users_user_id",
                    column: x => x.user_id,
                    principalTable: "users",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateTable(
            name: "items",
            columns: table => new
            {
                id = table.Column<Guid>(type: "uuid", nullable: false),
                custom_id = table.Column<string>(type: "text", nullable: false),
                inventory_id = table.Column<Guid>(type: "uuid", nullable: false),
                custom_field_values = table.Column<ICollection<CustomFieldValue>>(type: "jsonb", nullable: false),
                creator_id = table.Column<Guid>(type: "uuid", nullable: false),
                updater_id = table.Column<Guid>(type: "uuid", nullable: true),
                updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_items", x => x.id);
                table.ForeignKey(
                    name: "fk_items_inventories_inventory_id",
                    column: x => x.inventory_id,
                    principalTable: "inventories",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "fk_items_users_creator_id",
                    column: x => x.creator_id,
                    principalTable: "users",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "fk_items_users_updater_id",
                    column: x => x.updater_id,
                    principalTable: "users",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateTable(
            name: "likes",
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
                    principalTable: "comments",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "fk_likes_users_user_id",
                    column: x => x.user_id,
                    principalTable: "users",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateIndex(
            name: "ix_categories_name",
            table: "categories",
            column: "name",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "ix_comments_inventory_id",
            table: "comments",
            column: "inventory_id");

        migrationBuilder.CreateIndex(
            name: "ix_comments_user_id",
            table: "comments",
            column: "user_id");

        migrationBuilder.CreateIndex(
            name: "ix_inventories_category_id",
            table: "inventories",
            column: "category_id");

        migrationBuilder.CreateIndex(
            name: "ix_inventories_creator_id",
            table: "inventories",
            column: "creator_id");

        migrationBuilder.CreateIndex(
            name: "ix_inventory_tag_tags_id",
            table: "inventory_tag",
            column: "tags_id");

        migrationBuilder.CreateIndex(
            name: "ix_inventory_user_roles_inventory_id",
            table: "inventory_user_roles",
            column: "inventory_id");

        migrationBuilder.CreateIndex(
            name: "ix_items_creator_id",
            table: "items",
            column: "creator_id");

        migrationBuilder.CreateIndex(
            name: "ix_items_inventory_id_custom_id",
            table: "items",
            columns: ["inventory_id", "custom_id"],
            unique: true);

        migrationBuilder.CreateIndex(
            name: "ix_items_updater_id",
            table: "items",
            column: "updater_id");

        migrationBuilder.CreateIndex(
            name: "ix_likes_comment_id",
            table: "likes",
            column: "comment_id");

        migrationBuilder.CreateIndex(
            name: "ix_likes_user_id_comment_id",
            table: "likes",
            columns: ["user_id", "comment_id"],
            unique: true);

        migrationBuilder.CreateIndex(
            name: "ix_tags_name",
            table: "tags",
            column: "name",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "ix_user_sessions_token",
            table: "user_sessions",
            column: "token",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "ix_user_sessions_user_id",
            table: "user_sessions",
            column: "user_id");

        migrationBuilder.CreateIndex(
            name: "ix_users_email",
            table: "users",
            column: "email",
            unique: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "inventory_tag");

        migrationBuilder.DropTable(
            name: "inventory_user_roles");

        migrationBuilder.DropTable(
            name: "items");

        migrationBuilder.DropTable(
            name: "likes");

        migrationBuilder.DropTable(
            name: "user_sessions");

        migrationBuilder.DropTable(
            name: "tags");

        migrationBuilder.DropTable(
            name: "comments");

        migrationBuilder.DropTable(
            name: "inventories");

        migrationBuilder.DropTable(
            name: "categories");

        migrationBuilder.DropTable(
            name: "users");
    }
}
