using MyDairy.DAL.IRepositories;
using MyDairy.Service.Exceptions;
using MyDairy.Service.Helpers;
using MyDairy.Service.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MyDairy.Service.Services;

public class AuthService : IAuthService
{
    private readonly IConfiguration configuration;
    private readonly IUnitOfWork unitOfWork;
    public AuthService(IUnitOfWork unitOfWork, IConfiguration configuration)
    {
        this.configuration = configuration;
        this.unitOfWork = unitOfWork;
    }

    public async Task<string> GenerateTokenAsync(string username, string originalPassword)
    {
        var user = await this.unitOfWork.UserRepository.SelectAsync(u => u.Username.Equals(username));
        if (user is null)
            throw new NotFoundException("This user is not found");

        bool verifiedPassword = PasswordHasher.Verify(user.Password, originalPassword);
        if (!verifiedPassword)
            throw new CustomException(400, "Password is invalid");

        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenKey = Encoding.UTF8.GetBytes(configuration["JWT:Key"]);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
             new Claim("Username", user.Username),
             new Claim("Id", user.Id.ToString()),
            }),
            Expires = DateTime.UtcNow.AddDays(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);

        string result = tokenHandler.WriteToken(token);
        return result;
    }
}
