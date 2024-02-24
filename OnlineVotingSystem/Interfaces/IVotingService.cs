using OnlineVotingSystem.Entities;

namespace OnlineVotingSystem.Interfaces;

public interface IVotingService
{
    void AddCandidate(string name);
    void AddVoter(string name);
    bool Vote(int voterId, int candidateId);
    List<Candidate> GetCandidates();
    List<Account> GetVoters();
    void SaveCandidatesToFile();
    void SaveVotersToFile();
    void LoadCandidatesFromFile();
    void LoadVotersFromFile();
    void InitializeFromFiles();
}