using System.ComponentModel.DataAnnotations;

namespace Core.Packages.Domain;

public enum ESort
{
    [Display(Name = "OrderBy")]
    ASC = 1,

    [Display(Name = "OrderByDescending")]
    DESC = 2
}