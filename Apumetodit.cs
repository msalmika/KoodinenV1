using KoodinenV1.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace KoodinenV1
{
    public class Apumetodit
    {
        private readonly KoodinenDBContext _context;

        public Apumetodit(KoodinenDBContext context)
        {
            _context = context;
        }
        /// <summary>
        /// Metodi hakee käyttäjän tietokannasta id:n perusteella.
        /// </summary>
        /// <param name="id">int id</param>
        /// <returns>Palauttaa Kayttaja-olion</returns>
        public Kayttaja HaeKäyttäjä(int id)
        {
            var käyttäjä = new Kayttaja();
            käyttäjä = _context.Kayttajas.Find(id);
            return käyttäjä;
        }

        /// <summary>
        /// Metodi palauttaa käyttäjän tietokannasta Emailin perusteella.
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public Kayttaja HaeKäyttäjä(string email)
        {
            var käyttäjä = new Kayttaja();
            käyttäjä = _context.Kayttajas.Where(k => k.Email == email).First();
            return käyttäjä;
        }

        /// <summary>
        /// Metodi hakee kurssin tietokannasta id:n perusteella.
        /// </summary>
        /// <param name="id">int id</param>
        /// <returns>Palauttaa Kurssi-olion</returns>
        public Kurssi HaeKurssi(int id)
        {
            var kurssi = new Kurssi();
            kurssi = _context.Kurssis.Find(id);
            return kurssi;
        }

        /// <summary>
        /// Metodi hakee tehtävän tietokannasta id:n perusteella.
        /// </summary>
        /// <param name="id">int id</param>
        /// <returns>Palauttaa Tehtava-olion</returns>
        public Tehtava HaeTehtävä(int id)
        {
            var tehtävä = new Tehtava();
            tehtävä = _context.Tehtavas.Find(id);
            return tehtävä;
        }

        public List<Kurssi> HaeKurssit()
        {
            var kurssit = _context.Kurssis.Select(k => k).ToList();
            return kurssit;
        }

        /// <summary>
        /// Metodi hakee listan kurssisuorituksista tietokannasta.
        /// </summary>
        /// <param></param>
        /// <returns>Palauttaa listan KurssiSuoritu-olioita</returns>
        public List<KurssiSuoritu> HaeKurssisuoritukset()
        {
            var kurssisuoritukset = _context.KurssiSuoritus.Select(k => k).ToList();
            return kurssisuoritukset;
        }

        /// <summary>
        /// Metodi hakee listan tietyn käyttäjän kurssisuorituksista tietokannasta.
        /// </summary>
        /// <param name="käyttäjäId">int käyttäjäId</param>
        /// <returns>Palauttaa listan KurssiSuoritu-olioita</returns>
        public List<KurssiSuoritu> HaeKäyttäjänKurssisuoritukset(int käyttäjäId)
        {
            var kurssisuoritukset = _context.KurssiSuoritus.Where(k => k.KayttajaId == käyttäjäId).Select(k => k).ToList();
            return kurssisuoritukset;
        }

        /// <summary>
        /// Metodi hakee listan tietyn käyttäjän hyväksytysti suorittamista oppitunneista.
        /// </summary>
        /// <param name="käyttäjäId">int käyttäjäId</param>
        /// <returns>Palauttaa listan OppituntiSuoritu-olioita</returns>
        public List<OppituntiSuoritu> HaeKäyttäjänOppituntisuoritukset(int käyttäjäId)
        {
            var oppituntisuoritukset = _context.OppituntiSuoritus.Where(k => k.KayttajaId == käyttäjäId).Select(k => k).ToList();
            return oppituntisuoritukset;
        }

        /// <summary>
        /// Metodi hakee listan tietyn käyttäjän hyväksytysti suorittamista tehtävistä.
        /// </summary>
        /// <param name="käyttäjäId">int käyttäjäId</param>
        /// <returns>Palauttaa listan TehtavaSuoritu-olioita</returns>
        public List<TehtavaSuoritu> HaeKäyttäjänTehtäväsuoritukset(int käyttäjäId)
        {
            var tehtäväsuoritukset = _context.TehtavaSuoritus.Where(k => k.KayttajaId == käyttäjäId).Select(k => k).ToList();
            return tehtäväsuoritukset;
        }

        /// <summary>
        /// Metodi hakee listan oppituntisuorituksista tietokannasta.
        /// </summary>
        /// <param></param>
        /// <returns>Palauttaa listan OppituntiSuoritu-olioita</returns>
        public List<OppituntiSuoritu> HaeOppituntiSuoritukset()
        {
            var oppituntisuoritukset = _context.OppituntiSuoritus.Select(k => k).ToList();
            return oppituntisuoritukset;
        }

        /// <summary>
        /// Metodi hakee listan tehtäväsuorituksista tietokannasta.
        /// </summary>
        /// <param></param>
        /// <returns>Palauttaa listan TehtavaSuoritu-olioita</returns>
        public List<TehtavaSuoritu> TehtäväSuoritukset()
        {
            var tehtäväSuoritukset = _context.TehtavaSuoritus.Select(k => k).ToList();
            return tehtäväSuoritukset;
        }
        /// <summary>
        /// Lisää käyttäjän tietokantaan, ja palauttaa true, jos se onnistuu. Jos käyttäjän lisääminen ei onnistu,
        /// metodi palauttaa false. 
        /// </summary>
        /// <param name="email"></param>
        /// <param name="salasana"></param>
        /// <param name="nimi"></param>
        /// <returns>Boolean, joka kertoo, onnistuiko lisäys.</returns>
        public bool LisääKäyttäjä(string email, string salasana, string nimi)
        {
                var uusiKäyttäjä = new Kayttaja();
                uusiKäyttäjä.Nimi = nimi;
                uusiKäyttäjä.Email = email;
                uusiKäyttäjä.Salasana = HashSalasana(salasana);

                try
                {
                    _context.Kayttajas.Add(uusiKäyttäjä);
                    _context.SaveChanges();
                }

                catch
                {
                    return false;
                }
            return true;
        }
        /// <summary>
        /// Lisää tietokantaan uuden kurssisuoritustiedon. Palauttaa true, jos onnistui,
        /// falsen, jos ei.
        /// </summary>
        /// <param name="käyttäjäId"></param>
        /// <param name="kurssiId"></param>
        /// <returns>Boolean true/false</returns>
        public bool LisääKurssiSuoritus(int käyttäjäId, int kurssiId)
        {
            var uusiKurssisuoritus = new KurssiSuoritu();
            uusiKurssisuoritus.SuoritusPvm = DateTime.Today;
            uusiKurssisuoritus.KayttajaId = käyttäjäId;
            uusiKurssisuoritus.KurssiId = kurssiId;

            try
            {
                _context.KurssiSuoritus.Add(uusiKurssisuoritus);
                _context.SaveChanges();
            }
            catch
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// Lisää tietokantaan uuden oppituntisuoritustiedon. Palauttaa true, jos onnistui,
        /// falsen, jos ei.
        /// </summary>
        /// <param name="käyttäjäId"></param>
        /// <param name="oppituntiId"></param>
        /// <returns>Boolean true/false</returns>
        public bool LisääOppituntiSuoritus(int käyttäjäId, int oppituntiId)
        {
            var uusiOppituntisuoritus = new OppituntiSuoritu();
            uusiOppituntisuoritus.SuoritusPvm = DateTime.Today;
            uusiOppituntisuoritus.KayttajaId = käyttäjäId;
            uusiOppituntisuoritus.OppituntiId = oppituntiId;

            try
            {
                _context.OppituntiSuoritus.Add(uusiOppituntisuoritus);
                _context.SaveChanges();
            }
            catch
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Lisää tietokantaan uuden tehtäväsuoritustiedon. Palauttaa true, jos onnistui,
        /// falsen, jos ei.
        /// </summary>
        /// <param name="käyttäjäId"></param>
        /// <param name="tehtäväId"></param>
        /// <returns>Boolean true/false</returns>
        public bool LisääTehtäväSuoritus(int käyttäjäId, int tehtäväId)
        {
            var uusiTehtäväsuoritus = new TehtavaSuoritu();
            uusiTehtäväsuoritus.SuoritusPvm = DateTime.Today;
            uusiTehtäväsuoritus.KayttajaId = käyttäjäId;
            uusiTehtäväsuoritus.TehtavaId = tehtäväId;

            try
            {
                _context.TehtavaSuoritus.Add(uusiTehtäväsuoritus);
                _context.SaveChanges();
            }
            catch
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Muuttaa salasana-stringin hashcodeksi.
        /// </summary>
        /// <param name="salasana">string salasana</param>
        /// <returns>string salasana</returns>
        public string HashSalasana(string salasana)
        {
            var sha1 = new SHA1CryptoServiceProvider();
            byte[] salasana_bytes = Encoding.ASCII.GetBytes(salasana);
            byte[] encrypted_bytes = sha1.ComputeHash(salasana_bytes);
            return Convert.ToBase64String(encrypted_bytes);
        }

        public bool KäyttäjäOnOlemassa(int id)
        {
            var q = (from k in _context.Kayttajas
                     where k.KayttajaId == id
                     select k).FirstOrDefault().KayttajaId;
            return (q == id) ? true : false;
        }

        public bool KäyttäjäOnAdmin(int id)
        {
            var onAdmin = (from k in _context.Kayttajas
                           where k.KayttajaId == id
                           select k).FirstOrDefault().OnAdmin;
            return onAdmin;
        }
        
        public int? HaeAdmin(ISession sessio)
        {
            string käyttäjäserialized = sessio.GetString("käyttäjä");
            if (!string.IsNullOrEmpty(käyttäjäserialized))
            {
                int käyttäjä = Convert.ToInt32(käyttäjäserialized);
                return käyttäjä;
            }
            return null;
        }

        public void LisääAdminSessioon(ISession session, int? id)
        {
            session.SetString("käyttäjä", id.ToString());
        }
        public bool OnkoSessiossa(ISession sessio)
        {
            int? id = null;
            KoodinenV1.Apumetodit am = new(_context);
            var käyttäjä = from k in _context.Kayttajas
                           select k;
            foreach (var k in käyttäjä)
            {
                id = k.KayttajaId;
            }
            if (am.HaeAdmin(sessio) == id)
            {
                return true;
            }
            return false;
        }
        public bool TarkistaEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        public bool SalasanaHyväksytyssäMuodossa(string salasana)
        {
            bool salasanaOk = false;
            if (salasana.Length >= 6 && salasana.Any(c => char.IsUpper(c)) && salasana.Any(d => char.IsLower(d)) && salasana.Any(e => char.IsDigit(e)))
            {
                salasanaOk = true;
            }
            return salasanaOk;
        }

        public bool VaihdaSalasana(int id, string salasana)
        {
            try
            {
                var käyttäjä = HaeKäyttäjä(id);
                käyttäjä.Salasana = HashSalasana(salasana);
                _context.SaveChanges();
            }
            catch
            {
                return false;
            }
            return true;
        }
        
    }
}
