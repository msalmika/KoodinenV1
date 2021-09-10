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

       
        public IActionResult Profiili()
        {
            int id = HttpContext.Session.GetInt32("id") ?? 0;
            Apumetodit am = new Apumetodit(_context);
            var käyttäjä = am.HaeKäyttäjä(id);
            
            if (id == 0)
            {
                return RedirectToAction("Kirjautuminen","Etusivu");
            }

            KoodinenDBContext db = _context;

            //var käyt = db.Kayttajas.Where(k => k.KayttajaId == id).FirstOrDefault();
            //db.Entry(käyt).Collection(ku => ku.Kurssis).Load();
            //db.Entry(käyt).Collection(s => s.KurssiSuoritus).Load();
            //db.Entry(käyt).Collection(o => o.OppituntiSuoritus).Load();
            //db.Entry(käyt).Collection(t => t.TehtavaSuoritus).Load();

            //foreach (var kurssi in käyt.Kurssis)
            //{
            //    db.Entry(kurssi).Collection(x => x.Oppituntis).Load();
            //    foreach (var oppitunti in kurssi.Oppituntis)
            //    {
            //        db.Entry(oppitunti).Collection(x => x.Tehtavas).Load();
            //    }
            //}

            //var tehdyt = käyt.KurssiSuoritus.Where(y => y.Kesken == false).ToList();
            //var kesken = käyt.KurssiSuoritus.Where(x => x.Kesken == true).ToList();

            //ViewBag.kesken = kesken;
            //ViewBag.suoritetut = tehdyt;

            return View(käyttäjä);

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
        public async Task<IActionResult> Muokkaa(int id, string nimi, string email)
        {

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


        

    }
}
