using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Board_Game_Manager.DAL;
using Board_Game_Manager.Models;

namespace Board_Game_Manager.BLL {
    public class TournamentService {
        private readonly TournamentRepository _repository;
        public TournamentService(TournamentRepository repository) {
            _repository = repository;
        }
        public List<Tournament> GetAllTournaments() {
            return _repository.GetAllTournaments();
        }
        public Tournament? GetTournamentById(int id) {
            return _repository.GetTournamentById(id);
        }
        public void CreateTournament(Tournament tournament) {
            _repository.CreateTournament(tournament);
        }
        public void EditTournament(Tournament tournament) {
            _repository.EditTournament(tournament);
        }
        public void DeleteTournament(int id) {
            _repository.DeleteTournament(id);
        }
        public List<PlayerLeaderboardItem> GetTournamentLeaderboard(int tournamentId)
        {
            var tournament = _repository.GetTournamentById(tournamentId);

            if (tournament == null || tournament.GameSessions == null)
                return new List<PlayerLeaderboardItem>();

            return tournament.GameSessions
                .SelectMany(s => s.SessionParticipants)
                .GroupBy(p => p.PlayerID)
                .Select(g => new PlayerLeaderboardItem
                {
                    Name = g.First().Player?.Name ?? "Unknown",
                })
                .ToList();
        }
    }
}
