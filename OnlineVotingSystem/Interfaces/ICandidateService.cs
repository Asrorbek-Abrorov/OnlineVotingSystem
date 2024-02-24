using OnlineVotingSystem.Entities;

namespace OnlineVotingSystem.Interfaces;


public interface ICandidateService
{
    void AddCandidate(string name);
    List<Candidate> GetCandidates();
    Candidate GetCandidateById(int candidateId);
    void UpdateCandidateVotes(int candidateId, int votes);
}
