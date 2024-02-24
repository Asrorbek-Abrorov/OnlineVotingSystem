using Newtonsoft.Json;
using OnlineVotingSystem.Entities;

namespace OnlineVotingSystem.Servises;

public class AccountService
{
    private List<Account> accounts;
    private string accountsFilePath;

    public AccountService(string accountsFilePath)
    {
        accounts = new List<Account>();
        this.accountsFilePath = accountsFilePath;
        LoadAccounts();
    }

    public void Create(Account account)
    {
        accounts.Add(account);
        SaveAccounts();
    }

    public void ChangeName(Account account, string name)
    {
        if (account != null)
        {
            account.Name = name;
            SaveAccounts();
        }
    }

    public void ChangePassword(Account account, string password)
    {
        if (account != null)
        {
            account.Password = password;
            SaveAccounts();
        }
    }

    public void ChangeEmail(Account account, string email)
    {
        if (account != null)
        {
            account.Email = email;
            SaveAccounts();
        }
    }

    private void LoadAccounts()
    {
        if (File.Exists(accountsFilePath))
        {
            string json = File.ReadAllText(accountsFilePath);
            accounts = JsonConvert.DeserializeObject<List<Account>>(json);
        }
    }

    private void SaveAccounts()
    {
        string json = JsonConvert.SerializeObject(accounts, Formatting.Indented);
        File.WriteAllText(accountsFilePath, json);
    }

    public Account GetAccountByGmail(string gmail)
    {
        LoadAccounts();
        return accounts.Find(account => account.Email == gmail);
    }
}
