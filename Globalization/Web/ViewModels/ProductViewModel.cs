using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels;

public class ProductViewModel
{
    [Display(Name = "Name")]
    [Required(ErrorMessage = "You have to supply {0} for this to work.")]
    public string Name { get; set; }
}
