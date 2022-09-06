using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Elections.Application.Interfaces
{
    public interface ICsvFileParser
    {
        public Task<List<List<string>>> GetRecordsAsString(IFormFile file);
    }
}
