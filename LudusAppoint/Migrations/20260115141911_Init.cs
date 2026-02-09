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
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tenants",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Hostname = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tenants", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AgeGroups",
                columns: table => new
                {
                    AgeGroupId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MinAge = table.Column<int>(type: "int", nullable: false),
                    MaxAge = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AgeGroups", x => x.AgeGroupId);
                    table.ForeignKey(
                        name: "FK_AgeGroups_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationSetting",
                columns: table => new
                {
                    Key = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationSetting", x => x.Key);
                    table.ForeignKey(
                        name: "FK_ApplicationSetting_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Surname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastLogin = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Branches",
                columns: table => new
                {
                    BranchId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                    table.ForeignKey(
                        name: "FK_Branches_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OfferedServices",
                columns: table => new
                {
                    OfferedServiceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                    table.ForeignKey(
                        name: "FK_OfferedServices_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BookingFlowConfigs",
                columns: table => new
                {
                    BookingFlowConfigId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BranchId = table.Column<int>(type: "int", nullable: false),
                    AllStepsInOrder = table.Column<string>(type: "nvarchar(max)", nullable: false, defaultValue: "[\"Services\", \"DateTime\", \"RoomSelection\", \"Employee\"]"),
                    EnabledStepsInOrder = table.Column<string>(type: "nvarchar(max)", nullable: false, defaultValue: "[\"Services\", \"DateTime\", \"RoomSelection\", \"Employee\"]"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookingFlowConfigs", x => x.BookingFlowConfigId);
                    table.ForeignKey(
                        name: "FK_BookingFlowConfigs_Branches_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branches",
                        principalColumn: "BranchId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookingFlowConfigs_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    EmployeeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdentityUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                    table.ForeignKey(
                        name: "FK_Employees_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                    table.ForeignKey(
                        name: "FK_OfferedServiceLocalizations_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CustomerAppointments",
                columns: table => new
                {
                    CustomerAppointmentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Surname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Gender = table.Column<int>(type: "int", nullable: false),
                    ApproximateDuration = table.Column<TimeSpan>(type: "time", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    StartDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EMail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                        name: "FK_CustomerAppointments_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeLeaves",
                columns: table => new
                {
                    EmployeeLeaveId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                    table.ForeignKey(
                        name: "FK_EmployeeLeaves_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1", null, "Admin", "ADMIN" },
                    { "2", null, "Owner", "OWNER" },
                    { "3", null, "Editor", "EDITOR" },
                    { "4", null, "User", "USER" }
                });

            migrationBuilder.InsertData(
                table: "Tenants",
                columns: new[] { "Id", "CreatedAt", "Hostname", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(2023, 11, 17, 11, 37, 29, 0, DateTimeKind.Utc), "localhost", "Default Tenant", null },
                    { new Guid("11111111-1111-1111-1111-111111111111"), new DateTime(2024, 11, 17, 11, 37, 29, 0, DateTimeKind.Utc), "company1.com", "Company1", null },
                    { new Guid("22222222-2222-2222-2222-222222222222"), new DateTime(2025, 11, 17, 11, 37, 29, 0, DateTimeKind.Utc), "company2.com", "Company2", null }
                });

            migrationBuilder.InsertData(
                table: "AgeGroups",
                columns: new[] { "AgeGroupId", "MaxAge", "MinAge", "Status", "TenantId" },
                values: new object[,]
                {
                    { 1, 17, 0, true, new Guid("11111111-1111-1111-1111-111111111111") },
                    { 2, 75, 18, true, new Guid("11111111-1111-1111-1111-111111111111") },
                    { 3, 125, 76, true, new Guid("11111111-1111-1111-1111-111111111111") }
                });

            migrationBuilder.InsertData(
                table: "ApplicationSetting",
                columns: new[] { "Key", "LastModified", "TenantId", "Value" },
                values: new object[,]
                {
                    { "CompanyLogoURL", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("11111111-1111-1111-1111-111111111111"), "\\assets\\img\\logo.jpg" },
                    { "CompanyName", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("11111111-1111-1111-1111-111111111111"), "Hair Center" },
                    { "Currency", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("11111111-1111-1111-1111-111111111111"), "tr-TR" },
                    { "SupportedGenders", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("11111111-1111-1111-1111-111111111111"), "Male,Female" }
                });

            migrationBuilder.InsertData(
                table: "Branches",
                columns: new[] { "BranchId", "Address", "BranchEmail", "BranchName", "BranchPhoneNumber", "City", "Country", "District", "Neighbourhood", "ReservationInAdvanceDayLimit", "Status", "Street", "TenantId" },
                values: new object[,]
                {
                    { 1, "no 13 a", "businessmail@business.com", "Hacıhalil Şube", "+90 537 025 52 80", "Kocaeli", "Turkey", "Gebze", "Hacıhalil", 60, true, "Kızılay caddesi, 1203. Sk.", new Guid("11111111-1111-1111-1111-111111111111") },
                    { 2, "No:68", "businessmail@business.com", "Gebze Şube", "+90 537 025 52 80", "Kocaeli", "Turkey", "Gebze", "Osman Yılmaz", 60, false, "Kızılay Cd.", new Guid("11111111-1111-1111-1111-111111111111") }
                });

            migrationBuilder.InsertData(
                table: "OfferedServices",
                columns: new[] { "OfferedServiceId", "ApproximateDuration", "Genders", "OfferedServiceName", "Price", "Status", "TenantId" },
                values: new object[,]
                {
                    { 1, new TimeSpan(0, 0, 20, 0, 0), "[0]", "HairCut", 100m, true, new Guid("11111111-1111-1111-1111-111111111111") },
                    { 2, new TimeSpan(0, 0, 40, 0, 0), "[0]", "RazorShave", 200m, true, new Guid("11111111-1111-1111-1111-111111111111") },
                    { 3, new TimeSpan(0, 1, 0, 0, 0), "[0]", "HairColoring", 300m, true, new Guid("11111111-1111-1111-1111-111111111111") },
                    { 4, new TimeSpan(0, 1, 20, 0, 0), "[0,1]", "BrowShaping", 400m, true, new Guid("11111111-1111-1111-1111-111111111111") },
                    { 5, new TimeSpan(0, 1, 40, 0, 0), "[0,1]", "BeardGrooming", 500m, true, new Guid("11111111-1111-1111-1111-111111111111") },
                    { 6, new TimeSpan(0, 2, 0, 0, 0), "[0,1]", "ChildShave", 600m, true, new Guid("11111111-1111-1111-1111-111111111111") },
                    { 7, new TimeSpan(0, 2, 20, 0, 0), "[0,1]", "PermHair", 700m, false, new Guid("11111111-1111-1111-1111-111111111111") },
                    { 8, new TimeSpan(0, 2, 30, 0, 0), "[0,1]", "Manicure", 800m, false, new Guid("11111111-1111-1111-1111-111111111111") },
                    { 9, new TimeSpan(0, 2, 40, 0, 0), "[0,1]", "Pedicure", 900m, false, new Guid("11111111-1111-1111-1111-111111111111") },
                    { 10, new TimeSpan(0, 3, 0, 0, 0), "[0]", "GroomsCut", 1000m, true, new Guid("11111111-1111-1111-1111-111111111111") },
                    { 11, new TimeSpan(0, 3, 20, 0, 0), "[1]", "Makeup(Bride)", 1100m, false, new Guid("11111111-1111-1111-1111-111111111111") }
                });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "EmployeeId", "BranchId", "CanTakeClients", "DayOff", "EmployeeName", "EmployeeSurname", "EndOfWorkingHours", "IdentityUserId", "StartOfWorkingHours", "Status", "TenantId" },
                values: new object[,]
                {
                    { 1, 1, true, 0, "Aydın", "Sevim", new TimeSpan(0, 19, 0, 0, 0), "1", new TimeSpan(0, 10, 0, 0, 0), true, new Guid("11111111-1111-1111-1111-111111111111") },
                    { 2, 1, true, 0, "Alpay", "Aydıngüler", new TimeSpan(0, 19, 0, 0, 0), "2", new TimeSpan(0, 10, 0, 0, 0), true, new Guid("11111111-1111-1111-1111-111111111111") },
                    { 3, 1, false, 0, "Deniz", "Dağ", new TimeSpan(0, 19, 0, 0, 0), "3", new TimeSpan(0, 10, 0, 0, 0), true, new Guid("11111111-1111-1111-1111-111111111111") }
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
                columns: new[] { "OfferedServiceLocalizationId", "Language", "OfferedServiceId", "OfferedServiceLocalizationName", "TenantId" },
                values: new object[,]
                {
                    { 1, "en-GB", 1, "Hair Cut", new Guid("11111111-1111-1111-1111-111111111111") },
                    { 2, "tr-TR", 1, "Saç Kesimi", new Guid("11111111-1111-1111-1111-111111111111") },
                    { 3, "en-GB", 2, "Razor Shave", new Guid("11111111-1111-1111-1111-111111111111") },
                    { 4, "tr-TR", 2, "Jilet Traşı", new Guid("11111111-1111-1111-1111-111111111111") },
                    { 5, "en-GB", 3, "Hair Coloring", new Guid("11111111-1111-1111-1111-111111111111") },
                    { 6, "tr-TR", 3, "Saç Boyama", new Guid("11111111-1111-1111-1111-111111111111") },
                    { 7, "en-GB", 4, "Brow Shaping", new Guid("11111111-1111-1111-1111-111111111111") },
                    { 8, "tr-TR", 4, "Kaş Şekillendirme", new Guid("11111111-1111-1111-1111-111111111111") },
                    { 9, "en-GB", 5, "Beard Grooming", new Guid("11111111-1111-1111-1111-111111111111") },
                    { 10, "tr-TR", 5, "Sakal Bakımı", new Guid("11111111-1111-1111-1111-111111111111") },
                    { 11, "en-GB", 6, "Child Shave", new Guid("11111111-1111-1111-1111-111111111111") },
                    { 12, "tr-TR", 6, "Çocuk Tıraşı", new Guid("11111111-1111-1111-1111-111111111111") },
                    { 13, "en-GB", 7, "Perm Hair", new Guid("11111111-1111-1111-1111-111111111111") },
                    { 14, "tr-TR", 7, "Perma Saç", new Guid("11111111-1111-1111-1111-111111111111") },
                    { 15, "en-GB", 8, "Manicure", new Guid("11111111-1111-1111-1111-111111111111") },
                    { 16, "tr-TR", 8, "Manikür", new Guid("11111111-1111-1111-1111-111111111111") },
                    { 17, "en-GB", 9, "Pedicure", new Guid("11111111-1111-1111-1111-111111111111") },
                    { 18, "tr-TR", 9, "Pedikür", new Guid("11111111-1111-1111-1111-111111111111") },
                    { 19, "en-GB", 10, "Groom's Cut", new Guid("11111111-1111-1111-1111-111111111111") },
                    { 20, "tr-TR", 10, "Damat Kesimi", new Guid("11111111-1111-1111-1111-111111111111") },
                    { 21, "en-GB", 11, "Makeup (Bride)", new Guid("11111111-1111-1111-1111-111111111111") },
                    { 22, "tr-TR", 11, "Makyaj (Gelin)", new Guid("11111111-1111-1111-1111-111111111111") }
                });

            migrationBuilder.InsertData(
                table: "CustomerAppointments",
                columns: new[] { "CustomerAppointmentId", "AgeGroupId", "ApproximateDuration", "BranchId", "CreatedBy", "EMail", "EmployeeId", "Gender", "Name", "PhoneNumber", "Price", "StartDateTime", "Status", "Surname", "TenantId" },
                values: new object[,]
                {
                    { 1, 1, new TimeSpan(0, 0, 30, 0, 0), 1, null, "alice.smith@example.com", 1, 1, "Alice", "+90 123 456 7891", 150m, new DateTime(2026, 1, 18, 10, 0, 0, 0, DateTimeKind.Local), 1, "Smith", new Guid("11111111-1111-1111-1111-111111111111") },
                    { 2, 2, new TimeSpan(0, 0, 45, 0, 0), 1, null, "bob.johnson@example.com", 2, 0, "Bob", "+90 123 456 7892", 200m, new DateTime(2026, 1, 18, 11, 30, 0, 0, DateTimeKind.Local), 0, "Johnson", new Guid("11111111-1111-1111-1111-111111111111") },
                    { 3, 3, new TimeSpan(0, 1, 0, 0, 0), 1, null, "charlie.brown@example.com", 1, 0, "Charlie", "+90 123 456 7893", 250m, new DateTime(2026, 1, 18, 14, 0, 0, 0, DateTimeKind.Local), 3, "Brown", new Guid("11111111-1111-1111-1111-111111111111") },
                    { 4, 2, new TimeSpan(0, 0, 40, 0, 0), 1, null, "diana.prince@example.com", 1, 1, "Diana", "+90 123 456 7894", 180m, new DateTime(2026, 1, 18, 9, 45, 0, 0, DateTimeKind.Local), 2, "Prince", new Guid("11111111-1111-1111-1111-111111111111") },
                    { 5, 1, new TimeSpan(0, 0, 35, 0, 0), 1, null, "eve.adams@example.com", 2, 1, "Eve", "+90 123 456 7895", 160m, new DateTime(2026, 1, 18, 16, 15, 0, 0, DateTimeKind.Local), 1, "Adams", new Guid("11111111-1111-1111-1111-111111111111") },
                    { 6, 2, new TimeSpan(0, 0, 30, 0, 0), 1, null, "frank.miller@example.com", 3, 0, "Frank", "+90 123 456 7896", 120m, new DateTime(2026, 1, 16, 12, 30, 0, 0, DateTimeKind.Local), 0, "Miller", new Guid("11111111-1111-1111-1111-111111111111") },
                    { 7, 1, new TimeSpan(0, 1, 15, 0, 0), 1, null, "grace.hall@example.com", 2, 1, "Grace", "+90 123 456 7897", 450m, new DateTime(2026, 1, 16, 15, 0, 0, 0, DateTimeKind.Local), 1, "Hall", new Guid("11111111-1111-1111-1111-111111111111") },
                    { 8, 3, new TimeSpan(0, 1, 45, 0, 0), 1, null, "henry.ford@example.com", 1, 0, "Henry", "+90 123 456 7898", 700m, new DateTime(2026, 1, 16, 14, 30, 0, 0, DateTimeKind.Local), 3, "Ford", new Guid("11111111-1111-1111-1111-111111111111") },
                    { 9, 3, new TimeSpan(0, 0, 45, 0, 0), 1, null, "isabelle.clark@example.com", 3, 1, "Isabelle", "+90 123 456 7899", 250m, new DateTime(2026, 1, 16, 10, 0, 0, 0, DateTimeKind.Local), 2, "Clark", new Guid("11111111-1111-1111-1111-111111111111") },
                    { 10, 1, new TimeSpan(0, 1, 0, 0, 0), 1, null, "jack.white@example.com", 2, 0, "Jack", "+90 123 456 7890", 300m, new DateTime(2026, 1, 17, 9, 15, 0, 0, DateTimeKind.Local), 0, "White", new Guid("11111111-1111-1111-1111-111111111111") }
                });

            migrationBuilder.InsertData(
                table: "EmployeeLeaves",
                columns: new[] { "EmployeeLeaveId", "EmployeeId", "LeaveEndDateTime", "LeaveStartDateTime", "Reason", "TenantId" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2026, 1, 16, 18, 0, 0, 0, DateTimeKind.Local), new DateTime(2026, 1, 16, 8, 0, 0, 0, DateTimeKind.Local), "Sick", new Guid("11111111-1111-1111-1111-111111111111") },
                    { 2, 2, new DateTime(2026, 1, 17, 18, 0, 0, 0, DateTimeKind.Local), new DateTime(2026, 1, 17, 8, 0, 0, 0, DateTimeKind.Local), "Vacation", new Guid("11111111-1111-1111-1111-111111111111") },
                    { 3, 3, new DateTime(2026, 1, 18, 18, 0, 0, 0, DateTimeKind.Local), new DateTime(2026, 1, 18, 8, 0, 0, 0, DateTimeKind.Local), "Personal", new Guid("11111111-1111-1111-1111-111111111111") }
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
                name: "IX_AgeGroups_TenantId",
                table: "AgeGroups",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationSetting_TenantId",
                table: "ApplicationSetting",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_PhoneNumber",
                table: "AspNetUsers",
                column: "PhoneNumber",
                unique: true,
                filter: "[PhoneNumber] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_TenantId",
                table: "AspNetUsers",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_BookingFlowConfigs_BranchId",
                table: "BookingFlowConfigs",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_BookingFlowConfigs_TenantId_BranchId",
                table: "BookingFlowConfigs",
                columns: new[] { "TenantId", "BranchId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Branches_TenantId",
                table: "Branches",
                column: "TenantId");

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
                name: "IX_CustomerAppointments_EmployeeId",
                table: "CustomerAppointments",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerAppointments_TenantId",
                table: "CustomerAppointments",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeLeaves_EmployeeId",
                table: "EmployeeLeaves",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeLeaves_TenantId",
                table: "EmployeeLeaves",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeOfferedServices_OfferedServicesId",
                table: "EmployeeOfferedServices",
                column: "OfferedServicesId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_BranchId",
                table: "Employees",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_TenantId",
                table: "Employees",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_OfferedServiceAgeGroups_OfferedServiceId",
                table: "OfferedServiceAgeGroups",
                column: "OfferedServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_OfferedServiceLocalizations_OfferedServiceId",
                table: "OfferedServiceLocalizations",
                column: "OfferedServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_OfferedServiceLocalizations_TenantId",
                table: "OfferedServiceLocalizations",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_OfferedServices_TenantId",
                table: "OfferedServices",
                column: "TenantId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationSetting");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "BookingFlowConfigs");

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
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "CustomerAppointments");

            migrationBuilder.DropTable(
                name: "OfferedServices");

            migrationBuilder.DropTable(
                name: "AgeGroups");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "Branches");

            migrationBuilder.DropTable(
                name: "Tenants");
        }
    }
}
