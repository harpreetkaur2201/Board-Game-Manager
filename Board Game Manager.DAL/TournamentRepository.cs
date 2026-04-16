using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Board_Game_Manager.Models;

namespace Board_Game_Manager.DAL
{
    public class TournamentRepository
    {
        private readonly BoardGameContext _context;

        public TournamentRepository(BoardGameContext context)
        {
            _context = context;
        }

        public List<Tournament> GetAllTournaments()
        {
            return _context.Tournaments
                .Include(t => t.Game) 
                .Include(t => t.GameSessions)
                    .ThenInclude(gs => gs.GameMaster)
                    .ThenInclude(gm => gm.Player)
                .Include(t => t.GameSessions)
                    .ThenInclude(gs => gs.SessionParticipants)
                    .ThenInclude(sp => sp.Player)
                .OrderByDescending(t => t.StartDate)
                .ToList();
        }

        public Tournament? GetTournamentById(int id)
        {
            return _context.Tournaments
                .Include(t => t.Game)
                .Include(t => t.GameSessions)
                    .ThenInclude(gs => gs.GameMaster)
                    .ThenInclude(gm => gm.Player)
                .Include(t => t.GameSessions)
                    .ThenInclude(gs => gs.SessionParticipants)
                    .ThenInclude(sp => sp.Player)
                .FirstOrDefault(t => t.TournamentID == id);
        }

        public void CreateTournament(Tournament tournament)
        {
            _context.Tournaments.Add(tournament);
            _context.SaveChanges();
        }

        public void EditTournament(Tournament tournament)
        {
            _context.Tournaments.Update(tournament);
            _context.SaveChanges();
        }

        public void DeleteTournament(int id)
        {
            Tournament? tournament = _context.Tournaments
                .FirstOrDefault(t => t.TournamentID == id);
            if (tournament != null)
            {
                _context.Tournaments.Remove(tournament);
                _context.SaveChanges();
            }
        }
    }
}