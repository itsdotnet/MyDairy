using MyDairy.Service.DTOs.Attachment;
using MyDairy.Service.DTOs.Users;

namespace MyDairy.Service.DTOs.Notes;

public class NoteResultDto
{
    public long Id { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }

    public UserResultDto User { get; set; }

    public AttachmentResultDto Attachment { get; set; }
}