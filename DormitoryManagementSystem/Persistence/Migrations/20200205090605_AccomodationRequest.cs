using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Persistence.Migrations
{
    public partial class AccomodationRequest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "OfficeNumber",
                table: "Officers",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "AccomodationRequest",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RequestPlacedUtc = table.Column<DateTime>(nullable: false),
                    RequestorId = table.Column<string>(nullable: false),
                    AccomodationStartDateUtc = table.Column<DateTime>(nullable: false),
                    AccomodationEndDateUtc = table.Column<DateTime>(nullable: false),
                    State = table.Column<int>(nullable: false),
                    RequestorMessage = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccomodationRequest", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccomodationRequest_Guests_RequestorId",
                        column: x => x.RequestorId,
                        principalTable: "Guests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccomodationRequest_RequestorId",
                table: "AccomodationRequest",
                column: "RequestorId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccomodationRequest");

            migrationBuilder.AlterColumn<string>(
                name: "OfficeNumber",
                table: "Officers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string));
        }
    }
}
