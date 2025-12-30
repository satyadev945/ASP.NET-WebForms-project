using Films.Domain.DTOs;
using Films.Domain.Interfaces.Services;
using Films.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Films.Web.Controllers;

public class FilmsController : Controller
{
    private readonly IFilmService _filmService;
    private readonly ILogger<FilmsController> _logger;

    public FilmsController(IFilmService filmService, ILogger<FilmsController> logger)
    {
        _filmService = filmService ?? throw new ArgumentNullException(nameof(filmService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<IActionResult> Index(string? searchTerm, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Loading films index page with search term: {SearchTerm}", searchTerm ?? "none");

            var films = string.IsNullOrWhiteSpace(searchTerm)
                ? await _filmService.GetAllAsync(cancellationToken)
                : await _filmService.SearchAsync(searchTerm, cancellationToken);

            var viewModel = new FilmsIndexViewModel
            {
                Films = films,
                SearchTerm = searchTerm
            };

            return View(viewModel);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading films index page");
            TempData["ErrorMessage"] = "An error occurred while loading films. Please try again.";
            return View(new FilmsIndexViewModel { Films = new List<FilmDto>(), SearchTerm = searchTerm });
        }
    }

    public IActionResult Create()
    {
        _logger.LogInformation("Loading create film page");
        return View(new FilmCreateViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(FilmCreateViewModel model, CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Model state is invalid for film creation");
            return View(model);
        }

        try
        {
            _logger.LogInformation("Creating new film: {Name}", model.Name);

            // Manual mapping from ViewModel to DTO
            var filmCreateDto = new FilmCreateDto
            {
                Name = model.Name,
                Description = model.Description,
                Year = model.Year,
                Genre = model.Genre,
                ImageUrl = model.ImageUrl,
                CreatedBy = model.CreatedBy
            };

            var createdFilm = await _filmService.CreateAsync(filmCreateDto, cancellationToken);

            _logger.LogInformation("Film created successfully with id: {Id}", createdFilm.Id);
            TempData["SuccessMessage"] = $"Film '{createdFilm.Name}' was created successfully.";

            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating film: {Name}", model.Name);
            ModelState.AddModelError(string.Empty, "An error occurred while creating the film. Please try again.");
            return View(model);
        }
    }

    public async Task<IActionResult> Details(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Loading film details for id: {Id}", id);

            var film = await _filmService.GetByIdAsync(id, cancellationToken);
            if (film == null)
            {
                _logger.LogWarning("Film with id {Id} not found", id);
                return NotFound();
            }

            return View(film);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading film details for id: {Id}", id);
            TempData["ErrorMessage"] = "An error occurred while loading film details. Please try again.";
            return RedirectToAction(nameof(Index));
        }
    }

    public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Loading edit film page for id: {Id}", id);

            var film = await _filmService.GetByIdAsync(id, cancellationToken);
            if (film == null)
            {
                _logger.LogWarning("Film with id {Id} not found", id);
                return NotFound();
            }

            // Manual mapping from DTO to ViewModel
            var viewModel = new FilmUpdateViewModel
            {
                Name = film.Name,
                Description = film.Description,
                Year = film.Year,
                Genre = film.Genre,
                ImageUrl = film.ImageUrl,
                ModifiedBy = "System" // Default value
            };

            return View(viewModel);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading edit film page for id: {Id}", id);
            TempData["ErrorMessage"] = "An error occurred while loading the film for editing. Please try again.";
            return RedirectToAction(nameof(Index));
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, FilmUpdateViewModel model, CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Model state is invalid for film update with id: {Id}", id);
            return View(model);
        }

        try
        {
            _logger.LogInformation("Updating film with id: {Id}", id);

            // Manual mapping from ViewModel to DTO
            var filmUpdateDto = new FilmUpdateDto
            {
                Name = model.Name,
                Description = model.Description,
                Year = model.Year,
                Genre = model.Genre,
                ImageUrl = model.ImageUrl,
                ModifiedBy = model.ModifiedBy
            };

            var updatedFilm = await _filmService.UpdateAsync(id, filmUpdateDto, cancellationToken);

            _logger.LogInformation("Film updated successfully with id: {Id}", id);
            TempData["SuccessMessage"] = $"Film '{updatedFilm.Name}' was updated successfully.";

            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating film with id: {Id}", id);
            ModelState.AddModelError(string.Empty, "An error occurred while updating the film. Please try again.");
            return View(model);
        }
    }

    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Loading delete film page for id: {Id}", id);

            var film = await _filmService.GetByIdAsync(id, cancellationToken);
            if (film == null)
            {
                _logger.LogWarning("Film with id {Id} not found", id);
                return NotFound();
            }

            return View(film);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading delete film page for id: {Id}", id);
            TempData["ErrorMessage"] = "An error occurred while loading the film for deletion. Please try again.";
            return RedirectToAction(nameof(Index));
        }
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Deleting film with id: {Id}", id);

            var film = await _filmService.GetByIdAsync(id, cancellationToken);
            if (film == null)
            {
                _logger.LogWarning("Film with id {Id} not found for deletion", id);
                return NotFound();
            }

            await _filmService.DeleteAsync(id, cancellationToken);

            _logger.LogInformation("Film deleted successfully with id: {Id}", id);
            TempData["SuccessMessage"] = $"Film '{film.Name}' was deleted successfully.";

            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting film with id: {Id}", id);
            TempData["ErrorMessage"] = "An error occurred while deleting the film. Please try again.";
            return RedirectToAction(nameof(Index));
        }
    }
}