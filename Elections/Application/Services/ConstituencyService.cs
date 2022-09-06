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
    public class ConstituencyService : IConstituencyService
    {
        public ConstituencyService(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private IApplicationDbContext _dbContext;
        public async Task<List<Constituency>> GetAll()
        {
            return await _dbContext.Constituencies.ToListAsync();
        }

        public async Task<Constituency?> GetById(int id)
        {
            return await _dbContext.Constituencies.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<Constituency>> UpsertConstituencies(List<VotesModel> model)
        {
            try
            {
                List<string> constituencies = new List<string>();
                foreach (var item in model)
                {
                    constituencies.Add(item.Constituency);
                }
                var existingConstituencies = await _dbContext.Constituencies.Where(x => constituencies.Contains(x.Name)).ToListAsync();
                var newConstituencies = constituencies.Select(x => new Constituency()
                {
                    Name = x
                }).Where(x => !existingConstituencies.Any(y => y.Name == x.Name)).ToList();

                _dbContext.Constituencies.AddRange(newConstituencies);
                await _dbContext.SaveChangesAsync(CancellationToken.None);
                return newConstituencies;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
