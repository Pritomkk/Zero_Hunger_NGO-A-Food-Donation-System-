using Assignment_Zero_Hunger.Annotation;
using Assignment_Zero_Hunger.EF;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Assignment_Zero_Hunger.DTO
{
    public class EmployeeDTO
    {
        public int EmpId { get; set; }
        [ValidaetionName]
        [Required]

        public string Name { get; set; }
        [Required]
        [ValidationEmail]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        
        public int NGOId { get; set; }


       /* public virtual ICollection<FoodDistributionByEmployee> FoodDistributionByEmployees { get; set; */
    }
}