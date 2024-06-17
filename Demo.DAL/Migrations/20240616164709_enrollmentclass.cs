using Microsoft.EntityFrameworkCore.Migrations;

namespace Demo.DAL.Migrations
{
    public partial class enrollmentclass : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Proposals_SessionRequests_SessionRequestId",
                table: "Proposals");

            migrationBuilder.DropForeignKey(
                name: "FK_SessionRequests_AspNetUsers_ClientId",
                table: "SessionRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_SessionRequests_AspNetUsers_PhotographerId",
                table: "SessionRequests");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SessionRequests",
                table: "SessionRequests");

            migrationBuilder.RenameTable(
                name: "SessionRequests",
                newName: "SessionRequest");

            migrationBuilder.RenameIndex(
                name: "IX_SessionRequests_PhotographerId",
                table: "SessionRequest",
                newName: "IX_SessionRequest_PhotographerId");

            migrationBuilder.RenameIndex(
                name: "IX_SessionRequests_ClientId",
                table: "SessionRequest",
                newName: "IX_SessionRequest_ClientId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SessionRequest",
                table: "SessionRequest",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Proposals_SessionRequest_SessionRequestId",
                table: "Proposals",
                column: "SessionRequestId",
                principalTable: "SessionRequest",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SessionRequest_AspNetUsers_ClientId",
                table: "SessionRequest",
                column: "ClientId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SessionRequest_AspNetUsers_PhotographerId",
                table: "SessionRequest",
                column: "PhotographerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Proposals_SessionRequest_SessionRequestId",
                table: "Proposals");

            migrationBuilder.DropForeignKey(
                name: "FK_SessionRequest_AspNetUsers_ClientId",
                table: "SessionRequest");

            migrationBuilder.DropForeignKey(
                name: "FK_SessionRequest_AspNetUsers_PhotographerId",
                table: "SessionRequest");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SessionRequest",
                table: "SessionRequest");

            migrationBuilder.RenameTable(
                name: "SessionRequest",
                newName: "SessionRequests");

            migrationBuilder.RenameIndex(
                name: "IX_SessionRequest_PhotographerId",
                table: "SessionRequests",
                newName: "IX_SessionRequests_PhotographerId");

            migrationBuilder.RenameIndex(
                name: "IX_SessionRequest_ClientId",
                table: "SessionRequests",
                newName: "IX_SessionRequests_ClientId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SessionRequests",
                table: "SessionRequests",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Proposals_SessionRequests_SessionRequestId",
                table: "Proposals",
                column: "SessionRequestId",
                principalTable: "SessionRequests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SessionRequests_AspNetUsers_ClientId",
                table: "SessionRequests",
                column: "ClientId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SessionRequests_AspNetUsers_PhotographerId",
                table: "SessionRequests",
                column: "PhotographerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
