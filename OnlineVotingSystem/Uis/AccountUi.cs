using OnlineVotingSystem.Entities;
using OnlineVotingSystem.Servises;
using Spectre.Console;
using System.Xml.Linq;

namespace OnlineVotingSystem.UI;

public class AccountUI(AccountService accountService, UserService userService)
{
    public async Task Run(Account account)
    {
        while (true)
        {
            Console.Clear();
            var option = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[bold green]Account Management[/]")
                    .AddChoices(
                        new[]
                        {
                            "[bold cyan]View Details[/]",
                            "[bold cyan]Change Name[/]",
                            "[bold cyan]Change Password[/]",
                            "[bold cyan]Change Email[/]",
                            "[bold red]Exit[/]"
                        }));

            switch (option)
            {
                case "[bold cyan]View Details[/]":
                    var password = AnsiConsole.Prompt(new TextPrompt<string>("[yellow]Enter Password:[/]")
                        .Secret()
                        .PromptStyle("red"));
                    if (password == account.Password)
                    {
                        ViewDetails(account);
                    }
                    else
                    {
                        AnsiConsole.MarkupLine("[red]Wrong password[/]");
                    }
                    break;

                case "[bold cyan]Change Name[/]":
                    password = AnsiConsole.Prompt(new TextPrompt<string>("[yellow]Enter Password:[/]")
                        .Secret()
                        .PromptStyle("red"));

                    if (password == account.Password)
                    {
                        ChangeName(account);
                    }
                    else
                    {
                        AnsiConsole.MarkupLine("[red]Wrong password[/]");
                    }

                    break;

                case "[bold cyan]Change Password[/]":
                    password = AnsiConsole.Prompt(new TextPrompt<string>("[yellow]Enter Password:[/]")
                        .Secret()
                        .PromptStyle("red"));

                    if (password == account.Password)
                    {
                        ChangePassword(account);
                    }
                    else
                    {
                        AnsiConsole.MarkupLine("[red]Wrong password[/]");
                    }
                    break;

                case "[bold cyan]Change Email[/]":
                    password = AnsiConsole.Prompt(new TextPrompt<string>("[yellow]Enter Password:[/]")
                        .Secret()
                        .PromptStyle("red"));
                    if (password == account.Password)
                    {
                        ChangeEmail(account).GetAwaiter().GetResult();
                    }

                    break;

                case "[bold red]Exit[/]":
                    return;
            }

            Console.WriteLine();
            AnsiConsole.MarkupLine("[bold green]Press any key to continue...[/]");
            Console.ReadKey();
        }
    }

    private static void ViewDetails(Account account)
    {
        AnsiConsole.MarkupLine("");
        AnsiConsole.MarkupLine($"[bold green]Name:[/] [cyan]{account.Name}[/]");
        AnsiConsole.MarkupLine("");
        AnsiConsole.MarkupLine($"[bold yellow]Password:[/] [magenta]{account.Password}[/]");
        AnsiConsole.MarkupLine("");
        AnsiConsole.MarkupLine($"[bold blue]Email:[/] [yellow]{account.Email}[/]");
        AnsiConsole.MarkupLine("");
    }

    private void ChangeName(Account account)
    {
        var name = AnsiConsole.Ask<string>("Enter New Name:", account.Name);

        accountService.ChangeName(account, name);
        AnsiConsole.MarkupLine("[bold green]Name changed successfully.[/]");
    }

    private void ChangePassword(Account account)
    {
        var password = AnsiConsole.Ask<string>("Enter New Password:", account.Password);

        accountService.ChangePassword(account, password);
        AnsiConsole.MarkupLine("[bold green]Password changed successfully.[/]");
    }

    private async Task ChangeEmail(Account account)
    {
        string name = "***";
        var gmail = AnsiConsole.Ask<string>("Enter New Email:", account.Email);
        AnsiConsole.WriteLine();
        AnsiConsole.MarkupLine("[bold cyan]Sending verification code...[/]");

        var code = await userService.Send(gmail, name);

        AnsiConsole.WriteLine();
        AnsiConsole.MarkupLine("[bold green]Verification code sent successfully.[/]");
        var verificationCode = AnsiConsole.Ask<string>("[bold cyan]Enter the verification code:[/]");
        if (code == verificationCode)
        {
            accountService.ChangeEmail(account, gmail);
            AnsiConsole.MarkupLine("[bold green]Email changed successfully.[/]");
        }
        else
        {
            AnsiConsole.MarkupLine("[red]Couldn't authenticate ![/]");
        }

    }
}