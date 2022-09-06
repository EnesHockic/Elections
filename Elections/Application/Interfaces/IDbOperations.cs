
using Elections.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Elections.Application.Interfaces
{
    public interface ICandidateService<T>
    {
        public Task<Candidate> GetById(int Id);
        public Task<List<Candidate>> GetAll(int Id);
    }
}
