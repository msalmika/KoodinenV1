using KoodinenV1.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KoodinenV1.Controllers
{
    public class OppituntiController : Controller
    {
        private readonly KoodinenDBContext _context;

        public OppituntiController(KoodinenDBContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        public IActionResult Oppitunti1()
        {
            return View();
        }
    }




}
