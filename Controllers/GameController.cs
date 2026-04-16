using Board_Game_Manager.BLL;
using Board_Game_Manager.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Board_Game_Manager.Controllers
{
    [Authorize]
    public class GameController : Controller
    {
        private readonly GameService _service;

        public GameController(GameService service)
        {
            _service = service;
        }

        // ALL USERS (logged in)
        public IActionResult Index()
        {
            var games = _service.GetAllGames();
            return View(games);
        }

        public IActionResult Details(int id)
        {
            var game = _service.GetGameById(id);

            if (game == null)
                return NotFound();

            return View(game);
        }

        // ADMIN + GM ONLY
        [HttpGet]
        [Authorize(Roles = "Admin,GM")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin,GM")]
        public IActionResult Create(Game game)
        {
            if (!ModelState.IsValid)
                return View(game);

            _service.CreateGame(game);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [Authorize(Roles = "Admin,GM")]
        public IActionResult Edit(int id)
        {
            var game = _service.GetGameById(id);

            if (game == null)
                return NotFound();

            return View(game);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,GM")]
        public IActionResult Edit(Game update)
        {
            if (!ModelState.IsValid)
                return View(update);

            var game = _service.GetGameById(update.GameID);

            if (game == null)
                return NotFound();

            game.Name = update.Name;
            game.URL = update.URL;
            game.Description = update.Description;
            game.AvgDuration = update.AvgDuration;
            game.AgeReq = update.AgeReq;
            game.MinPlayerCount = update.MinPlayerCount;
            game.MaxPlayerCount = update.MaxPlayerCount;

            _service.EditGame(game);

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            var game = _service.GetGameById(id);

            if (game != null)
            {
                _service.DeleteGame(id);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}