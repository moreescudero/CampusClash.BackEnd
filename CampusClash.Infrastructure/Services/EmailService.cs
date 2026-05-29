using CampusClash.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Resend;

namespace CampusClash.Infrastructure.Services;

public class EmailService : IEmailService
{
    private readonly IResend _resend;
    private readonly string _from;
    private readonly string _adminEmail;

    public EmailService(IResend resend, IConfiguration configuration)
    {
        _resend = resend;
        _from = configuration["Resend:From"] ?? "CampusClash <onboarding@resend.dev>";
        _adminEmail = configuration["Resend:AdminEmail"] ?? "validacionescampusclash@gmail.com";
    }

    public async Task SendValidationRequestNotificationAsync(
        string userEmail, string username,
        string legajo, string faculty, string career, int year,
        string filePath)
    {
        var message = new EmailMessage
        {
            From = _from,
            Subject = $"Nueva solicitud de validación — {username}",
            HtmlBody = $"""
                <h2>Nueva solicitud de validación universitaria</h2>
                <table>
                  <tr><td><b>Usuario</b></td><td>{username} ({userEmail})</td></tr>
                  <tr><td><b>Legajo</b></td><td>{legajo}</td></tr>
                  <tr><td><b>Facultad</b></td><td>{faculty}</td></tr>
                  <tr><td><b>Carrera</b></td><td>{career}</td></tr>
                  <tr><td><b>Año</b></td><td>{year}</td></tr>
                </table>
                """
        };
        message.To.Add(_adminEmail);

        if (File.Exists(filePath))
        {
            var bytes = await File.ReadAllBytesAsync(filePath);
            message.Attachments ??= [];
            message.Attachments.Add(new EmailAttachment
            {
                Filename = Path.GetFileName(filePath),
                Content = bytes
            });
        }

        await _resend.EmailSendAsync(message);
    }

    public async Task SendValidationApprovedAsync(string userEmail, string username)
    {
        var message = new EmailMessage
        {
            From = _from,
            Subject = "¡Tu validación universitaria fue aprobada! — CampusClash",
            HtmlBody = $"""
                <h2>¡Felicitaciones, {username}!</h2>
                <p>Tu solicitud de validación universitaria fue <strong>aprobada</strong>.</p>
                <p>Ya podés participar en los torneos universitarios de CampusClash.</p>
                """
        };
        message.To.Add(userEmail);

        await _resend.EmailSendAsync(message);
    }
}
