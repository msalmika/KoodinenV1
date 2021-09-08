using KoodinenV1.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KoodinenV1.Testaus;

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
        public IActionResult Oppitunti1([FromForm] string Tekstialue)
        {
            TestiFunc.TestaaKoodi(Tekstialue);
            
            return View();
        }
        public IActionResult Oppitunti2()
        {
            return View();
        }
    }




}
