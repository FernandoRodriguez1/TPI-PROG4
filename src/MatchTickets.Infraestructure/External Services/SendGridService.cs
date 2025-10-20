using MatchTickets.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using SendGrid;
using SendGrid.Helpers.Mail;
using Microsoft.Extensions.Options;


namespace MatchTickets.Infraestructure.External_Services
{
    public class SendGridService : IMailService
    {
        // se inyecta la clase HttpClient para conectar a la API de SendGrid y enviar emails.
        private readonly HttpClient _httpClient;
        private readonly SendGridOptions _options;

        public SendGridService(HttpClient httpClient, IOptions<SendGridOptions> options)
        {
            _httpClient = httpClient;
            _options = options.Value;
        }

        public async Task SendMembershipCreatedEmailAsync(string toEmail, string memberName, string clubName, string membershipNumber)
        {
            // Configurar cliente SendGrid con API Key
            var client = new SendGridClient(_options.ApiKey);
            //SendGridClient internamente usa HttpClient para hacer la petición POST con los datos del email.


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
            await client.SendEmailAsync(msg);
        }
    }



}
