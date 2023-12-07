using AutoMapper;
using Application.Commons;
using Domain.Entities;
using Application.ViewModels.ProjectViewModels;
using Application.ViewModels.EmployeeViewModels;
using Application.ViewModels.GroupViewModels;

namespace Infrastructures.Mappers
{
    public class MapperConfigurationsProfile : Profile
    {
        public MapperConfigurationsProfile()
        {
            CreateMap<Project, ProjectViewModel>();
            CreateMap<CreateProjectViewModel, Project>();
            CreateMap<Project, UpdateProjectViewModel>().ReverseMap();

            CreateMap<CreateGroupViewModel, Group>();
            CreateMap<Group, GroupViewModel>();
            CreateMap<Group, UpdateGroupViewModel>().ReverseMap();

            CreateMap<CreateEmployeeViewModel, Employee>();
            CreateMap<Employee, EmployeeViewModel>();
            CreateMap<Employee, UpdateEmployeeViewModel>().ReverseMap();

            CreateMap(typeof(Pagination<>), typeof(Pagination<>));
        }
    }
}
