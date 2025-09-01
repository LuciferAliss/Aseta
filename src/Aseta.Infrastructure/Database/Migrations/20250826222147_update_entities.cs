using System;
using System.Collections.Generic;
using Aseta.Domain.Entities.CustomId;
using Aseta.Domain.Entities.Items;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Aseta.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class update_entities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "description",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "image_url",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "name",
                table: "Items");

            migrationBuilder.AlterColumn<string>(
                name: "custom_id",
                table: "Items",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AddColumn<DateTime>(
                name: "created_at",
                table: "Items",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "creator_id",
                table: "Items",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<List<CustomField>>(
                name: "custom_fields",
                table: "Items",
                type: "jsonb",
                nullable: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "updated_at",
                table: "Items",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "updater_id",
                table: "Items",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "created_at",
                table: "Inventories",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "creator_id",
                table: "Inventories",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<List<CustomIdPart>>(
                name: "custom_id_parts",
                table: "Inventories",
                type: "jsonb",
                nullable: false);

            migrationBuilder.CreateIndex(
                name: "ix_items_creator_id",
                table: "Items",
                column: "creator_id");

            migrationBuilder.CreateIndex(
                name: "ix_items_updater_id",
                table: "Items",
                column: "updater_id");

            migrationBuilder.CreateIndex(
                name: "ix_inventories_creator_id",
                table: "Inventories",
                column: "creator_id");

            migrationBuilder.AddForeignKey(
                name: "fk_inventories_asp_net_users_creator_id",
                table: "Inventories",
                column: "creator_id",
                principalTable: "AspNetUsers",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_items_asp_net_users_creator_id",
                table: "Items",
                column: "creator_id",
                principalTable: "AspNetUsers",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_items_asp_net_users_updater_id",
                table: "Items",
                column: "updater_id",
                principalTable: "AspNetUsers",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_inventories_asp_net_users_creator_id",
                table: "Inventories");

            migrationBuilder.DropForeignKey(
                name: "fk_items_asp_net_users_creator_id",
                table: "Items");

            migrationBuilder.DropForeignKey(
                name: "fk_items_asp_net_users_updater_id",
                table: "Items");

            migrationBuilder.DropIndex(
                name: "ix_items_creator_id",
                table: "Items");

            migrationBuilder.DropIndex(
                name: "ix_items_updater_id",
                table: "Items");

            migrationBuilder.DropIndex(
                name: "ix_inventories_creator_id",
                table: "Inventories");

            migrationBuilder.DropColumn(
                name: "created_at",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "creator_id",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "custom_fields",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "updated_at",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "updater_id",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "created_at",
                table: "Inventories");

            migrationBuilder.DropColumn(
                name: "creator_id",
                table: "Inventories");

            migrationBuilder.DropColumn(
                name: "custom_id_parts",
                table: "Inventories");

            migrationBuilder.AlterColumn<string>(
                name: "custom_id",
                table: "Items",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "description",
                table: "Items",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "image_url",
                table: "Items",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "name",
                table: "Items",
                type: "character varying(300)",
                maxLength: 300,
                nullable: false,
                defaultValue: "");
        }
    }
}
