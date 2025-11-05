using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Book_Movie_Ticket.Migrations
{
    /// <inheritdoc />
    public partial class AddTableOTP : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "applicationuserOtps",
                columns: table => new
                {
                    id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    OTP = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Validto = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Isvalid = table.Column<bool>(type: "bit", nullable: false),
                    Applicationuserid = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_applicationuserOtps", x => x.id);
                    table.ForeignKey(
                        name: "FK_applicationuserOtps_AspNetUsers_Applicationuserid",
                        column: x => x.Applicationuserid,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_applicationuserOtps_Applicationuserid",
                table: "applicationuserOtps",
                column: "Applicationuserid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "applicationuserOtps");
        }
    }
}
