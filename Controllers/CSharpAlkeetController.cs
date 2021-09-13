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
     
        public IActionResult Esittely()
        {
            int? id = HttpContext.Session.GetInt32("Id");
            return View();
        }
        public IActionResult Oppitunti1()
        {

            return View();
        }
        
        [HttpPost]
        public IActionResult Oppitunti1(string Tekstialue)
        {
            string email = HttpContext.Session.GetString("email");

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
