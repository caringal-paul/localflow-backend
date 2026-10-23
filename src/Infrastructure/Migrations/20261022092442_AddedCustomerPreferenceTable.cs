using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedCustomerPreferenceTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "allow_email",
                table: "customers");

            migrationBuilder.DropColumn(
                name: "allow_sms",
                table: "customers");
                
            migrationBuilder.CreateTable(
                name: "customer_preference",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    tenant_id = table.Column<Guid>(type: "uuid", nullable: false),
                    customer_id = table.Column<Guid>(type: "uuid", nullable: false),
                    allow_email = table.Column<bool>(type: "boolean", nullable: false),
                    allow_sms = table.Column<bool>(type: "boolean", nullable: false),
                    allow_push = table.Column<bool>(type: "boolean", nullable: false),
                    customer_id1 = table.Column<Guid>(type: "uuid", nullable: true),
                    created_by = table.Column<Guid>(type: "uuid", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_customer_preference", x => x.id);
                    table.ForeignKey(
                        name: "fk_customer_preference_customers_customer_id",
                        column: x => x.customer_id,
                        principalTable: "customers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_customer_preference_customers_customer_id1",
                        column: x => x.customer_id1,
                        principalTable: "customers",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "ix_customer_preference_customer_id",
                table: "customer_preference",
                column: "customer_id",
                unique: true,
                filter: "is_deleted = false");

            migrationBuilder.CreateIndex(
                name: "ix_customer_preference_customer_id1",
                table: "customer_preference",
                column: "customer_id1",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "customer_preference");

            migrationBuilder.AddColumn<bool>(
                name: "allow_email",
                table: "customers",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "allow_sms",
                table: "customers",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
