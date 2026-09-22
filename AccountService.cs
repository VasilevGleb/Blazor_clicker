
using System.Security.Cryptography;
using System.Text.Json;
using NIT.Models;
namespace NIT.Service;

public class AccountService
{
// THERE Creates THE SAVE_FILE FOR ACCOUNT
private readonly string _filePath = "account_save.json";
public bool AccountExists() => File.Exists(_filePath);

public void CreateAccount(string nickname, string password)
    {
        byte[] saltBytes= RandomNumberGenerator.GetBytes(16);
        byte[] hashBytes = Rfc2898DeriveBytes.Pbkdf2(
            password, 
            saltBytes, 
            100_000, 
            HashAlgorithmName.SHA256, 
            32
        );
        var account = new AccountData{
        Nickname = nickname, 
        PasswordHash = Convert.ToBase64String(hashBytes),
        Salt = Convert.ToBase64String(saltBytes)
        }; 

        string json = JsonSerializer.Serialize(account);
        File.WriteAllText(_filePath, json);
    
        
    }

    public bool ValidateLogin(string nickname, string password)
    {
        if (!File.Exists(_filePath))
        {
            return false;
        }
        var account = JsonSerializer.Deserialize<AccountData>(File.ReadAllText(_filePath));
        if(account is null || account.Nickname != nickname)
        {
            return false;
        }

        byte[] saltBytes = Convert.FromBase64String(account.Salt);
        byte[] storedHash = Convert.FromBase64String(account.PasswordHash);
        byte[] enteredHash = Rfc2898DeriveBytes.Pbkdf2(password, saltBytes, 100_000, HashAlgorithmName.SHA256,32);
        return CryptographicOperations.FixedTimeEquals(storedHash, enteredHash);
    }

}