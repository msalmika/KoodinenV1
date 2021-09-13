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
    {dbKoodinenContext _context;

        public AdminController(dbKoodinenContext context)
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
    }




}
