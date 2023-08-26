using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using MyDairy.DAL.IRepositories;
using MyDairy.DAL.Repositories;
using MyDairy.DAL.Repository;
using MyDairy.Service.Interfaces;
using MyDairy.Service.Interfaces.Attacments;
using MyDairy.Service.Interfaces.Notes;
using MyDairy.Service.Interfaces.Users;
using MyDairy.Service.Mappers;
using MyDairy.Service.Services;
using MyDairy.Service.Services.Attacments;
using MyDairy.Service.Services.Notes;
using MyDairy.Service.Services.Users;
using System.Text;

namespace MyDairy.WebApi.Extensions;

#pragma warning disable CS8604

public static class ServicesCollection
{
    public static void AddServices(this IServiceCollection services)
    {
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<INoteService, NoteService>();
        services.AddScoped<IAttachmentService, AttachmentService>();
        services.AddAutoMapper(typeof(MappingProfile));
    }

    public static void AddJwt(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(o =>
        {
            var key = Encoding.UTF8.GetBytes(configuration["JWT:Key"]);
            o.SaveToken = true;
            o.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = configuration["JWT:Issuer"],
                ValidAudience = configuration["JWT:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(key)
            };
        });
    }
}
