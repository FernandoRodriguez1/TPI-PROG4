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
    public class ClientProfile : Profile
    {
        public ClientProfile()
        {
            CreateMap<ClientDTO, Client>();
            CreateMap<Client, ClientDTO>();
        }
    }
}
