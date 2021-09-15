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
            ViewBag.id = id;
            ViewBag.Viesti = viesti;
            return View();
        }
        public IActionResult AloitaKurssi()
        {
            int? id = HttpContext.Session.GetInt32("id");
            if (id == null)
            {
                return RedirectToAction("Esittely", new { viesti = "Kurssille rekisteröityminen vaatii sivulle kirjautumisen, " });
            }
            if (_context.KurssiSuoritus.Where(k => k.KayttajaId == id && k.KurssiId == 4).ToList().Count() == 0)
            {
                _context.KurssiSuoritus.Add(new KurssiSuoritu() { KayttajaId = id, Kesken = true, KurssiId = 4, SuoritusPvm = DateTime.Today });
                _context.SaveChanges();
                return RedirectToAction("Esittely", new {viesti = "Ilmoittautuminen onnistui, tervetuloa kurssille!" });
            }
            return RedirectToAction("Esittely", new { viesti = "Olet jo ilmoittautunut tai suorittanut kurssin" });
        }
        public IActionResult Oppitunti1_Teht1(string vihje1 = null, string vihje2 = null, string vihje3 = null)
        {
            ViewBag.vihje1 = vihje1;
            ViewBag.vihje2 = vihje2;
            ViewBag.vihje3 = vihje3;
            int? id = HttpContext.Session.GetInt32("id");
            var suoritettu = _context.TehtavaSuoritus.Where(x => x.KayttajaId == id && x.TehtavaId == 10).FirstOrDefault();
            ViewBag.Suoritettu = suoritettu;
            return View();
        }
        public IActionResult HaeVihje1()
        {
            var vihje1 = (from p in _context.Vihjes
                          where p.TehtavaId == 10
                          select p.Vihje1).FirstOrDefault();
            return RedirectToAction("Oppitunti1_Teht1", new { vihje1 });
        }
        public IActionResult HaeVihje2()
        {
            var vihje2 = (from p in _context.Vihjes
                          where p.TehtavaId == 10
                          select p.Vihje2).FirstOrDefault();
            return RedirectToAction("Oppitunti1_Teht1", new { vihje2 });
        }
        public IActionResult HaeVihje3()
        {
            var vihje3 = (from p in _context.Vihjes
                          where p.TehtavaId == 10
                          select p.Vihje3).FirstOrDefault();
            return RedirectToAction("Oppitunti1_Teht1", new { vihje3 });
        }
        [HttpPost]
        public IActionResult Oppitunti1_Teht1(string Tekstialue)
        {
            string email = HttpContext.Session.GetString("email");
            if (Tekstialue == null)
            {
                return View();
            }
            try
            {
                if (Tekstialue.Length >= 50)
                {
                    Tekstialue = "Virhe";
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

            }
            catch (Exception)
            {

                throw;
            }
            ViewBag.Tekstialue = Tekstialue;

            int? id = HttpContext.Session.GetInt32("id");
            var suoritettu = _context.TehtavaSuoritus.Where(x => x.KayttajaId == id && x.TehtavaId == 10).FirstOrDefault();
            ViewBag.Suoritettu = suoritettu;

            return View();
        }
        public IActionResult Oppitunti1_Teht2()
        {
            int? id = HttpContext.Session.GetInt32("id");
            var suoritettu = _context.TehtavaSuoritus.Where(x => x.KayttajaId == id && x.TehtavaId == 24).FirstOrDefault();
            ViewBag.Suoritettu = suoritettu;
            return View();
        }
        [HttpPost]
        public IActionResult Oppitunti1_Teht2(string Tekstialue)
        {

            string email = HttpContext.Session.GetString("email");
            if (Tekstialue == null)
            {
                return View();
            }
            try
            {
                if (Tekstialue.Split("\n").Length != 3)
                {
                    Tekstialue = "Virhe4";
                }
                else
                {
                    string rivi1 = Tekstialue.Split("\n")[0];
                    string rivi2 = Tekstialue.Split("\n")[1];
                    string rivi3 = Tekstialue.Split("\n")[2];
                    if (rivi1 == null || rivi2 == null || rivi3 == null)
                    {
                        return View();
                    }
                    int x = 0;
                    int y = 0;

                    if (rivi1.StartsWith("int x = ") && rivi1.EndsWith(";\r"))
                    {
                        rivi1 = rivi1.Replace("int x = ", "");
                        rivi1 = rivi1.Replace(";\r", "");
                        Int32.TryParse(rivi1, out x);
                        if (rivi2.StartsWith("int y = ") && rivi2.EndsWith(";\r"))
                        {
                            rivi2 = rivi2.Replace("int y = ", "");
                            rivi2 = rivi2.Replace(";\r", "");
                            Int32.TryParse(rivi2, out y);
                            if (rivi3.Contains("Console.WriteLine(x + y);"))
                            {
                                Tekstialue = Convert.ToString(x + y);

                                if (email != null)
                                {
                                    Suoritus suoritus = new Suoritus() { email = email, tehtavaid = 24 };
                                    TehtävänLähetys.Tarkista(suoritus);
                                }
                            }
                            else Tekstialue = "Virhe3";
                        }
                        else Tekstialue = "Virhe2";
                    }
                    else Tekstialue = "Virhe";
                }
            }
            catch (Exception e)
            {
                throw;
            }

            ViewBag.Tekstialue = Tekstialue;

            int? id = HttpContext.Session.GetInt32("id");
            var suoritettu = _context.TehtavaSuoritus.Where(x => x.KayttajaId == id && x.TehtavaId == 24).FirstOrDefault();
            ViewBag.Suoritettu = suoritettu;
            return View();
        }
        public IActionResult Oppitunti1_Teht3()
        {
            int? id = HttpContext.Session.GetInt32("id");
            var suoritettu = _context.TehtavaSuoritus.Where(x => x.KayttajaId == id && x.TehtavaId == 25).FirstOrDefault();
            ViewBag.Suoritettu = suoritettu;
            return View();
        }
        [HttpPost]
        public IActionResult Oppitunti1_Teht3(/*[FromForm] int tehtava_id,*/ string Tekstialue, string ReadLine = null)
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

                if (ReadLine == null)
                {
                    ViewBag.Tekstialue = Tekstialue;
                    ViewBag.ReadLine = syöte;
                    ViewBag.Valmis = false;
                    return View();
                }

                var lukusyntaksipituus = lukurivi.Length;
                var lukusyntaksiOK = "var syöte = Console.ReadLine();".Length;
                //var vikasyöte = syöterivit[2];
                var odotettu = "Console.WriteLine(\"Tähän konsoliprinttiin on istutettu muuttuja nimeltään:\" + syöte +\".\");";
                var suoritettu = $"Tähän konsoliprinttiin on istutettu muuttuja nimeltään: {ReadLine}.";
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
                    if (email != null)
                    {
                        Suoritus suoritus = new Suoritus() { email = email, tehtavaid = 25 };
                        TehtävänLähetys.Tarkista(suoritus);
                    }
                }
                else
                {
                    Tekstialue = "Virheellinen määrä pyydettyjä koodirivejä.";
                }
            }

            ViewBag.Tekstialue = Tekstialue;

            int? id = HttpContext.Session.GetInt32("id");
            var tehtäväSuoritettu = _context.TehtavaSuoritus.Where(x => x.KayttajaId == id && x.TehtavaId == 25).FirstOrDefault();
            ViewBag.Suoritettu = tehtäväSuoritettu;
            ViewBag.Valmis = true;

            return View();
        }
        public IActionResult Oppitunti2(string OpViesti = null)
        {
            ViewBag.OpViesti = OpViesti;
            return View();
        }
        public IActionResult Oppitunti3(string OpViesti = null)
        {
            int id = HttpContext.Session.GetInt32("id") ?? 0;

            string leipäteksti = _context.Ohjeistus.Where(o => o.OppituntiId == 11).Select(x => x.TekstiKentta).First();
            var tehtävät = _context.Tehtavas.Where(t => t.OppituntiId == 11).ToList();
            List<int> suoritetut = new List<int>();
            if (id != 0)
            {
                foreach (var tehtävä in tehtävät)
                {
                    _context.Entry(tehtävä).Collection(t => t.TehtavaSuoritus).Load();
                    var lkm = tehtävä.TehtavaSuoritus.Where(i => i.KayttajaId == id && i.TehtavaId == tehtävä.TehtavaId).Count();
                    if (lkm != 0)
                    { 
                        suoritetut.Add(tehtävä.TehtavaId); 
                    }
                }
            }
            ViewBag.Suoritetut = suoritetut;
            ViewBag.Tehtävät = tehtävät;
            ViewBag.Ohjeistus = leipäteksti;
            ViewBag.OpViesti = OpViesti;
            return View();
        }
        public IActionResult Oppitunti4(string viesti = null, string OpViesti = null)
        {

            ViewBag.Viesti = viesti;
            ViewBag.OpViesti = OpViesti;
            return View();
        }
        public IActionResult RekisteröiKurssi(int opId, int sivuNro)
        {
            int? id = HttpContext.Session.GetInt32("id");
            if (id == null)
            {
                return RedirectToAction($"Oppitunti{sivuNro}", new { viesti = "Kurssin rekisteröiminen vaatii kirjautumisen ja kaikkien kurssin oppituntien suorittamisen" });
            }
            var db = _context;
            Kurssi kurssi = db.Kurssis.Where(k => k.KurssiId == 4).First();
            db.Entry(kurssi).Collection(o => o.Oppituntis).Load();
            var oppituntiIDt = kurssi.Oppituntis.Select(o => o.OppituntiId).ToList();

            var oppituntiSuoritukset = _context.OppituntiSuoritus.Where(i => i.KayttajaId == id/* && i.Kesken != false*/).ToList().Select(o => o.OppituntiId).ToList();
            bool oppitunnitSuoritettu = true;
            foreach (var o in oppituntiIDt)
            {
                if (!oppituntiSuoritukset.Contains(o))
                {
                    oppitunnitSuoritettu = false;
                }
            }
            if (id == null || oppitunnitSuoritettu == false)
            {
                return RedirectToAction($"Oppitunti{sivuNro}", new { viesti = "Kurssin rekisteröiminen vaatii kirjautumisen ja kaikkien kurssin oppituntien suorittamisen" });
            }

            if (_context.KurssiSuoritus.Where(k => k.KayttajaId == id && k.KurssiId == 4 && k.Kesken == false) == null)
            {
                _context.KurssiSuoritus.Add(new KurssiSuoritu() { KayttajaId = id, Kesken = false, KurssiId = 4, SuoritusPvm = DateTime.Today });
                _context.KurssiSuoritus.Remove(_context.KurssiSuoritus.Where(k => k.KayttajaId == id && k.KurssiId == 4 && k.Kesken == true).FirstOrDefault());
                _context.SaveChanges();
                return RedirectToAction($"Oppitunti{sivuNro}", new { viesti = "Onneksi olkoon! Kurssi suoritettu onnistuneesti!" });
            }
            return RedirectToAction($"Oppitunti{sivuNro}", new { viesti = "Sinulle on jo merkattu kurssisuoritus kyseiseltä kurssilta" });
        }

        public IActionResult RekisteröiOppitunti(int opId, int sivuNro)
        {
            int? id = HttpContext.Session.GetInt32("id");
            if (id == null)
            {
                return RedirectToAction($"Oppitunti{sivuNro}", new { viesti = "Kurssin rekisteröiminen vaatii kirjautumisen ja kaikkien kurssin oppituntien suorittamisen" });
            }
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
                return RedirectToAction($"Oppitunti{sivuNro}", new { OpViesti = "Oppitunnin rekisteröiminen vaatii kirjautumisen ja kaikkien oppitunnin tehtävien suorittamisen" });
            }

            _context.OppituntiSuoritus.Add(new OppituntiSuoritu() { KayttajaId = id, OppituntiId = opId, SuoritusPvm = DateTime.Today, /*Kesken = false*/ });
            _context.SaveChanges();
            return RedirectToAction($"Oppitunti{sivuNro}", new { OpViesti = "Onneksi olkoon! Oppitunti suoritettu onnistuneesti!" });
        }
        public IActionResult Palaute()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Palaute(string Teksti)
        {
            _context.Palautes.Add(new Palaute() { Teksti = Teksti, Pvm = DateTime.Today });
            _context.SaveChanges();
            ViewBag.Viesti = "Kiitos palautteestasi!";
            return View();
        }
        public IActionResult UKK()
        {
            return View();
        }
    }

}
