using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Board_Game_Manager.DAL;
using Board_Game_Manager.Models;
namespace Board_Game_Manager.BLL
{
    public class GameSessionService
    {
        private readonly GameSessionRepository _repository;
        public GameSessionService(GameSessionRepository repository)
        {
            _repository = repository;
        }        
                 public List<GameSession> GetAllGameSessions() {
            return _repository.GetAllGameSessions();
        }        public GameSession? GetGameSessionById(int id)
        {
            return _repository.GetGameSessionById(id);
        }
        public List<GameSession> GetGameSessionByTournament(int id)
        {
            return _repository.GetSessionsByTournament(id);
        }
        public List<GameSession> GetGameSessionByGame(int id)
        {
            return _repository.GetSessionsByGame(id);
        }
        public void CreateGameSession(GameSession gamesession)
        {
            _repository.CreateSession(gamesession);
        }
        public void EditGameSession(GameSession gamesession)
        {
            _repository.EditSession(gamesession);
        }
        public void DeletegameSession(int id)
        {
            _repository.DeleteSession(id);
        }        
                 
        public bool CreateParticipant(SessionParticipant participant) {
            return _repository.CreateParticipant(participant);
        }
public SessionParticipant? GetParticipant(int sessionId, int playerId)
{
    return _repository.GetParticipant(sessionId, playerId);
}
public bool EditParticipant(SessionParticipant participant)
{
    return _repository.EditParticipant(participant);
}
public bool DeleteParticipant(int sessionId, int playerId)
{
    return _repository.DeleteParticipant(sessionId, playerId);
}

public List<Game> GetAllGames() {
return _repository.GetAllGames();
        }
        public List<GameMaster> GetAllGameMasters()
{
    return _repository.GetAllGameMasters();
}
public List<Tournament> GetAllTournaments()
{
    return _repository.GetAllTournaments();
}
public List<Player> GetAllPlayers()
{
    return _repository.GetAllPlayers();
}
    }
}


 
 