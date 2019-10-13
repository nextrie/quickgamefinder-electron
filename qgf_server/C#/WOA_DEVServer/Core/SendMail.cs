using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace WOA_DEVServer.Core
{
    public class SendMail
    {
        public enum MailType
        {
            MISSINGGAME,
            CONTACT
        }
        public static void Mail(string content, MailType m)
        {
            if(content != "")
            {
                SmtpClient client = new SmtpClient();
                client.Port = 587;
                client.Host = "smtp.gmail.com";
                client.EnableSsl = true;
                client.Timeout = 10000;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new System.Net.NetworkCredential("nextriesh@gmail.com", "Yujilaosyalere94");
                string subject = "";
                if(m == MailType.MISSINGGAME)
                {
                    subject = "Jeu manquant";
                }
                else
                {
                    subject = "Contact support";
                }
                MailMessage mm = new MailMessage("nextriesh@gmail.com", "contact@quickgamefinder.com", subject, content);
                // mm.BodyEncoding = UTF8Encoding.UTF8;
                //mm.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
                try
                {
                    client.Send(mm);
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            }
        }
    }
}
