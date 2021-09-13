﻿using KoodinenV1.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace KoodinenV1.Controllers
{
    public class EtusivuController : Controller
    {
        private readonly ILogger<EtusivuController> _logger;

        private readonly dbKoodinenContext _context;

        private readonly IConfiguration _configuration;
        
        public EtusivuController(ILogger<EtusivuController> logger, dbKoodinenContext context, IConfiguration configuration)
        {
            _logger = logger;
            _context = context;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Kirjautuminen()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Kirjautuminen(string email, string salasana)
        {
            int id = 0;
            Apumetodit am = new Apumetodit(_context);

            if (!(string.IsNullOrEmpty(email) || string.IsNullOrEmpty(salasana)))
            {
                var kirjautuja = am.HaeKäyttäjä(email);
                if (kirjautuja != null && kirjautuja.Email == email && kirjautuja.Salasana == am.HashSalasana(salasana))
                {
                    id = kirjautuja.KayttajaId;
                    var k = am.HaeKäyttäjä(kirjautuja.KayttajaId);
                    HttpContext.Session.SetInt32("id", k.KayttajaId);
                    HttpContext.Session.SetString("email", k.Email);
                    if (am.KäyttäjäOnOlemassa(id) == true)
                    {
                        if (am.KäyttäjäOnAdmin(id))
                        {
                            am.LisääAdminSessioon(this.HttpContext.Session, id);
                            return RedirectToAction("AdminPääsivu", "Admin");
                        }
                        return RedirectToAction("Profiili", "Kayttaja");
                    }
                }
                else
                {
                    ModelState.AddModelError("Salasana", "Väärä sähköpostiosoite tai salasana");
                    return View(kirjautuja);
                }
            }

            else
            {
                ModelState.AddModelError("Email", "Kirjoita sähköpostiosoite ja salasana");
            }
            return View();
        }

        
        public IActionResult Chat()
        {
            return View();
        }

        public IActionResult Kurssit()
        {
            Apumetodit am = new Apumetodit(_context);
            var kurssit = am.HaeKurssit();
            return View(kurssit);
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
