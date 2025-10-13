using AutoMapper;
using MatchTickets.Application.DTOs;
using MatchTickets.Application.Interfaces;
using MatchTickets.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatchTickets.Application.Services
{
    public class TicketService : ITicketService
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly IMapper _mapper;
        private readonly ISoccerMatchRepository _soccerMatchRepository;

        public TicketService(ITicketRepository ticketRepository, IMapper mapper, ISoccerMatchRepository soccerMatchRepository)
        {
            _ticketRepository = ticketRepository;
            _mapper = mapper;
            _soccerMatchRepository = soccerMatchRepository;
        }

        public async Task<IEnumerable<TicketDTO>> GetTicketsByClientAsync(int clientId)
        {
            var tickets = await _ticketRepository.GetTicketsByClientIdAsync(clientId);
            return _mapper.Map<IEnumerable<TicketDTO>>(tickets);
        }

        public async Task<TicketDTO> CreateTicketAsync(int clientId, int matchId)
        {
            var newTicket = await _ticketRepository.CreateTicketAsync(clientId, matchId);
            return _mapper.Map<TicketDTO>(newTicket);
        }
        public async Task<TicketDTO> BuyTicketAsync(int clientId, int matchId)
        {
            var match = await _soccerMatchRepository.GetByIdAsync(matchId);
            if (match == null)
                throw new KeyNotFoundException("Partido no encontrado.");

            var membership = match.Club.MembershipCards
                .FirstOrDefault(mc => mc.ClientId == clientId);

            if (membership == null)
                throw new InvalidOperationException("El cliente no tiene una membership activa para este club.");

            var newTicket = await _ticketRepository.CreateTicketAsync(clientId, matchId);
            return _mapper.Map<TicketDTO>(newTicket);
        }

    }

}
