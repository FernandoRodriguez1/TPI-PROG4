using AutoMapper;

using MatchTickets.Domain.Entities;
using MatchTickets.Application.DTOs;

namespace MatchTickets.Application.Mappings
{
    public class AdminProfile : Profile
    {
        public AdminProfile()
        {
            CreateMap<Admin, AdminDTO>();
            CreateMap<AdminDTO, Admin>();
        }
    }
}
