using System.Collections.Generic;

namespace Elections.Models
{
    public class VotesModel
    {
        public int ConstituencyId { get; set; }
        public string Constituency { get; set; }
        public int CandidateId { get; set; }
        public string CandidateName { get; set; }
        public int? NumberOfVotes { get; set; }
        public string? Error { get; set; }
    }
}
