using AutoMapper;
using MatchTickets.Application.DTOs;
using MatchTickets.Application.Exceptions;
using MatchTickets.Application.Interfaces;
using MatchTickets.Domain.Entities;
using MatchTickets.Domain.Interfaces;


namespace MatchTickets.Application.Services
{
    public class ClubService : IClubService
    {
        private readonly IClubRepository _clubRepository;
        private readonly IMapper _mapper;

        public ClubService(IClubRepository clubRepository, IMapper mapper)
        {
            _clubRepository = clubRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ClubDTO>> GetAllAsync()
        {
            var clubs = await _clubRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<ClubDTO>>(clubs);
        }

        public async Task<Club?> GetByIdAsync(int clubId)
        {
            var club = await _clubRepository.GetByIdAsync(clubId);

            if (club == null)
                throw new NotFoundException($"Club con ID {clubId} no encontrado.", "CLUB_NOT_FOUND");

            return club;
        }

        public async Task<IEnumerable<SoccerMatchDTO>> GetMatchesAsync(int clubId)
        {
            var matches = await _clubRepository.GetMatchesAsync(clubId);

            if (!matches.Any())
                throw new NotFoundException($"No se encontraron partidos para el club con ID {clubId}.", "MATCHES_NOT_FOUND");

            var matchDtos = matches.Select(m => new SoccerMatchDTO
            {
                SoccerMatchId = m.SoccerMatchId,
                DayOfTheMatch = m.DayOfTheMatch,
                TimeOfTheMatch = m.TimeOfTheMatch,
                ClubId = m.ClubId,
                ClubName = m.ClubName,
                MatchLocation = m.MatchLocation,
                NumberTicketsAvailable = m.Tickets?.Count(t => t.IsAvailable) ?? 0
            }).ToList();

            return matchDtos;
        }

        public async Task<int> GetMembersCountAsync(int clubId)
        {
            var count = await _clubRepository.GetMembersCountAsync(clubId);
            return count;
        }

        public async Task AddAsync(ClubDTO clubDto)
        {
            if (clubDto == null)
                throw new AppValidationException("El club no puede ser nulo.", "CLUB_NULL");

            var club = _mapper.Map<Club>(clubDto);
            await _clubRepository.AddAsync(club);
            await _clubRepository.SaveChangesAsync();
        }

        public async Task UpdateAsync(ClubDTO clubDto)
        {
            if (clubDto == null)
                throw new AppValidationException("El club no puede ser nulo.", "CLUB_NULL");

            var existingClub = await _clubRepository.GetByIdAsync(clubDto.ClubId);
            if (existingClub == null)
                throw new NotFoundException($"Club con ID {clubDto.ClubId} no encontrado.", "CLUB_NOT_FOUND");

            // actualiza los valores de la entidad existente
            _mapper.Map(clubDto, existingClub);
            _clubRepository.Update(existingClub);
            await _clubRepository.SaveChangesAsync();
        }

        public async Task DeleteAsync(int clubId)
        {
            var club = await _clubRepository.GetByIdAsync(clubId);
            if (club == null)
                throw new NotFoundException($"Club con ID {clubId} no encontrado.", "CLUB_NOT_FOUND");

            _clubRepository.Delete(club);
            await _clubRepository.SaveChangesAsync();
        }
    }


}
