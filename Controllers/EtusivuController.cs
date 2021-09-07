using KoodinenV1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace KoodinenV1.Controllers
{
    public class EtusivuController : Controller
    {
        private readonly ILogger<EtusivuController> _logger;

        private readonly KoodinenDBContext _context;


        private readonly IConfiguration _configuration;
        //
        
        public EtusivuController(ILogger<EtusivuController> logger, KoodinenDBContext context, IConfiguration configuration)
        {
            _logger = logger;
            _context = context;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Kirjautuminen()
        {
            return View();
        }
        
        public IActionResult Chat()
        {
            return View();
        }

        public IActionResult Kurssit()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
