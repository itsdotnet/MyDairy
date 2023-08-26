using MyDairy.Domain.Entities;

namespace MyDairy.DAL.IRepositories;

public interface IUnitOfWork : IDisposable
{
    IRepository<User> UserRepository { get; }
    IRepository<Note> NoteRepository { get; }
    IRepository<Attachment> AttachmentRepository { get; }
    Task<bool> SaveAsync();
}