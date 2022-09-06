using Elections.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Elections.Application.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<Candidate> Candidates { get; set; }
        DbSet<Constituency> Constituencies { get; set; }
        DbSet<Votes> Votes { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
