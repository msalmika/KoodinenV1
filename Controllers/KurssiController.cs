using KoodinenV1.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KoodinenV1.Controllers
{
    public class KurssiController : Controller
    {
        private readonly KoodinenDBContext _context;

        public KurssiController(KoodinenDBContext context)
        {
            _context = context;
        }
     
        public IActionResult Pääsivu()
        {
            int? id = HttpContext.Session.GetInt32("Id");
            string? nimi = HttpContext.Session.GetString("Nimi");
            ViewBag.nimi = nimi;
            return View();
        }
        public IActionResult Oppitunti1()
        {

            return View();
        }
    }




}
