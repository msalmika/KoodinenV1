using KoodinenV1.Models;
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
     
        public IActionResult Oppitunti1()
        {
            return View();
        }
    }




}
