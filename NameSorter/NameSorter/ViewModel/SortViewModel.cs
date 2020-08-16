using Microsoft.AspNetCore.Http;
using NameSorter.Helper;
using NameSorter.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NameSorter.ViewModel
{
    /// <summary>
    /// Author: Francis Decena
    /// Date: 16/8/2020
    /// Description: This will handle the transaction viewmodel for TextFileController
    /// </summary>
    public class SortViewModel
    {
        public List<NamesModel> NamesModels { get; set; }

        [Required(ErrorMessage = "Please upload a text file")]
        [DataType(DataType.Upload)]
        [ValidateFile(new string[] { ".txt" })]
        [Display(Name = "TextFile")]
        public IFormFile TextFile { get; set; }
    }
}
