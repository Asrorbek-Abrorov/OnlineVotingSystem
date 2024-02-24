using OnlineVotingSystem.Entities.Commons;

namespace OnlineVotingSystem.Entities;

public class Candidate : Auditable
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Votes { get; set; }
}
