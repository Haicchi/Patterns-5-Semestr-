using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Lab1.Migrations
{
    /// <inheritdoc />
    public partial class ConvertToUnidirectionalManyToMany : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NewspaperColumns_Newspapers_NewspaperId",
                table: "NewspaperColumns");

            migrationBuilder.DropTable(
                name: "AlmanacBooks");

            migrationBuilder.DropIndex(
                name: "IX_NewspaperColumns_NewspaperId",
                table: "NewspaperColumns");

            migrationBuilder.DropColumn(
                name: "NewspaperId",
                table: "NewspaperColumns");

            migrationBuilder.CreateTable(
                name: "AlmanacBooksJoin",
                columns: table => new
                {
                    AlmanacId = table.Column<int>(type: "integer", nullable: false),
                    BooksId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlmanacBooksJoin", x => new { x.AlmanacId, x.BooksId });
                    table.ForeignKey(
                        name: "FK_AlmanacBooksJoin_Almanacs_AlmanacId",
                        column: x => x.AlmanacId,
                        principalTable: "Almanacs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AlmanacBooksJoin_Books_BooksId",
                        column: x => x.BooksId,
                        principalTable: "Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NewspaperColumnsJoin",
                columns: table => new
                {
                    ColumnsId = table.Column<int>(type: "integer", nullable: false),
                    NewspaperId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NewspaperColumnsJoin", x => new { x.ColumnsId, x.NewspaperId });
                    table.ForeignKey(
                        name: "FK_NewspaperColumnsJoin_NewspaperColumns_ColumnsId",
                        column: x => x.ColumnsId,
                        principalTable: "NewspaperColumns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NewspaperColumnsJoin_Newspapers_NewspaperId",
                        column: x => x.NewspaperId,
                        principalTable: "Newspapers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AlmanacBooksJoin_BooksId",
                table: "AlmanacBooksJoin",
                column: "BooksId");

            migrationBuilder.CreateIndex(
                name: "IX_NewspaperColumnsJoin_NewspaperId",
                table: "NewspaperColumnsJoin",
                column: "NewspaperId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AlmanacBooksJoin");

            migrationBuilder.DropTable(
                name: "NewspaperColumnsJoin");

            migrationBuilder.AddColumn<int>(
                name: "NewspaperId",
                table: "NewspaperColumns",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "AlmanacBooks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AlmanacId = table.Column<int>(type: "integer", nullable: false),
                    Author = table.Column<string>(type: "text", nullable: false),
                    PageCount = table.Column<int>(type: "integer", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlmanacBooks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AlmanacBooks_Almanacs_AlmanacId",
                        column: x => x.AlmanacId,
                        principalTable: "Almanacs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NewspaperColumns_NewspaperId",
                table: "NewspaperColumns",
                column: "NewspaperId");

            migrationBuilder.CreateIndex(
                name: "IX_AlmanacBooks_AlmanacId",
                table: "AlmanacBooks",
                column: "AlmanacId");

            migrationBuilder.AddForeignKey(
                name: "FK_NewspaperColumns_Newspapers_NewspaperId",
                table: "NewspaperColumns",
                column: "NewspaperId",
                principalTable: "Newspapers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
