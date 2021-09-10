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

            var suoritetut = (from x in _context.KurssiSuoritus
                                join k in _context.Kurssis on x.KurssiId equals k.KurssiId
                                where x.KayttajaId == id && x.Kesken == false
                                orderby x.SuoritusPvm
                                select new ProfiiliViewModel { Nimi = k.Nimi, SuoritusPVM = x.SuoritusPvm }).ToList();

            var kesken = (from x in _context.KurssiSuoritus
                          join k in _context.Kurssis on x.KurssiId equals k.KurssiId
                          where x.KayttajaId == id && x.Kesken == true
                          orderby x.SuoritusPvm
                          select new ProfiiliViewModel{ Nimi = k.Nimi}).ToList();



            ViewBag.kesken = kesken;
            ViewBag.suoritetut = suoritetut/*tehdyt*/;

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
