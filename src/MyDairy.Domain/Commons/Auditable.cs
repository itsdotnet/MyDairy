using MyDairy.Domain.Constants;

namespace MyDairy.Domain.Commons;

public class Auditable
{
    public long Id { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow.AddHours(TimeConstants.UTC);
    public DateTime? ModifiedAt { get; set;}
}
