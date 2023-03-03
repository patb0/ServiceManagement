using System.ComponentModel.DataAnnotations;

namespace ServiceManagment.Data.Enum
{
    public enum OrderStatus
    {
        New,
        [Display(Name = "In progress")]
        InProgress,
        Finished
    }
}
