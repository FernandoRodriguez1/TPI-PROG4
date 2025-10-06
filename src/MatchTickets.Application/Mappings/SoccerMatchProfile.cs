using AutoMapper;
using MatchTickets.Application.DTOs;
using MatchTickets.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatchTickets.Application.Mappings
{
    public class SoccerMatchProfile : Profile
    {
        public SoccerMatchProfile()
        {
            
            CreateMap<SoccerMatchDTO, SoccerMatch>();

            
            CreateMap<SoccerMatch, SoccerMatchDTO>()
                .ForMember(dest => dest.ClubName,
                    opt => opt.MapFrom(src => src.Club.ClubName)) 
                .ForMember(dest => dest.NumberTicketsAvailable,
                    opt => opt.MapFrom(src => src.Tickets != null
                        ? src.Tickets.Count(t => t.IsAvailable)
                        : 0)); 
        }
    }

}
