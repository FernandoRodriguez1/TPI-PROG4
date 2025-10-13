﻿using AutoMapper;
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
        private readonly ISoccerMatchRepository _soccerMatchRepository;
        private readonly IMapper _mapper;

        public SoccerMatchService(ISoccerMatchRepository soccerMatchRepository, IMapper mapper)
        {
            _soccerMatchRepository = soccerMatchRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<SoccerMatchDTO>> GetAllMatchesAsync()
        {
            var matches = await _soccerMatchRepository.GetAllAsync();

            var matchDtos = _mapper.Map<IEnumerable<SoccerMatchDTO>>(matches);

            foreach (var dto in matchDtos)
            {
                var matchEntity = matches.FirstOrDefault(m => m.SoccerMatchId == dto.SoccerMatchId);
                if (matchEntity != null)
                {
                    dto.MaxTickets = matchEntity.MaxTickets;
                    dto.NumberTicketsAvailable = matchEntity.NumberTicketsAvailable;
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
            dto.MaxTickets = match.MaxTickets;
            dto.NumberTicketsAvailable = match.NumberTicketsAvailable;

            return dto;
        }

        public async Task<IEnumerable<SoccerMatchDTO>> GetMatchesByClubAsync(int clubId)
        {
            var matches = await _soccerMatchRepository.GetByClubIdAsync(clubId);
            var dtos = _mapper.Map<IEnumerable<SoccerMatchDTO>>(matches);

            foreach (var dto in dtos)
            {
                var matchEntity = matches.FirstOrDefault(m => m.SoccerMatchId == dto.SoccerMatchId);
                if (matchEntity != null)
                {
                    dto.MaxTickets = matchEntity.MaxTickets;
                    dto.NumberTicketsAvailable = matchEntity.NumberTicketsAvailable;
                }
            }

            return dtos;
        }

        public async Task AddMatchAsync(SoccerMatchDTO matchDto)
        {
            if (matchDto == null)
                throw new ArgumentNullException(nameof(matchDto));

            var entity = _mapper.Map<SoccerMatch>(matchDto);

            // instancia de valores por defecto
            if (entity.MaxTickets <= 0)
                entity.MaxTickets = 30;

            await _soccerMatchRepository.AddAsync(entity);
            await _soccerMatchRepository.SaveChangesAsync();
        }

        public async Task UpdateMatchAsync(SoccerMatchDTO matchDto)
        {
            if (matchDto == null)
                throw new ArgumentNullException(nameof(matchDto));

            var existingMatch = await _soccerMatchRepository.GetByIdAsync(matchDto.SoccerMatchId);
            if (existingMatch == null)
                throw new KeyNotFoundException($"Partido con ID {matchDto.SoccerMatchId} no encontrado.");

            // no se permiten actualizaciones de campos no permitidos
            existingMatch.DayOfTheMatch = matchDto.DayOfTheMatch;
            existingMatch.TimeOfTheMatch = matchDto.TimeOfTheMatch;
            existingMatch.MatchLocation = matchDto.MatchLocation;
            existingMatch.MaxTickets = matchDto.MaxTickets;

            _soccerMatchRepository.Update(existingMatch);
            await _soccerMatchRepository.SaveChangesAsync();
        }

        public async Task DeleteMatchAsync(int matchId)
        {
            var match = await _soccerMatchRepository.GetByIdAsync(matchId);
            if (match == null)
                throw new KeyNotFoundException($"Partido con ID {matchId} no encontrado.");

            _soccerMatchRepository.Delete(match);
            await _soccerMatchRepository.SaveChangesAsync();
        }
    }

}
