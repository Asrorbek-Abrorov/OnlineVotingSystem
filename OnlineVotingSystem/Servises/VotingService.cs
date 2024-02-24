using Newtonsoft.Json;
using OnlineVotingSystem.Entities;

namespace OnlineVotingSystem;

public class VotingService
{
    private List<Candidate> candidates;
    private List<Account> accounts;

    public VotingService()
    {
        candidates = new List<Candidate>();
        accounts = new List<Account>();
    }

    public void AddCandidate(string name)
    {
        int id = candidates.Count + 1;
        candidates.Add(new Candidate { Id = id, Name = name, Votes = 0 });
        SaveCandidatesToFile();
    }

    public void AddVoter(string name)
    {
        int id = accounts.Count + 1;
        accounts.Add(new Account { Id = id, Name = name, HasVoted = false });
        SaveVotersToFile();
    }

    public bool Vote(int voterId, int candidateId)
    {
        var voter = accounts.Find(x => x.Id == voterId);
        Candidate candidate = candidates.Find(c => c.Id == candidateId);

        if (voter == null || candidate == null)
        {
            return false;
        }

        if (voter.HasVoted)
        {
            return false;
        }

        candidate.Votes++;
        voter.HasVoted = true;
        SaveCandidatesToFile();
        SaveVotersToFile();

        return true;
    }

    public List<Candidate> GetCandidates()
    {
        LoadCandidatesFromFile();
        return candidates;
    }

    public List<Account> GetVoters()
    {
        LoadVotersFromFile();
        return accounts;
    }

    public void SaveCandidatesToFile()
    {
        var candidatesJson = JsonConvert.SerializeObject(candidates, Formatting.Indented);
        File.WriteAllText("../../../../OnlineVotingSystem/Data/candidates.json", candidatesJson);
    }

    public void SaveVotersToFile()
    {
        var votersJson = JsonConvert.SerializeObject(accounts, Formatting.Indented);
        File.WriteAllText("../../../../OnlineVotingSystem/Data/accounts.json", votersJson);
    }

    public void LoadCandidatesFromFile()
    {
        if (File.Exists("../../../../OnlineVotingSystem/Data/candidates.json"))
        {
            var candidatesJson = File.ReadAllText("../../../../OnlineVotingSystem/Data/candidates.json");
            candidates = JsonConvert.DeserializeObject<List<Candidate>>(candidatesJson);
        }
    }

    public void LoadVotersFromFile()
    {
        if (File.Exists("../../../../OnlineVotingSystem/Data/accounts.json"))
        {
            var votersJson = File.ReadAllText("../../../../OnlineVotingSystem/Data/accounts.json");
            accounts = JsonConvert.DeserializeObject<List<Account>>(votersJson);
        }
    }

    public void InitializeFromFiles()
    {
        LoadCandidatesFromFile();
        LoadVotersFromFile();
    }
}
