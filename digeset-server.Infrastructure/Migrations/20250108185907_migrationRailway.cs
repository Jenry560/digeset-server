using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace digeset_server.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class migrationRailway : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Agente",
                columns: table => new
                {
                    AgenteId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Cedula = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Clave = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Estado = table.Column<bool>(type: "boolean", nullable: false),
                    UsuarioId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Agente__EA09D85DF09E5DC3", x => x.AgenteId);
                });

            migrationBuilder.CreateTable(
                name: "Concepto",
                columns: table => new
                {
                    ConceptoId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Descripcion = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Monto = table.Column<double>(type: "numeric(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Concepto__BB30F135A80A6E3B", x => x.ConceptoId);
                });

            migrationBuilder.CreateTable(
                name: "Usuario",
                columns: table => new
                {
                    UsuarioId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Cedula = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Clave = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Usuario__2B3DE7B84F7C7FC9", x => x.UsuarioId);
                });

            migrationBuilder.CreateTable(
                name: "Multa",
                columns: table => new
                {
                    MultaId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Cedula = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ConceptoId = table.Column<int>(type: "integer", nullable: false),
                    AgenteId = table.Column<int>(type: "integer", nullable: false),
                    Descripcion = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Latitud = table.Column<decimal>(type: "numeric(9,6)", nullable: false),
                    Longitud = table.Column<decimal>(type: "numeric(9,6)", nullable: false),
                    Foto = table.Column<byte[]>(type: "bytea", nullable: true),
                    FechaCreacion = table.Column<DateTime>(type: "timestamp", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    EstadoId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Multa__DA090DE0F9E588A4", x => x.MultaId);
                    table.ForeignKey(
                        name: "FK__Multa__AgenteId__44FF419A",
                        column: x => x.AgenteId,
                        principalTable: "Agente",
                        principalColumn: "AgenteId");
                    table.ForeignKey(
                        name: "FK__Multa__ConceptoI__4316F928",
                        column: x => x.ConceptoId,
                        principalTable: "Concepto",
                        principalColumn: "ConceptoId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Multa_AgenteId",
                table: "Multa",
                column: "AgenteId");

            migrationBuilder.CreateIndex(
                name: "IX_Multa_ConceptoId",
                table: "Multa",
                column: "ConceptoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Multa");

            migrationBuilder.DropTable(
                name: "Usuario");

            migrationBuilder.DropTable(
                name: "Agente");

            migrationBuilder.DropTable(
                name: "Concepto");
        }
    }
}
