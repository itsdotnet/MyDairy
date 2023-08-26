using MyDairy.DAL.Constexts;
using MyDairy.DAL.IRepositories;
using MyDairy.DAL.Repositories;
using MyDairy.Domain.Entities;

namespace MyDairy.DAL.Repository;

public class UnitOfWork : IUnitOfWork
{

    private readonly MyDairyDbContext dbContext;

    public UnitOfWork(MyDairyDbContext dbContext)
    {
        this.dbContext = dbContext;
        UserRepository = new Repository<User>(dbContext);
        NoteRepository = new Repository<Note>(dbContext);
        AttachmentRepository = new Repository<Attachment>(dbContext);
    }

    public IRepository<User> UserRepository { get; }

    public IRepository<Note> NoteRepository { get; }

    public IRepository<Attachment> AttachmentRepository { get; }

    public void Dispose()
    {
        GC.SuppressFinalize(true);
    }

    public async Task<bool> SaveAsync()
    {
        return await dbContext.SaveChangesAsync() > 0;
    }
}