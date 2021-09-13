using KoodinenV1.FuncServModels;
using KoodinenV1.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
//using KoodinenV1.Testaus;

namespace KoodinenV1.Controllers
{
    public class CSharpAlkeetController : Controller
    {
        

        private readonly KoodinenDBContext _context;

        public CSharpAlkeetController(KoodinenDBContext context)
        {
            _context = context;
        }
     
        public IActionResult Esittely(string viesti = null)
        {
            int? id = HttpContext.Session.GetInt32("id");
            ViewBag.Viesti = viesti;
            return View();
        }
        public IActionResult AloitaKurssi()
        {
            int? id = HttpContext.Session.GetInt32("id");
            if (id == null)
            {
                return RedirectToAction("Esittely", new { viesti = "Kurssille rekisteröityminen vaatii sivulle kirjautumisen!" });
            }
            _context.KurssiSuoritus.Add(new KurssiSuoritu() { KayttajaId = id, Kesken = true, KurssiId = 4, SuoritusPvm = DateTime.Today });
            _context.SaveChanges();
            return RedirectToAction("Oppitunti1");
        }
        public IActionResult Oppitunti1()
        {

            return View();
        }
        
        [HttpPost]
        public IActionResult Oppitunti1(string Tekstialue)
        {
            string email = HttpContext.Session.GetString("email");
            if (Tekstialue == null)
            {
                return View();
            }

            if (Tekstialue.StartsWith("Console.WriteLine(\"") && Tekstialue.EndsWith("\");"))
            {
                Tekstialue = Tekstialue.Replace("Console.WriteLine(\"", "");
                Tekstialue = Tekstialue.Replace("\");", "");

                if (email != null)
                {
                    Suoritus suoritus = new Suoritus() { email = email, tehtavaid = 10 };
                    TehtävänLähetys.Tarkista(suoritus);
                }
            }
            else
            {
                Tekstialue = "Virhe";
            }
            ViewBag.Tekstialue = Tekstialue;
            return View();
        }
        public IActionResult Oppitunti2()
        {
            return View();
        }
        public IActionResult Oppitunti3()
        {
            return View();
        }
        
    }
    
}
