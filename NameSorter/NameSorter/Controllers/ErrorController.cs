using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NameSorter.Models;

namespace NameSorter.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult Index(ErrorModel errorModel)
        {
            return View(errorModel);
        }
    }
}
