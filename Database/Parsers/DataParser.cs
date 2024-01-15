using System.Text.RegularExpressions;
using Database.Records;
using NetTopologySuite.Geometries;

namespace Database.Parsers;

public static class DataParser
{
    static string GenerateMigration(List<OrganizationRecord> records, string organizationType, int firstId)
    {
        var code = new System.Text.StringBuilder();
        code.AppendLine("using Database.Entities;");
        code.AppendLine("using Database.Records;");
        code.AppendLine("using Microsoft.EntityFrameworkCore;");
        code.AppendLine("using NetTopologySuite.Geometries;");
        code.AppendLine("");
        code.AppendLine("namespace Database;");
        code.AppendLine("");
        code.AppendLine($"public static class Seed{organizationType}");
        code.AppendLine("{");
        code.AppendLine($"\t public static void FullSeed{organizationType}(ModelBuilder modelBuilder)");
        code.AppendLine("{");
        code.AppendLine($"\t\t modelBuilder.Entity<OrganizationEntity>().HasData(");
        foreach (var record in records)
        {
            string rating = record.Rating != null ? record.Rating.ToString() : "null";
            code.AppendLine($"    new");
            code.AppendLine("    {");
            code.AppendLine($"        Id = {records.IndexOf(record) + firstId}L,");
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
            code.AppendLine($"        TwoGisUrl = \"{record.TwoGisUrl?.Replace("\"", "")}\",");
            code.AppendLine($"        Type = OrganizationTypes.{organizationType}");
            code.AppendLine(!ReferenceEquals(record, records.LastOrDefault()) ? "    }," : "    }");
        }

        code.AppendLine(");");
        code.AppendLine("}}");

        return code.ToString();
    }
    
    public static List<OrganizationRecord> ParseCsvFile(string filePath)
    {
        var records = new List<OrganizationRecord>();
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
            var record = new OrganizationRecord
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

            try
            {
                double latitude = Convert.ToDouble(values[latitudeIndex]);
                double longitude = Convert.ToDouble(values[longitudeIndex]);
                record.Location = new Point(longitude, latitude);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                continue;
            }
            

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
    
    public static void GeneratePharmaciesSeeder()
    {
        string filePath = "/home/valentine/dev/NearbyOrgFinder/Data/pharm_spb.csv";
        List<OrganizationRecord> records = ParseCsvFile(filePath);
        string seedMigration = GenerateMigration(records, OrganizationTypes.Pharmacy.ToString(), 581);

        Console.WriteLine(seedMigration);

        string newFilePAth = "/home/valentine/dev/NearbyOrgFinder/Database/SeedPharmacies.cs";

        using (StreamWriter writer = new StreamWriter(newFilePAth))
        {
            writer.Write(seedMigration);
        }
    }
    
    public static void GenerateVetsSeeder()
    {
        string filePath = "/home/valentine/dev/NearbyOrgFinder/Data/veterinary_spb.csv";
        List<OrganizationRecord> records = ParseCsvFile(filePath);
        string seedMigration = GenerateMigration(records, OrganizationTypes.Vet.ToString(), 2257);

        Console.WriteLine(seedMigration);

        string newFilePAth = "/home/valentine/dev/NearbyOrgFinder/Database/SeedVets.cs";

        using (StreamWriter writer = new StreamWriter(newFilePAth))
        {
            writer.Write(seedMigration);
        }
    }
}