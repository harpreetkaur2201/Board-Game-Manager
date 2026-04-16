using System.Collections.Generic;
using System.Linq;
using Board_Game_Manager.Models;
using Microsoft.EntityFrameworkCore;

namespace Board_Game_Manager.DAL {
    public class GameMasterRepository {
        private readonly BoardGameContext _context;

        public GameMasterRepository(BoardGameContext context) {
            _context = context;
        }
        public List<GameMaster> GetAllGameMasters() {
            return _context.GameMasters
                .Include(gm => gm.Player)
                .ToList();
        }
        public GameMaster? GetGameMasterById(int id) {
            return _context.GameMasters
                .Include(gm => gm.Player)
                .Include(gm => gm.GameSessions)
                .FirstOrDefault(gm => gm.GMID == id);
        }
        public GameMaster? GetGameMasterByPlayerId(int playerId) {
            return _context.GameMasters
                .Include(gm => gm.Player)
                .FirstOrDefault(gm => gm.PlayerID == playerId);
        }
        public void AddGameMaster(GameMaster gamemaster) {
            _context.GameMasters.Add(gamemaster);
            _context.SaveChanges();
        }
        public void EditGameMaster(GameMaster gamemaster) {
            _context.GameMasters.Update(gamemaster);
            _context.SaveChanges();
        }
        public void DeleteGameMasterByPlayerId(int id) {
            GameMaster? gamemaster = _context.GameMasters
                .FirstOrDefault(gm => gm.PlayerID == id);

            if (gamemaster != null) {
                _context.GameMasters.Remove(gamemaster);
                _context.SaveChanges();
            }
        }
    }
}