using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ASP_P42.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedDK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UsersData",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BirthDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RegisteredAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersData", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UsersRoles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateLevel = table.Column<int>(type: "int", nullable: false),
                    ReadLevel = table.Column<int>(type: "int", nullable: false),
                    UpdateLevel = table.Column<int>(type: "int", nullable: false),
                    DeleteLevel = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserAccess",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Login = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Salt = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Dk = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAccess", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserAccess_UsersData_UserId",
                        column: x => x.UserId,
                        principalTable: "UsersData",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserAccess_UsersRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "UsersRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "UsersData",
                columns: new[] { "Id", "BirthDate", "DeletedAt", "Email", "FullName", "Phone", "RegisteredAt" },
                values: new object[] { new Guid("190052ca-f844-498a-a05f-1d4ba2adc0e8"), new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "CHANGE@ME", "Адміністратор Системи", "CHANGE_ME", new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.InsertData(
                table: "UsersRoles",
                columns: new[] { "Id", "CreateLevel", "DeleteLevel", "Description", "Name", "ReadLevel", "UpdateLevel" },
                values: new object[,]
                {
                    { new Guid("21f7c25a-629b-4beb-9339-0c37ac9a8444"), 10, 10, "Корневой администратор", "Admin", 10, 10 },
                    { new Guid("c741df27-da81-4d54-b61b-c4c9a2ae7a73"), 0, 0, "Самозарегистрировавшийся пошльзователь", "User", 0, 0 }
                });

            migrationBuilder.InsertData(
                table: "UserAccess",
                columns: new[] { "Id", "Dk", "Login", "RoleId", "Salt", "UserId" },
                values: new object[] { new Guid("96dcbbba-9aee-44a2-8835-72dfe4e1a710"), "FCB57CECE720632FDBB68958CF953E46", "Admin", new Guid("21f7c25a-629b-4beb-9339-0c37ac9a8444"), "96DCBBBA-9AEE-44A2-8835-72DFE4E1A710", new Guid("190052ca-f844-498a-a05f-1d4ba2adc0e8") });

            migrationBuilder.CreateIndex(
                name: "IX_UserAccess_Login",
                table: "UserAccess",
                column: "Login",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserAccess_RoleId",
                table: "UserAccess",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAccess_UserId",
                table: "UserAccess",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserAccess");

            migrationBuilder.DropTable(
                name: "UsersData");

            migrationBuilder.DropTable(
                name: "UsersRoles");
        }
    }
}
