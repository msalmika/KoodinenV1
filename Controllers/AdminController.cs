﻿using KoodinenV1.Models;
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
            //var oppitunti = _context.Ohjeistus.Where
            return View();
        }
    }




}
