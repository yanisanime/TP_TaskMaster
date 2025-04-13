using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace taskMasterProjet.Migrations
{
    /// <inheritdoc />
    public partial class MigrationAddProject : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProjetId",
                table: "Taches",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Projets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nom = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreateurId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Projets_Utilisateurs_CreateurId",
                        column: x => x.CreateurId,
                        principalTable: "Utilisateurs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Taches_ProjetId",
                table: "Taches",
                column: "ProjetId");

            migrationBuilder.CreateIndex(
                name: "IX_Projets_CreateurId",
                table: "Projets",
                column: "CreateurId");

            migrationBuilder.AddForeignKey(
                name: "FK_Taches_Projets_ProjetId",
                table: "Taches",
                column: "ProjetId",
                principalTable: "Projets",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Taches_Projets_ProjetId",
                table: "Taches");

            migrationBuilder.DropTable(
                name: "Projets");

            migrationBuilder.DropIndex(
                name: "IX_Taches_ProjetId",
                table: "Taches");

            migrationBuilder.DropColumn(
                name: "ProjetId",
                table: "Taches");
        }
    }
}
