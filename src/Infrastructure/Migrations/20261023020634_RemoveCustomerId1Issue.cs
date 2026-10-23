using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveCustomerId1Issue : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_customer_preference_customers_customer_id",
                table: "customer_preference");

            migrationBuilder.DropForeignKey(
                name: "fk_customer_preference_customers_customer_id1",
                table: "customer_preference");

            migrationBuilder.DropPrimaryKey(
                name: "pk_customer_preference",
                table: "customer_preference");

            migrationBuilder.DropIndex(
                name: "ix_customer_preference_customer_id1",
                table: "customer_preference");

            migrationBuilder.DropColumn(
                name: "allowed_channels",
                table: "customers");

            migrationBuilder.DropColumn(
                name: "customer_id1",
                table: "customer_preference");

            migrationBuilder.RenameTable(
                name: "customer_preference",
                newName: "customer_preferences");

            migrationBuilder.RenameIndex(
                name: "ix_customer_preference_customer_id",
                table: "customer_preferences",
                newName: "ix_customer_preferences_customer_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_customer_preferences",
                table: "customer_preferences",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_customer_preferences_customers_customer_id",
                table: "customer_preferences",
                column: "customer_id",
                principalTable: "customers",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_customer_preferences_customers_customer_id",
                table: "customer_preferences");

            migrationBuilder.DropPrimaryKey(
                name: "pk_customer_preferences",
                table: "customer_preferences");

            migrationBuilder.RenameTable(
                name: "customer_preferences",
                newName: "customer_preference");

            migrationBuilder.RenameIndex(
                name: "ix_customer_preferences_customer_id",
                table: "customer_preference",
                newName: "ix_customer_preference_customer_id");

            migrationBuilder.AddColumn<int>(
                name: "allowed_channels",
                table: "customers",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "customer_id1",
                table: "customer_preference",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "pk_customer_preference",
                table: "customer_preference",
                column: "id");

            migrationBuilder.CreateIndex(
                name: "ix_customer_preference_customer_id1",
                table: "customer_preference",
                column: "customer_id1",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "fk_customer_preference_customers_customer_id",
                table: "customer_preference",
                column: "customer_id",
                principalTable: "customers",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_customer_preference_customers_customer_id1",
                table: "customer_preference",
                column: "customer_id1",
                principalTable: "customers",
                principalColumn: "id");
        }
    }
}
