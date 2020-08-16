using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NameSorter.Models;
using System;

namespace NameSorter.Controllers
{
    /// <summary>
    /// Author: Francis Decena
    /// Date: 16/8/2020
    /// Description: This will handle the generic modal error page of the application
    /// </summary>
    public class ErrorController : Controller
    {
        private readonly ILogger<ErrorController> _logger;

        public ErrorController(ILogger<ErrorController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index(ErrorModel errorModel)
        {
            try
            {
                return View(errorModel);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception error was caught ErrorController:Index {ex.Message}");
                throw ex;
            }   
        }
    }
}
