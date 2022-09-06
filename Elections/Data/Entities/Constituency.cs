using System.Collections.Generic;

namespace Elections.Data.Entities
{
    public class Constituency
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Votes> Votes { get; set; }
    }
}
