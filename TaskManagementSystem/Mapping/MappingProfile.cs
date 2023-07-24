using AutoMapper;
using Microsoft.AspNetCore.Identity;
using TaskManagementSystem.Business.DTOs;
using TaskManagementSystem.DataAccess.Models;

namespace TaskManagementSystem.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<TaskModel, TaskDto>();
        CreateMap<TaskDto, TaskModel>();
        CreateMap<LoginDto, IdentityUser>();
        CreateMap<IdentityUser, LoginDto>();
    }
}

