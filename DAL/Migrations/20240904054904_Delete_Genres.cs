using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class Delete_Genres : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MovieGenre_Genres_GenreId",
                table: "MovieGenre");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Genres",
                table: "Genres");

            migrationBuilder.RenameTable(
                name: "Genres",
                newName: "TranslatableEntity");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "TranslatableEntity",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(64)",
                oldMaxLength: 64);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "TranslatableEntity",
                type: "nvarchar(21)",
                maxLength: 21,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TranslatableEntity",
                table: "TranslatableEntity",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "TranslatableEntityField",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TranslatableEntityId = table.Column<int>(type: "int", nullable: false),
                    LanguageCode = table.Column<int>(type: "int", nullable: false),
                    FieldType = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TranslatableEntityField", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TranslatableEntityField_TranslatableEntity_TranslatableEntityId",
                        column: x => x.TranslatableEntityId,
                        principalTable: "TranslatableEntity",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_TranslatableEntityField_TranslatableEntityId",
                table: "TranslatableEntityField",
                column: "TranslatableEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_MovieGenre_TranslatableEntity_GenreId",
                table: "MovieGenre",
                column: "GenreId",
                principalTable: "TranslatableEntity",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MovieGenre_TranslatableEntity_GenreId",
                table: "MovieGenre");

            migrationBuilder.DropTable(
                name: "TranslatableEntityField");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TranslatableEntity",
                table: "TranslatableEntity");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "TranslatableEntity");

            migrationBuilder.RenameTable(
                name: "TranslatableEntity",
                newName: "Genres");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Genres",
                type: "nvarchar(64)",
                maxLength: 64,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Genres",
                table: "Genres",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MovieGenre_Genres_GenreId",
                table: "MovieGenre",
                column: "GenreId",
                principalTable: "Genres",
                principalColumn: "Id");
        }
    }
}
