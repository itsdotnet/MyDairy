using Microsoft.AspNetCore.Http;

namespace MyDairy.Service.DTOs.Attachment;

public class AttachmentCreationDto
{
    public IFormFile File { get; set; }
}
