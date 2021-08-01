namespace AnnualLeaveSystem.Services.Emails
{
    using MailKit.Net.Smtp;
    using MailKit.Security;
    using MimeKit;
    using MimeKit.Text;
    using System.Text;

    public class EmailSenderService : IEmailSenderService
    {
        public void SendEmail(string subject, string content)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("myaspcoreprojectserver@gmail.com"));
            email.To.Add(MailboxAddress.Parse("myprojectdefence@gmail.com"));
            email.Subject = subject;
        
            email.Body = new TextPart(TextFormat.Html) { Text = content };

            var emailProject = "myaspcoreprojectserver@gmail.com";
            var password = "123123Asp";


            using var smtp = new SmtpClient();
            smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            smtp.Authenticate(emailProject, password);
            smtp.Send(email);
            smtp.Disconnect(true);
        }
    }
}
