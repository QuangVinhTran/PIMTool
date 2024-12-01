using AutoMapper;
using PIMTool.Core.Domain.Entities;
using PIMTool.Dtos;

namespace PIMTool.MappingProfiles
{
    public class ProjectAutoMapperProfile : Profile
    {
        public ProjectAutoMapperProfile()
        {
            CreateMap<Project, ProjectDto>();
            CreateMap<Project, CreateProjectRequestDto>()
                .ForMember(dest => dest.Members, opt => opt.MapFrom(src => src.ProjectEmployees.Select(pe => pe.EmployeeId).ToList()));
            CreateMap<CreateProjectRequestDto, Project>();
        }
    }
}