using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjektBDwAI.Migrations
{
    /// <inheritdoc />
    public partial class SecondMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                    name: "Password",
                    table: "Users",
                    type: "nvarchar(255)",
                    maxLength: 255,
                    nullable: false,
                    oldClrType: typeof(string),
                    oldType: "nvarchar(50)",
                    oldMaxLength: 50);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
