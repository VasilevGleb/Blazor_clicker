namespace NIT.Models;

public class AccountData
{
    public string Nickname{get; set;} = "";
    public string PasswordHash{get; set;} = "";
    public string Salt {get; set;} = "";
}