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

                var viesti = LuoViesti(vastaanottajanNimi);

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

        private string LuoViesti(string nimi)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(@"<!DOCTYPE html>
                                <html lang=""en"">
                                <head>
                                    <meta charset=""UTF-8"">
                                    <meta http-equiv=""X-UA-Compatible"" content=""IE=edge"">
                                    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
                                    <title> Document </title>
                                    <link rel=""stylesheet"" href=""https://maxcdn.bootstrapcdn.com/bootstrap/4.0.0/css/bootstrap.min.css"" integrity=""sha384-Gn5384xqQ1aoWXA+058RXPxPg6fy4IWvTNh0E263XmFcJlSAwiGgFAW/dAiS6JXm"" crossorigin=""anonymous"">
                                </head>
                                <body style=""max-width: 80%"">");
            sb.AppendLine(@$"<h4>Kiitos rekisteröitymisestäsi Koodiseen, {nimi}!</h4>");
            sb.AppendLine("</br>");
            sb.AppendLine("<p>Tervetuloa oppimaan koodaamisen saloja Koodisen kurssien avulla! :)</p>");
            sb.AppendLine("</br></br>");
            sb.AppendLine("<p>Ystävällisin terveisin,</p>");
            sb.AppendLine("</br></br>");
            sb.AppendLine(@"<p>Koodisen tiimi</p>
                        </body>
                        </html>");
            return sb.ToString();
        }
    }
}
