using Elections.Application.Interfaces;
using Elections.Data.Entities;
using Elections.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Elections.Application.Services
{
    public class CandidateService : ICandidateService
    {
        public CandidateService(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private IApplicationDbContext _dbContext;
        public async Task<List<Candidate>> GetAll()
        {
            return await _dbContext.Candidates.ToListAsync();
        }

        public async Task<List<Candidate>> UpsertCandidates(List<VotesModel> model)
        {
            try
            {
                List<string> candidates = new List<string>();
                foreach (var item in model)
                {
                    candidates.Add(item.CandidateName);
                }
                var existingCandidates = await _dbContext.Candidates.Where(x => candidates.Contains(x.Name)).ToListAsync();
                var newCandidates = candidates.Select(x => new Candidate()
                {
                    Name = x
                }).Where(x => !existingCandidates.Any(y => y.Name == x.Name)).ToList();

                _dbContext.Candidates.AddRange(newCandidates);
                await _dbContext.SaveChangesAsync(CancellationToken.None);
                return newCandidates;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
