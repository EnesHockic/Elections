namespace Elections.Data.Entities
{
#nullable disable
    public class Votes
    {
        public int Id { get; set; }
        public int CandidateId { get; set; }
        public int ConstituencyId { get; set; }
        public int? NumberOfVotes { get; set; }
        public string? Error { get; set; }
        public virtual Constituency Constituency { get; set; }
        public virtual Candidate Candidate { get; set; }
    }
}
