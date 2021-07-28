namespace AnnualLeaveSystem.Services.Emails
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public interface IEmailSenderService
    {
        void SendMail(string from, string to, string subject, string html);
    }
}
