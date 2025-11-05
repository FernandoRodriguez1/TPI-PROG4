using AutoMapper;
using MatchTickets.Application.DTOs;
using MatchTickets.Application.Exceptions;
using MatchTickets.Application.Interfaces;
using MatchTickets.Domain.Interfaces;


namespace MatchTickets.Application.Services
{   
    public class TicketService : ITicketService
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly IMapper _mapper;
        private readonly ISoccerMatchRepository _soccerMatchRepository;
        private readonly IMembershipCardRepository _membershipCardRepository;

        public TicketService(ITicketRepository ticketRepository, IMapper mapper, ISoccerMatchRepository soccerMatchRepository, IMembershipCardRepository membershipCardRepository)
        {
            _ticketRepository = ticketRepository;
            _mapper = mapper;
            _soccerMatchRepository = soccerMatchRepository;
            _membershipCardRepository = membershipCardRepository;
        }

        public async Task<IEnumerable<TicketDTO>> GetTicketsByClientAsync(int clientId)
        {
            var tickets = await _ticketRepository.GetTicketsByClientIdAsync(clientId);
            return _mapper.Map<IEnumerable<TicketDTO>>(tickets);
        }

        public async Task<TicketDTO> BuyTicketAsync(int clientId, int matchId)
        {
            
            var match = await _soccerMatchRepository.GetByIdAsync(matchId);
            if (match == null)
                throw new NotFoundException($"Partido con ID {matchId} no encontrado.");

            var availableCount = await _ticketRepository.GetAvailableTicketsCountAsync(matchId);
            if (availableCount <= 0)
                throw new AppValidationException("No hay más entradas disponibles para este partido.");

           
            var hasTicket = await _ticketRepository.ClientHasTicketAsync(clientId, matchId);
            if (hasTicket)
                throw new AppValidationException("El cliente ya tiene un ticket para este partido.");

            
            var newTicket = await _ticketRepository.CreateTicketAsync(clientId, matchId);

            return _mapper.Map<TicketDTO>(newTicket);
        }

    }

}
