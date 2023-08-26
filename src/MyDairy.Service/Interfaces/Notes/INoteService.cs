using MyDairy.Service.DTOs.Notes;

namespace MyDairy.Service.Interfaces.Notes;

public interface INoteService
{
    Task<NoteResultDto> CreateAsync(NoteCreationDto user);

    Task<NoteResultDto> UpdateAsync(NoteUpdateDto user);

    Task<bool> DeleteAsync(long id);

    Task<NoteResultDto> GetByIdAsync(long id);

    Task<IEnumerable<NoteResultDto>> GetAllAsync();

    Task<IEnumerable<NoteResultDto>> GetAllByTitleAsync(string title);

    Task<IEnumerable<NoteResultDto>> GetAllByUserIdAsync(long userId);
}
