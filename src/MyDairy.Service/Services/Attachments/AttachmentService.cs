using AutoMapper;
using MyDairy.DAL.IRepositories;
using MyDairy.Domain.Entities;
using MyDairy.Service.DTOs.Attachment;
using MyDairy.Service.Exceptions;
using MyDairy.Service.Extensions;
using MyDairy.Service.Helpers;
using MyDairy.Service.Interfaces.Attacments;

namespace MyDairy.Service.Services.Attacments;

public class AttachmentService : IAttachmentService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public AttachmentService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<AttachmentResultDto> CreateAsync(AttachmentCreationDto attachmentDto)
    {
        var webrootPath = Path.Combine(PathHelper.WebRootPath, "images");

        if (!Directory.Exists(webrootPath))
            Directory.CreateDirectory(webrootPath);

        var fileExtension = Path.GetExtension(attachmentDto.File.FileName);
        var fileName = $"{Guid.NewGuid().ToString("N")}{fileExtension}";
        var fullPath = Path.Combine(webrootPath, fileName);

        var fileStream = new FileStream(fullPath, FileMode.OpenOrCreate);
        await fileStream.WriteAsync(attachmentDto.File.ToByte());

        var attachment = new Attachment()
        {
            Name = fileName,
            Path = fullPath
        };

        await _unitOfWork.AttachmentRepository.AddAsync(attachment);
        await _unitOfWork.SaveAsync();

        return _mapper.Map<AttachmentResultDto>(attachment);
    }

    public async Task<AttachmentResultDto> UpdateAsync(AttachmentUpdateDto attachmentDto)
    {
        var existingAttachment = await _unitOfWork.AttachmentRepository.SelectAsync(a => a.Id == attachmentDto.Id);
        if (existingAttachment is null)
            throw new NotFoundException("Attachment not found");

        var webrootPath = Path.Combine(PathHelper.WebRootPath, "Portfolios");

        if (!Directory.Exists(webrootPath))
            Directory.CreateDirectory(webrootPath);

        var fileExtension = Path.GetExtension(attachmentDto.File.FileName);
        var fileName = $"{Guid.NewGuid().ToString("N")}{fileExtension}";
        var fullPath = Path.Combine(webrootPath, fileName);

        var fileStream = new FileStream(fullPath, FileMode.OpenOrCreate);
        await fileStream.WriteAsync(attachmentDto.File.ToByte());

        existingAttachment.Name = fileName;
        existingAttachment.Path = fullPath;

        await _unitOfWork.AttachmentRepository.UpdateAsync(existingAttachment);
        await _unitOfWork.SaveAsync();

        return _mapper.Map<AttachmentResultDto>(existingAttachment);
    }

    public async Task<bool> DeleteAsync(long id)
    {
        var attachment = await _unitOfWork.AttachmentRepository.SelectAsync(a => a.Id == id);
        if (attachment is null)
            throw new NotFoundException("Attachment not found");

        await _unitOfWork.AttachmentRepository.DeleteAsync(a => a == attachment);
        await _unitOfWork.SaveAsync();

        return true;
    }

    public async Task<AttachmentResultDto> GetByIdAsync(long id)
    {
        var attachment = await _unitOfWork.AttachmentRepository.SelectAsync(a => a.Id == id);
        if (attachment is null)
            throw new NotFoundException("Attachment not found");

        return _mapper.Map<AttachmentResultDto>(attachment);
    }

    public async Task<IEnumerable<AttachmentResultDto>> GetAllAsync()
    {
        var attachments = _unitOfWork.AttachmentRepository.SelectAll();
        return _mapper.Map<IEnumerable<AttachmentResultDto>>(attachments);
    }
}
