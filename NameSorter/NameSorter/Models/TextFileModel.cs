using Microsoft.AspNetCore.Http;
using NameSorter.Helper;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace NameSorter.Models
{
    /// <summary>
    /// Author: Francis Decena 
    /// Date: 15/8/2020
    /// Description: This is the model for uploading a file. 
    /// </summary>
    public class TextFileModel
    {
        [Required(ErrorMessage = "Please upload a text file")]
        [DataType(DataType.Upload)]
        [ValidateFile(new string[] { ".txt" })]
        [Description("TextFile")]
        public IFormFile TextFile{ get; set; }
        public DateTime DateUnsorted { get; set; }
        public DateTime DateSorted { get; set; }
    }
}
