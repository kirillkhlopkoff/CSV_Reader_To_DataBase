using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using CsvHelper;
using CsvHelper.Configuration;
using System.Data.SqlClient;
using System.Formats.Asn1;

class Program
{
    static void Main(string[] args)
    {
        string csvFilePath = "C:\\Users\\user\\Desktop\\carriers.csv";
        string connectionString = "Data Source=.\\MSSQLSERVER01;Initial Catalog=SnetDB_UAT;User Id = sa; Password =sa";

        using (var reader = new StreamReader(csvFilePath))
        using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)))
        {
            var records = csv.GetRecords<Carrier>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                foreach (var record in records)
                {
                    string insertQuery = "INSERT INTO Carriers (Name, DID, Status, Region, SMS, CNAM, CreatedDate, ModifiedDate) VALUES " +
                     "(@Name, @DID, @Status, @Region, @SMS, @CNAM, @CreatedDate, @ModifiedDate)";

                    using (SqlCommand cmd = new SqlCommand(insertQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("@Name", record.Name);
                        cmd.Parameters.AddWithValue("@DID", record.DID);
                        cmd.Parameters.AddWithValue("@Status", record.Status);
                        cmd.Parameters.AddWithValue("@Region", record.Region);
                        cmd.Parameters.AddWithValue("@SMS", record.SMS);
                        cmd.Parameters.AddWithValue("@CNAM", record.CNAM);
                        cmd.Parameters.AddWithValue("@CreatedDate", record.CreatedDate);
                        cmd.Parameters.AddWithValue("@ModifiedDate", record.ModifiedDate);

                        cmd.ExecuteNonQuery();
                    }

                }
            }
        }

        Console.WriteLine("Данные успешно добавлены в базу данных.");
    }
}

public class Carrier
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string DID { get; set; }
    public string Status { get; set; }
    public string Region { get; set; }
    public string SMS { get; set; }
    public string CNAM { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime ModifiedDate { get; set; }
}

