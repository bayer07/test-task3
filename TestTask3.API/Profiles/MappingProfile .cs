using AutoMapper;
using TestTask3.Common.Entities;
using TestTask3.Models;
using AutoMapper.Extensions.EnumMapping;

namespace TestTask3.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //CreateMap<FileUploadEntity, FileUploadModel>()
            CreateMap<FileUploadEntity, FileUploadModel>()
                .ForMember(dest => dest.State, opt => opt.MapFrom(src => src.State));
        }
    }
}
