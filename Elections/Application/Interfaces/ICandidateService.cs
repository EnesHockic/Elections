using Elections.Data.Entities;
using Elections.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Elections.Application.Interfaces
{
    public interface ICandidateService
    {
        public Task<List<Candidate>> GetAll();
        public Task<List<Candidate>> UpsertCandidates(List<VotesModel> model);
    }
}
