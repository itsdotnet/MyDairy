using MyDairy.Service.DTOs.Attachment;
using MyDairy.Service.DTOs.Notes;
using System.ComponentModel.DataAnnotations;

namespace MyDairy.Service.DTOs.Users;

public class UserResultDto
{
    public long Id { get; set; }

    public string Firstname { get; set; }

    public string Lastname { get; set; }

    public string Username { get; set; }

    public AttachmentResultDto Attachment { get; set; }

    public ICollection<NoteResultDto> Notes { get; set; }
}