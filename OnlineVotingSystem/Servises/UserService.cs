using System;
using System.Threading.Tasks;
using SendGrid.Helpers.Mail;
using SendGrid;
using Spectre.Console;
using OnlineVotingSystem.Entities;
using OnlineVotingSystem.Servises;

namespace OnlineVotingSystem;

public class UserService(AccountService accountService)
{
    public readonly AccountService accountService = accountService;

    public async Task<string> Send(string gmail, string name)
        =>await Execute(gmail, name);

    private static async Task<string> Execute(string gmail, string name)
    {
        var apiKey = Environment.GetEnvironmentVariable("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");
        var client = new SendGridClient(apiKey ?? "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");
        var from = new EmailAddress("as.abrorov@gmail.com", "From Asror");
        var subject = "Verification Code";
        var to = new EmailAddress(gmail, name);
        var verificationCode = GenerateVerificationCode();
        var plainTextContent = $"Your verification code is: {verificationCode}";
        var htmlContent = $"<p>Your verification code is: <strong>{verificationCode}</strong></p>";
        var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
        var response = await client.SendEmailAsync(msg);
        return verificationCode;
    }

    private static string GenerateVerificationCode()
    {
        const int codeLength = 6;
        const string characters = "0123456789";
        var random = new Random();
        var code = new char[codeLength];

        for (int i = 0; i < codeLength; i++)
        {
            code[i] = characters[random.Next(characters.Length)];
        }

        return new string(code);
    }
    public Account GetAccount(string gmail) 
        => accountService.GetAccountByGmail(gmail);
    
}