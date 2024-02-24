using OnlineVotingSystem.Entities;

namespace OnlineVotingSystem.Interfaces;

public interface IUserService
{
    Task<string> Send(string gmail, string name);
    Account GetAccount(string gmail);
}
