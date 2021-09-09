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
//using KoodinenV1.Testaus;

namespace KoodinenV1.Controllers
{
    public class RekisteröityminenController : Controller
    {
        private readonly ILogger<RekisteröityminenController> _logger;

        private readonly KoodinenDBContext _context;


        private readonly IConfiguration _configuration;
        //

        public RekisteröityminenController(ILogger<RekisteröityminenController> logger, KoodinenDBContext context, IConfiguration configuration)
        {
            _logger = logger;
            _context = context;
            _configuration = configuration;
        }
        public IActionResult Index()
        {
            return RedirectToAction("Rekisteröityminen");
        }
        public IActionResult Rekisteröityminen()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Rekisteröityminen(string Nimi, string Email, string Salasana, string tarkistaSalasana)
        {
            Apumetodit am = new Apumetodit(_context);
            KoodinenDBContext db = _context;
            //Tarkistetaan että jokaisessa kentässä on tekstiä, email on uniikki, salasanat täsmäävät ja salasana on tietyn mallinen
            if (!string.IsNullOrWhiteSpace(Nimi) && !string.IsNullOrWhiteSpace(Email) && !string.IsNullOrWhiteSpace(Salasana) && !string.IsNullOrWhiteSpace(tarkistaSalasana))
            {
                if (am.TarkistaEmail(Email) == true)
                {
                    var emailTarkistus = from a in db.Kayttajas
                                         select a.Email;
                    foreach (var email in emailTarkistus)
                    {
                        if (Email != email)
                        {
                            continue;
                        }
                        else
                        {
                            ModelState.AddModelError("Email", "Sähköpostiosoitteella on jo käyttäjätunnus!");
                            return View();
                        }
                    }
                    if (Salasana.Length >= 6 && Salasana.Any(c => char.IsUpper(c)) && Salasana.Any(d => char.IsLower(d)) && Salasana.Any(e => char.IsDigit(e)))
                    {

                        if (Salasana == tarkistaSalasana)
                        {
                            try
                            {
                                am.LisääKäyttäjä(Email, Salasana, Nimi);
                                return RedirectToAction("Kirjautuminen", "Etusivu");
                            }
                            catch (Exception e)
                            {
                                Trace.WriteLine(e);
                                return View();
                            }
                        }
                        else
                        {
                            ModelState.AddModelError("Salasana", "Salasanat eivät täsmää!");
                            return View();
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("Salasana", "Salasanan täytyy olla vähintään 6 merkkiä pitkä ja sen täytyy sisältää\n" +
                            "vähintään yksi pieni kirjain, yksi iso kirjain ja yksi numero");
                        return View();
                    }
                }
                else
                {
                    ModelState.AddModelError("Email", "Sähköpostiosoite on väärässä muodossa");
                    return View();
                }
            }
            else
            {
                ModelState.AddModelError("Nimi", "Täytä kaikki kentät");
                return View();
            }
        }
        //public IActionResult Testaus()
        //{
        //    string syöte = "return " + '"' + "Terve mualima!" + '"' + ';';
        //    string expected = "Terve mualima!";
        //    string onnistuiko = TestiFunc.TestaaKoodi(syöte, expected);
        //    return Content(onnistuiko);
        //}
      

        //public IActionResult TestausUT()
        //{
        //    TestiLuokka tl = new TestiLuokka();
        //    //tl.KirjoitaPäälle();
        //    tl.MuodostaTesti("public int Palauta(){\treturn 2;}\n");
        //    var onnistuiko = tl.TestaaSyöte();
        //    tl.KirjoitaPäälle();
        //    return Content(onnistuiko);
        //}
    }
}
