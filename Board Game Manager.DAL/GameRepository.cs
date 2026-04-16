using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Board_Game_Manager.Models;
using Microsoft.EntityFrameworkCore;

namespace Board_Game_Manager.DAL {
    public class GameRepository {
        private readonly BoardGameContext _context;
        public GameRepository(BoardGameContext context) {
            _context = context;
        }
        public List<Game> GetAllGames() {
            return _context.Games
                .OrderBy(g => g.Name)
                .ToList();
        }
        public Game? GetGameById(int id) {
            return _context.Games
                .Include(g => g.GameSessions)
                .Include(g => g.Tournaments)
                .FirstOrDefault(g => g.GameID == id);
        }
        public void CreateGame(Game game) {
            _context.Games.Add(game);
            _context.SaveChanges();
        }
        public void EditGame(Game game) {
            _context.Games.Update(game);
            _context.SaveChanges();
        }
        public void DeleteGame(int id) {
            Game? game = _context.Games.FirstOrDefault(g => g.GameID == id);
            if ((game != null)) {
                _context.Games.Remove(game);
                _context.SaveChanges();
            }
        }
    }
}
