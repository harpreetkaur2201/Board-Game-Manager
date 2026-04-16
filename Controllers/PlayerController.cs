using Board_Game_Manager.BLL;
using Board_Game_Manager.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Board_Game_Manager.Controllers {
    [Authorize]
    public class PlayerController : Controller {
        private readonly PlayerService _service;
        private readonly GameMasterService _gmservice;

        public PlayerController(PlayerService service, GameMasterService gmservice) {
            _service = service;
            _gmservice = gmservice;

        }

        public IActionResult Index() {
            var p = _service.GetAllPlayers();
            return View(p);

        }

        [HttpGet]
        [Authorize(Roles = "Admin,GM")]
        public IActionResult Create() {
            return View();

        }

        [HttpPost]
        [Authorize(Roles = "Admin,GM")]
        public IActionResult Create(Player p) {
            if (ModelState.IsValid) {
                p.RegisteredAt = DateTime.Now;
                _service.CreatePlayer(p);

                return RedirectToAction("Index");

            }
            return View(p);

        }

        
        [HttpGet]
        [Authorize(Roles = "Admin,GM")]
        public IActionResult Edit(int id) {
            Player? p = _service.GetPlayerById(id);

            if (p == null) {
                return NotFound();

            }
            GameMaster? existingGM = _gmservice.GetGameMasterByPlayerId(id);

            PlayerEditViewModel vm = new PlayerEditViewModel {
                PlayerID = p.PlayerID,
                Name = p.Name,
                Email = p.Email,
                Age = p.Age,
                RegisteredAt = p.RegisteredAt,

                IsGameMaster = existingGM != null
            };

            return View(vm);

        }

        [HttpPost]
        [Authorize(Roles = "Admin,GM")]
        public IActionResult Edit(PlayerEditViewModel update) {
            if (!ModelState.IsValid) {
                return View(update);

            }
            Player? p = _service.GetPlayerById(update.PlayerID);

            if (p == null) {
                return NotFound();

            }
            p.Name = update.Name;

            p.Email = update.Email;

            p.Age = update.Age;

            p.RegisteredAt = update.RegisteredAt;

            _service.EditPlayer(p);

            _gmservice.SaveOrUpdateGameMaster(update.PlayerID, update.IsGameMaster);

            return RedirectToAction("Index");

        }


        [Authorize(Roles = "Admin")]

        public IActionResult Delete(int id) {

            Player? p = _service.GetPlayerById(id);

            if (p != null) {

                _service.DeletePlayer(id);

            }

            return RedirectToAction("Index");

        }


        public IActionResult Details(int id) {

            Player? p = _service.GetPlayerById(id);

            if (p == null) {

                return NotFound();

            }

            GameMaster? gm = _gmservice.GetGameMasterByPlayerId(id);

            ViewBag.IsGameMaster = gm != null;
            ViewBag.GMContribution = gm?.GameSessions?.Count ?? 0;

            return View(p);

        }

    }

}
