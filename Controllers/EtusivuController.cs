using KoodinenV1.Models;
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

        private readonly KoodinenDBContext _context;


        private readonly IConfiguration _configuration;
        //
        
        public EtusivuController(ILogger<EtusivuController> logger, KoodinenDBContext context, IConfiguration configuration)
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
            KoodinenV1.Apumetodit am = new Apumetodit(_context);

            var kirjautuja = _context.Kayttajas.Where(k => k.Email == email).FirstOrDefault();

            if (kirjautuja != null)
            {

                if (kirjautuja.Salasana == am.HashSalasana(salasana))
                {
                    id = kirjautuja.KayttajaId;
                    var k = am.HaeKäyttäjä(kirjautuja.KayttajaId);
                    HttpContext.Session.SetInt32("Id", k.KayttajaId);
                    if (am.KäyttäjäOnOlemassa(id) == true)
                    {

                        if (am.KäyttäjäOnAdmin(id) == true)
                        {
                            am.LisääAdminSessioon(this.HttpContext.Session, id);
                            return RedirectToAction("Pääsivu", "Kurssi");
                        }
                        return RedirectToAction("Pääsivu", "Kurssi");
                    }
                    else
                    {
                        ModelState.AddModelError("Salasana", "Väärä käyttäjätunnus tai salasana");
                    return View(kirjautuja);
                    }
                }
            }
            return View();
        }

        
        public IActionResult Chat()
        {
            return View();
        }

        public IActionResult Kurssit()
        {
            return View();
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
