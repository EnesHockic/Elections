using Elections.Data.Entities;
using Elections.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Elections.Application.Interfaces
{
    public interface IConstituencyService
    {
        public Task<Constituency?> GetById(int id);
        public Task<List<Constituency>?> GetAll();
        public Task<List<Constituency>?> UpsertConstituencies(List<VotesModel> model);
    }
}
