namespace OnlineVotingSystem.Entities;

public class Account : Voter
{
    private static int id = 1;
    public int Id = ++id;
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}
