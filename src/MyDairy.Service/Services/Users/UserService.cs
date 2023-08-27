using AutoMapper;
using MyDairy.DAL.IRepositories;
using MyDairy.Domain.Entities;
using MyDairy.Service.DTOs.Attachment;
using MyDairy.Service.DTOs.Users;
using MyDairy.Service.Exceptions;
using MyDairy.Service.Helpers;
using MyDairy.Service.Interfaces.Attacments;
using MyDairy.Service.Interfaces.Users;

namespace MyDairy.Service.Services.Users;

public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAttachmentService attachmentService;
    private readonly IMapper mapper;

    public UserService(IUnitOfWork unitOfWork, IAttachmentService attachmentService, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        this.attachmentService = attachmentService;
        this.mapper = mapper;
    }

    public async Task<UserResultDto> CreateAsync(UserCreationDto userDto)
    {
        var existingUser = await _unitOfWork.UserRepository.SelectAsync(u => u.Username == userDto.Username);
        if (existingUser is not null)
            throw new AlreadyExistException("User already exist with this username");

        var newUser = mapper.Map<User>(userDto);
        newUser.Password = userDto.Password.Hash();

        await _unitOfWork.UserRepository.AddAsync(newUser);
        await _unitOfWork.SaveAsync();

        return mapper.Map<UserResultDto>(newUser);
    }

    public async Task<UserResultDto> UpdateAsync(UserUpdateDto userDto)
    {
        var existingUser = await _unitOfWork.UserRepository.SelectAsync(u => u.Id == userDto.Id);
        if (existingUser is null)
            throw new NotFoundException("User not found");

        var username = existingUser.Username;
        var existingUser2 = await _unitOfWork.UserRepository.SelectAsync(u => u.Username == userDto.Username);
        if (username != userDto.Username && existingUser2 is not null)
            throw new AlreadyExistException("User already exist with this username");


        mapper.Map(userDto, existingUser);
        await _unitOfWork.UserRepository.UpdateAsync(existingUser);
        await _unitOfWork.SaveAsync();

        return mapper.Map<UserResultDto>(existingUser);
    }

    public async Task<bool> DeleteAsync(long id)
    {
        var user = await _unitOfWork.UserRepository.SelectAsync(u => u.Id == id);
        if (user is null)
            throw new NotFoundException("User not found");

        await _unitOfWork.UserRepository.DeleteAsync(x => x == user);
        await _unitOfWork.SaveAsync();

        return true;
    }

    public async Task<UserResultDto> GetByIdAsync(long id)
    {
        var user = await _unitOfWork.UserRepository.SelectAsync(u => u.Id == id, new string[] { "Attachment" });
        if (user is null)
            throw new NotFoundException("User not found");

        return mapper.Map<UserResultDto>(user);
    }

    public async Task<IEnumerable<UserResultDto>> GetAllAsync()
    {
        var users = _unitOfWork.UserRepository.SelectAll(null, new string[] { "Attachment" });
        return mapper.Map<IEnumerable<UserResultDto>>(users);
    }

    public async Task<IEnumerable<UserResultDto>> GetAllByNameAsync(string name)
    {
        var users = _unitOfWork.UserRepository.SelectAll(u =>
            u.Firstname.Contains(name) || u.Lastname.Contains(name), new string[] { "Attachment" });

        return mapper.Map<IEnumerable<UserResultDto>>(users);
    }

    public async Task<IEnumerable<UserResultDto>> GetAllByUsernameAsync(string username)
    {
        var users = _unitOfWork.UserRepository
            .SelectAll(u => u.Username.StartsWith(username.ToLower().Trim()), new string[] { "Attachment" });

        return mapper.Map<IEnumerable<UserResultDto>>(users);
    }

    public async Task<bool> CheckCredentialsAsync(string username, string password)
    {
        var user = await _unitOfWork.UserRepository.SelectAsync(u => u.Username == username);
        if (user is null)
            throw new NotFoundException("User not found");

        var isTruePass = password.Verify(user.Password);

        return isTruePass;
    }

    public async Task<bool> ChangePasswordAsync(long userId, string currentPassword, string newPassword)
    {
        var user = await _unitOfWork.UserRepository.SelectAsync(u => u.Id == userId);
        if (user is null)
            throw new NotFoundException("User not found");

        var isTruePass = currentPassword.Verify(user.Password);
        if (isTruePass)
        {
            user.Password = newPassword.Hash();
            return true;
        }
        else
            return false;
    }

    public async Task<AttachmentResultDto> UploadPhotoAsync(long userId, AttachmentCreationDto dto)
    {
        var user = await _unitOfWork.UserRepository.SelectAsync(u => u.Id == userId);
        if (user is null)
            throw new NotFoundException("User not found");

        var newPhoto = await attachmentService.CreateAsync(dto);
        user.AttachmentId = newPhoto.Id;
        
        await _unitOfWork.UserRepository.UpdateAsync(user);
        await _unitOfWork.SaveAsync();

        return newPhoto;
    }
}
