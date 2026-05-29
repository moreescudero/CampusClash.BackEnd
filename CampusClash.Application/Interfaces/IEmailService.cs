namespace CampusClash.Application.Interfaces;

public interface IEmailService
{
    Task SendValidationRequestNotificationAsync(
        string userEmail, string username,
        string legajo, string faculty, string career, int year,
        string filePath);

    Task SendValidationApprovedAsync(string userEmail, string username);
}
