using AutoMapper;
using MatchTickets.Application.DTOs;
using MatchTickets.Application.Interfaces;
using MatchTickets.Domain.Entities;
using MatchTickets.Domain.Interfaces;
using MatchTickets.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                throw new KeyNotFoundException($"Club con ID {clubId} no encontrado.");

            return club;
        }

        public async Task<IEnumerable<SoccerMatch>> GetMatchesAsync(int clubId)
        {
            var matches = await _clubRepository.GetMatchesAsync(clubId);

            if (!matches.Any())
                throw new KeyNotFoundException($"No se encontraron partidos para el club con ID {clubId}.");

            return matches;
        }

        public async Task<int> GetMembersCountAsync(int clubId)
        {
            var count = await _clubRepository.GetMembersCountAsync(clubId);
            return count;
        }

        public async Task UpdateAsync(ClubDTO clubDto)
        {
            if (clubDto == null)
                throw new ArgumentNullException(nameof(clubDto), "El club no puede ser nulo.");

            var existingClub = await _clubRepository.GetByIdAsync(clubDto.ClubId);
            if (existingClub == null)
                throw new KeyNotFoundException($"Club con ID {clubDto.ClubId} no encontrado.");

            _mapper.Map(clubDto, existingClub);
            await _clubRepository.UpdateAsync(existingClub);
        }


        public async Task DeleteAsync(int clubId)
        {
            var club = await _clubRepository.GetByIdAsync(clubId);
            if (club == null)
                throw new KeyNotFoundException($"Club con ID {clubId} no encontrado.");

            await _clubRepository.DeleteAsync(clubId);
        }

        public async Task AddAsync(ClubDTO clubDto)
        {
            if (clubDto == null)
                throw new ArgumentNullException(nameof(clubDto));
            var club = _mapper.Map<Club>(clubDto);

            await _clubRepository.AddAsync(club);
        }


    }

}
