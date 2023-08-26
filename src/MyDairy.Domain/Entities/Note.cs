using MyDairy.Domain.Commons;

namespace MyDairy.Domain.Entities;

public class Note : Auditable
{
    public string Title { get; set; }

    public string Description { get; set; }

    public long UserId { get; set; }
    public User User { get; set; }

    public long? AttachmentId { get; set; }
    public Attachment Attachment { get; set; }
}
