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
                    .Title("Account Management")
                    .AddChoices(
                    [
                        "Change Name",
                        "Change Password",
                        "Change Email",
                        "Exit"
                    ]));

            switch (option)
            {
                case "Change Name":
                    ChangeName(account);
                    break;
                case "Change Password":
                    ChangePassword(account);
                    break;
                case "Change Email":
                    await ChangeEmail(account);
                    break;
                case "Exit":
                    return;
            }
            Console.WriteLine();
            AnsiConsole.MarkupLine("[bold green]Account changed successfully.[/]");
            Console.ReadKey();
        }
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