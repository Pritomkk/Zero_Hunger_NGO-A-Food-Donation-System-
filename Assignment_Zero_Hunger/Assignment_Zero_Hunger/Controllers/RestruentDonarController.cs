using Assignment_Zero_Hunger.DTO;
using Assignment_Zero_Hunger.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Assignment_Zero_Hunger.Controllers
{
    public class RestruentDonarController : Controller
    {
        [HttpGet]
        public ActionResult RestruentDonarDash()
        {
            return View();
        }
        [HttpGet]
        public ActionResult RestruentDonarLogin()
        {
            return View();
        }
        [HttpPost]
        public ActionResult RestruentDonarLogin(RestruentDonarLoginDTO LogIn)
        {
            // Check if the database connection is available
            if (IsDatabaseConnected())
            {
                // Proceed with login process if database connection is available
                if (ModelState.IsValid)
                {
                    using (var db = new MidprojectEntities4())
                    {
                        var verify = db.RestruentDonors.FirstOrDefault(u => u.Email == LogIn.Email && u.Password == LogIn.Password);

                        if (verify != null)
                        {
                            Session["id"] = verify.RestruentDonorId;
                            Session["user"] = verify.Email;
                            Session["password"] = verify.Password.Trim();
                            Session["type"] = "RestruentDonar";

                            return RedirectToAction("RestruentDonarDash", "RestruentDonar");
                        }
                        else
                        {
                            ModelState.AddModelError("", "Invalid login credentials.");
                        }
                    }
                }
            }
            else
            {
                // Set a custom error message for the user if database connection fails
                ViewBag.ErrorMessage = "Database connection is not available. Please try again later.";
                return View("Error"); // Render a custom error view
            }

            return View(LogIn);
        }

        // Helper method to check if the database connection is available
        private bool IsDatabaseConnected()
        {
            using (var db = new MidprojectEntities4())
            {
                try
                {
                    db.Database.Connection.Open();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }
        [HttpGet]
        public ActionResult R_DonarCollectRequest()
        {

            return View();

        }


        [HttpPost]
        public ActionResult R_DonarCollectRequest(R_DonarCollectRequestDTO R)
        {
            if (ModelState.IsValid)
            {
                var db = new MidprojectEntities4();
                db.CollectRequests.Add(Convert(R));
                db.SaveChanges();
                TempData["Message2"] = "Food Donated SucessFully";
                return RedirectToAction("RestruentDonarDash");
            }
            return View(R);
        }

        public static List<R_DonarCollectRequestDTO> Convert(List<CollectRequest> data)
        {
            var list = new List<R_DonarCollectRequestDTO>();
            foreach (var item in data)
            {
                list.Add(Convert(item));
            }
            return list;
        }
        public static R_DonarCollectRequestDTO Convert(CollectRequest e)
        {
            return new R_DonarCollectRequestDTO()
            {
                CollectRequest_Id = e.CollectRequest_Id,
                FoodType = e.FoodType,
                RestruentDonorId = e.RestruentDonorId,
                FoodDscription =e.FoodDscription,
                Address = e.Address,
                Expired_Time = e.Expired_Time,
                Status = e.Status,
                Quantity = e.Quantity,
            
                


            };
        }
        public static CollectRequest Convert(R_DonarCollectRequestDTO e)
        {
            return new CollectRequest()
            {
                CollectRequest_Id = e.CollectRequest_Id,
                FoodType = e.FoodType,
                FoodDscription = e.FoodDscription,
                Address = e.Address,
                Expired_Time = e.Expired_Time,
                Status = e.Status,
                Quantity = e.Quantity,
                RestruentDonorId = e.RestruentDonorId,
            };
        }


        public ActionResult ViewDonationStatus()
        {
            var db = new MidprojectEntities4();
            int RestruentDonorId = (int)Session["id"]; 
            var foodDonations = db.CollectRequests.Where(c => c.RestruentDonorId == RestruentDonorId).ToList();

            return View(foodDonations);
        }





        [HttpGet]
        public ActionResult RestruentDonarSignUp()
        {
            return View();
        }
        [HttpPost]
        public ActionResult RestruentDonarSignUp(RestruentDonarDTO obj)
        {
            if (ModelState.IsValid)
            {

              var db = new MidprojectEntities4();
                var restruentDonor = new RestruentDonor();
                restruentDonor.Name = obj.Name;
                restruentDonor.Address = obj.Address;
                restruentDonor.Phone = obj.Phone;
                restruentDonor.Email = obj.Email;
                restruentDonor.Password = obj.Password;

                db.RestruentDonors.Add(restruentDonor);
                db.SaveChanges();

                TempData["Message"] = "RestruentDonor SignUp successfully";
                return RedirectToAction("RestruentDonarLogin");
            }
            return View(obj);
        }


        [HttpGet]
        public ActionResult ViewDistributionStatus()
        {
            try
            {
                var db = new MidprojectEntities4();
                var d_Food = db.FoodDistributions.Where(cr => cr.Status == "Donated").ToList();
                return View(d_Food);
            }
            catch
            {
                ViewBag.ErrorMessage = "An error occurred while fetching collect requests. Please try again later.";
                return View("Error");
            }

        }


    }
}