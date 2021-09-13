using KoodinenV1.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace KoodinenV1
{
    public class Email
    {
        private IConfiguration _configuration;
        private KoodinenDBContext _context;

        public Email(IConfiguration configuration, KoodinenDBContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        public bool LähetäEmail(string vastaanottajaEmail, string vastaanottajanNimi)
        {
            try
            {
                var koodinenEmail = _configuration.GetConnectionString("Email");
                var koodinenSalasana = _configuration.GetConnectionString("EmailSalasana");

                MailMessage mailMessage = new MailMessage(koodinenEmail, vastaanottajaEmail);
                mailMessage.Subject = "Tervetuloa Koodiseen!";

                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
                smtpClient.Credentials = new System.Net.NetworkCredential()
                {
                    UserName = koodinenEmail,
                    Password = koodinenSalasana
                };
                smtpClient.EnableSsl = true;

                var viesti = LuoViesti(vastaanottajaEmail, vastaanottajanNimi);

                mailMessage.BodyEncoding = System.Text.Encoding.UTF8;
                mailMessage.IsBodyHtml = true;
                mailMessage.Body = viesti;
                smtpClient.Send(mailMessage);
            }
            catch
            {
                return false;
            }
            return true;
        }

        private string LuoViesti(string vastaanottajaEmail, string nimi)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(@$"<h4>Kiitos rekisteröitymisestäsi Koodiseen, {nimi}!</h4>");
            sb.AppendLine("</br>");
            sb.AppendLine("Tervetuloa oppimaan koodaamisen saloja Koodisen kurssien avulla! :)");
            sb.AppendLine("</br>");
            sb.AppendLine("");
            sb.AppendLine("Ystävällsin terveisin,");
            sb.AppendLine("</br>");
            sb.AppendLine("");
            sb.AppendLine("Koodisen tiimi");
            return null;
        }
    }
}
