using Board_Game_Manager.DAL;
using Board_Game_Manager.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Board_Game_Manager.BLL {
    public class PlayerService {
        private readonly PlayerRepository _repository;
        private readonly BoardGameContext _context;

        public PlayerService(PlayerRepository repository) {
            _repository = repository;
        }
        public List<Player> GetAllPlayers() {
            return _repository.GetAllPlayers();
        }
        public Player? GetPlayerById(int id) {
            return _repository.GetPlayerById(id);
        }
        public void CreatePlayer(Player player) {
            _repository.CreatePlayer(player);
        }
        public void EditPlayer(Player player) {
            _repository.EditPlayer(player);
        }
        public void DeletePlayer(int id) {
            _repository.DeletePlayer(id);
        }

        public List<PlayerLeaderboardItem> GetTotalScoreLeaderboard()
        {
            return _repository.GetAllPlayers()
                .OrderByDescending(p => p.TotalScore)
                .Select(p => new PlayerLeaderboardItem
                {
                    PlayerID = p.PlayerID,
                    Name = p.Name,
                    TotalScore = p.TotalScore,
                    TotalHours = p.TotalHours
                }).ToList();
        }

        public List<PlayerLeaderboardItem> GetTotalHoursLeaderboard()
        {
            return _repository.GetAllPlayers()
                .OrderByDescending(p => p.TotalHours)
                .Select(p => new PlayerLeaderboardItem
                {
                    PlayerID = p.PlayerID,
                    Name = p.Name,
                    TotalHours = p.TotalHours,
                    TotalScore = p.TotalScore
                }).ToList();
        }
        public List<PlayerLeaderboardItem> GetTournamentLeaderboard(int tournamentId)
        {
            var result = _context.SessionParticipants
                .Where(sp => sp.GameSession.TournamentID == tournamentId)
                .GroupBy(sp => sp.Player)
                .Select(g => new PlayerLeaderboardItem
                {
                    PlayerID = g.Key.PlayerID,
                    Name = g.Key.Name,

                    TotalScore = g.Sum(x => x.Score),

                    TotalHours = g.Sum(x => x.GameSession.PlayTimeMinutes) / 60.0,

                    GMContribution = _context.GameSessions
                        .Count(gs => gs.GMID == g.Key.PlayerID && gs.TournamentID == tournamentId)
                })
                .ToList();

            return result;
        }
    }
}
