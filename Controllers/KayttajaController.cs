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


        public KayttajaController(ILogger<RekisteröityminenController> logger, KoodinenDBContext context, IConfiguration configuration)
        {
            _logger = logger;
            _context = context;
            _configuration = configuration;
        }


        public IActionResult Profiili(string viesti = null)
        {
            int id = HttpContext.Session.GetInt32("id") ?? 0;
            if (id == 0)
            {
                return RedirectToAction("Kirjautuminen", "Etusivu");
            }
            Apumetodit am = new Apumetodit(_context);
            var käyttäjä = am.HaeKäyttäjä(id);

            if (id == 0)
            {
                return RedirectToAction("Kirjautuminen", "Etusivu");
            }

            var suoritetut = (from x in _context.KurssiSuoritus
                              join k in _context.Kurssis on x.KurssiId equals k.KurssiId
                              where x.KayttajaId == id && x.Kesken == false
                              orderby x.SuoritusPvm
                              select new ProfiiliViewModel { Nimi = k.Nimi, SuoritusPVM = x.SuoritusPvm }).ToList();

            var aloitettu = (from x in _context.KurssiSuoritus
                             join k in _context.Kurssis on x.KurssiId equals k.KurssiId
                             where x.KayttajaId == id && x.Kesken == true
                             orderby x.SuoritusPvm
                             select new ProfiiliViewModel { Nimi = k.Nimi }).ToList();


            if (viesti != null)
            {
                ViewBag.Viesti = viesti;
            }

            ViewBag.aloitettu = aloitettu;
            ViewBag.suoritetut = suoritetut;

            return View(käyttäjä);

        }
        public async Task<IActionResult> Muokkaa()
        {
            int id = HttpContext.Session.GetInt32("id") ?? 0;
            if (id == 0)
            {
                return RedirectToAction("Kirjautuminen", "Etusivu");
            }
            //if (id == null)
            //{
            //    return NotFound();
            //}

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
        public async Task<IActionResult> Muokkaa(string nimi, string email)
        {
            int id = HttpContext.Session.GetInt32("id") ?? 0;
            if (id == 0)
            {
                return RedirectToAction("Kirjautuminen", "Etusivu");
            }
            Apumetodit am = new Apumetodit(_context);
            var kayttaja = am.HaeKäyttäjä(id);
            if (kayttaja == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (nimi.Length >= 100)
                {
                    ModelState.AddModelError("Nimi", "Nimi on liian pitkä!");
                    return View();
                }
                if (email.Length >= 100)
                {
                    ModelState.AddModelError("Email", "Sähköpostiosoite on liian pitkä!");
                    return View();
                }
                try
                {
                    kayttaja.Nimi = nimi;
                    kayttaja.Email = email;
                    _context.Update(kayttaja);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!am.KäyttäjäOnOlemassa(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Profiili), new { id = id });
            }
            return View(kayttaja);
        }

        public async Task<IActionResult> SalasananVaihto()
        {
            int id = HttpContext.Session.GetInt32("id") ?? 0;
            if (id == 0)
            {
                return RedirectToAction("Kirjautuminen", "Etusivu");
            }
            //if (id == null)
            //{
            //    return NotFound();
            //}
            var kayttaja = await _context.Kayttajas.FindAsync(id);
            if (kayttaja == null)
            {
                return NotFound();
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SalasananVaihto(SalasananVaihtoViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            Apumetodit am = new Apumetodit(_context);
            model.Id = (int)HttpContext.Session.GetInt32("id");
            var salasana = am.HaeKäyttäjä(model.Id).Salasana;
            if (am.HashSalasana(model.VanhaSalasana) != salasana)
            {
                ModelState.AddModelError("SalasananVaihto", "Vanha salasana on väärin");
                return View();
            }
            if (model.UusiSalasana.Length >= 50)
            {
                ModelState.AddModelError("UusiSalasana", "Salasana on liian pitkä!");
                return View();
            }
            if (!am.SalasanaHyväksytyssäMuodossa(model.UusiSalasana))
            {
                ModelState.AddModelError("SalasananVaihto", "Salasanan on oltava vähintään 6 merkkiä pitkä,\nja sen tulee sisältää ainakin yksi pieni ja suuri\nkirjain sekä numero.");
                return View();
            }
            if (am.VaihdaSalasana(model.Id, model.UusiSalasana))
            {
                string viesti = "Salasana vaihdettu onnistuneesti!";
                return RedirectToAction("Profiili", null, new { viesti }, null);
            }
            ViewBag.Viesti = "Jokin meni pieleen, yritä uudelleen. Jos ongelma toistuu, ota yhteyttä ylläpitoon.";
            return View();
        }
        public IActionResult KurssistaAloitetut(int id/*,int kurssiId*/)
        {
            // TARVITAANKO kurssiId:tä, jos käyttäjällä useampi kurssi kesken? Voiko ottaa [BIND]-> from form, että ottaisi jotenkin profiilin hyppylinkistä kurssin tähän näkymää?

            id = HttpContext.Session.GetInt32("id") ?? 0;
            Apumetodit am = new Apumetodit(_context);
            var käyt = am.HaeKäyttäjä(id);

            var suoritetutOppitunnit = (from os in _context.OppituntiSuoritus
                                        join o in _context.Oppituntis on os.OppituntiId equals o.OppituntiId
                                        join k in _context.Kurssis on o.KurssiId equals k.KurssiId
                                        join ks in _context.KurssiSuoritus on k.KurssiId equals ks.KurssiId
                                        join kä in _context.Kayttajas on ks.KayttajaId equals kä.KayttajaId
                                        where kä.KayttajaId == id && ks.Kesken == true && ks.Kesken != null && os.Kesken == false && os.Kesken != null
                                        select new Oppitunti { Nimi = o.Nimi, Kuvaus = o.Kuvaus, OppituntiId = o.OppituntiId }).AsNoTracking().Distinct().ToList();

            var suorOtTehtävät = (from ts in _context.TehtavaSuoritus
                                  join t in _context.Tehtavas on ts.TehtavaId equals t.TehtavaId
                                  join o in _context.Oppituntis on t.OppituntiId equals o.OppituntiId
                                  join os in _context.OppituntiSuoritus on o.OppituntiId equals os.OppituntiId
                                  join ks in _context.KurssiSuoritus on os.KayttajaId equals ks.KayttajaId
                                  where ks.KayttajaId == id && ks.Kesken == true && ks.Kesken != null && os.Kesken == false && os.Kesken != null
                                  select new Tehtava { Nimi = t.Nimi, Kuvaus = t.Kuvaus, OppituntiId = o.OppituntiId }).AsNoTracking().Distinct().ToList();

            var keskenOppitunnit = (from os in _context.OppituntiSuoritus
                                    join o in _context.Oppituntis on os.OppituntiId equals o.OppituntiId
                                    join k in _context.Kurssis on o.KurssiId equals k.KurssiId
                                    join ks in _context.KurssiSuoritus on k.KurssiId equals ks.KurssiId
                                    join kä in _context.Kayttajas on ks.KayttajaId equals kä.KayttajaId
                                    where kä.KayttajaId == id && ks.Kesken == true && ks.Kesken != null && os.Kesken == true && os.Kesken != null
                                    select new Oppitunti { Nimi = o.Nimi, Kuvaus = o.Kuvaus, OppituntiId = o.OppituntiId }).AsNoTracking().Distinct().ToList();

            var keskenOTsuortehtävät = (from t in _context.Tehtavas
                                        join o in _context.Oppituntis on t.OppituntiId equals o.OppituntiId
                                        join os in _context.OppituntiSuoritus on o.OppituntiId equals os.OppituntiId
                                        join ks in _context.KurssiSuoritus on os.KayttajaId equals ks.KayttajaId
                                        where ks.KayttajaId == id && ks.Kesken == true && ks.Kesken != null && os.Kesken == true && os.Kesken != null
                                        select new Tehtava { TehtavaId = t.TehtavaId, Nimi = t.Nimi, Kuvaus = t.Kuvaus, OppituntiId = o.OppituntiId }).AsNoTracking().Distinct().ToList();

            /// TOIMII tällä versiolla, jos tehtäväsuorituksiin lisätty myös Kesken-kenttä:
            /// var keskenOTsuortehtävät = (from ts in _context.TehtavaSuoritus
            //var keskenOTsuortehtävät = (from ts in _context.TehtavaSuoritus
            //                            join t in _context.Tehtavas on ts.TehtavaId equals t.TehtavaId
            //                            join o in _context.Oppituntis on t.OppituntiId equals o.OppituntiId
            //                            join os in _context.OppituntiSuoritus on o.OppituntiId equals os.OppituntiId
            //                            join ks in _context.KurssiSuoritus on os.KayttajaId equals ks.KayttajaId
            //                            where ks.KayttajaId == id && ks.Kesken == true && ks.Kesken != null && os.Kesken == true && os.Kesken != null && ts.Kesken == false && ts.Kesken != null
            //                            select new Tehtava { Nimi = t.Nimi, Kuvaus = t.Kuvaus, OppituntiId = o.OppituntiId, TehtavaId = t.TehtavaId }).AsNoTracking().Distinct().ToList();

            // NÄILLÄ NÄKYMIN PRINTTAA HULLUN TOISTOMÄÄRÄN SAMAA SARAKETTA, EI TEE TÄTÄ PAIKALLISELLA DB:LLÄ). SAA NÄKYVIIN KUN KÄY KOMMENTOIMASSA AUKI
            // PROFIILI CSHTML: RIVIN 89-91 JA KOMMENTOI POIS RIVIT 92-94.

            var käyttäjä = _context.Kayttajas.Where(k => k.KayttajaId == id).FirstOrDefault();
            _context.Entry(käyttäjä).Collection(ku => ku.Kurssis).Load(); // kurssit
            foreach (var kurssi in käyt.Kurssis)
            {
                _context.Entry(kurssi).Collection(x => x.Oppituntis).Load();
                foreach (var oppitunti in kurssi.Oppituntis)
                {
                    _context.Entry(oppitunti).Collection(x => x.Tehtavas).Load();
                }
            }
            var kurssinteht = _context.Tehtavas.ToList();

            var keskenOTkeskentehtävät = new List<Tehtava>();

            foreach (var t in kurssinteht)
            {
                foreach (var teht in keskenOTsuortehtävät)
                {
                    if (teht.TehtavaId != t.TehtavaId)
                        keskenOTkeskentehtävät.Add(t);
                }
            }

            ViewBag.tehdytoppit = suoritetutOppitunnit;
            ViewBag.tehdytteht = suorOtTehtävät;
            ViewBag.keskenoppit = keskenOppitunnit;
            ViewBag.keskenOTsuorT = keskenOTsuortehtävät;
            ViewBag.keskenOTkeskentT = keskenOTkeskentehtävät;

            //ViewBag.kurssinimi = kurssinimi; // haettava myös

            return View(käyt);
        }

    }
}


