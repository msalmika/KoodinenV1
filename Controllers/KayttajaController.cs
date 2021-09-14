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
                return RedirectToAction("Kirjautuminen","Etusivu");
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
                          select new ProfiiliViewModel{ Nimi = k.Nimi}).ToList();


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
        public IActionResult KurssistaAloitetut(int id,/*[FromForm]*/int kurssiId)
        {
            id = HttpContext.Session.GetInt32("id") ?? 0;
           /* kurssiId = 4;*/ // otetaan myöhemmin postista

            KoodinenDBContext db = _context;

            var käyt = db.Kayttajas.Where(k => k.KayttajaId == id).FirstOrDefault();
            db.Entry(käyt).Collection(ku => ku.Kurssis).Load(); // kurssit
            db.Entry(käyt).Collection(ks => ks.KurssiSuoritus).Load(); // kurssisuoritukset
            db.Entry(käyt).Collection(t => t.TehtavaSuoritus).Load(); // tehtäväsuoritukset
            foreach (var kurssi in käyt.Kurssis)
            {
                db.Entry(kurssi).Collection(x => x.Oppituntis).Load();
                foreach (var oppitunti in kurssi.Oppituntis)
                {
                    db.Entry(oppitunti).Collection(x => x.Tehtavas).Load();
                }
            }

            //var kurssinimi = (from x in db.Kurssis
            //                  where x.KurssiId == kurssiId
            //                  select x.Nimi).FirstOrDefault();
            
            var tehdytoppit= db.OppituntiSuoritus.Where(x => x.Kesken == false).ToList();
            var keskenoppit = db.OppituntiSuoritus.Where(o => o.Kesken == true).ToList();
            var kaikkiteht = db.Tehtavas.ToList();

            var tehdytteht = new List<Tehtava>();
            var keskenteht = new List<Tehtava>();

            for (int i = 0; i < kaikkiteht.Count; i++)
            {
                foreach(var item in tehdytoppit)
                {
                    if(item.OppituntiId == kaikkiteht[i].OppituntiId)
                    {
                        tehdytteht.Add(kaikkiteht[i]);
                    }
                }
            }
            for (int i = 0; i < kaikkiteht.Count; i++)
            {
                foreach (var item in keskenoppit)
                {
                    if (item.OppituntiId == kaikkiteht[i].OppituntiId)
                    {
                        keskenteht.Add(kaikkiteht[i]);
                    }
                }
            }

            //ViewBag.kurssinimi = kurssinimi.ToString();
            ViewBag.tehdytoppit = tehdytoppit;
            ViewBag.keskenoppit = keskenoppit;
            ViewBag.tehdytteht = tehdytteht;
            ViewBag.keskenteht = keskenteht;

            //ViewBag.nimi = käyttäjä.Nimi;
            //ViewBag.id = käyttäjä.KayttajaId;

            return View(käyt);
        }
        //public IActionResult KurssistaSuoritetut(int id,/*[FromForm]*/int kurssiId)
        //{
        //    id = HttpContext.Session.GetInt32("id") ?? 0;
        //    if (id == 0)
        //    {
        //        return RedirectToAction("Kirjautuminen", "Etusivu");
        //    }
        //    kurssiId = 2; // otetaan myöhemmin postista

        //    KoodinenDBContext db = _context;

        //    var käyt = db.Kayttajas.Where(k => k.KayttajaId == id).FirstOrDefault();
        //    db.Entry(käyt).Collection(ku => ku.Kurssis).Load();

        //    foreach (var kurssi in käyt.Kurssis)
        //    {
        //        db.Entry(kurssi).Collection(x => x.Oppituntis).Load();
        //        foreach (var oppitunti in kurssi.Oppituntis)
        //        {
        //            db.Entry(oppitunti).Collection(x => x.Tehtavas).Load();
        //        }
        //    }

        //    var oppitunnit = db.Oppituntis.Where(x => x.KurssiId == kurssiId).ToList();

        //    var tehtävät = db.Tehtavas.ToList();


        //    ViewBag.kurssinimi = "C# alkeet";
        //    ViewBag.oppitunnit = oppitunnit;
        //    ViewBag.tehtävät = tehtävät;
        //    //ViewBag.nimi = käyttäjä.Nimi;
        //    //ViewBag.id = käyttäjä.KayttajaId;

        //    return View(käyt);

        //}
    }
}
