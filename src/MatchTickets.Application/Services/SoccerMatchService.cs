using AutoMapper;
using MatchTickets.Application.DTOs;
using MatchTickets.Application.Interfaces;
using MatchTickets.Domain.Entities;
using MatchTickets.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatchTickets.Application.Services
{
    public class SoccerMatchService : ISoccerMatchService
    {
        private readonly IMapper _mapper;
        private readonly ISoccerMatchRepository _soccerMatchRepository;

        public SoccerMatchService(IMapper mapper, ISoccerMatchRepository soccerMatchRepository)
        {
            _mapper = mapper;
            _soccerMatchRepository = soccerMatchRepository;
        }

        public async Task AddMatchAsync(SoccerMatchDTO matchDto)
        {
            var entity = _mapper.Map<SoccerMatch>(matchDto);
            await _soccerMatchRepository.AddAsync(entity);
        }

        public async Task DeleteMatchAsync(int matchId)
        {
            await _soccerMatchRepository.DeleteAsync(matchId);
        }

        public async Task<IEnumerable<SoccerMatchDTO>> GetAllMatchesAsync()
        {
            var matches = await _soccerMatchRepository.GetAllAsync();
            
            var matchDtos = _mapper.Map<IEnumerable<SoccerMatchDTO>>(matches);

            foreach (var dto in matchDtos)
            {
                var matchEntity = matches.FirstOrDefault(m => m.SoccerMatchId == dto.SoccerMatchId);
                if (matchEntity?.Tickets != null)
                {
                    dto.NumberTicketsAvailable = matchEntity.Tickets.Count(t => t.IsAvailable);
                }
            }

            return matchDtos;
        }

        public async Task<SoccerMatchDTO?> GetMatchByIdAsync(int matchId)
        {
            var match = await _soccerMatchRepository.GetByIdAsync(matchId);
            if (match == null)
                return null;

            var dto = _mapper.Map<SoccerMatchDTO>(match);

            if (match.Tickets != null)
            {
                dto.NumberTicketsAvailable = match.Tickets.Count(t => t.IsAvailable);
            }

            return dto;
        }

        public async Task<IEnumerable<SoccerMatchDTO>> GetMatchesByClubAsync(int clubId)
        {
            var matches = await _soccerMatchRepository.GetByClubIdAsync(clubId);
            var dtos = _mapper.Map<IEnumerable<SoccerMatchDTO>>(matches);

            foreach (var dto in dtos)
            {
                var matchEntity = matches.FirstOrDefault(m => m.SoccerMatchId == dto.SoccerMatchId);
                if (matchEntity?.Tickets != null)
                {
                    dto.NumberTicketsAvailable = matchEntity.Tickets.Count(t => t.IsAvailable);
                }
            }

            return dtos;
        }

        public async Task UpdateMatchAsync(SoccerMatchDTO matchDto)
        {
            var entity = _mapper.Map<SoccerMatch>(matchDto);
            await _soccerMatchRepository.UpdateAsync(entity);
        }
    }

}
