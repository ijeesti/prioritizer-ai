namespace Prioritizer.Contracts.Interfaces;

// NOTE: In a real enterprise app, this would wrap a service like SendGrid, Mailgun, 
// or an internal SMTP client, and would be injected into the final agent.
public interface IEmailService
{
    Task SendDecisionEmailAsync(string recipient, string subject, string bodyHtml);
}
