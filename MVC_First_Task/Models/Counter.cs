
using System.ComponentModel.DataAnnotations;

namespace MVC_First_Task.Models
{
    public class Counter
    {
        public int CounterID { get; set; }

        [MaxLength(50, ErrorMessage = "The Counter Name must be less than 51 charachters")]
        [MinLength(3, ErrorMessage = "The Counter Name must be greater than 2 charachters")]
        [Required(ErrorMessage = "The Counter Name field is required.")]
        [RegularExpression(@"^[a-zA-Z][-_\ '.a-zA-Z0-9]{0,50}$", ErrorMessage ="Invalid Counter Name")]
        public string Name { get; set; }
        [Range(1,1000, ErrorMessage = "The Counter Number must be greater than 0")]
        [Required(ErrorMessage = "The Counter Number field is required.")]
        public int Number { get; set; }

        public int BranchID { get; set; }
    }
}