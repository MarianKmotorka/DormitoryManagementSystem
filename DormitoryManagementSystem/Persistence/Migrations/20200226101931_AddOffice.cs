using Microsoft.EntityFrameworkCore.Migrations;

namespace Persistence.Migrations
{
    public partial class AddOffice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OfficeNumber",
                table: "Officers");

            migrationBuilder.AddColumn<int>(
                name: "OfficeId",
                table: "Officers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Offices",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Number = table.Column<string>(nullable: false),
                    Capacity = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Offices", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Officers_OfficeId",
                table: "Officers",
                column: "OfficeId");

            migrationBuilder.CreateIndex(
                name: "IX_Offices_Number",
                table: "Offices",
                column: "Number",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Officers_Offices_OfficeId",
                table: "Officers",
                column: "OfficeId",
                principalTable: "Offices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Officers_Offices_OfficeId",
                table: "Officers");

            migrationBuilder.DropTable(
                name: "Offices");

            migrationBuilder.DropIndex(
                name: "IX_Officers_OfficeId",
                table: "Officers");

            migrationBuilder.DropColumn(
                name: "OfficeId",
                table: "Officers");

            migrationBuilder.AddColumn<string>(
                name: "OfficeNumber",
                table: "Officers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
