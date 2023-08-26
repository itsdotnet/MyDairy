using MyDairy.Service.DTOs.Attachment;

namespace MyDairy.Service.Interfaces.Attacments;

public interface IAttachmentService
{
    Task<AttachmentResultDto> CreateAsync(AttachmentCreationDto user);

    Task<AttachmentResultDto> UpdateAsync(AttachmentUpdateDto user);

    Task<bool> DeleteAsync(long id);

    Task<AttachmentResultDto> GetByIdAsync(long id);

    Task<IEnumerable<AttachmentResultDto>> GetAllAsync();

}
