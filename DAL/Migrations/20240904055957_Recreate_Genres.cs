using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class Recreate_Genres : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MovieGenre_TranslatableEntity_GenreId",
                table: "MovieGenre");

            migrationBuilder.DropForeignKey(
                name: "FK_TranslatableEntityField_TranslatableEntity_TranslatableEntityId",
                table: "TranslatableEntityField");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TranslatableEntityField",
                table: "TranslatableEntityField");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TranslatableEntity",
                table: "TranslatableEntity");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "TranslatableEntity");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "TranslatableEntity");

            migrationBuilder.RenameTable(
                name: "TranslatableEntityField",
                newName: "TranslatableEntityFields");

            migrationBuilder.RenameTable(
                name: "TranslatableEntity",
                newName: "TranslatableEntities");

            migrationBuilder.RenameIndex(
                name: "IX_TranslatableEntityField_TranslatableEntityId",
                table: "TranslatableEntityFields",
                newName: "IX_TranslatableEntityFields_TranslatableEntityId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TranslatableEntityFields",
                table: "TranslatableEntityFields",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TranslatableEntities",
                table: "TranslatableEntities",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Genres",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genres", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Genres_TranslatableEntities_Id",
                        column: x => x.Id,
                        principalTable: "TranslatableEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_MovieGenre_Genres_GenreId",
                table: "MovieGenre",
                column: "GenreId",
                principalTable: "Genres",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TranslatableEntityFields_TranslatableEntities_TranslatableEntityId",
                table: "TranslatableEntityFields",
                column: "TranslatableEntityId",
                principalTable: "TranslatableEntities",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MovieGenre_Genres_GenreId",
                table: "MovieGenre");

            migrationBuilder.DropForeignKey(
                name: "FK_TranslatableEntityFields_TranslatableEntities_TranslatableEntityId",
                table: "TranslatableEntityFields");

            migrationBuilder.DropTable(
                name: "Genres");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TranslatableEntityFields",
                table: "TranslatableEntityFields");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TranslatableEntities",
                table: "TranslatableEntities");

            migrationBuilder.RenameTable(
                name: "TranslatableEntityFields",
                newName: "TranslatableEntityField");

            migrationBuilder.RenameTable(
                name: "TranslatableEntities",
                newName: "TranslatableEntity");

            migrationBuilder.RenameIndex(
                name: "IX_TranslatableEntityFields_TranslatableEntityId",
                table: "TranslatableEntityField",
                newName: "IX_TranslatableEntityField_TranslatableEntityId");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "TranslatableEntity",
                type: "nvarchar(21)",
                maxLength: 21,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "TranslatableEntity",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TranslatableEntityField",
                table: "TranslatableEntityField",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TranslatableEntity",
                table: "TranslatableEntity",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MovieGenre_TranslatableEntity_GenreId",
                table: "MovieGenre",
                column: "GenreId",
                principalTable: "TranslatableEntity",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TranslatableEntityField_TranslatableEntity_TranslatableEntityId",
                table: "TranslatableEntityField",
                column: "TranslatableEntityId",
                principalTable: "TranslatableEntity",
                principalColumn: "Id");
        }
    }
}
