using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NameSorter.Models
{
    /// <summary>
    /// Author: Francis Decena 
    /// Date: 15/8/2020
    /// Description: This is the model for generating error view modal
    /// </summary>
    public class ErrorModel
    {
        public string ErrorTitle { get; set; }
        public string ErrorMessage { get; set; }
        public string RedirectAction { get; set; }
        public string RedirectController { get; set; }
    }
}
