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
        public IActionResult Rekisteröityminen(Kayttaja kayttaja)
        {

            try
            {
                _context.Kayttajas.Add(kayttaja);
                _context.SaveChanges();
                Apumetodit am = new Apumetodit(_context);
                var k = am.HaeKäyttäjä(kayttaja.Email);
                HttpContext.Session.SetInt32("id", k.KayttajaId);
                HttpContext.Session.SetString("Nimi", k.Nimi);
                return RedirectToAction("RekOnnistui", kayttaja);
            }
            catch (Exception e)
            {
                Trace.WriteLine(e);
                return RedirectToAction("RekEpäonnistui");
            }
        }
        public IActionResult RekOnnistui()
        {
            
            return View();
        }
        public IActionResult RekEpäonnistui()
        {
            return View();
        }
    }
}
