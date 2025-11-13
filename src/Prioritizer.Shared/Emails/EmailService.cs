using Prioritizer.Contracts.Interfaces;
using System.Net.Mail;
using System.Text;

namespace Prioritizer.Shared.Emails;

// <summary>
/// A utility class for sending email, 
/// </summary>
public class EmailService : IEmailService
{
    private readonly SmtpClient _smtpClient = new("your-host", 5000)
    {
        // You would configure credentials, SSL, etc., here
        EnableSsl = true
    };
  
    private readonly string _defaultSender = "automation@Prioritizer.com";
      
    /// <summary>
    /// Sends the final decision email.
    /// </summary>
    /// <param name="recipient">The email address of the requester (e.g., derived from the Conversation ID or initial payload).</param>
    /// <param name="subject">The subject line (drafted by the LLM).</param>
    /// <param name="bodyHtml">The HTML body of the email (drafted by the LLM).</param>
    public async Task SendDecisionEmailAsync(string recipient, string subject, string bodyHtml)
    {
        try
        {
            var mailMessage = new MailMessage(_defaultSender, recipient)
            {
                Subject = subject,
                Body = bodyHtml,
                IsBodyHtml = true,
                BodyEncoding = Encoding.UTF8
            };

            // In a real application, you would log the attempt and potentially use a try-catch block
            // to handle SMTP exceptions and move the message to a Dead Letter Queue (DLQ).
            await _smtpClient.SendMailAsync(mailMessage);

            Console.WriteLine($"Successfully sent decision email to: {recipient}");
        }
        catch (SmtpException ex)
        {
            // Log error and handle retry/DLQ logic
            Console.Error.WriteLine($"ERROR sending email to {recipient}: {ex.Message}");
            // Re-throw to indicate a critical failure in the final step
        }
    }
}