using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Persistence.Migrations
{
    public partial class RepairRequest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RepairRequests",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoomId = table.Column<int>(nullable: false),
                    GuestId = table.Column<string>(nullable: false),
                    FixedById = table.Column<string>(nullable: true),
                    State = table.Column<int>(nullable: false),
                    WillBeFixedOn = table.Column<DateTime>(nullable: true),
                    ProblemDesciption = table.Column<string>(nullable: false),
                    RepairerReply = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RepairRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RepairRequests_Repairers_FixedById",
                        column: x => x.FixedById,
                        principalTable: "Repairers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RepairRequests_Guests_GuestId",
                        column: x => x.GuestId,
                        principalTable: "Guests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RepairRequests_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RepairRequests_FixedById",
                table: "RepairRequests",
                column: "FixedById");

            migrationBuilder.CreateIndex(
                name: "IX_RepairRequests_GuestId",
                table: "RepairRequests",
                column: "GuestId");

            migrationBuilder.CreateIndex(
                name: "IX_RepairRequests_RoomId",
                table: "RepairRequests",
                column: "RoomId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RepairRequests");
        }
    }
}
