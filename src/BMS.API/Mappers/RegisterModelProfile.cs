using AutoMapper;
using BMS.API.Data.Entities;
using BMS.API.Models;

namespace BMS.API.Mappers;

public class RegisterModelProfile : Profile
{
    public RegisterModelProfile()
    {
        CreateMap<RegisterModel, ApplicationUser>();
    }
}
