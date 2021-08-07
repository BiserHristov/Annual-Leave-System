namespace AnnualLeaveSystem.Services.Emails
{
    public interface IEmailSenderService
    {
        void SendEmail(string subject, string content);
    }
}
