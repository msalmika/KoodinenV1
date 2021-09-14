using KoodinenV1.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KoodinenV1.Controllers
{
    public class AdminController : Controller
    {KoodinenDBContext _context;

        public AdminController(KoodinenDBContext context)
        {
            _context = context;
        }

        public IActionResult AdminPääsivu()
        {
            int? id = HttpContext.Session.GetInt32("Id");
            return View();
        }
        public IActionResult Oppitunti1()
        {

            return View();
        }
        public IActionResult Oppitunti3()
        {

            //AdminViewModel AVM = new AdminViewModel() { Ohje = _context.Ohjeistus.Where(o => o.OppituntiId == 11).First(), Tehtävät = _context.Tehtavas.Where(t => t.OppituntiId == 11).ToList() };

            return View();
        }
    }




}
