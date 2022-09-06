using Elections.Data.Entities;
using Elections.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Elections.Application.Interfaces
{
    public interface IVotesService
    {
        public Task<List<Votes>> GetAll();
        public Task<List<Votes>> StoreVotes(List<VotesModel> model);
        public Task<List<VotesViewModel>> GetVotesAsViewModel();

    }
}
