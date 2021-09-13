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
        

        private readonly dbKoodinenContext _context;

        public CSharpAlkeetController(dbKoodinenContext context)
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
        public IActionResult Oppitunti1_Teht2()
        {

            return View();
        }
        [HttpPost]
        public IActionResult Oppitunti1_Teht2(/*[FromForm] int tehtava_id,*/ string Tekstialue)
        {
            string email = HttpContext.Session.GetString("email");

            if (Tekstialue == null)
            {
                Tekstialue = "Tyhjä tekstikenttä.";
            }
            else if (Tekstialue.Split("\n").Length < 3 || Tekstialue.Split("\n")[0].Length < 20 || !Tekstialue.Split("\n")[0].Contains("\""))
            {
                Tekstialue = "Virheellinen syntaksi tai virheellinen määrä pyydettyjä koodirivejä.";
            }
            else
            {
                var syöterivit = Tekstialue.Split("\n");
                var rivimäärä = syöterivit.Count();
                var syöterivi = syöterivit[0];
                var lukurivi = syöterivit[1].Remove(syöterivit[1].Length - 1);
                //var syötepituus = syöterivi.Length;
                var syöte = syöterivi.Split("\"")[1];
                var lukusyntaksipituus = lukurivi.Length;
                var lukusyntaksiOK = "var syöte = Console.ReadLine();".Length;
                //var vikasyöte = syöterivit[2];
                var odotettu = "Console.WriteLine(\"Tähän konsoliprinttiin on istutettu muuttuja nimeltään:\" + syöte +\".\");";
                var suoritettu = $"Tähän konsoliprinttiin on istutettu muuttuja nimeltään: {syöte}.";
                if (rivimäärä == 3)
                {
                    if (lukusyntaksipituus == lukusyntaksiOK && syöterivit[2] == odotettu)
                    {
                        Tekstialue = suoritettu;

                    }
                    else
                    {
                        Tekstialue = "Virheellinen Console.WriteLine() -tai Console.ReadLine() -syntaksi. Yritä uudelleen!";
                    }
                    //if (email != null)
                    //{
                    //    Suoritus suoritus = new Suoritus() { email = email, tehtavaid = tehtava_id};
                    //    TehtävänLähetys.Tarkista(suoritus);
                    //}
                }
                else
                {
                    Tekstialue = "Virheellinen määrä pyydettyjä koodirivejä.";
                }
            }

            ViewBag.Tekstialue = Tekstialue;


            return View();
        }
        public IActionResult Oppitunti2(string OpViesti = null)
        {
            return View();
        }
        public IActionResult Oppitunti3(string viesti = null, string OpViesti = null)
        {
            ViewBag.Viesti = viesti;
            ViewBag.OpViesti = OpViesti;
            return View();
        }
        public IActionResult RekisteröiKurssi()
        {
            int? id = HttpContext.Session.GetInt32("id");
            var db = _context;
            Kurssi kurssi = db.Kurssis.Where(k => k.KurssiId == 4).First();
            db.Entry(kurssi).Collection(o => o.Oppituntis).Load();
            var oppituntiIDt = kurssi.Oppituntis.Select(o => o.OppituntiId).ToList();

            var oppituntiSuoritukset = _context.OppituntiSuoritus.Where(i => i.KayttajaId == id/* && i.Kesken != false*/).ToList().Select(o => o.OppituntiId).ToList();
            bool oppitunnitSuoritettu = true;
            foreach(var o in oppituntiIDt)
            {
                if (!oppituntiSuoritukset.Contains(o))
                {
                    oppitunnitSuoritettu = false;
                }
            }
            if (id == null || oppitunnitSuoritettu == false)
            {
                return RedirectToAction("Oppitunti3", new { viesti = "Kurssin rekisteröiminen vaatii kirjautumisen ja kaikkien kurssin oppituntien suorittamisen" });
            }
            
            _context.KurssiSuoritus.Add(new KurssiSuoritu() { KayttajaId = id, Kesken = false, KurssiId = 4, SuoritusPvm = DateTime.Today });
            _context.SaveChanges();
            return RedirectToAction("Oppitunti3", new {viesti = "Onneksi olkoon! Kurssi suoritettu onnistuneesti!" });
        }

        public IActionResult RekisteröiOppitunti(int opId)
        {
            int? id = HttpContext.Session.GetInt32("id");
            var db = _context;
            Oppitunti oppitunti = db.Oppituntis.Where(o => o.OppituntiId == opId).First();
            db.Entry(oppitunti).Collection(o => o.Tehtavas).Load();
            var tehtavaIDt = oppitunti.Tehtavas.Select(t => t.TehtavaId).ToList();

            var tehtavaSuoritukset = _context.TehtavaSuoritus.Where(i => i.KayttajaId == id).ToList().Select(t => t.TehtavaId).ToList();
            bool oppituntiSuoritettu = true;
            foreach (var t in tehtavaIDt)
            {
                if (!tehtavaSuoritukset.Contains(t))
                {
                    oppituntiSuoritettu = false;
                }
            }
            if (id == null || oppituntiSuoritettu == false)
            {
                return RedirectToAction($"Oppitunti{opId}" , new { OpViesti = "Oppitunnin rekisteröiminen vaatii kirjautumisen ja kaikkien kurssin tehtävien suorittamisen" });
            }

            _context.OppituntiSuoritus.Add(new OppituntiSuoritu() {KayttajaId = id, OppituntiId = opId, SuoritusPvm = DateTime.Today, /*Kesken = false*/ });
            _context.SaveChanges();
            return RedirectToAction($"Oppitunti{opId}", new { OpViesti = "Onneksi olkoon! Kurssi suoritettu onnistuneesti!" });
        }
    }
    
}
