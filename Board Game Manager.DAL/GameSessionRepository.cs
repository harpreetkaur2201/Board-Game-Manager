using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Board_Game_Manager.Models;
using Microsoft.EntityFrameworkCore;
namespace Board_Game_Manager.DAL
{
    public class GameSessionRepository
    {
        private readonly BoardGameContext _context;
        public GameSessionRepository(BoardGameContext context)
        {
            _context = context;
        }
        public List<GameSession> GetAllGameSessions()
        {
            return _context.GameSessions
                .Include(gs => gs.Game)
                .Include(gs => gs.GameMaster)
                    .ThenInclude(gm => gm.Player)
                .Include(gs => gs.Tournament)
                .OrderByDescending(gs => gs.SessionID)
                .ToList();
        }
        public GameSession? GetGameSessionById(int id)
        {
            return _context.GameSessions
                .Include(gs => gs.Game)
                .Include(gs => gs.GameMaster)
                    .ThenInclude(gm => gm.Player)
                .Include(gs => gs.Tournament)
                .Include(gs => gs.SessionParticipants)
                    .ThenInclude(sp => sp.Player)
                .FirstOrDefault(gs => gs.SessionID == id);
        }
        public void CreateSession(GameSession gamesession)
        {
            _context.GameSessions.Add(gamesession);
            _context.SaveChanges();
        }
        public void EditSession(GameSession gamesession)
        {
            _context.GameSessions.Update(gamesession);
            _context.SaveChanges();
        }
        public void DeleteSession(int id)
        {
            GameSession? gamesession = _context.GameSessions
                .FirstOrDefault(gs => gs.SessionID == id);

            if (gamesession != null)
            {
                _context.GameSessions.Remove(gamesession);
                _context.SaveChanges();
            }
        }
        public bool CreateParticipant(SessionParticipant participant)
        {
            bool exist = _context.SessionParticipants
                .Any(sp => sp.SessionID == participant.SessionID && sp.PlayerID == participant.PlayerID);
            if (exist)
            {
                return false;
            }
            GameSession? session = _context.GameSessions
                .Include(gs => gs.Game)
                .Include(gs => gs.SessionParticipants)
                .FirstOrDefault(gs => gs.SessionID == participant.SessionID);

            if (session == null || session.Game == null)
            {
                return false;
            }
            Player? player = _context.Players
                .FirstOrDefault(p => p.PlayerID == participant.PlayerID);

            if (player == null)
            {
                return false;
            }
            if (player.Age < session.Game.AgeReq)
            {
                return false;
            }
            if (session.SessionParticipants.Count >= session.Game.MaxPlayerCount)
            {
                return false;
            }
            if (participant.JoinedAt == default)
            {
                participant.JoinedAt = DateTime.Now;
            }
            _context.SessionParticipants.Add(participant);
            player.TotalScore += participant.Score;
            _context.SaveChanges();
            return true;
        }
        public SessionParticipant? GetParticipant(int sessionId, int playerId)
        {
            return _context.SessionParticipants
                .Include(sp => sp.Player)
                .Include(sp => sp.GameSession)
                .FirstOrDefault(sp => sp.SessionID == sessionId && sp.PlayerID == playerId);
        }
        public bool EditParticipant(SessionParticipant participant)
        {
            SessionParticipant? existing = _context.SessionParticipants
                .FirstOrDefault(sp => sp.SessionID == participant.SessionID && sp.PlayerID == participant.PlayerID);

            if (existing == null)
            {
                return false;
            }
            Player? player = _context.Players
                .FirstOrDefault(p => p.PlayerID == participant.PlayerID);

            if (player == null)
            {
                return false;
            }
            int scoreDifference = participant.Score - existing.Score;

            existing.Score = participant.Score;
            existing.Rank = participant.Rank;
            existing.IsWinner = participant.IsWinner;
            existing.JoinedAt = participant.JoinedAt;

            player.TotalScore += scoreDifference;

            if (player.TotalScore < 0)
            {
                player.TotalScore = 0;
            }
            _context.SaveChanges();
            return true;
        }
        public bool DeleteParticipant(int sessionId, int playerId)
        {
            SessionParticipant? participant = _context.SessionParticipants
                .FirstOrDefault(sp => sp.SessionID == sessionId && sp.PlayerID == playerId);

            if (participant == null)
            {
                return false;
            }
            Player? player = _context.Players
                .FirstOrDefault(p => p.PlayerID == playerId);

            if (player != null)
            {
                player.TotalScore -= participant.Score;

                if (player.TotalScore < 0)
                {
                    player.TotalScore = 0;
                }
            }
            _context.SessionParticipants.Remove(participant);
            _context.SaveChanges();

            return true;
        }
        public List<GameSession> GetSessionsByTournament(int id)
        {
            return _context.GameSessions
                .Include(gs => gs.Game)
                .Include(gs => gs.GameMaster)
                .ThenInclude(gm => gm.Player)
                .Include(gs => gs.Tournament)
                .Where(gs => gs.TournamentID == id)
                .OrderByDescending(gs => gs.GameStartedAt)
                .ToList();
        }

        public List<GameSession> GetSessionsByGame(int id)
        {
            return _context.GameSessions
                .Include(gs => gs.Game)
                .Include(gs => gs.GameMaster)
                .ThenInclude(gm => gm.Player)
                .Include(gs => gs.Tournament)
                .Where(gs => gs.GameID == id)
                .OrderByDescending(gs => gs.GameStartedAt)
                .ToList();
        }

        public List<Game> GetAllGames()
        {
            return _context.Games
                .OrderBy(g => g.Name)
                .ToList();
        }
        public List<GameMaster> GetAllGameMasters()
        {
            return _context.GameMasters
                .Include(gm => gm.Player)
                .OrderBy(gm => gm.Player!.Name)
                .ToList();
        }
        public List<Tournament> GetAllTournaments()
        {
            return _context.Tournaments
                .OrderBy(t => t.Name)
                .ToList();
        }
        public List<Player> GetAllPlayers()
        {
            return _context.Players
                .OrderBy(p => p.Name)
                .ToList();
        }
    }
}

