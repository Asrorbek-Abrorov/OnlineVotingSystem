using OnlineVotingSystem.Entities.Commons;

namespace OnlineVotingSystem;

public class Voter : Auditable
{
    public int Id { get; set; }
    public string Name { get; set; }
    public bool HasVoted { get; set; }
}
