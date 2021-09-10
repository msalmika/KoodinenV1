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
        string URL = "https://funclogickoodinen.azurewebsites.net/api/TehtavaSuoritusTietoKantaan?code=NudohY8CkjsRXFxqHZgBaLKia5Oko0SAmidmBjCEShYaLxY9B3hIZQ==";
        

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
                    Suoritus s = new Suoritus() { email = email, tehtavaid = 10 };
                    var data = JsonConvert.SerializeObject(s);
                    using (var client = new HttpClient())
                    {
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        var content = new StringContent(data, UTF8Encoding.UTF8, "application/json");
                        content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                        var response = client.PostAsync(URL, content).Result;
                        var json = response.Content.ReadAsStringAsync().Result;
                    }
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
    class Suoritus
    {
        public string email { get; set; }
        public int tehtavaid { get; set; }

    }
}
