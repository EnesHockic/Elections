using System.Collections.Generic;

namespace Elections.Models
{
    public class VotesViewModel
    {
        public string Constituency { get; set; }
        public List<Votes> votes { get; set; }
        public class Votes
        {
            public string CandidateName { get; set; }
            public int? NumberOfVotes { get; set; }

            public decimal? Percentage { get; set; }
            public string? Error { get; set; }
        }
    }
}
