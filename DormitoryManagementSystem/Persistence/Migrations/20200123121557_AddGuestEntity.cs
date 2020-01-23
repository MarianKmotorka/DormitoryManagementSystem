using Microsoft.EntityFrameworkCore.Migrations;

namespace Persistence.Migrations
{
    public partial class AddGuestEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address_City",
                table: "User",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address_Country",
                table: "User",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address_HouseNumber",
                table: "User",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address_PostCode",
                table: "User",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address_Street",
                table: "User",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Guests",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    IdCardNumber = table.Column<string>(nullable: false),
                    DormitoryCardNumber = table.Column<string>(nullable: true),
                    RoomNumber = table.Column<string>(nullable: true),
                    DistanceFromHome = table.Column<int>(nullable: false),
                    AppUserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Guests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Guests_User_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Guests_AppUserId",
                table: "Guests",
                column: "AppUserId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Guests");

            migrationBuilder.DropColumn(
                name: "Address_City",
                table: "User");

            migrationBuilder.DropColumn(
                name: "Address_Country",
                table: "User");

            migrationBuilder.DropColumn(
                name: "Address_HouseNumber",
                table: "User");

            migrationBuilder.DropColumn(
                name: "Address_PostCode",
                table: "User");

            migrationBuilder.DropColumn(
                name: "Address_Street",
                table: "User");
        }
    }
}
