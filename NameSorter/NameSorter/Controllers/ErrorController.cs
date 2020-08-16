using Microsoft.AspNetCore.Mvc;
using NameSorter.Models;

namespace NameSorter.Controllers
{
    /// <summary>
    /// Author: Francis Decena
    /// Date: 16/8/2020
    /// Description: This will handle the Error Page of the application
    /// </summary>
    public class ErrorController : Controller
    {
        public IActionResult Index(ErrorModel errorModel)
        {
            return View(errorModel);
        }
    }
}
