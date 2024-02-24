using OnlineVotingSystem.Entities;

namespace OnlineVotingSystem.Interfaces;

public interface IAccountService
{
    void Create(Account account);
    void ChangeName(Account account, string name);
    void ChangePassword(Account account, string password);
    void ChangeEmail(Account account, string email);
    Account GetAccountByGmail(string gmail);
}
