using Microsoft.EntityFrameworkCore.Migrations;

namespace MoviesMicroservice.Data.Migrations
{
    public partial class ImdbId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "TmdbId",
                table: "Movies",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TmdbId",
                table: "Movies");
        }
    }
}
