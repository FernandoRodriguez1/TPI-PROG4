using MatchTickets.Application.Interfaces;
using SendGrid;
using SendGrid.Helpers.Mail;
using Microsoft.Extensions.Options;


namespace MatchTickets.Infraestructure.External_Services
{

    public class SendGridService : IMailService
    {
        private readonly HttpClient _httpClient;
        private readonly SendGridOptions _options;

        public SendGridService(HttpClient httpClient, IOptions<SendGridOptions> options)
        {
            _httpClient = httpClient;
            _options = options.Value;
        }

        public async Task SendMembershipCreatedEmailAsync(string toEmail, string memberName, string clubName, string membershipNumber)
        {
            
            var client = new SendGridClient(
                apiKey: _options.ApiKey,
                httpClient: _httpClient
            );

            var from = new EmailAddress(_options.FromEmail, _options.FromName);
            var to = new EmailAddress(toEmail, memberName);

            string subject = $"¡Bienvenido al club {clubName}!";

            string plainTextContent = $"Hola {memberName},\n\n" +
                $"Tu membresía ha sido creada exitosamente.\n" +
                $"Número de carnet: {membershipNumber}\n" +
                $"Club: {clubName}\n\n" +
                $"¡Gracias por ser parte!";

            string htmlContent = $@"
        <h2>¡Bienvenido al club {clubName}!</h2>
        <p>Hola <strong>{memberName}</strong>,</p>
        <p>Tu membresía ha sido creada exitosamente 🎉</p>
        <ul>
            <li><strong>Número de carnet:</strong> {membershipNumber}</li>
            <li><strong>Club:</strong> {clubName}</li>
        </ul>
        <p>¡Gracias por ser parte!</p>";

            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);

            var response = await client.SendEmailAsync(msg);

           
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Error al enviar mail a {toEmail} - Código: {response.StatusCode}");
            }
        }
    }



}
