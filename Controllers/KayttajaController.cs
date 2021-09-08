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
            //id = 1; // kovakoodattu, korvataan
            Kayttaja kayttaja = new();

            HttpContext.Session.SetInt32("id", kayttaja.KayttajaId); // KORVAAMAAN KOVAKOODIA
            
            //kayttaja.KayttajaId = id;
            //KoodinenDBContext db = _context;
            // käyttäjän tiedot
            var käyttäjä = (from k in _context.Kayttajas
                           where k.KayttajaId == id
                           select k).FirstOrDefault();
           
            ViewBag.kurssisuoritukset = käyttäjä.KurssiSuoritus.ToList();
            ViewBag.oppituntisuoritukset = käyttäjä.OppituntiSuoritus.ToList();
            ViewBag.tehtäväsuoritukset = käyttäjä.TehtavaSuoritus.ToList();

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
        public IActionResult Profiili2(int id)
        {
            id = 2; // kovakoodattu, korvataan
            //var id= HttpContext.Session.GetInt32("personId");

            Kayttaja kayttaja = new();
            kayttaja.KayttajaId = id;
            KoodinenDBContext db = _context;
            // käyttäjän tiedot
            ViewBag.käyttäjä = (from k in db.Kayttajas
                            where k.KayttajaId == id
                            select k).FirstOrDefault();

            // haetaaan käyttäjän suoritukset
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


            return View(ViewBag.käyttäjä);
        }

    }
}
