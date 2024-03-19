using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Assignment_Zero_Hunger.DTO
{
    public class FoodDistributionDTO
    {
        public int Distribution_Id { get; set; }
        public string Address { get; set; }
        public System.DateTime Time { get; set; }
        public int CollectRequest_Id { get; set; }
        public string Status { get; set; }
    }
}