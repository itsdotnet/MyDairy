using MyDairy.Service.DTOs.Attachment;
using MyDairy.Service.DTOs.Users;

namespace MyDairy.Service.Interfaces.Users;

public interface IUserService
{
    Task<UserResultDto> CreateAsync(UserCreationDto user);

    Task<UserResultDto> UpdateAsync(UserUpdateDto user);

    Task<bool> DeleteAsync(long id);

    Task<UserResultDto> GetByIdAsync(long id);

    Task<IEnumerable<UserResultDto>> GetAllAsync();

    Task<IEnumerable<UserResultDto>> GetAllByNameAsync(string name);

    Task<IEnumerable<UserResultDto>> GetAllByUsernameAsync(string name);

    Task<bool> CheckCredentialsAsync(string username, string password);

    Task<bool> ChangePasswordAsync(long userId, string currentPassword, string newPassword);

    Task<AttachmentResultDto> UploadPhoto(AttachmentCreationDto dto);
}