using Assignment_Zero_Hunger.Annotation;
using Assignment_Zero_Hunger.EF;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Assignment_Zero_Hunger.DTO
{
    public class RestruentDonarDTO
    {
        public RestruentDonarDTO()
        {
            this.CollectRequests = new HashSet<CollectRequest>();
        }
        public int RestruentDonorId { get; set; }
        [ValidaetionName]
        [Required]
        public string Name { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string Phone { get; set; }
            
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }

        public virtual ICollection<CollectRequest> CollectRequests { get; set; }

    }
}