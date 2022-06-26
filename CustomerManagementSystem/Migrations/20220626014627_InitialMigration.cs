using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CustomerManagementSystem.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StateOfResidence = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LGA = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberVerified = table.Column<bool>(type: "bit", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Locations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CountryName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CountryCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StateName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StateCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LgaName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LgaCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OtpValidations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Otp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OtpExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OtpStatusFlag = table.Column<int>(type: "int", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OtpValidations", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Locations",
                columns: new[] { "Id", "CountryCode", "CountryName", "DateCreated", "LgaCode", "LgaName", "StateCode", "StateName" },
                values: new object[] { 1, "116", "Nigeria", new DateTime(2022, 6, 26, 2, 46, 26, 780, DateTimeKind.Utc).AddTicks(8074), "754", "EPE", "36", "LAGOS" });

            migrationBuilder.InsertData(
                table: "Locations",
                columns: new[] { "Id", "CountryCode", "CountryName", "DateCreated", "LgaCode", "LgaName", "StateCode", "StateName" },
                values: new object[] { 2, "116", "Nigeria", new DateTime(2022, 6, 26, 2, 46, 26, 783, DateTimeKind.Utc).AddTicks(6512), "753", "IBEJU LEKKI", "36", "LAGOS" });

            migrationBuilder.InsertData(
                table: "Locations",
                columns: new[] { "Id", "CountryCode", "CountryName", "DateCreated", "LgaCode", "LgaName", "StateCode", "StateName" },
                values: new object[] { 3, "116", "Nigeria", new DateTime(2022, 6, 26, 2, 46, 26, 783, DateTimeKind.Utc).AddTicks(6552), "748", "LAGOS MAINLAND", "36", "LAGOS" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "Locations");

            migrationBuilder.DropTable(
                name: "OtpValidations");
        }
    }
}
