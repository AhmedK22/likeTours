using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LikeTours.Migrations
{
    /// <inheritdoc />
    public partial class db_init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AboutUs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MainImage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WhoAreImage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WhoAreText = table.Column<string>(type: "Text", nullable: false),
                    Lang = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Main = table.Column<bool>(type: "bit", nullable: false),
                    AboutId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AboutUs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AboutUs_AboutUs_AboutId",
                        column: x => x.AboutId,
                        principalTable: "AboutUs",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Contacts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContactWay = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contacts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Desc = table.Column<string>(type: "text", nullable: false),
                    Lang = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Main = table.Column<bool>(type: "bit", nullable: false),
                    PaymentId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Payments_Payments_PaymentId",
                        column: x => x.PaymentId,
                        principalTable: "Payments",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Places",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Lang = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Main = table.Column<bool>(type: "bit", nullable: false),
                    PlaceId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Places", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Places_Places_PlaceId",
                        column: x => x.PlaceId,
                        principalTable: "Places",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Questions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Question = table.Column<string>(type: "text", nullable: false),
                    Answer = table.Column<string>(type: "text", nullable: false),
                    Lang = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Main = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    QuestionId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Questions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Questions_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Types",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Lang = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Main = table.Column<bool>(type: "bit", nullable: false),
                    TourTypeId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Types", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Types_Types_TourTypeId",
                        column: x => x.TourTypeId,
                        principalTable: "Types",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Packages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Lang = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AdultAgeFrom = table.Column<int>(type: "int", nullable: true),
                    PriceAdult = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    AdultFinalTo = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ChildAgeFrom = table.Column<int>(type: "int", nullable: true),
                    ChildFinalTo = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PriceChild = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DaysOfWeek = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EstimateDuration = table.Column<int>(type: "int", nullable: false),
                    PlaceId = table.Column<int>(type: "int", nullable: false),
                    TourTypeId = table.Column<int>(type: "int", nullable: false),
                    Main = table.Column<bool>(type: "bit", nullable: false),
                    PackageId = table.Column<int>(type: "int", nullable: true),
                    HasSale = table.Column<bool>(type: "bit", nullable: false),
                    SaleType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SaleAmount = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Packages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Packages_Packages_PackageId",
                        column: x => x.PackageId,
                        principalTable: "Packages",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Packages_Places_PlaceId",
                        column: x => x.PlaceId,
                        principalTable: "Places",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Packages_Types_TourTypeId",
                        column: x => x.TourTypeId,
                        principalTable: "Types",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Images",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PackageId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Images", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Images_Packages_PackageId",
                        column: x => x.PackageId,
                        principalTable: "Packages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Reviews",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Header = table.Column<string>(type: "text", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Body = table.Column<string>(type: "text", nullable: false),
                    TypeId = table.Column<int>(type: "int", nullable: false),
                    PackageId = table.Column<int>(type: "int", nullable: false),
                    IsConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    Lang = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Main = table.Column<bool>(type: "bit", nullable: false),
                    ReviewId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reviews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reviews_Packages_PackageId",
                        column: x => x.PackageId,
                        principalTable: "Packages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reviews_Reviews_ReviewId",
                        column: x => x.ReviewId,
                        principalTable: "Reviews",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Sections",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SectionDetails = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PackageId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sections_Packages_PackageId",
                        column: x => x.PackageId,
                        principalTable: "Packages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AboutUs_AboutId",
                table: "AboutUs",
                column: "AboutId");

            migrationBuilder.CreateIndex(
                name: "IX_Images_PackageId",
                table: "Images",
                column: "PackageId");

            migrationBuilder.CreateIndex(
                name: "IX_Packages_Lang_PackageId",
                table: "Packages",
                columns: new[] { "Lang", "PackageId" },
                unique: true,
                filter: "[PackageId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Packages_PackageId",
                table: "Packages",
                column: "PackageId");

            migrationBuilder.CreateIndex(
                name: "IX_Packages_PlaceId",
                table: "Packages",
                column: "PlaceId");

            migrationBuilder.CreateIndex(
                name: "IX_Packages_TourTypeId",
                table: "Packages",
                column: "TourTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_Lang_PaymentId",
                table: "Payments",
                columns: new[] { "Lang", "PaymentId" },
                unique: true,
                filter: "[PaymentId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_PaymentId",
                table: "Payments",
                column: "PaymentId");

            migrationBuilder.CreateIndex(
                name: "IX_Places_Lang_PlaceId",
                table: "Places",
                columns: new[] { "Lang", "PlaceId" },
                unique: true,
                filter: "[PlaceId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Places_Name",
                table: "Places",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Places_PlaceId",
                table: "Places",
                column: "PlaceId");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_Lang_QuestionId",
                table: "Questions",
                columns: new[] { "Lang", "QuestionId" },
                unique: true,
                filter: "[QuestionId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_QuestionId",
                table: "Questions",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_Lang_ReviewId",
                table: "Reviews",
                columns: new[] { "Lang", "ReviewId" },
                unique: true,
                filter: "[ReviewId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_PackageId",
                table: "Reviews",
                column: "PackageId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_ReviewId",
                table: "Reviews",
                column: "ReviewId");

            migrationBuilder.CreateIndex(
                name: "IX_Sections_PackageId",
                table: "Sections",
                column: "PackageId");

            migrationBuilder.CreateIndex(
                name: "IX_Types_Lang_TourTypeId",
                table: "Types",
                columns: new[] { "Lang", "TourTypeId" },
                unique: true,
                filter: "[TourTypeId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Types_Name",
                table: "Types",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Types_TourTypeId",
                table: "Types",
                column: "TourTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AboutUs");

            migrationBuilder.DropTable(
                name: "Contacts");

            migrationBuilder.DropTable(
                name: "Images");

            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DropTable(
                name: "Questions");

            migrationBuilder.DropTable(
                name: "Reviews");

            migrationBuilder.DropTable(
                name: "Sections");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Packages");

            migrationBuilder.DropTable(
                name: "Places");

            migrationBuilder.DropTable(
                name: "Types");
        }
    }
}
