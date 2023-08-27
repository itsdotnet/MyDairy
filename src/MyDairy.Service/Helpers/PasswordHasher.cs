namespace MyDairy.Service.Helpers;

public static class PasswordHasher
{
    public static string Hash(this string password)
    {
        string hash = BCrypt.Net.BCrypt.HashPassword(password);
        return hash;
    }

    public static bool Verify(this string password, string hash)
    {
        return BCrypt.Net.BCrypt.Verify(password, hash);
    }
}
