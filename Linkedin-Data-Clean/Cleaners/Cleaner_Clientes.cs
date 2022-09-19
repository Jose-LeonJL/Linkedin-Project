using System;
using MoreLinq;
using CsvHelper;
using System.IO;
using System.Text;
using System.Linq;
using Linkedin.Net.Models;
using System.Globalization;
using System.Collections.Generic;

namespace Linkedin_Data_Clean.Cleaners
{
    internal class Cleaner_Clientes
    {
        public static bool CleanClients(string DirectoryFile)
        {
            var result = false;
            using (var reader = new StreamReader(DirectoryFile))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var records = csv.GetRecords<clients>().ToList();
                var res = (from data in records
                           select new clients
                           {
                               client_id = data.client_id,
                               client_name = data.client_name.Trim(),
                               client_url = data.client_url.Trim(),
                               client_company_industry = data.client_company_industry,
                               client_company_linkedin_url = data.client_company_linkedin_url,
                               client_email = data.client_email,
                               client_company_name = data.client_company_name,
                               client_title = data.client_title,
                               client_company_website = data.client_company_website,
                               client_description = data.client_description,
                               client_location = data.client_location,
                               create_at = data.create_at
                           }).DistinctBy(x => x.client_url).ToList();
                using (var writer = new StreamWriter(Path.Combine(Directory.GetCurrentDirectory(), $"{res.Count}-Clientes-{DateTime.Now.Day}-{DateTime.Now.Month}-{DateTime.Now.Year}.csv")))
                using (var csvw = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    csvw.WriteRecords(records);
                    result = true;
                }
            }
            return result;
        }
    }
}
