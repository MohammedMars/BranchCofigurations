using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVC_First_Task.Models
{
    public class Branch
    {
        public int BranchID { get; set; }
        [MaxLength(50, ErrorMessage = "The Branch Name must be less than 51 charachters")]
        [MinLength(3,ErrorMessage = "The Branch Name must be greater than 2 charachters")]
        [Required(ErrorMessage = "The Branch Name field is required.")]
        [RegularExpression(@"^[a-zA-Z][-_\ '.a-zA-Z0-9]{0,50}$", ErrorMessage = "Invalid Branch Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "The Branch Descriptions field is required.")]
        [MaxLength(250, ErrorMessage = "The Branch Descriptions must be less than 251 charachters")]
        [MinLength(5, ErrorMessage = "The Branch Descriptions must be greter than 4 charachters")]
        [RegularExpression(@"^[a-zA-Z][a-zA-Z0-9 !@#$%^%&()-`\'.+,]*$", ErrorMessage = "Invalid Branhc Descriptions")]
        public string Descriptions { get; set; }
        [Range(1,1000, ErrorMessage = "Number of counters must be between  0 and 1000")]
        [Required(ErrorMessage = "The Number Of Counters field is required.")]
        public int ConuntersNumber { get; set; }

        public List<Counter> Counters { get; set; }
    }
}