using Microsoft.EntityFrameworkCore.Migrations;

namespace Persistence.Migrations
{
    public partial class RoomItemTypeInsteadOfRoom : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RepairRequests_Rooms_RoomId",
                table: "RepairRequests");

            migrationBuilder.DropIndex(
                name: "IX_RepairRequests_RoomId",
                table: "RepairRequests");

            migrationBuilder.DropColumn(
                name: "RoomId",
                table: "RepairRequests");

            migrationBuilder.AddColumn<int>(
                name: "RoomItemTypeId",
                table: "RepairRequests",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_RepairRequests_RoomItemTypeId",
                table: "RepairRequests",
                column: "RoomItemTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_RepairRequests_RoomItemTypes_RoomItemTypeId",
                table: "RepairRequests",
                column: "RoomItemTypeId",
                principalTable: "RoomItemTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RepairRequests_RoomItemTypes_RoomItemTypeId",
                table: "RepairRequests");

            migrationBuilder.DropIndex(
                name: "IX_RepairRequests_RoomItemTypeId",
                table: "RepairRequests");

            migrationBuilder.DropColumn(
                name: "RoomItemTypeId",
                table: "RepairRequests");

            migrationBuilder.AddColumn<int>(
                name: "RoomId",
                table: "RepairRequests",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_RepairRequests_RoomId",
                table: "RepairRequests",
                column: "RoomId");

            migrationBuilder.AddForeignKey(
                name: "FK_RepairRequests_Rooms_RoomId",
                table: "RepairRequests",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
