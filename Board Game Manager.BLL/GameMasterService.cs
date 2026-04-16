using System.Collections.Generic;
using Board_Game_Manager.DAL;
using Board_Game_Manager.Models;

namespace Board_Game_Manager.BLL {
    public class GameMasterService {
        private readonly GameMasterRepository _repository;
        public GameMasterService(GameMasterRepository repository) {
            _repository = repository;
        }
        public List<GameMaster> GetAllGameMasters() {
            return _repository.GetAllGameMasters();
        }
        public GameMaster? GetGameMasterById(int id) {
            return _repository.GetGameMasterById(id);
        }
        public GameMaster? GetGameMasterByPlayerId(int id) {
            return _repository.GetGameMasterByPlayerId(id);
        }
        public bool AddGameMaster(GameMaster gamemaster) {
            if (_repository.GetGameMasterByPlayerId(gamemaster.PlayerID) != null) {
                return false;
            }

            _repository.AddGameMaster(gamemaster);
            return true;
        }
        public bool UpdateGameMaster(GameMaster gamemaster) {
            GameMaster? existingGamemaster = _repository.GetGameMasterById(gamemaster.GMID);

            if (existingGamemaster == null) {
                return false;
            }
            GameMaster? duplicateGamemaster = _repository.GetGameMasterByPlayerId(gamemaster.PlayerID);

            if (duplicateGamemaster != null && duplicateGamemaster.GMID != gamemaster.GMID) {
                return false;
            }

            _repository.EditGameMaster(gamemaster);
            return true;
        }

        public bool RemoveGameMasterByPlayerId(int playerId) {
            GameMaster? existingGamemaster = _repository.GetGameMasterByPlayerId(playerId);

            if (existingGamemaster == null) {
                return false;
            }

            _repository.DeleteGameMasterByPlayerId(playerId);
            return true;
        }

        public void SaveOrUpdateGameMaster(int playerId, bool isGameMaster) {
            GameMaster? existingGamemaster = _repository.GetGameMasterByPlayerId(playerId);

            if (isGameMaster) {
                if (existingGamemaster == null) {
                    GameMaster newGameMaster = new GameMaster {
                        PlayerID = playerId,
                    };

                    _repository.AddGameMaster(newGameMaster);
                }
            } else {
                if (existingGamemaster != null) {
                    _repository.DeleteGameMasterByPlayerId(playerId);
                }
            }
        }

        public List<PlayerLeaderboardItem> GetContributionLeaderboard()
        {
            var gms = _repository.GetAllGameMasters(); // make sure this method exists in repository

            return gms.Select(gm => new PlayerLeaderboardItem
            {
                PlayerID = gm.PlayerID,
                Name = gm.Player?.Name ?? "Unknown",
                GMContribution = gm.GameSessions.Count
            })
            .OrderByDescending(x => x.GMContribution)
            .ToList();
        }
    }
}