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
    public class TicketProfile : Profile
    {
        public TicketProfile()
        {
            CreateMap<Ticket, TicketDTO>()
                // llena el campo ClientMembershipNumber del DTO con el número de la tarjeta del cliente (si existe)
                .ForMember(dest => dest.ClientMembershipNumber,
                           opt => opt.MapFrom(src => src.Client != null ? src.Client.MembershipCard.MembershipCardNumber : null))
                // llena el campo SoccerMatchDescription con el nombre del club y la fecha del partido
                .ForMember(dest => dest.SoccerMatchDescription,
                           opt => opt.MapFrom(src => src.SoccerMatch != null
                               ? $"{src.SoccerMatch.Club.ClubName} - {src.SoccerMatch.DayOfTheMatch}"
                               : null));
            CreateMap<TicketDTO, Ticket>();
        }
    }


}
