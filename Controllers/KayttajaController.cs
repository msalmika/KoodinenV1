using KoodinenV1.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KoodinenV1.Controllers
{
    public class KayttajaController : Controller
    {

        private readonly ILogger<RekisteröityminenController> _logger;

        private readonly KoodinenDBContext _context;


        private readonly IConfiguration _configuration;
        //

        public KayttajaController(ILogger<RekisteröityminenController> logger, KoodinenDBContext context, IConfiguration configuration)
        {
            _logger = logger;
            _context = context;
            _configuration = configuration;
        }

       
        public IActionResult Profiili(int id)
        {
            id = 1; // kovakoodattu, korvataan alla:
            //id = (int)HttpContext.Session.GetInt32("id");

            var käyttäjä = (from k in _context.Kayttajas
                            where k.KayttajaId == id
                            select k).FirstOrDefault();

            var kurssisuoritukset = from x in _context.KurssiSuoritus
                                    where x.KayttajaId == id
                                    orderby x.SuoritusPvm
                                    select x;

            ViewBag.kurssisuoritukset = kurssisuoritukset;
            return View(käyttäjä);
        }
        public IActionResult KurssistaSuoritetut(int id, int kurssiId)
        {
            id = 1; //korvataan sessionin käyttäjäIdllä
            //id = (int)HttpContext.Session.GetInt32("id");

            kurssiId = 2; // kovakoodattu

            KoodinenDBContext db = _context;

            var käyt = db.Kayttajas.Where(k => k.KayttajaId == id).FirstOrDefault();
            db.Entry(käyt).Collection(ku => ku.Kurssis).Load();

            //LISÄTÄÄN VÄLIAIKAISEEN db:hen KÄYTTÄJÄN KURSSIEN OPPITUNNIT JA TEHTÄVÄT
            foreach (var kurssi in käyt.Kurssis)
            {
                db.Entry(kurssi).Collection(x => x.Oppituntis).Load();
                foreach (var oppitunti in kurssi.Oppituntis)
                {
                    db.Entry(oppitunti).Collection(x => x.Tehtavas).Load();
                }
            }

            var oppitunnit = db.Oppituntis.Where(x => x.KurssiId == kurssiId).ToList();
            var tehtävät = db.Tehtavas.ToList();

            ViewBag.kurssinimi = "C# perusteet"; // kovakoodattu, korvataan
            ViewBag.oppitunnit = oppitunnit;
            ViewBag.tehtävät = tehtävät;

            return View(käyt);
            
        }
        public async Task<IActionResult> Muokkaa(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kayttaja = await _context.Kayttajas.FindAsync(id);
            if (kayttaja == null)
            {
                return NotFound();
            }
            return View(kayttaja);
        }

        // POST: People/Edit/5
        /// Lähettää kirjautuneen henkilön pävitetyt tiedot muokkauksen lomakkeesta
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Muokkaa(int id, [Bind("KayttajaId,Nimi,Email,Salasana")] Kayttaja kayttaja)
        {
            Apumetodit am = new Apumetodit(_context);
            if (id != kayttaja.KayttajaId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    kayttaja.Salasana = am.HashSalasana(kayttaja.Salasana);
                        _context.Update(kayttaja);
                        await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!am.KäyttäjäOnOlemassa(kayttaja.KayttajaId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Profiili), new { id = kayttaja.KayttajaId });
            }
            return View(kayttaja);
        }
        public IActionResult Testi()
        {
            var query = _context.TehtavaSuoritus.Where(x => x.KayttajaId == 1).ToList();
            string res = "";
            foreach (var q in query)
            {
                res += q.KayttajaId + " " + q.SuoritusPvm + " " + q.TehtavaId + Environment.NewLine;
            }
            return Content(res);
        }

        

    }
}
