using OnlineVotingSystem.Entities;
using OnlineVotingSystem.Servises;
using Spectre.Console;

namespace OnlineVotingSystem.Uis;

public class UserUi(AccountService accountService)
{
    private readonly UserService userService = new UserService(accountService);
    public async Task<Account> Register()
    {
        while (true)
        {
            var name = AnsiConsole.Ask<string>("[bold cyan]Enter your name:[/]");

            var gmail = AnsiConsole.Ask<string>("[bold cyan]Enter your Gmail address:[/]");

            var foundGmail = accountService.GetAccountByGmail(gmail);

            if (foundGmail != null)
            {
                return await LogIn(gmail);
            }
            else
            {
                var password = AnsiConsole.Prompt<string>(new TextPrompt<string>("[bold cyan]Enter your password:[/]")
                .Secret());

                var password2 = AnsiConsole.Prompt<string>(new TextPrompt<string>("[bold cyan]Repeat your password:[/]")
                    .Secret());

                if (password == password2)
                {
                    AnsiConsole.WriteLine();
                    AnsiConsole.MarkupLine("[bold cyan]Sending verification code...[/]");

                    try
                    {
                        var code = await userService.Send(gmail, name);

                        AnsiConsole.WriteLine();
                        AnsiConsole.MarkupLine("[bold green]Verification code sent successfully.[/]");
                        var verificationCode = AnsiConsole.Ask<string>("[bold cyan]Enter the verification code:[/]");
                        if (code == verificationCode)
                        {
                            Account account = new Account();
                            account.Name = name;
                            account.Password = password;
                            account.Email = gmail;

                            accountService.Create(account);

                            AnsiConsole.MarkupLine("[bold green]Account created successfully.[/]");
                            return account;
                        }
                        else
                        {
                            AnsiConsole.MarkupLine("[bold red]Failed to verify the code![/]");
                        }
                        break;
                    }
                    catch (Exception ex)
                    {
                        AnsiConsole.WriteLine();
                        AnsiConsole.MarkupLine("[bold red]Failed to send verification code.[/]");
                        AnsiConsole.WriteLine($"Error: {ex.Message}");
                    }
                    AnsiConsole.Clear();
                }
                else
                {
                    AnsiConsole.MarkupLine("[bold red]Passwords didn't match![/]");
                }
            }
        }

        return null;
    }
    public async Task<Account> LogIn(string gmail)
    {
        var account = accountService.GetAccountByGmail(gmail);
        if (account is not null)
        {
            var password = AnsiConsole.Prompt<string>(new TextPrompt<string>("[bold cyan]Enter your password:[/]")
                .Secret());
            if (password == account.Password)
            {
                return account;
            }
            else
            {
                AnsiConsole.MarkupLine("[red]Wrong password[/]");
                return null;
            }
        }
        else
        {
            AnsiConsole.MarkupLine("Couldn't find account with this ", gmail);
            return null;
        }
    }
}
