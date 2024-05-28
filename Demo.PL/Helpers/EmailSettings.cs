using System.Net.Mail;
using System.Net;

namespace Demo.PL.Helpers
{
    public static class EmailSettings
    {
        public static void SendEmail(Email email)
        {
            var clinte = new SmtpClient("smtp.gmail.com", 587); // host , port
            clinte.EnableSsl = true;
            clinte.Credentials = new NetworkCredential("mai.hassan4528@gmail.com", "dqsd rjlk tlpi rjlx");
            clinte.Send("mai.hassan4528@gmail.com", email.To, email.Subject, email.Body);
        }
    }
}
