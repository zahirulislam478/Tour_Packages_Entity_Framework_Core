using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Tour_Packages_and_Bookings.Migrations
{
    /// <inheritdoc />
    public partial class InitiaCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TourPackages",
                columns: table => new
                {
                    TourPackageId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PackageName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Duration = table.Column<int>(type: "int", nullable: false),
                    Destinations = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Price = table.Column<decimal>(type: "money", nullable: false),
                    Picture = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PackageType = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TourPackages", x => x.TourPackageId);
                });

            migrationBuilder.CreateTable(
                name: "Bookings",
                columns: table => new
                {
                    BookingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TravelerName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NumberOfTravelers = table.Column<int>(type: "int", nullable: false),
                    BookingDate = table.Column<DateTime>(type: "date", nullable: false),
                    BookingStatus = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TourPackageId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bookings", x => x.BookingId);
                    table.ForeignKey(
                        name: "FK_Bookings_TourPackages_TourPackageId",
                        column: x => x.TourPackageId,
                        principalTable: "TourPackages",
                        principalColumn: "TourPackageId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "TourPackages",
                columns: new[] { "TourPackageId", "Description", "Destinations", "Duration", "IsActive", "PackageName", "PackageType", "Picture", "Price" },
                values: new object[,]
                {
                    { 1, "Description for Package 1", "Destination 1", 3, true, "Package 1", 1, "package_1.jpg", 4169.00m },
                    { 2, "Description for Package 2", "Destination 2", 9, true, "Package 2", 2, "package_2.jpg", 1802.00m },
                    { 3, "Description for Package 3", "Destination 3", 6, true, "Package 3", 1, "package_3.jpg", 1811.00m },
                    { 4, "Description for Package 4", "Destination 4", 5, true, "Package 4", 1, "package_4.jpg", 4902.00m },
                    { 5, "Description for Package 5", "Destination 5", 4, true, "Package 5", 1, "package_5.jpg", 1900.00m }
                });

            migrationBuilder.InsertData(
                table: "Bookings",
                columns: new[] { "BookingId", "BookingDate", "BookingStatus", "NumberOfTravelers", "PhoneNumber", "TourPackageId", "TravelerName" },
                values: new object[,]
                {
                    { 1, new DateTime(2023, 9, 7, 22, 56, 24, 423, DateTimeKind.Local).AddTicks(3708), "Pending", 3, "123-456-7891", 1, "Traveler 1" },
                    { 2, new DateTime(2023, 9, 7, 22, 56, 24, 423, DateTimeKind.Local).AddTicks(3738), "Confirmed", 4, "123-456-7892", 2, "Traveler 2" },
                    { 3, new DateTime(2023, 9, 20, 22, 56, 24, 423, DateTimeKind.Local).AddTicks(3747), "Pending", 4, "123-456-7893", 3, "Traveler 3" },
                    { 4, new DateTime(2023, 9, 30, 22, 56, 24, 423, DateTimeKind.Local).AddTicks(3755), "Confirmed", 3, "123-456-7894", 4, "Traveler 4" },
                    { 5, new DateTime(2023, 9, 3, 22, 56, 24, 423, DateTimeKind.Local).AddTicks(3763), "Pending", 1, "123-456-7895", 5, "Traveler 5" },
                    { 6, new DateTime(2023, 9, 15, 22, 56, 24, 423, DateTimeKind.Local).AddTicks(3772), "Confirmed", 2, "123-456-7896", 1, "Traveler 6" },
                    { 7, new DateTime(2023, 9, 26, 22, 56, 24, 423, DateTimeKind.Local).AddTicks(3781), "Pending", 4, "123-456-7897", 2, "Traveler 7" },
                    { 8, new DateTime(2023, 9, 13, 22, 56, 24, 423, DateTimeKind.Local).AddTicks(3789), "Confirmed", 3, "123-456-7898", 3, "Traveler 8" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_TourPackageId",
                table: "Bookings",
                column: "TourPackageId");
            string sqlInsert =
                @"CREATE PROCEDURE InsertTour 
                    @packageName VARCHAR(50),
                    @description VARCHAR(50),
                    @duration INT,
                    @destinations VARCHAR(50),
                    @price MONEY,
                    @picture VARCHAR(50),
                    @packageType INT,
                    @isActive BIT
                AS
                INSERT INTO TourPackages (PackageName, Description, Duration, Destinations, Price,Picture,PackageType,IsActive)
                VALUES (@packageName, @description, @duration, @destinations, @price, @picture, @packageType, @isActive )
                GO";
            migrationBuilder.Sql(sqlInsert);
            string sqlUpdate =
                @"CREATE PROCEDURE UpdateTour
                    @tourPackageId INT,
                    @packageName VARCHAR(50),
                    @description VARCHAR(50),
                    @duration INT,
                    @destinations VARCHAR(50),
                    @price MONEY,
                    @picture VARCHAR(50),
                    @packageType INT,
                    @isActive BIT
                AS
                Update TourPackages 
                SET PackageName=@packageName, 
                    Description=@description, 
                    Duration=@duration, 
                    Destinations=@destinations, 
                    Price=@price,
                    Picture=@picture,
                    PackageType=@packageType,
                    IsActive=@isActive
                WHERE TourPackageId = @tourPackageId
                GO";
            migrationBuilder.Sql(sqlUpdate);

            string sqlDelete =
                @"CREATE PROCEDURE  DeleteTour 
                    @tourPackageId INT
                AS
                DELETE  FROM  TourPackages 
                WHERE TourPackageId = @tourPackageId
                GO";
            migrationBuilder.Sql(sqlDelete);

            string sqlInsertBooking =
                @"CREATE PROCEDURE InsertBooking
                    @travelerName VARCHAR(255),
                    @phoneNumber VARCHAR(100),
                    @numberOfTravelers INT,
                    @bookingDate DATE,
                    @bookingStatus VARCHAR(100),
                    @tourPackageId INT
                AS
                INSERT INTO Bookings (TravelerName, PhoneNumber, NumberOfTravelers, BookingDate, BookingStatus, TourPackageId)
                VALUES (@travelerName, @phoneNumber, @numberOfTravelers, @bookingDate, @bookingStatus, @tourPackageId)
                GO";
            migrationBuilder.Sql(sqlInsertBooking);

            string sqlUpdateBooking =
                @"CREATE PROCEDURE UpdateBooking
                    @bookingId INT,
                    @travelerName VARCHAR(255),
                    @phoneNumber VARCHAR(100),
                    @numberOfTravelers INT,
                    @bookingDate DATE,
                    @bookingStatus VARCHAR(100),
                    @tourPackageId INT
                AS
                UPDATE Bookings
                SET TravelerName = @travelerName,
                    PhoneNumber = @phoneNumber,
                    NumberOfTravelers = @numberOfTravelers,
                    BookingDate = @bookingDate,
                    BookingStatus = @bookingStatus,
                    TourPackageId = @tourPackageId
                WHERE BookingId = @bookingId
                GO";
            migrationBuilder.Sql(sqlUpdateBooking);

            string sqlDeleteBooking =
                @"CREATE PROCEDURE DeleteBooking
                    @bookingId INT
                AS
                DELETE FROM Bookings
                WHERE BookingId = @bookingId
                GO";
            migrationBuilder.Sql(sqlDeleteBooking);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bookings");

            migrationBuilder.DropTable(
                name: "TourPackages");

            migrationBuilder.Sql("DROP PROCEDURE InsertTour");
            migrationBuilder.Sql("DROP PROCEDURE UpdateTour");
            migrationBuilder.Sql("DROP PROCEDURE DeleteTour");
            migrationBuilder.Sql("DROP PROCEDURE InsertBooking");
            migrationBuilder.Sql("DROP PROCEDURE UpdateBooking");
            migrationBuilder.Sql("DROP PROCEDURE DeleteBooking");
        }
    }
}
