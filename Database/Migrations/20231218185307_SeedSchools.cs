using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Database.Migrations
{
    /// <inheritdoc />
    public partial class SeedSchools : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "geo_data",
                table: "schools",
                columns: new[] { "id", "address", "address_comment", "city_id", "country", "description", "district", "location", "mail_index", "name", "okrug", "rating", "region", "timezone", "two_gis_url", "working_hours" },
                values: new object[,]
                {
                    { 1L, "Большая Пушкарская, 35", "", 1L, "Россия", "частная школа", "Петроградский район", (NetTopologySuite.Geometries.Point)new NetTopologySuite.IO.WKTReader().Read("SRID=0;POINT (30.308695 59.961798)"), "197101", "Крылья", "", 5.0, "Санкт-Петербург", "+03:00", "https://2gis.com/firm/70000001068931674", "Пн: 08:00-20:00; Вт: 08:00-20:00; Ср: 08:00-20:00; Чт: 08:00-20:00; Пт: 08:00-20:00" },
                    { 2L, "Средний проспект В.О., 28", "2 этаж", 1L, "Россия", "частная школа", "Василеостровский район", (NetTopologySuite.Geometries.Point)new NetTopologySuite.IO.WKTReader().Read("SRID=0;POINT (30.279865 59.942844)"), "199004", "Ювента", "", 4.0, "Санкт-Петербург", "+03:00", "https://2gis.com/firm/5348552838706876", "Пн: 09:00-18:00; Вт: 09:00-18:00; Ср: 09:00-18:00; Чт: 09:00-18:00; Пт: 09:00-18:00" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "geo_data",
                table: "schools",
                keyColumn: "id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                schema: "geo_data",
                table: "schools",
                keyColumn: "id",
                keyValue: 2L);
        }
    }
}
