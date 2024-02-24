using Newtonsoft.Json;
using OnlineVotingSystem.Entities;

namespace OnlineVotingSystem.Servises;

class CandidateService
{
    private List<Candidate> candidates;

    public CandidateService()
    {
        candidates = new List<Candidate>();
        LoadCandidatesFromFile();
    }

    public void AddCandidate(string name)
    {
        int id = candidates.Count + 1;
        candidates.Add(new Candidate { Id = id, Name = name, Votes = 0 });
        SaveCandidatesToFile();
    }

    public List<Candidate> GetCandidates()
    {
        LoadCandidatesFromFile();
        return candidates;
    }

    public Candidate GetCandidateById(int candidateId)
    {
        LoadCandidatesFromFile();
        return candidates.Find(c => c.Id == candidateId);
    }

    public void UpdateCandidateVotes(int candidateId, int votes)
    {
        Candidate candidate = GetCandidateById(candidateId);
        if (candidate != null)
        {
            candidate.Votes = votes;
        }
        SaveCandidatesToFile();
    }
    public void SaveCandidatesToFile()
    {
        var candidatesJson = JsonConvert.SerializeObject(candidates, Formatting.Indented);
        File.WriteAllText("../../../../OnlineVotingSystem/Data/candidates.json", candidatesJson);
    }
    public void LoadCandidatesFromFile()
    {
        if (File.Exists("../../../../OnlineVotingSystem/Data/candidates.json"))
        {
            var candidatesJson = File.ReadAllText("../../../../OnlineVotingSystem/Data/candidates.json");
            candidates = JsonConvert.DeserializeObject<List<Candidate>>(candidatesJson);
        }
    }
}
