using CheckWorkers.Entity;
using CheckWorkers.Services.Abstraction;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Text;

namespace CheckWorkers.Services.Implementation
{
    public class SendEmail : ISendEmail
    {
        private readonly IConfiguration configuration;
        private readonly string sender;
        private readonly string to;
        private readonly string smtp;
        private readonly string subject;
        private readonly int port;
        private string workerTemplate;

        public SendEmail(IConfiguration configuration)
        {
            this.configuration = configuration;
            sender = configuration["Email:Sender"];
            to = configuration["Email:To"];
            smtp = configuration["Email:Smtp"];
            subject = configuration["Email:Subject"];
            port = int.Parse(configuration["Email:Port"]);
            workerTemplate = "Worker name = {0}, company name = {1} was updated {2} at {3}.\n";
        }
        public void SendBasicEmail(IEnumerable<Worker> workers)
        {
            SendTo(to, workers);
        }

        public void SendTo(string email, IEnumerable<Worker> workers)
        {
            var format = MimeKit.Text.TextFormat.Text;
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("Workers updates", sender));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;

            emailMessage.Body = new TextPart(format)
            {
                Text = GetBodyText(workers)
            };

            Send(emailMessage);
        }

        private string GetBodyText(IEnumerable<Worker> workers)
        {
            var sb = new StringBuilder();
            var formatted = default(string);
            var date = default(DateTime);

            foreach (var worker in workers)
            {
                date = worker.TimeUpdated;
                formatted = string.Format(workerTemplate, worker.Name, worker.Company.CompanyName, date.ToShortDateString(), date.ToShortTimeString());
                sb.Append(formatted);
            }

            return sb.ToString();
        }

        private void Send(MimeMessage emailMessage)
        {
            using (var client = new SmtpClient())
            {
                client.Connect(smtp, port);
                client.Authenticate(sender, configuration["Email:Password"]);
                client.Send(emailMessage);
                client.Disconnect(true);
            }
        }
    }
}
