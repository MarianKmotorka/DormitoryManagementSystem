using Microsoft.EntityFrameworkCore.Migrations;

namespace Persistence.Migrations
{
    public partial class InventoryItems : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InventoryItemTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InventoryNumber = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    PricePerPiece = table.Column<decimal>(nullable: false),
                    TotalQuantity = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryItemTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RoomItemTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoomId = table.Column<int>(nullable: false),
                    InventoryItemTypeId = table.Column<int>(nullable: false),
                    Quantity = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomItemTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoomItemTypes_InventoryItemTypes_InventoryItemTypeId",
                        column: x => x.InventoryItemTypeId,
                        principalTable: "InventoryItemTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RoomItemTypes_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InventoryItemTypes_InventoryNumber",
                table: "InventoryItemTypes",
                column: "InventoryNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RoomItemTypes_InventoryItemTypeId",
                table: "RoomItemTypes",
                column: "InventoryItemTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_RoomItemTypes_RoomId",
                table: "RoomItemTypes",
                column: "RoomId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RoomItemTypes");

            migrationBuilder.DropTable(
                name: "InventoryItemTypes");
        }
    }
}
