using System.ComponentModel.DataAnnotations;
using Films.Domain.DTOs;

namespace Films.Web.ViewModels;

/// <summary>
/// View model for creating films
/// </summary>
public class FilmCreateViewModel
{
    [Required(ErrorMessage = "Film name is required")]
    [StringLength(255, ErrorMessage = "Film name cannot exceed 255 characters")]
    [Display(Name = "Film Name")]
    public string Name { get; set; } = string.Empty;

    [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters")]
    public string? Description { get; set; }

    [Required(ErrorMessage = "Year is required")]
    [Range(1800, 2100, ErrorMessage = "Year must be between 1800 and 2100")]
    public int Year { get; set; } = DateTime.Now.Year;

    [StringLength(100, ErrorMessage = "Genre cannot exceed 100 characters")]
    public string? Genre { get; set; }

    [Url(ErrorMessage = "Please enter a valid URL")]
    [StringLength(500, ErrorMessage = "Image URL cannot exceed 500 characters")]
    [Display(Name = "Image URL")]
    public string? ImageUrl { get; set; }

    [Required(ErrorMessage = "Created by is required")]
    [StringLength(255, ErrorMessage = "Created by cannot exceed 255 characters")]
    [Display(Name = "Created By")]
    public string CreatedBy { get; set; } = "System";
}

/// <summary>
/// View model for updating films
/// </summary>
public class FilmUpdateViewModel
{
    [Required(ErrorMessage = "Film name is required")]
    [StringLength(255, ErrorMessage = "Film name cannot exceed 255 characters")]
    [Display(Name = "Film Name")]
    public string Name { get; set; } = string.Empty;

    [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters")]
    public string? Description { get; set; }

    [Required(ErrorMessage = "Year is required")]
    [Range(1800, 2100, ErrorMessage = "Year must be between 1800 and 2100")]
    public int Year { get; set; }

    [StringLength(100, ErrorMessage = "Genre cannot exceed 100 characters")]
    public string? Genre { get; set; }

    [Url(ErrorMessage = "Please enter a valid URL")]
    [StringLength(500, ErrorMessage = "Image URL cannot exceed 500 characters")]
    [Display(Name = "Image URL")]
    public string? ImageUrl { get; set; }

    [Required(ErrorMessage = "Modified by is required")]
    [StringLength(255, ErrorMessage = "Modified by cannot exceed 255 characters")]
    [Display(Name = "Modified By")]
    public string ModifiedBy { get; set; } = "System";
}

/// <summary>
/// View model for films index page
/// </summary>
public class FilmsIndexViewModel
{
    public IEnumerable<FilmDto> Films { get; set; } = new List<FilmDto>();
    public string? SearchTerm { get; set; }
}