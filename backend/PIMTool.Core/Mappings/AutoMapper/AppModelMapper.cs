using System.ComponentModel;
using AutoMapper;
using PIMTool.Core.Domain.Entities;
using PIMTool.Core.Helpers;
using PIMTool.Core.Models;
using PIMTool.Core.Models.Dtos;
using PIMTool.Core.Models.Request;

namespace PIMTool.Core.Mappings.AutoMapper;

public static class AppModelMapper
{
    public static void MappingDto(IMapperConfigurationExpression config)
    {
        MapModels(config);
    }

    private static void MapModels(IMapperConfigurationExpression config)
    {
        #region Project mappings

        config.CreateMap<Project, DtoProject>().ReverseMap();
        config.CreateMap<Project, DtoProjectDetail>()
            .AfterMap((proj, detail) => detail.EndDate = proj.EndDate);
        config.CreateMap<CreateProjectRequest, Project>()
            .AfterMap((req, proj) => proj.Id = Guid.NewGuid());
        config.CreateMap<UpdateProjectRequest, Project>();

        #endregion

        #region Employee mappings

        config.CreateMap<Employee, DtoEmployee>();
        config.CreateMap<Employee, DtoEmployeeDetail>();
        config.CreateMap<CreateEmployeeRequest, Employee>();
        config.CreateMap<UpdateEmployeeRequest, Employee>();

        #endregion

        #region Group mappings

        config.CreateMap<CreateGroupRequest, Group>();
        config.CreateMap<UpdateGroupRequest, Group>();
        config.CreateMap<Group, DtoGroup>();
        config.CreateMap<Group, DtoGroupDetail>();
        
        #endregion

        #region User mappings
        
        config.CreateMap<UserRegisterModel, PIMUser>()
            .AfterMap((model, user) =>
            {
                user.Password = EncryptionHelper.Encrypt(model.Password);
                user.Role = "Employee";
            });

        #endregion
    }
}