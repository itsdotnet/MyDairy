namespace MyDairy.Service.DTOs.Notes;

public class NoteUpdateDto
{
    public long Id { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }

    public long UserId { get; set; }

    public long? AttachmentId { get; set; }
}
