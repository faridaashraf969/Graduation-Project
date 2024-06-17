using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Demo.DAL.Migrations
{
    public partial class CourseEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CartItems_CourseId",
                table: "CartItems");

            migrationBuilder.RenameColumn(
                name: "Rate",
                table: "Courses",
                newName: "TotalHours");

            migrationBuilder.RenameColumn(
                name: "Feedback",
                table: "Courses",
                newName: "VideoContentUrl");

            migrationBuilder.AddColumn<int>(
                name: "Category",
                table: "Courses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CourseStatus",
                table: "Courses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "EnrollmentStartDate",
                table: "Courses",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "ApplicationUserCourse",
                columns: table => new
                {
                    PurchasedCoursesId = table.Column<int>(type: "int", nullable: false),
                    PurchasersId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUserCourse", x => new { x.PurchasedCoursesId, x.PurchasersId });
                    table.ForeignKey(
                        name: "FK_ApplicationUserCourse_AspNetUsers_PurchasersId",
                        column: x => x.PurchasersId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicationUserCourse_Courses_PurchasedCoursesId",
                        column: x => x.PurchasedCoursesId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_CourseId",
                table: "CartItems",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserCourse_PurchasersId",
                table: "ApplicationUserCourse",
                column: "PurchasersId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationUserCourse");

            migrationBuilder.DropIndex(
                name: "IX_CartItems_CourseId",
                table: "CartItems");

            migrationBuilder.DropColumn(
                name: "Category",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "CourseStatus",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "EnrollmentStartDate",
                table: "Courses");

            migrationBuilder.RenameColumn(
                name: "VideoContentUrl",
                table: "Courses",
                newName: "Feedback");

            migrationBuilder.RenameColumn(
                name: "TotalHours",
                table: "Courses",
                newName: "Rate");

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_CourseId",
                table: "CartItems",
                column: "CourseId",
                unique: true,
                filter: "[CourseId] IS NOT NULL");
        }
    }
}
