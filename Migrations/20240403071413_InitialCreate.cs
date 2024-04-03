using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NightClubTestCase.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "IdentityCard",
                columns: table => new
                {
                    identityCardId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    firstName = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    lastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    birthDate = table.Column<DateTime>(type: "date", nullable: false),
                    nationalRegisterNumber = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    validityDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    expirationDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    hasExpired = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityCard", x => x.identityCardId);
                });

            migrationBuilder.CreateTable(
                name: "MemberCard",
                columns: table => new
                {
                    memberCardId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    isLost = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemberCard", x => x.memberCardId);
                });

            migrationBuilder.CreateTable(
                name: "Member",
                columns: table => new
                {
                    memberId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    identityCardId = table.Column<int>(type: "int", nullable: false),
                    mailAddress = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    phoneNumber = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    blacklistEndDate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Member", x => x.memberId);
                    table.ForeignKey(
                        name: "FK_Member_IdentityCardId",
                        column: x => x.identityCardId,
                        principalTable: "IdentityCard",
                        principalColumn: "identityCardId");
                });

            migrationBuilder.CreateTable(
                name: "Record",
                columns: table => new
                {
                    memberId = table.Column<int>(type: "int", nullable: false),
                    memberCardId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Record", x => new { x.memberId, x.memberCardId });
                    table.ForeignKey(
                        name: "FK_Record_Member",
                        column: x => x.memberId,
                        principalTable: "Member",
                        principalColumn: "memberId");
                    table.ForeignKey(
                        name: "FK_Record_MemberCardId",
                        column: x => x.memberCardId,
                        principalTable: "MemberCard",
                        principalColumn: "memberCardId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Member_identityCardId",
                table: "Member",
                column: "identityCardId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Member_identityCardId1",
                table: "Member",
                column: "identityCardId");

            migrationBuilder.CreateIndex(
                name: "IX_Record_memberCardId",
                table: "Record",
                column: "memberCardId");

            migrationBuilder.CreateIndex(
                name: "IX_Record_memberId",
                table: "Record",
                column: "memberId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Record");

            migrationBuilder.DropTable(
                name: "Member");

            migrationBuilder.DropTable(
                name: "MemberCard");

            migrationBuilder.DropTable(
                name: "IdentityCard");
        }
    }
}
