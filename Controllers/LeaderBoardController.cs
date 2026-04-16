using Board_Game_Manager.BLL;
using Board_Game_Manager.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Collections.Generic;

namespace Board_Game_Manager.Controllers
{
    [Authorize]
    public class LeaderboardController : Controller
    {
        private readonly PlayerService _playerService;
        private readonly GameMasterService _gmService;

        public LeaderboardController(PlayerService playerService, GameMasterService gmService)
        {
            _playerService = playerService;
            _gmService = gmService;
        }

        public IActionResult Index(int? tournamentId)
        {
            List<PlayerLeaderboardItem> leaderboard;

            var gmContribution = _gmService.GetContributionLeaderboard();
            var totalHours = _playerService.GetTotalHoursLeaderboard();
            var totalScores = _playerService.GetTotalScoreLeaderboard();

            if (tournamentId.HasValue)
            {
                leaderboard = _playerService.GetTournamentLeaderboard(tournamentId.Value);
            }
            else
            {
                leaderboard = totalScores.Select(p => new PlayerLeaderboardItem
                {
                    PlayerID = p.PlayerID,
                    Name = p.Name,
                    TotalScore = p.TotalScore,
                    TotalHours = totalHours.FirstOrDefault(h => h.PlayerID == p.PlayerID)?.TotalHours ?? 0,
                    GMContribution = gmContribution.FirstOrDefault(c => c.PlayerID == p.PlayerID)?.GMContribution ?? 0
                }).ToList();
            }

            return View(leaderboard);
        }
    }
}