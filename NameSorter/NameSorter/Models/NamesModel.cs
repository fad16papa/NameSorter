using System.ComponentModel.DataAnnotations;

namespace NameSorter.Models
{
    /// <summary>
    /// Author: Francis Decena 
    /// Date: 16/8/2020
    /// Description: This is the model for generating sort name
    /// </summary>
    public class NamesModel
    {
        [Display(Name = "Name")]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
    }
}
