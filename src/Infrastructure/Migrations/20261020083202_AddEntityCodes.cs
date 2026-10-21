using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddEntityCodes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence(
                name: "user_code_seq",
                startValue: 10001L);

            migrationBuilder.AddColumn<string>(
                name: "code",
                table: "users",
                type: "character varying(32)",
                maxLength: 32,
                nullable: false,
                defaultValueSql: "'USR-' || nextval('user_code_seq')");

            migrationBuilder.AddColumn<string>(
                name: "code",
                table: "tenant_members",
                type: "character varying(32)",
                maxLength: 32,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "code",
                table: "roles",
                type: "character varying(64)",
                maxLength: 64,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "ix_users_code",
                table: "users",
                column: "code",
                unique: true,
                filter: "is_deleted = false");

            migrationBuilder.CreateIndex(
                name: "ix_tenant_members_tenant_id_code",
                table: "tenant_members",
                columns: new[] { "tenant_id", "code" },
                unique: true,
                filter: "is_deleted = false");

            migrationBuilder.CreateIndex(
                name: "ix_roles_code",
                table: "roles",
                column: "code",
                unique: true,
                filter: "is_deleted = false AND tenant_id IS NULL");

            migrationBuilder.CreateIndex(
                name: "ix_roles_tenant_id_code",
                table: "roles",
                columns: new[] { "tenant_id", "code" },
                unique: true,
                filter: "is_deleted = false AND tenant_id IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_users_code",
                table: "users");

            migrationBuilder.DropIndex(
                name: "ix_tenant_members_tenant_id_code",
                table: "tenant_members");

            migrationBuilder.DropIndex(
                name: "ix_roles_code",
                table: "roles");
    
            migrationBuilder.DropIndex(
                name: "ix_roles_tenant_id_code",
                table: "roles");

            migrationBuilder.DropColumn(
                name: "code",
                table: "users");

            migrationBuilder.DropColumn(
                name: "code",
                table: "tenant_members");

            migrationBuilder.DropColumn(
                name: "code",
                table: "roles");

            migrationBuilder.DropSequence(
                name: "user_code_seq");
        }
    }
}
