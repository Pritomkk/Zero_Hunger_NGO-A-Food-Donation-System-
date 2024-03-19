using Assignment_Zero_Hunger.EF;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Assignment_Zero_Hunger.DTO
{
    public class R_DonarCollectRequestDTO
    {
        public int CollectRequest_Id { get; set; }
        [Required]
        public string FoodDscription { get; set; }
        [Required]
        public string FoodType { get; set; }
        [Required]
        public string Quantity { get; set; }
        [Required]
        public string Expired_Time { get; set; }
        [Required]
        public string Address { get; set; }
        public int RestruentDonorId { get; set; }

        public string Status { get; set; }





    }

}
