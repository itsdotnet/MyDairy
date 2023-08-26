namespace MyDairy.Service.Interfaces;

public interface IAuthService
{
	Task<string> GenerateTokenAsync(string username, string originalPassword);
}
