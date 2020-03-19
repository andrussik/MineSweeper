using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class InitialDbCreation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GameBoards",
                columns: table => new
                {
                    GameBoardId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    BoardName = table.Column<string>(maxLength: 255, nullable: false),
                    JsonString = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameBoards", x => x.GameBoardId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GameBoards");
        }
    }
}
