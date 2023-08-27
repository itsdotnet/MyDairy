using MyDairy.DAL.IRepositories;
using MyDairy.DAL.Repositories;
using MyDairy.DAL.Repository;
using MyDairy.Service.Interfaces.Attacments;
using MyDairy.Service.Interfaces.Notes;
using MyDairy.Service.Interfaces.Users;
using MyDairy.Service.Mappers;
using MyDairy.Service.Services.Attacments;
using MyDairy.Service.Services.Notes;
using MyDairy.Service.Services.Users;

namespace MyDairy.WebApi.Extensions;

#pragma warning disable CS8604

public static class ServicesCollection
{
    public static void AddServices(this IServiceCollection services)
    {
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<INoteService, NoteService>();
        services.AddScoped<IAttachmentService, AttachmentService>();
        services.AddAutoMapper(typeof(MappingProfile));
    }
}
