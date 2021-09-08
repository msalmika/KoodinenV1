using KoodinenV1.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
            id = 1; // kovakoodattu, korvataan
            //Kayttaja kayttaja = new();

           /* HttpContext.Session.SetInt32("id", kayttaja.KayttajaId); */// KORVAAMAAN KOVAKOODIA
            
            //kayttaja.KayttajaId = id;
            //KoodinenDBContext db = _context;
            // käyttäjän tiedot
            var käyttäjä = (from k in _context.Kayttajas
                           where k.KayttajaId == id
                           select k).FirstOrDefault();


            var kurssisuoritukset = from x in _context.KurssiSuoritus
                                         where x.KayttajaId == id
                                         select x;

            ViewBag.oppituntisuoritukset = (from z in _context.OppituntiSuoritus
                                            where z.KayttajaId == id
                                            select z).FirstOrDefault();

            ViewBag.tehtäväsuoritukset = (from t in _context.TehtavaSuoritus
                                         where t.KayttajaId == id
                                         select t).FirstOrDefault();

            ViewBag.kurssisuoritukset = kurssisuoritukset.ToList();
            //haetaaan käyttäjän suoritukset
            //foreach (var x in käyttäjä)
            //{
            //    _context.Entry(x).Collection(e => e.KurssiSuoritus).Load();
            //}
            //foreach (var x in käyttäjä)
            //{
            //    _context.Entry(x).Collection(e => e.OppituntiSuoritus).Load();
            //}
            //foreach (var x in käyttäjä)
            //{
            //    _context.Entry(x).Collection(e => e.TehtavaSuoritus).Load();
            //}


            return View(käyttäjä);
        }

        public IActionResult testi()
        {
            return View();
        }
        
    }
}
