using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace ServiceManagment.Data.Enum
{
    public enum OrderStatus
    {
        [Display(Name = "New")]
        New,
        [Display(Name = "In progress")]
        InProgress,
        [Display(Name = "Finished")]
        Finished
    }
}
