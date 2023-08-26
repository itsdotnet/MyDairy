using Microsoft.AspNetCore.Http;

namespace MyDairy.Service.DTOs.Attachment;

public class AttachmentUpdateDto
{
    public long Id { get; set; }

    public IFormFile File { get; set; }
}
