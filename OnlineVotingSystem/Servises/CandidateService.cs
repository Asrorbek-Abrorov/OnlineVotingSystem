using EllipticCurve.Utils;
using Newtonsoft.Json;
using OnlineVotingSystem.Entities;
using OnlineVotingSystem.Interfaces;
using System.IO;
using File = System.IO.File;

namespace OnlineVotingSystem.Servises;

public class CandidateService : ICandidateService
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
    public void UpdateCandidateName(int candidateId, string name)
    {
        LoadCandidatesFromFile();
        Candidate candidate = GetCandidateById(candidateId);
        if (candidate != null)
        {
            candidate.Name = name;
        }
        SaveCandidatesToFile();
    }

    public void UpdateCandidateVotes(int candidateId, int votes)
    {
        LoadCandidatesFromFile();
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
        File.WriteAllText(Configurations.Constants.CandidatesPath, candidatesJson);
    }
    public void LoadCandidatesFromFile()
    {
        if (File.Exists(Configurations.Constants.CandidatesPath))
        {
            var candidatesJson = File.ReadAllText(Configurations.Constants.CandidatesPath);
            candidates = JsonConvert.DeserializeObject<List<Candidate>>(candidatesJson);
        }
    }
}
