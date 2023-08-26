using AutoMapper;
using MyDairy.Domain.Entities;
using MyDairy.Service.DTOs.Attachment;
using MyDairy.Service.DTOs.Notes;
using MyDairy.Service.DTOs.Users;

namespace MyDairy.Service.Mappers;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        //user

        CreateMap<UserCreationDto, User>().ReverseMap();
        CreateMap<UserUpdateDto, User>().ReverseMap();
        CreateMap<User, UserResultDto>().ReverseMap();

        //note

        CreateMap<NoteCreationDto, Note>().ReverseMap();
        CreateMap<NoteUpdateDto, Note>().ReverseMap();
        CreateMap<Note, NoteResultDto>().ReverseMap();

        //attachment

        CreateMap<Attachment, AttachmentResultDto>().ReverseMap();
    }
}
