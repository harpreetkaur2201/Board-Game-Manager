using Board_Game_Manager.BLL;
using Board_Game_Manager.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Numerics;
namespace Board_Game_Manager.Controllers
{
    [Authorize]
    public class GameSessionController : Controller
    {
        private readonly GameSessionService _sessionService;
        private readonly GameService _gameService;
        private readonly GameMasterService _gmService;
        private readonly TournamentService _tournamentService;
        private readonly PlayerService _playerService;

        public GameSessionController(GameSessionService sessionservice, GameService gameservice, GameMasterService gmservice, TournamentService tournamentservice, PlayerService playerservice)
        {
            _sessionService = sessionservice; _gameService = gameservice; _gmService = gmservice; _tournamentService = tournamentservice; _playerService = playerservice;
        }
        public IActionResult Index()
        {
            List<GameSession> sessions = _sessionService.GetAllGameSessions();

            return View(sessions);

        }
        [HttpGet]
        [Authorize(Roles = "Admin,GM")]
        public IActionResult Create()
        {
            LoadDropdowns(); return View();
        }
        [HttpPost]
        [Authorize(Roles = "Admin,GM")]
        public IActionResult Create(GameSession gameSession)
        {
            if (ModelState.IsValid)
            {
                _sessionService.CreateGameSession(gameSession); return RedirectToAction("Index");
            }
            LoadDropdowns(gameSession.GameID, gameSession.GMID, gameSession.TournamentID);
            return View(gameSession);
        }
        [HttpGet]
        [Authorize(Roles = "Admin,GM")]
        public IActionResult Edit(int id)
        {
            GameSession? gs = _sessionService.GetGameSessionById(id);

            if (gs == null)
            {
                return NotFound();

            }
            LoadDropdowns(gs.GameID, gs.GMID, gs.TournamentID); return View(gs);

        }
        [HttpPost]
        [Authorize(Roles = "Admin,GM")]
        public IActionResult Edit(GameSession update)
        {

            if (!ModelState.IsValid)
            {

                LoadDropdowns(update.GameID, update.GMID, update.TournamentID);
                return View(update);

            }
            GameSession? gs = _sessionService.GetGameSessionById(update.SessionID);

            if (gs == null)
            {

                return NotFound();

            }
            if (gs.Game != null)
            {
                if (gs.SessionParticipants.Count() > gs.Game.MaxPlayerCount)
                {
                    return View(update);

                }
            }
            gs.GMID = update.GMID;

            gs.GameID = update.GameID;

            gs.TournamentID = update.TournamentID;

            gs.GameStartedAt = update.GameStartedAt;

            gs.PlayTimeMinutes = update.PlayTimeMinutes;

            foreach (SessionParticipant p in gs.SessionParticipants)
            {

                if (p.Player != null)
                {

                    p.Player.TotalHours += gs.PlayTimeMinutes / 60;

                    _playerService.EditPlayer(p.Player);
                }
            }

            _sessionService.EditGameSession(gs);

            return RedirectToAction("Index");

        }

        [Authorize(Roles = "Admin")]

        public IActionResult Delete(int id)
        {

            GameSession? gs = _sessionService.GetGameSessionById(id);

            if (gs != null)
            {

                _sessionService.DeletegameSession(id);
            }
            return RedirectToAction("Index");

        }

        public IActionResult Details(int id)
        {

            GameSession? gs = _sessionService.GetGameSessionById(id);

            if (gs == null)
            {

                return NotFound();

            }
            return View(gs);

        }
        //participants
        [HttpGet]
        [Authorize(Roles = "Admin,GM")]
        public IActionResult CreateParticipant(int id)
        {
            GameSession? gs = _sessionService.GetGameSessionById(id);
            if (gs == null)
            {
                return NotFound();
            }
            ViewBag.Session = gs;
            ViewBag.Players = new SelectList(
                _playerService.GetAllPlayers(),
                "PlayerID",
                "Name"); SessionParticipant participant = new SessionParticipant
                {
                    SessionID = id,
                    JoinedAt = DateTime.Now
                }; return View(participant);
        }
        [HttpPost]
        [Authorize(Roles = "Admin,GM")]
        public IActionResult CreateParticipant(SessionParticipant participant)
        {
            GameSession? gs = _sessionService.GetGameSessionById(participant.SessionID);

            if (gs == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                bool success = _sessionService.CreateParticipant(participant);

                if (success)
                {
                    return RedirectToAction("Details", new { id = participant.SessionID });
                }
                ModelState.AddModelError("", "Unable to add participant. The player may already be in this session, may not meet the age requirement, or the session may already be full.");
            }
            ViewBag.Session = gs;
            ViewBag.Players = new SelectList(
                _playerService.GetAllPlayers(),
                "PlayerID",
                "Name",
                participant.PlayerID
            );
            return View(participant);
        }
        [HttpGet]
        [Authorize(Roles = "Admin,GM")]
        public IActionResult EditParticipant(int sessionId, int playerId)
        {
            GameSession? gs = _sessionService.GetGameSessionById(sessionId);

            if (gs == null)
            {
                return NotFound();
            }
            SessionParticipant? participant = gs.SessionParticipants
                .FirstOrDefault(sp => sp.PlayerID == playerId);

            if (participant == null)
            {
                return NotFound();
            }

            ViewBag.Session = gs;
            return View(participant);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,GM")]
        public IActionResult EditParticipant(SessionParticipant participant)
        {
            GameSession? gs = _sessionService.GetGameSessionById(participant.SessionID);

            if (gs == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Session = gs;
                return View(participant);
            }

            bool success = _sessionService.EditParticipant(participant);

            if (!success)
            {
                ModelState.AddModelError("", "Unable to update participant.");
                ViewBag.Session = gs;
                return View(participant);
            }

            return RedirectToAction("Details", new { id = participant.SessionID });
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteParticipant(int sessionId, int playerId)
        {
            bool success = _sessionService.DeleteParticipant(sessionId, playerId);

            if (!success)
            {
                TempData["Error"] = "Unable to delete participant";
            }

            return RedirectToAction("Details", new { id = sessionId });
        }

        private void LoadDropdowns(int? selectedGameId = null, int? selectedGmId = null, int? selectedTournamentId = null)
        {
            ViewBag.Games = new SelectList(
                _gameService.GetAllGames(),
                "GameID",
                "Name",
                selectedGameId
            );

            ViewBag.GameMasters = _gmService.GetAllGameMasters()
                .Where(gm => gm.Player != null)
                .Select(gm => new SelectListItem
                {
                    Value = gm.GMID.ToString(),
                    Text = gm.Player.Name
                })
                .ToList();

            ViewBag.Tournaments = new SelectList(
                _tournamentService.GetAllTournaments(),
                "TournamentID",
                "Name",
                selectedTournamentId
            );
        }
    }

}
