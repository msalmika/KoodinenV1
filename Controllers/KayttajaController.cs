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
                          select new ProfiiliViewModel{ Nimi = k.Nimi, KurssiId = x.KurssiId}).ToList();


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
        public IActionResult KurssistaAloitetut(int kurssiId)
        {
            int id = HttpContext.Session.GetInt32("id") ?? 0;
            Apumetodit am = new Apumetodit(_context);
            if (id == 0)
            {
                return RedirectToAction("Kirjautuminen", "Etusivu");
            }

            var käyttäjä = _context.Kayttajas.Where(k => k.KayttajaId == id).FirstOrDefault();
            var oppitunnit = _context.Oppituntis.Where(k => k.KurssiId == kurssiId).ToList();
            List<Tehtava> tehtavat = new List<Tehtava>();
            foreach(var oppitunti in oppitunnit)
            {
                tehtavat.AddRange(_context.Tehtavas.Where(o => o.OppituntiId == oppitunti.OppituntiId).ToList());
            }

            List<int?> suoritetutTehtavat = _context.TehtavaSuoritus.Where(k => k.KayttajaId == id).Select(x => x.TehtavaId).ToList();
            
            KurssistaAloitettuViewModel KAVM = new KurssistaAloitettuViewModel()
            {
                Kurssi = _context.Kurssis.Find(kurssiId),
                Oppitunnit = oppitunnit,
                Tehtävät = tehtavat,
                Suoritetut = suoritetutTehtavat
            };

            return View(KAVM);
        }

    }
}


