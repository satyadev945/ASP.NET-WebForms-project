namespace Films.Web.ViewModels;

/// <summary>
/// View model for error pages
/// </summary>
public class ErrorViewModel
{
    public string? RequestId { get; set; }

    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
}