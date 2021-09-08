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
using KoodinenV1.Testaus;

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
        public IActionResult Rekisteröityminen(string Nimi, string Email, string Salasana)
        {

            try
            {
                Apumetodit am = new Apumetodit(_context);
                am.LisääKäyttäjä(Email, Salasana, Nimi);
                return RedirectToAction("Kirjautuminen", "Etusivu");
            }
            catch (Exception e)
            {
                Trace.WriteLine(e);
                return RedirectToAction("RekEpäonnistui");
            }
        }
        public IActionResult RekEpäonnistui()
        {
            return View();
        }
        public IActionResult Testaus()
        {
            string syöte = "return " + '"' + "Terve mualima!" +'"' + ';';
            string onnistuiko = TestiFunc.TestaaKoodi(syöte);
            return Content(onnistuiko);
        }

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
