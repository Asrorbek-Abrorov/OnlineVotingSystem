using System;
using System.Collections.Generic;
using OnlineVotingSystem.Entities;
using OnlineVotingSystem.UI;
using Spectre.Console;

namespace OnlineVotingSystem.Uis;

public class VotingUI(VotingService votingService, AccountUI accountUI)
{
    public async Task Run(Account account)
    {
        votingService.InitializeFromFiles();

        while (true)
        {
            AnsiConsole.Clear();
            var option = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[bold cyan]===== Online Voting System =====[/]")
                    .AddChoices(
                        ["[bold green]Vote[/]", "[bold green]View Results[/]", "[bold green]Account Settings[/]", "[bold red]Exit[/]"]
                    )
            );

            switch (option)
            {
                case "[bold green]Vote[/]":
                    Vote(account);
                    break;
                case "[bold green]View Results[/]":
                    ViewResults();
                    break;
                case "[bold green]Account Settings[/]":
                    await accountUI.Run(account);
                    break;
                case "[bold red]Exit[/]":
                    votingService.SaveCandidatesToFile();
                    votingService.SaveVotersToFile();
                    return;
                default:
                    AnsiConsole.MarkupLine("[bold red]Invalid choice.[/]");
                    break;
            }

            AnsiConsole.WriteLine();
            AnsiConsole.MarkupLine("[bold cyan]Press any key to continue...[/]");
            Console.ReadKey();
        }
    }

    private string GetUserInput(string message)
    {
        AnsiConsole.Markup(message);
        return Console.ReadLine();
    }
    private void Vote(Account account)
    {
        List<Candidate> candidates = votingService.GetCandidates();
        var candidate = AnsiConsole.Prompt(
            new SelectionPrompt<Candidate>()
                .Title("[bold cyan]Available candidates:[/]")
                .PageSize(10)
                .AddChoices(candidates)
                .UseConverter(c => c.Name)
        );

        bool voteResult = votingService.Vote(account.Id, candidate.Id);
        if (voteResult)
        {
            AnsiConsole.MarkupLine("[bold green]Vote recorded successfully.[/]");
        }
        else
        {
            AnsiConsole.MarkupLine("[bold red]Voter has already voted.[/]");
        }
    }

    private void ViewResults()
    {
        AnsiConsole.MarkupLine("[bold cyan]===== Voting Results =====[/]");
        List<Candidate> results = votingService.GetCandidates();
        foreach (var candidate in results)
        {
            AnsiConsole.MarkupLine($"[bold white]{candidate.Name}[/]: [bold yellow]{candidate.Votes} votes[/]");
        }
    }
}