using Elections.Application.Interfaces;
using Elections.Data.Entities;
using Elections.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Elections.Application.Services
{
    public class VotesService : IVotesService
    {
        public VotesService(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private IApplicationDbContext _dbContext;
        public async Task<List<Votes>> GetAll()
        {
            return await _dbContext.Votes
                .Include(x => x.Candidate)
                .Include(x => x.Constituency).ToListAsync();
        }
        public async Task<List<Votes>> StoreVotes(List<VotesModel> model)
        {
            var allVotes = await GetAll();
            var newVotes = new List<Votes>();
            foreach (var item in model)
            {
                if (allVotes.Any(x => x.Constituency.Name == item.Constituency &&
                    x.Candidate.Name == item.CandidateName))
                {
                    var voteForUpdate = allVotes.First(x => x.Constituency.Name == item.Constituency &&
                        x.Candidate.Name == item.CandidateName);
                    voteForUpdate.NumberOfVotes = item.NumberOfVotes;
                    voteForUpdate.Error = item.Error;
                }
                else
                {
                    newVotes.Add(new Votes
                    {
                        CandidateId = item.CandidateId,
                        ConstituencyId = item.ConstituencyId,
                        NumberOfVotes = item.NumberOfVotes,
                        Error = item.Error
                    });
                }
            }
            if(newVotes.Count > 0)
            {
                _dbContext.Votes.AddRange(newVotes);
            }
            await _dbContext.SaveChangesAsync(CancellationToken.None);

            return newVotes;
        }

        public async Task<List<VotesViewModel>> GetVotesAsViewModel()
        {
            var votes = await GetAll();
            var votesGrouped = votes.GroupBy(x => x.Constituency);
            var model = new List<VotesViewModel>();
            var totalVotes = 0;
            foreach (var item in votesGrouped)
            {
                var modelItem = new VotesViewModel();
                modelItem.Constituency = item.Key.Name;
                modelItem.votes = new List<VotesViewModel.Votes>();
                foreach (var item2 in item)
                {
                    modelItem.votes.Add(new VotesViewModel.Votes
                    {
                        CandidateName = item2.Candidate.Name,
                        NumberOfVotes = item2.NumberOfVotes,
                        Error = item2.Error != null ? "Yes" : "-"
                        
                    });
                    if(item2.NumberOfVotes != null)
                    {
                        totalVotes += (int)item2.NumberOfVotes;
                    }
                }
                model.Add(modelItem);
            }
            
            return CalculatePercentages(model, totalVotes);
        }
        private List<VotesViewModel> CalculatePercentages(List<VotesViewModel> model, int totalVotes)
        {
            foreach (var item in model)
            {
                foreach (var item2 in item.votes)
                {
                    item2.Percentage = item2.NumberOfVotes != null ? item2.NumberOfVotes.Value / (decimal)totalVotes * 100: null;
                }
            }
            return model;
        }
    }
}
