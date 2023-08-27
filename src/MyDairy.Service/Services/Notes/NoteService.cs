using AutoMapper;
using MyDairy.DAL.IRepositories;
using MyDairy.Domain.Entities;
using MyDairy.Service.DTOs.Notes;
using MyDairy.Service.Exceptions;
using MyDairy.Service.Interfaces.Notes;

namespace MyDairy.Service.Services.Notes;

public class NoteService : INoteService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public NoteService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<NoteResultDto> CreateAsync(NoteCreationDto dto)
    {
        var user = await _unitOfWork.UserRepository.SelectAsync(u => u.Id == dto.UserId);
        if (user is null)
            throw new NotFoundException("User not found");

        var attachment = await _unitOfWork.AttachmentRepository.SelectAsync(a => a.Id == dto.AttachmentId);
        if (dto.AttachmentId is not null && attachment is null)
            throw new NotFoundException("Attachment not found");

        var portfolio = _mapper.Map<Note>(dto);

        await _unitOfWork.NoteRepository.AddAsync(portfolio);
        await _unitOfWork.SaveAsync();

        return _mapper.Map<NoteResultDto>(portfolio);
    }

    public async Task<NoteResultDto> UpdateAsync(NoteUpdateDto dto)
    {
        var existingNote = await _unitOfWork.NoteRepository.SelectAsync(p => p.Id == dto.Id);
        if (existingNote is null)
            throw new NotFoundException("Note not found");

        var user = await _unitOfWork.UserRepository.SelectAsync(u => u.Id == dto.UserId);
        if (user is null)
            throw new NotFoundException("User not found");

        var attachment = await _unitOfWork.AttachmentRepository.SelectAsync(a => a.Id == dto.AttachmentId);
        if (attachment is null)
            throw new NotFoundException("Attachment not found");

        _mapper.Map(dto, existingNote);

        await _unitOfWork.NoteRepository.UpdateAsync(existingNote);
        await _unitOfWork.SaveAsync();

        return _mapper.Map<NoteResultDto>(existingNote);
    }

    public async Task<bool> DeleteAsync(long id)
    {
        var portfolio = await _unitOfWork.NoteRepository.SelectAsync(p => p.Id == id);
        if (portfolio is null)
            throw new NotFoundException("Note not found");

        await _unitOfWork.NoteRepository.DeleteAsync(p => p == portfolio);
        await _unitOfWork.SaveAsync();

        return true;
    }

    public async Task<NoteResultDto> GetByIdAsync(long id)
    {
        var portfolio = await _unitOfWork.NoteRepository.SelectAsync(p => p.Id == id, new string[] { "User", "Attachment" });
        if (portfolio is null)
            throw new NotFoundException("Note not found");

        return _mapper.Map<NoteResultDto>(portfolio);
    }

    public async Task<IEnumerable<NoteResultDto>> GetAllAsync()
    {
        var portfolios = _unitOfWork.NoteRepository.SelectAll();
        return _mapper.Map<IEnumerable<NoteResultDto>>(portfolios);
    }


    public async Task<IEnumerable<NoteResultDto>> GetAllByUserIdAsync(long userId)
    {
        var portfolios = _unitOfWork.NoteRepository.SelectAll(p => p.UserId == userId);
        return _mapper.Map<IEnumerable<NoteResultDto>>(portfolios);
    }

    public async Task<IEnumerable<NoteResultDto>> GetAllByTitleAsync(string title)
    {
        var portfolios = _unitOfWork.NoteRepository.SelectAll(p =>
            p.Title.Contains(title));

        return _mapper.Map<IEnumerable<NoteResultDto>>(portfolios);
    }
}
