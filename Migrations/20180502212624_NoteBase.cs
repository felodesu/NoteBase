using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace NoteBase.Migrations
{
    public partial class NoteBase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DbSetUsers",
                columns: table => new
                {
                    User_Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DbSetUsers", x => x.User_Id);
                });

            migrationBuilder.CreateTable(
                name: "DbSetNotes",
                columns: table => new
                {
                    Note_Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Content = table.Column<string>(nullable: true),
                    Header = table.Column<string>(nullable: true),
                    Timestamp = table.Column<long>(nullable: false),
                    User_Id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DbSetNotes", x => x.Note_Id);
                    table.ForeignKey(
                        name: "FK_DbSetNotes_DbSetUsers_User_Id",
                        column: x => x.User_Id,
                        principalTable: "DbSetUsers",
                        principalColumn: "User_Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DbSetShares",
                columns: table => new
                {
                    Note_Id = table.Column<int>(nullable: false),
                    Owner_Id = table.Column<int>(nullable: false),
                    User_Id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DbSetShares", x => new { x.Note_Id, x.Owner_Id });
                    table.ForeignKey(
                        name: "FK_DbSetShares_DbSetNotes_Note_Id",
                        column: x => x.Note_Id,
                        principalTable: "DbSetNotes",
                        principalColumn: "Note_Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DbSetShares_DbSetUsers_User_Id",
                        column: x => x.User_Id,
                        principalTable: "DbSetUsers",
                        principalColumn: "User_Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DbSetNotes_User_Id",
                table: "DbSetNotes",
                column: "User_Id");

            migrationBuilder.CreateIndex(
                name: "IX_DbSetShares_User_Id",
                table: "DbSetShares",
                column: "User_Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DbSetShares");

            migrationBuilder.DropTable(
                name: "DbSetNotes");

            migrationBuilder.DropTable(
                name: "DbSetUsers");
        }
    }
}
