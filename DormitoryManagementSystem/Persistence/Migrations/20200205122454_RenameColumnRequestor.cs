using Microsoft.EntityFrameworkCore.Migrations;

namespace Persistence.Migrations
{
    public partial class RenameColumnRequestor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccomodationRequest_Guests_RequestorId",
                table: "AccomodationRequest");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AccomodationRequest",
                table: "AccomodationRequest");

            migrationBuilder.DropIndex(
                name: "IX_AccomodationRequest_RequestorId",
                table: "AccomodationRequest");

            migrationBuilder.DropColumn(
                name: "RequestorId",
                table: "AccomodationRequest");

            migrationBuilder.DropColumn(
                name: "RequestorMessage",
                table: "AccomodationRequest");

            migrationBuilder.RenameTable(
                name: "AccomodationRequest",
                newName: "AccomodationRequests");

            migrationBuilder.AddColumn<string>(
                name: "RequesterId",
                table: "AccomodationRequests",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RequesterMessage",
                table: "AccomodationRequests",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_AccomodationRequests",
                table: "AccomodationRequests",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_AccomodationRequests_RequesterId",
                table: "AccomodationRequests",
                column: "RequesterId");

            migrationBuilder.AddForeignKey(
                name: "FK_AccomodationRequests_Guests_RequesterId",
                table: "AccomodationRequests",
                column: "RequesterId",
                principalTable: "Guests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccomodationRequests_Guests_RequesterId",
                table: "AccomodationRequests");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AccomodationRequests",
                table: "AccomodationRequests");

            migrationBuilder.DropIndex(
                name: "IX_AccomodationRequests_RequesterId",
                table: "AccomodationRequests");

            migrationBuilder.DropColumn(
                name: "RequesterId",
                table: "AccomodationRequests");

            migrationBuilder.DropColumn(
                name: "RequesterMessage",
                table: "AccomodationRequests");

            migrationBuilder.RenameTable(
                name: "AccomodationRequests",
                newName: "AccomodationRequest");

            migrationBuilder.AddColumn<string>(
                name: "RequestorId",
                table: "AccomodationRequest",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RequestorMessage",
                table: "AccomodationRequest",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_AccomodationRequest",
                table: "AccomodationRequest",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_AccomodationRequest_RequestorId",
                table: "AccomodationRequest",
                column: "RequestorId");

            migrationBuilder.AddForeignKey(
                name: "FK_AccomodationRequest_Guests_RequestorId",
                table: "AccomodationRequest",
                column: "RequestorId",
                principalTable: "Guests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
