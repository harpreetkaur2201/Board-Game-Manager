using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Board_Game_Manager.DAL;
using Board_Game_Manager.Models;

namespace Board_Game_Manager.BLL {
    public class GameService {
        private readonly GameRepository _repository;
        public GameService(GameRepository repository) {
            _repository = repository;
        }
        public List<Game> GetAllGames() {
            return _repository.GetAllGames();
        }
        public Game? GetGameById(int id) {
            return _repository.GetGameById(id);
        }
        public void CreateGame(Game game) {
            _repository.CreateGame(game);
        }
        public void EditGame(Game game) {
            _repository.EditGame(game);
        }
        public void DeleteGame(int id) {
            _repository.DeleteGame(id);
        }

    }
}
