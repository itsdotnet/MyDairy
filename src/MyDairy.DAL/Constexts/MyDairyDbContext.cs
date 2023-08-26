using MyDairy.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MyDairy.DAL.Constexts;

public class MyDairyDbContext : DbContext
{
    public MyDairyDbContext(DbContextOptions<MyDairyDbContext> options) : base(options)
    { }

    DbSet<User> Users { get; set; }

    DbSet<Note> Notes { get; set; }

    DbSet<Attachment> Attachments { get; set; }
}