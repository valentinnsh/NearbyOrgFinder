using System.Text.RegularExpressions;
using Database.Records;
using NetTopologySuite;
using NetTopologySuite.Geometries;

namespace Database.Parsers;

public class SchoolDataParser
{
    static List<SchoolRecord> ParseCsvFile(string filePath)
    {
        var records = new List<SchoolRecord>();
        var lines = File.ReadAllLines(filePath);

        var headers = Regex.Split(lines[0], ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)");

        var nameIndex = Array.IndexOf(headers, "Наименование");
        var descriptionIndex = Array.IndexOf(headers, "Описание");
        var addressIndex = Array.IndexOf(headers, "Адрес");
        var addressCommentIndex = Array.IndexOf(headers, "Комментарий к адресу");
        var mailIndexIndex = Array.IndexOf(headers, "Почтовый индекс");
        var districtIndex = Array.IndexOf(headers, "Район");
        var cityIdIndex = Array.IndexOf(headers, "Город");
        var okrugIndex = Array.IndexOf(headers, "Округ");
        var regionIndex = Array.IndexOf(headers, "Регион");
        var countryIndex = Array.IndexOf(headers, "Страна");
        var workingHoursIndex = Array.IndexOf(headers, "Часы работы");
        var timezoneIndex = Array.IndexOf(headers, "Часовой пояс");
        var ratingIndex = Array.IndexOf(headers, "Рейтинг");
        var latitudeIndex = Array.IndexOf(headers, "Широта");
        var longitudeIndex = Array.IndexOf(headers, "Долгота");
        var twoGisUrlIndex = Array.IndexOf(headers, "2GIS URL");

        var dataLines = lines.Skip(1);

        foreach (var line in dataLines)
        {
            var values = Regex.Split(line, ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)");
            var record = new SchoolRecord
            {
                Name = values[nameIndex],
                Description = values[descriptionIndex],
                Address = values[addressIndex],
                AddressComment = values[addressCommentIndex],
                MailIndex = values[mailIndexIndex],
                District = values[districtIndex],
                CityId = 1,
                Okrug = values[okrugIndex],
                Region = values[regionIndex],
                Country = values[countryIndex],
                WorkingHours = values[workingHoursIndex],
                Timezone = values[timezoneIndex],
                TwoGisUrl = values[twoGisUrlIndex]
            };

            double latitude = Convert.ToDouble(values[latitudeIndex]);
            double longitude = Convert.ToDouble(values[longitudeIndex]);
            record.Location = new Point(longitude, latitude);

            try
            {
                record.Rating = Convert.ToDouble(values[ratingIndex]);
            }
            catch (Exception e)
            {
                Console.WriteLine($"No rating for {record.Name} {record.Id}");
            }

            records.Add(record);
        }

        return records;
    }
    
    Point CreatePoint(double latitude, double longitude)
    {
        var geometryFactory =
            NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326); // WGS84 coordinate system (EPSG:4326)
        return geometryFactory.CreatePoint(new Coordinate(longitude, latitude));
    }

    static string GenerateSeedMigration(List<SchoolRecord> records)
    {
        var code = new System.Text.StringBuilder();

        code.AppendLine("modelBuilder.Entity<SchoolRecord>().HasData(");

        double? a = 5D;
        foreach (var record in records)
        {
            string rating = record.Rating != null ? record.Rating.ToString() : "null";
            code.AppendLine($"    new");
            code.AppendLine("    {");
            code.AppendLine($"        Id = {records.IndexOf(record) + 1}L,");
            code.AppendLine($"        Name = \"{record.Name.Replace("\"", "")}\",");
            code.AppendLine($"        Description = \"{record.Description?.Replace("\"", "")}\",");
            code.AppendLine($"        Address = \"{record.Address.Replace("\"", "")}\",");
            code.AppendLine($"        Location = new Point({record.Location.X}, {record.Location.Y}),");
            code.AppendLine($"        AddressComment = \"{record.AddressComment?.Replace("\"", "")}\",");
            code.AppendLine($"        MailIndex = \"{record.MailIndex?.Replace("\"", "")}\",");
            code.AppendLine($"        District = \"{record.District?.Replace("\"", "")}\",");
            code.AppendLine($"        CityId = {record.CityId}L,");
            code.AppendLine($"        Okrug = \"{record.Okrug?.Replace("\"", "")}\",");
            code.AppendLine($"        Region = \"{record.Region?.Replace("\"", "")}\",");
            code.AppendLine($"        Country = \"{record.Country?.Replace("\"", "")}\",");
            code.AppendLine($"        WorkingHours = \"{record.WorkingHours?.Replace("\"", "")}\",");
            code.AppendLine($"        Timezone = \"{record.Timezone?.Replace("\"", "")}\",");
            if (record.Rating is not null) code.AppendLine($"        Rating = {rating}D,");
            code.AppendLine($"        TwoGisUrl = \"{record.TwoGisUrl?.Replace("\"", "")}\"");
            code.AppendLine("    },");
        }

        code.AppendLine(");");

        return code.ToString();
    }

    public static void ParseSchools()
    {
        string filePath = "/home/valentine/dev/MyGeoApp/Data/shools_spb.csv";
        List<SchoolRecord> records = ParseCsvFile(filePath);
        string seedMigration = GenerateSeedMigration(records);

        Console.WriteLine(seedMigration);

        string newFilePAth = "/home/valentine/dev/MyGeoApp/Data/shools_spb_seed.txt";

        using (StreamWriter writer = new StreamWriter(newFilePAth))
        {
            writer.Write(seedMigration);
        }
    }
}