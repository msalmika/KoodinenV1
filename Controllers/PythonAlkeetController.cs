using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KoodinenV1.Controllers
{
    public class PythonAlkeetController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
