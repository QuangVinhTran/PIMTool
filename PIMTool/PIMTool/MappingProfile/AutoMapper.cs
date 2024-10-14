using AutoMapper;
using PIMTool.Entities;
using PIMTool.Payload.Request.Authentication;
using PIMTool.Payload.Request.Service;
using PIMTool.Payload.Response;

namespace PIMTool.MappingProfile;

public class AutoMapper : Profile
{
    public AutoMapper()
    {
        MapProject();   
        MapEmployee();
        MapGroup();
        MapAuthentication();
    }

    private void MapAuthentication()
    {
        CreateMap<UserAuthentication, UserEntity>();
    }

    private void MapProject()
    {
        //Response
        CreateMap<ProjectEntity, ProjectResponse>();
        //Create project request
        CreateMap<CProjectRequest, ProjectEntity>();
    }

    private void MapEmployee()
    {
        CreateMap<EmployeeEntity, EmployeeEntity>();
        //Response
        CreateMap<EmployeeEntity, EmployeeResponse>();
        //Create employee request
        CreateMap<CEmployeeRequest, EmployeeEntity>();
        //Delete employee request
        CreateMap<DEmployeeRequest, EmployeeEntity>();
        //Update employee request
        CreateMap<UEmployeeRequest, EmployeeEntity>();
    }

    private void MapGroup()
    {
        //Response
        CreateMap<GroupEntity, GroupResponse>();
        //Create group request
        CreateMap<CGroupRequest, GroupEntity>();
        //Delete group request
        CreateMap<DGroupRequest, GroupEntity>();
    }
}