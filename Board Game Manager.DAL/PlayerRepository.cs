using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Board_Game_Manager.Models;
using Microsoft.EntityFrameworkCore;

namespace Board_Game_Manager.DAL {
    public class PlayerRepository {
        private readonly BoardGameContext _context;
        public PlayerRepository(BoardGameContext context) {
            _context = context;
        }
        public List<Player> GetAllPlayers() {
            return _context.Players
                .Include(p => p.GameMaster)
                .OrderBy(p => p.Name)
                .ToList();
        }
        public Player? GetPlayerById(int id) {
            return _context.Players
                .Include(p => p.GameMaster)
                .Include(p => p.SessionParticipants)
                .ThenInclude(sp => sp.GameSession)
                .FirstOrDefault(p => p.PlayerID == id);
        }
        public void CreatePlayer(Player player) {
            _context.Players.Add(player);
            _context.SaveChanges();
        }
        public void EditPlayer(Player player) {
            _context.Players.Update(player);
            _context.SaveChanges();
        }
        public void DeletePlayer(int id) {
            Player? player = _context.Players.FirstOrDefault(p => p.PlayerID == id);
            if (player != null) {
                _context.Remove(player);
                _context.SaveChanges();
            }
        }
        public List<GameMaster> GetAllGameMasters()
        {
            return _context.GameMasters.Include(gm => gm.Player)
                                       .Include(gm => gm.GameSessions)
                                       .ToList();
        }
    }
}
