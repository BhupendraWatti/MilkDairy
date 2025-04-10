using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MIlkDairy.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class addingMilkTypeAndadditionalInfo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MilkSubTypes",
                columns: table => new
                {
                    MilkId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MilkType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MilkName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FatContent = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProteinContent = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsOrganic = table.Column<bool>(type: "bit", nullable: false),
                    PackagingType = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MilkSubTypes", x => x.MilkId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MilkSubTypes");
        }
    }
}
