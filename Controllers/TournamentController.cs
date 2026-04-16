using Board_Game_Manager.DAL;
using Board_Game_Manager.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

public class TournamentController : Controller
{
    private readonly GameRepository _gameRepo;
    private readonly TournamentRepository _tournamentRepo;

    public TournamentController(BoardGameContext context)
    {
        _gameRepo = new GameRepository(context);
        _tournamentRepo = new TournamentRepository(context);
    }

    // Index
    public IActionResult Index()
    {
        var tournaments = _tournamentRepo.GetAllTournaments();
        return View(tournaments);
    }

    // Create GET
    public IActionResult Create()
    {
        // Load games for dropdown
        ViewBag.Games = new SelectList(_gameRepo.GetAllGames(), "GameID", "Name");
        return View();
    }

    // Create POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Tournament tournament)
    {
        if (ModelState.IsValid)
        {
            _tournamentRepo.CreateTournament(tournament);
            return RedirectToAction(nameof(Index));
        }

        // reload games if form invalid
        ViewBag.Games = new SelectList(_gameRepo.GetAllGames(), "GameID", "Name", tournament.GameID);
        return View(tournament);
    }

    // Edit GET
    public IActionResult Edit(int id)
    {
        var tournament = _tournamentRepo.GetTournamentById(id);
        if (tournament == null) return NotFound();

        ViewBag.Games = new SelectList(_gameRepo.GetAllGames(), "GameID", "Name", tournament.GameID);
        return View(tournament);
    }

    // Edit POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(Tournament tournament)
    {
        if (ModelState.IsValid)
        {
            _tournamentRepo.EditTournament(tournament);
            return RedirectToAction(nameof(Index));
        }
        ViewBag.Games = new SelectList(_gameRepo.GetAllGames(), "GameID", "Name", tournament.GameID);
        return View(tournament);
    }

    // Details
    public IActionResult Details(int id)
    {
        var tournament = _tournamentRepo.GetTournamentById(id);
        if (tournament == null) return NotFound();
        return View(tournament);
    }

    // Delete GET
    public IActionResult Delete(int id)
    {
        var tournament = _tournamentRepo.GetTournamentById(id);
        if (tournament == null) return NotFound();
        return View(tournament);
    }

    // Delete POST
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        _tournamentRepo.DeleteTournament(id);
        return RedirectToAction(nameof(Index));
    }
}