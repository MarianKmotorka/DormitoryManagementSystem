using Microsoft.EntityFrameworkCore.Migrations;

namespace Persistence.Migrations
{
    public partial class AddOfficer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Guests_AppUserId",
                table: "Guests");

            migrationBuilder.CreateTable(
                name: "Officers",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    IdCardNumber = table.Column<string>(nullable: false),
                    OfficeNumber = table.Column<string>(nullable: false),
                    AppUserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Officers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Officers_AppUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Guests_AppUserId",
                table: "Guests",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Officers_AppUserId",
                table: "Officers",
                column: "AppUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Officers");

            migrationBuilder.DropIndex(
                name: "IX_Guests_AppUserId",
                table: "Guests");

            migrationBuilder.CreateIndex(
                name: "IX_Guests_AppUserId",
                table: "Guests",
                column: "AppUserId",
                unique: true);
        }
    }
}
