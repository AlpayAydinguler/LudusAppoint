using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LudusAppoint.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AgeGroups",
                columns: table => new
                {
                    AgeGroupId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MinAge = table.Column<int>(type: "int", nullable: false),
                    MaxAge = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AgeGroups", x => x.AgeGroupId);
                });

            migrationBuilder.CreateTable(
                name: "Branches",
                columns: table => new
                {
                    BranchId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BranchName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    District = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Neighbourhood = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Street = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BranchPhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BranchEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReservationInAdvanceDayLimit = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Branches", x => x.BranchId);
                });

            migrationBuilder.CreateTable(
                name: "IdentityUser",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityUser", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OfferedServices",
                columns: table => new
                {
                    OfferedServiceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OfferedServiceName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Genders = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApproximateDuration = table.Column<TimeSpan>(type: "time", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OfferedServices", x => x.OfferedServiceId);
                    table.CheckConstraint("CK_OfferedService_ApproximateDuration", "ApproximateDuration >= '00:01' AND ApproximateDuration <= '23:59'");
                });

            migrationBuilder.CreateTable(
                name: "ShopSettings",
                columns: table => new
                {
                    Key = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShopSettings", x => x.Key);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    EmployeeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdentityUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BranchId = table.Column<int>(type: "int", nullable: false),
                    EmployeeName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmployeeSurname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CanTakeClients = table.Column<bool>(type: "bit", nullable: false),
                    DayOff = table.Column<int>(type: "int", nullable: false),
                    StartOfWorkingHours = table.Column<TimeSpan>(type: "time", nullable: false),
                    EndOfWorkingHours = table.Column<TimeSpan>(type: "time", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.EmployeeId);
                    table.ForeignKey(
                        name: "FK_Employees_Branches_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branches",
                        principalColumn: "BranchId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OfferedServiceAgeGroups",
                columns: table => new
                {
                    AgeGroupsId = table.Column<int>(type: "int", nullable: false),
                    OfferedServiceId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OfferedServiceAgeGroups", x => new { x.AgeGroupsId, x.OfferedServiceId });
                    table.ForeignKey(
                        name: "FK_OfferedServiceAgeGroups_AgeGroups_AgeGroupsId",
                        column: x => x.AgeGroupsId,
                        principalTable: "AgeGroups",
                        principalColumn: "AgeGroupId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OfferedServiceAgeGroups_OfferedServices_OfferedServiceId",
                        column: x => x.OfferedServiceId,
                        principalTable: "OfferedServices",
                        principalColumn: "OfferedServiceId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OfferedServiceLocalizations",
                columns: table => new
                {
                    OfferedServiceLocalizationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Language = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OfferedServiceLocalizationName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OfferedServiceId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OfferedServiceLocalizations", x => x.OfferedServiceLocalizationId);
                    table.ForeignKey(
                        name: "FK_OfferedServiceLocalizations_OfferedServices_OfferedServiceId",
                        column: x => x.OfferedServiceId,
                        principalTable: "OfferedServices",
                        principalColumn: "OfferedServiceId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CustomerAppointments",
                columns: table => new
                {
                    CustomerAppointmentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Surname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Gender = table.Column<int>(type: "int", nullable: false),
                    ApproximateDuration = table.Column<TimeSpan>(type: "time", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    StartDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EMail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    AgeGroupId = table.Column<int>(type: "int", nullable: false),
                    BranchId = table.Column<int>(type: "int", nullable: false),
                    EmployeeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerAppointments", x => x.CustomerAppointmentId);
                    table.ForeignKey(
                        name: "FK_CustomerAppointments_AgeGroups_AgeGroupId",
                        column: x => x.AgeGroupId,
                        principalTable: "AgeGroups",
                        principalColumn: "AgeGroupId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CustomerAppointments_Branches_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branches",
                        principalColumn: "BranchId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CustomerAppointments_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "EmployeeId");
                    table.ForeignKey(
                        name: "FK_CustomerAppointments_IdentityUser_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "IdentityUser",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EmployeeLeaves",
                columns: table => new
                {
                    EmployeeLeaveId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LeaveStartDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LeaveEndDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmployeeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeLeaves", x => x.EmployeeLeaveId);
                    table.ForeignKey(
                        name: "FK_EmployeeLeaves_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeOfferedServices",
                columns: table => new
                {
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    OfferedServicesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeOfferedServices", x => new { x.EmployeeId, x.OfferedServicesId });
                    table.ForeignKey(
                        name: "FK_EmployeeOfferedServices_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmployeeOfferedServices_OfferedServices_OfferedServicesId",
                        column: x => x.OfferedServicesId,
                        principalTable: "OfferedServices",
                        principalColumn: "OfferedServiceId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CustomerAppointmentOfferedServices",
                columns: table => new
                {
                    CustomerAppointmentId = table.Column<int>(type: "int", nullable: false),
                    OfferedServicesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerAppointmentOfferedServices", x => new { x.CustomerAppointmentId, x.OfferedServicesId });
                    table.ForeignKey(
                        name: "FK_CustomerAppointmentOfferedServices_CustomerAppointments_CustomerAppointmentId",
                        column: x => x.CustomerAppointmentId,
                        principalTable: "CustomerAppointments",
                        principalColumn: "CustomerAppointmentId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CustomerAppointmentOfferedServices_OfferedServices_OfferedServicesId",
                        column: x => x.OfferedServicesId,
                        principalTable: "OfferedServices",
                        principalColumn: "OfferedServiceId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "AgeGroups",
                columns: new[] { "AgeGroupId", "MaxAge", "MinAge", "Status" },
                values: new object[,]
                {
                    { 1, 17, 0, true },
                    { 2, 75, 18, true },
                    { 3, 125, 76, true }
                });

            migrationBuilder.InsertData(
                table: "Branches",
                columns: new[] { "BranchId", "Address", "BranchEmail", "BranchName", "BranchPhoneNumber", "City", "Country", "District", "Neighbourhood", "ReservationInAdvanceDayLimit", "Status", "Street" },
                values: new object[,]
                {
                    { 1, "no 13 a", "businessmail@business.com", "Hacıhalil Şube", "+90 537 025 52 80", "Kocaeli", "Turkey", "Gebze", "Hacıhalil", 60, true, "Kızılay caddesi, 1203. Sk." },
                    { 2, "No:68", "businessmail@business.com", "Gebze Şube", "+90 537 025 52 80", "Kocaeli", "Turkey", "Gebze", "Osman Yılmaz", 60, false, "Kızılay Cd." }
                });

            migrationBuilder.InsertData(
                table: "OfferedServices",
                columns: new[] { "OfferedServiceId", "ApproximateDuration", "Genders", "OfferedServiceName", "Price", "Status" },
                values: new object[,]
                {
                    { 1, new TimeSpan(0, 0, 20, 0, 0), "[0]", "HairCut", 100m, true },
                    { 2, new TimeSpan(0, 0, 40, 0, 0), "[0]", "RazorShave", 200m, true },
                    { 3, new TimeSpan(0, 1, 0, 0, 0), "[0]", "HairColoring", 300m, true },
                    { 4, new TimeSpan(0, 1, 20, 0, 0), "[0,1]", "BrowShaping", 400m, true },
                    { 5, new TimeSpan(0, 1, 40, 0, 0), "[0,1]", "BeardGrooming", 500m, true },
                    { 6, new TimeSpan(0, 2, 0, 0, 0), "[0,1]", "ChildShave", 600m, true },
                    { 7, new TimeSpan(0, 2, 20, 0, 0), "[0,1]", "PermHair", 700m, false },
                    { 8, new TimeSpan(0, 2, 30, 0, 0), "[0,1]", "Manicure", 800m, false },
                    { 9, new TimeSpan(0, 2, 40, 0, 0), "[0,1]", "Pedicure", 900m, false },
                    { 10, new TimeSpan(0, 3, 0, 0, 0), "[0]", "GroomsCut", 1000m, true },
                    { 11, new TimeSpan(0, 3, 20, 0, 0), "[1]", "Makeup(Bride)", 1100m, false }
                });

            migrationBuilder.InsertData(
                table: "ShopSettings",
                columns: new[] { "Key", "LastModified", "Value" },
                values: new object[,]
                {
                    { "CompanyLogoURL", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "\\assets\\img\\logo.jpg" },
                    { "CompanyName", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Hair Center" },
                    { "SupportedGenders", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Male,Female" }
                });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "EmployeeId", "BranchId", "CanTakeClients", "DayOff", "EmployeeName", "EmployeeSurname", "EndOfWorkingHours", "IdentityUserId", "StartOfWorkingHours", "Status" },
                values: new object[,]
                {
                    { 1, 1, true, 0, "Aydın", "Sevim", new TimeSpan(0, 19, 0, 0, 0), "1", new TimeSpan(0, 10, 0, 0, 0), true },
                    { 2, 1, true, 0, "Alpay", "Aydıngüler", new TimeSpan(0, 19, 0, 0, 0), "2", new TimeSpan(0, 10, 0, 0, 0), true },
                    { 3, 1, false, 0, "Deniz", "Dağ", new TimeSpan(0, 19, 0, 0, 0), "3", new TimeSpan(0, 10, 0, 0, 0), true }
                });

            migrationBuilder.InsertData(
                table: "OfferedServiceAgeGroups",
                columns: new[] { "AgeGroupsId", "OfferedServiceId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 1, 3 },
                    { 1, 4 },
                    { 1, 5 },
                    { 1, 6 },
                    { 2, 2 },
                    { 2, 3 },
                    { 2, 5 },
                    { 2, 7 },
                    { 2, 8 },
                    { 2, 9 },
                    { 2, 10 },
                    { 2, 11 }
                });

            migrationBuilder.InsertData(
                table: "OfferedServiceLocalizations",
                columns: new[] { "OfferedServiceLocalizationId", "Language", "OfferedServiceId", "OfferedServiceLocalizationName" },
                values: new object[,]
                {
                    { 1, "en-GB", 1, "Hair Cut" },
                    { 2, "tr-TR", 1, "Saç Kesimi" },
                    { 3, "en-GB", 2, "Razor Shave" },
                    { 4, "tr-TR", 2, "Jilet Traşı" },
                    { 5, "en-GB", 3, "Hair Coloring" },
                    { 6, "tr-TR", 3, "Saç Boyama" },
                    { 7, "en-GB", 4, "Brow Shaping" },
                    { 8, "tr-TR", 4, "Kaş Şekillendirme" },
                    { 9, "en-GB", 5, "Beard Grooming" },
                    { 10, "tr-TR", 5, "Sakal Bakımı" },
                    { 11, "en-GB", 6, "Child Shave" },
                    { 12, "tr-TR", 6, "Çocuk Tıraşı" },
                    { 13, "en-GB", 7, "Perm Hair" },
                    { 14, "tr-TR", 7, "Perma Saç" },
                    { 15, "en-GB", 8, "Manicure" },
                    { 16, "tr-TR", 8, "Manikür" },
                    { 17, "en-GB", 9, "Pedicure" },
                    { 18, "tr-TR", 9, "Pedikür" },
                    { 19, "en-GB", 10, "Groom's Cut" },
                    { 20, "tr-TR", 10, "Damat Kesimi" },
                    { 21, "en-GB", 11, "Makeup (Bride)" },
                    { 22, "tr-TR", 11, "Makyaj (Gelin)" }
                });

            migrationBuilder.InsertData(
                table: "CustomerAppointments",
                columns: new[] { "CustomerAppointmentId", "AgeGroupId", "ApproximateDuration", "BranchId", "CreatedById", "EMail", "EmployeeId", "Gender", "Name", "PhoneNumber", "Price", "StartDateTime", "Status", "Surname" },
                values: new object[,]
                {
                    { 1, 1, new TimeSpan(0, 0, 30, 0, 0), 1, null, "alice.smith@example.com", 1, 1, "Alice", "+90 123 456 7891", 150m, new DateTime(2025, 2, 13, 10, 0, 0, 0, DateTimeKind.Local), 1, "Smith" },
                    { 2, 2, new TimeSpan(0, 0, 45, 0, 0), 1, null, "bob.johnson@example.com", 2, 0, "Bob", "+90 123 456 7892", 200m, new DateTime(2025, 2, 13, 11, 30, 0, 0, DateTimeKind.Local), 0, "Johnson" },
                    { 3, 3, new TimeSpan(0, 1, 0, 0, 0), 1, null, "charlie.brown@example.com", 1, 0, "Charlie", "+90 123 456 7893", 250m, new DateTime(2025, 2, 13, 14, 0, 0, 0, DateTimeKind.Local), 3, "Brown" },
                    { 4, 2, new TimeSpan(0, 0, 40, 0, 0), 1, null, "diana.prince@example.com", 1, 1, "Diana", "+90 123 456 7894", 180m, new DateTime(2025, 2, 13, 9, 45, 0, 0, DateTimeKind.Local), 2, "Prince" },
                    { 5, 1, new TimeSpan(0, 0, 35, 0, 0), 1, null, "eve.adams@example.com", 2, 1, "Eve", "+90 123 456 7895", 160m, new DateTime(2025, 2, 13, 16, 15, 0, 0, DateTimeKind.Local), 1, "Adams" },
                    { 6, 2, new TimeSpan(0, 0, 30, 0, 0), 1, null, "frank.miller@example.com", 3, 0, "Frank", "+90 123 456 7896", 120m, new DateTime(2025, 2, 11, 12, 30, 0, 0, DateTimeKind.Local), 0, "Miller" },
                    { 7, 1, new TimeSpan(0, 1, 15, 0, 0), 1, null, "grace.hall@example.com", 2, 1, "Grace", "+90 123 456 7897", 450m, new DateTime(2025, 2, 11, 15, 0, 0, 0, DateTimeKind.Local), 1, "Hall" },
                    { 8, 3, new TimeSpan(0, 1, 45, 0, 0), 1, null, "henry.ford@example.com", 1, 0, "Henry", "+90 123 456 7898", 700m, new DateTime(2025, 2, 11, 14, 30, 0, 0, DateTimeKind.Local), 3, "Ford" },
                    { 9, 3, new TimeSpan(0, 0, 45, 0, 0), 1, null, "isabelle.clark@example.com", 3, 1, "Isabelle", "+90 123 456 7899", 250m, new DateTime(2025, 2, 11, 10, 0, 0, 0, DateTimeKind.Local), 2, "Clark" },
                    { 10, 1, new TimeSpan(0, 1, 0, 0, 0), 1, null, "jack.white@example.com", 2, 0, "Jack", "+90 123 456 7890", 300m, new DateTime(2025, 2, 12, 9, 15, 0, 0, DateTimeKind.Local), 0, "White" }
                });

            migrationBuilder.InsertData(
                table: "EmployeeLeaves",
                columns: new[] { "EmployeeLeaveId", "EmployeeId", "LeaveEndDateTime", "LeaveStartDateTime", "Reason" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2025, 2, 11, 18, 0, 0, 0, DateTimeKind.Local), new DateTime(2025, 2, 11, 8, 0, 0, 0, DateTimeKind.Local), "Sick" },
                    { 2, 2, new DateTime(2025, 2, 12, 18, 0, 0, 0, DateTimeKind.Local), new DateTime(2025, 2, 12, 8, 0, 0, 0, DateTimeKind.Local), "Vacation" },
                    { 3, 3, new DateTime(2025, 2, 13, 18, 0, 0, 0, DateTimeKind.Local), new DateTime(2025, 2, 13, 8, 0, 0, 0, DateTimeKind.Local), "Personal" }
                });

            migrationBuilder.InsertData(
                table: "EmployeeOfferedServices",
                columns: new[] { "EmployeeId", "OfferedServicesId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 1, 2 },
                    { 1, 3 },
                    { 1, 4 },
                    { 1, 5 },
                    { 1, 6 },
                    { 1, 7 },
                    { 1, 8 },
                    { 1, 9 },
                    { 1, 10 },
                    { 2, 2 }
                });

            migrationBuilder.InsertData(
                table: "CustomerAppointmentOfferedServices",
                columns: new[] { "CustomerAppointmentId", "OfferedServicesId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 1, 3 },
                    { 2, 1 },
                    { 2, 2 },
                    { 2, 3 },
                    { 3, 1 },
                    { 4, 1 },
                    { 4, 2 },
                    { 5, 1 },
                    { 5, 2 },
                    { 6, 2 },
                    { 6, 3 },
                    { 7, 2 },
                    { 8, 1 },
                    { 8, 2 },
                    { 8, 4 },
                    { 9, 2 },
                    { 10, 1 },
                    { 10, 2 },
                    { 10, 3 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AgeGroups_MinAge_MaxAge",
                table: "AgeGroups",
                columns: new[] { "MinAge", "MaxAge" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CustomerAppointmentOfferedServices_OfferedServicesId",
                table: "CustomerAppointmentOfferedServices",
                column: "OfferedServicesId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerAppointments_AgeGroupId",
                table: "CustomerAppointments",
                column: "AgeGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerAppointments_BranchId",
                table: "CustomerAppointments",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerAppointments_CreatedById",
                table: "CustomerAppointments",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerAppointments_EmployeeId",
                table: "CustomerAppointments",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeLeaves_EmployeeId",
                table: "EmployeeLeaves",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeOfferedServices_OfferedServicesId",
                table: "EmployeeOfferedServices",
                column: "OfferedServicesId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_BranchId",
                table: "Employees",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_OfferedServiceAgeGroups_OfferedServiceId",
                table: "OfferedServiceAgeGroups",
                column: "OfferedServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_OfferedServiceLocalizations_OfferedServiceId",
                table: "OfferedServiceLocalizations",
                column: "OfferedServiceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomerAppointmentOfferedServices");

            migrationBuilder.DropTable(
                name: "EmployeeLeaves");

            migrationBuilder.DropTable(
                name: "EmployeeOfferedServices");

            migrationBuilder.DropTable(
                name: "OfferedServiceAgeGroups");

            migrationBuilder.DropTable(
                name: "OfferedServiceLocalizations");

            migrationBuilder.DropTable(
                name: "ShopSettings");

            migrationBuilder.DropTable(
                name: "CustomerAppointments");

            migrationBuilder.DropTable(
                name: "OfferedServices");

            migrationBuilder.DropTable(
                name: "AgeGroups");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "IdentityUser");

            migrationBuilder.DropTable(
                name: "Branches");
        }
    }
}
