using System.ComponentModel.DataAnnotations;

namespace NameSorter.Models
{
    public class NamesModel
    {
        [Display(Name = "Name")]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
    }
}
