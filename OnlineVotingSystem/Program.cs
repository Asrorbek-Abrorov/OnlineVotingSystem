using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.IO;
using Spectre.Console;
using OnlineVotingSystem.Servises;
using OnlineVotingSystem.Uis;

namespace OnlineVotingSystem;

public class Program
{
    private static readonly VotingService votingService = new VotingService();
    private static readonly CandidateService candidateService = new CandidateService();
    private static async Task Main(string[] args)
    {
        MainUi mainUi = new MainUi();
        await mainUi.Run();
    }
}