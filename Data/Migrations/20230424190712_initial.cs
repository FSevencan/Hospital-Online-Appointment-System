using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hastane.Data.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Departmanlar",
                columns: table => new
                {
                    DepartmanId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DepartmanAdı = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DepartmanFotograf = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DepartmanAcıklama = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departmanlar", x => x.DepartmanId);
                });

            migrationBuilder.CreateTable(
                name: "Doktorlar",
                columns: table => new
                {
                    DoktorID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AdSoyad = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DoktorFotograf = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DoktorAcıklama = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DepartmanId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Doktorlar", x => x.DoktorID);
                    table.ForeignKey(
                        name: "FK_Doktorlar_Departmanlar_DepartmanId",
                        column: x => x.DepartmanId,
                        principalTable: "Departmanlar",
                        principalColumn: "DepartmanId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Randevular",
                columns: table => new
                {
                    RandevuId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DoktorID = table.Column<int>(type: "int", nullable: false),
                    Tarih = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AdSoyad = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Telefon = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TcKimlik = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DoktorSaatId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Randevular", x => x.RandevuId);
                    table.ForeignKey(
                        name: "FK_Randevular_Doktorlar_DoktorID",
                        column: x => x.DoktorID,
                        principalTable: "Doktorlar",
                        principalColumn: "DoktorID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RandevuTakvimleri",
                columns: table => new
                {
                    RandevuTakvimiId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DoktorId = table.Column<int>(type: "int", nullable: false),
                    Tarih = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Saat9 = table.Column<bool>(type: "bit", nullable: false),
                    Saat10 = table.Column<bool>(type: "bit", nullable: false),
                    Saat11 = table.Column<bool>(type: "bit", nullable: false),
                    Saat13 = table.Column<bool>(type: "bit", nullable: false),
                    Saat14 = table.Column<bool>(type: "bit", nullable: false),
                    Saat15 = table.Column<bool>(type: "bit", nullable: false),
                    Saat16 = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RandevuTakvimleri", x => x.RandevuTakvimiId);
                    table.ForeignKey(
                        name: "FK_RandevuTakvimleri_Doktorlar_DoktorId",
                        column: x => x.DoktorId,
                        principalTable: "Doktorlar",
                        principalColumn: "DoktorID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Doktorlar_DepartmanId",
                table: "Doktorlar",
                column: "DepartmanId");

            migrationBuilder.CreateIndex(
                name: "IX_Randevular_DoktorID",
                table: "Randevular",
                column: "DoktorID");

            migrationBuilder.CreateIndex(
                name: "IX_RandevuTakvimleri_DoktorId",
                table: "RandevuTakvimleri",
                column: "DoktorId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Randevular");

            migrationBuilder.DropTable(
                name: "RandevuTakvimleri");

            migrationBuilder.DropTable(
                name: "Doktorlar");

            migrationBuilder.DropTable(
                name: "Departmanlar");
        }
    }
}
