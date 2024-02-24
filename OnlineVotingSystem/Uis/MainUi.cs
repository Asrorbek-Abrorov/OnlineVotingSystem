using OnlineVotingSystem.Entities;
using OnlineVotingSystem.Servises;
using OnlineVotingSystem.UI;
using Spectre.Console;

namespace OnlineVotingSystem.Uis;

public class MainUi
{
    public static string accountsPath = "../../../../OnlineVotingSystem/Data/accounts.json";
    private static readonly VotingService votingService = new VotingService();
    private static readonly CandidateService candidateService = new CandidateService();
    private static readonly AccountService accountService = new AccountService(accountsPath);
    private static readonly UserService userService = new UserService(accountService);
    private static readonly AccountUI accountUI = new AccountUI(accountService, userService);
    private readonly VotingUI votinUi = new VotingUI(votingService, accountUI);
    public async Task Run()
    {
        while (true)
        {
            var start = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("===Start Application===")
                .PageSize(5)
                .AddChoices(["[green]Start[/]", "[red]Exit[/]"])
                );
            if (start == "[green]Start[/]")
            {
            
                votingService.InitializeFromFiles();
                Console.Clear();
                UserUi userUi = new UserUi(accountService);
                var account = await userUi.Register();


                if (account == null)
                {
                    AnsiConsole.MarkupLine("[bold red]Invalid credentials.[/]");
                    AnsiConsole.MarkupLine("[bold white]Press any key to[/] [italic yellow]Repeat Autentification...[/]");
                    Console.ReadKey();
                }
                else if (account.Password == "admin" && account.Email == "asrorbekabrorov5@gmail.com")
                {
                    while (true)
                    {
                        AnsiConsole.Clear();
                        var option = AnsiConsole.Prompt(
                            new SelectionPrompt<string>()
                                .Title("[bold cyan]===== Online Voting System =====[/]")
                                .AddChoices(
                                    ["[bold green]Add Candidate[/]", "[bold yellow]View Results[/]", "[bold yellow]View Voters[/]", "[bold red]Exit[/]"]
                                ));

                        switch (option)
                        {
                            case "[bold green]Add Candidate[/]":
                                string candidateName = AnsiConsole.Ask<string>("[bold white]Enter candidate name:[/]");
                                candidateService.AddCandidate(candidateName);
                                AnsiConsole.MarkupLine("[bold green]Candidate added successfully.[/]");
                                break;
                            case "[bold yellow]View Results[/]":
                                AnsiConsole.MarkupLine("[bold cyan]===== Voting Results =====[/]");
                                List<Candidate> results = votingService.GetCandidates();
                                foreach (var candidate in results)
                                {
                                    AnsiConsole.MarkupLine($"[bold white]{candidate.Name}[/]: [bold yellow]{candidate.Votes} votes[/]");
                                }
                                break;
                            case "[bold yellow]View Voters[/]":
                                AnsiConsole.MarkupLine("[bold cyan]===== Voting Voters =====[/]");
                                List<Account> voters = votingService.GetVoters();
                                foreach (var voter in voters)
                                {
                                    AnsiConsole.MarkupLine("[bold white]======================================================[/]");
                                    AnsiConsole.MarkupLine($"[bold yellow]Name[/]: {voter.Name}");
                                    AnsiConsole.MarkupLine($"[bold white]Email[/]: {voter.Email}");
                                    AnsiConsole.MarkupLine($"[bold yellow]Has Voted[/]: {voter.HasVoted}");
                                    await Console.Out.WriteLineAsync();
                                }
                                break;
                            case "[bold red]Exit[/]":
                                return;
                        }

                        AnsiConsole.WriteLine();
                        AnsiConsole.MarkupLine("[bold white]Press any key to continue...[/]");
                        Console.ReadLine();
                    }
                }
                else
                {
                    await votinUi.Run(account);
                }
            }
            else
            {
                return;
            }
        }
    }
}

