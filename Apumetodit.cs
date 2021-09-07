using KoodinenV1.Models;
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

        /// <summary>
        /// Metodi hakee listan kurssisuorituksista tietokannasta.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Palauttaa listan KurssiSuoritu-olioita</returns>
        public List<KurssiSuoritu> HaeKurssi()
        {
            var kurssisuoritukset = _context.KurssiSuoritus.Select(k => k).ToList();
            return kurssisuoritukset;
        }

        public bool LisääKäyttäjä(string email, string salasana, string nimi = null)
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
    }
}
