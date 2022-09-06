using Elections.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elections.Files
{
    public class CsvFileParser : ICsvFileParser
    {
        public async Task<List<List<string>>> GetRecordsAsString(IFormFile file)
        {
            var builder = new StringBuilder();
            var result = new List<List<string>>();
            string[] row = new string[0];
            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                while (reader.Peek() >= 0)
                {
                    var a = await reader.ReadLineAsync();
                    row = a.Split(',');
                    result.Add(row.Select(x => x.TrimStart().TrimEnd()).ToList());
                }
            }

            return result;
        }
    }
}
