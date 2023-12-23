using System.Text.RegularExpressions;
using Database.Records;
using NetTopologySuite;
using NetTopologySuite.Geometries;

namespace Database.Parsers;

public class SchoolDataParser
{
    static string GenerateSeedMigration(List<OrganizationRecord> records)
    {
        var code = new System.Text.StringBuilder();

        code.AppendLine("modelBuilder.Entity<SchoolRecord>().HasData(");

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
        List<OrganizationRecord> records = DataParser.ParseCsvFile(filePath);
        string seedMigration = GenerateSeedMigration(records);

        Console.WriteLine(seedMigration);

        string newFilePAth = "/home/valentine/dev/MyGeoApp/Data/shools_spb_seed.txt";

        using (StreamWriter writer = new StreamWriter(newFilePAth))
        {
            writer.Write(seedMigration);
        }
    }
}