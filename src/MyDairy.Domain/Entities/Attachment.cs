using MyDairy.Domain.Commons;

namespace MyDairy.Domain.Entities;

public class Attachment : Auditable
{
    public string Name { get; set; }

    public string Path { get; set; }
}
