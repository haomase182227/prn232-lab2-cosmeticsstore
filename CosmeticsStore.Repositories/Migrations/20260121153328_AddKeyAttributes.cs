using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CosmeticsStore.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class AddKeyAttributes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CosmeticCategory",
                columns: table => new
                {
                    CategoryID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    CategoryName = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    UsagePurpose = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    FormulationType = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Cosmetic__19093A2B806C6EC8", x => x.CategoryID);
                });

            migrationBuilder.CreateTable(
                name: "SystemAccount",
                columns: table => new
                {
                    AccountID = table.Column<int>(type: "int", nullable: false),
                    AccountPassword = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    EmailAddress = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    AccountNote = table.Column<string>(type: "nvarchar(240)", maxLength: 240, nullable: false),
                    Role = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__SystemAc__349DA5861F02AFCD", x => x.AccountID);
                });

            migrationBuilder.CreateTable(
                name: "CosmeticInformation",
                columns: table => new
                {
                    CosmeticID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    CosmeticName = table.Column<string>(type: "nvarchar(160)", maxLength: 160, nullable: false),
                    SkinType = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ExpirationDate = table.Column<string>(type: "nvarchar(160)", maxLength: 160, nullable: false),
                    CosmeticSize = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: false),
                    DollarPrice = table.Column<decimal>(type: "decimal(18,0)", nullable: false),
                    CategoryID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Cosmetic__98ED527EE44B0CFB", x => x.CosmeticID);
                    table.ForeignKey(
                        name: "FK__CosmeticI__Categ__3C69FB99",
                        column: x => x.CategoryID,
                        principalTable: "CosmeticCategory",
                        principalColumn: "CategoryID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CosmeticInformation_CategoryID",
                table: "CosmeticInformation",
                column: "CategoryID");

            migrationBuilder.CreateIndex(
                name: "UQ__SystemAc__49A147406F942315",
                table: "SystemAccount",
                column: "EmailAddress",
                unique: true,
                filter: "[EmailAddress] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CosmeticInformation");

            migrationBuilder.DropTable(
                name: "SystemAccount");

            migrationBuilder.DropTable(
                name: "CosmeticCategory");
        }
    }
}
