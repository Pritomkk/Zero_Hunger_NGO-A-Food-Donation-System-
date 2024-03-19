using Assignment_Zero_Hunger.DTO;
using Assignment_Zero_Hunger.EF;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Assignment_Zero_Hunger.Controllers
{
    public class EmployeeController : Controller
    {


        [HttpGet]
        public ActionResult EmployeeDash()
        {
            return View();

        }


        [HttpGet]
        public ActionResult EmployeeLogin()
        {
            return View();
        }

        [HttpPost]
        public ActionResult EmployeeLogin(EmployeeLoginDTO LogIn)
        {
            if (ModelState.IsValid)
            {
                var db = new MidprojectEntities4();
                var verify = db.Employees.FirstOrDefault(u => u.Email == LogIn.Email && u.Password == LogIn.Password);

                if (verify != null)
                {
                    Session["id"] = verify.EmpId;
                    Session["user"] = verify.Email;
                    Session["EmpName"]=verify.Name;
                    Session["password"] = verify.Password.Trim();

                    return RedirectToAction("EmployeeDash");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid login credentials.");
                }
            }

            return View(LogIn);
        }




        [HttpGet]
        public ActionResult CollectFood()
        {
            try
            {
                var db = new MidprojectEntities4();
                {
                    var collectRequests = db.CollectRequests.ToList();
                    return View(collectRequests);
                }
            }
            catch 
            {
               
                ViewBag.ErrorMessage = "An error occurred while fetching collect requests. Please try again later.";
                return View("Error"); // Render a custom error view
            }
        }


        [HttpGet]
        public ActionResult CollectedByEmployee(int requestId)
        {
            var db = new MidprojectEntities4();
            var c_request = db.CollectRequests.Find(requestId);

            if (c_request != null)
            {
                c_request.Status = "Collected";

                var collectedFood = new CollectedFoodByEmp();
                int empId = (int)Session["id"];
                collectedFood.EmpId = empId;
                collectedFood.CollectRequest_Id = requestId;
                db.CollectedFoodByEmps.Add(collectedFood);
                db.SaveChanges();

                return RedirectToAction("DistributeCollectedFood");
            }
            else
            {
                TempData["Msg"] = "Invalid Request";
                return RedirectToAction("CollectFood");
            }
        }


        [HttpGet]
        public ActionResult CollectedFoodByEmp()
        {
            int empId = (int)Session["id"];

            var db = new MidprojectEntities4();
            var collectedFood = db.CollectedFoodByEmps.Where(fd => fd.EmpId == empId).ToList();

            ViewBag.CollectedFoodByEmps = collectedFood;
            return View(collectedFood);
        }

        [HttpGet]
        public ActionResult DistributeCollectedFood()
        {
            try
            {
                var db = new MidprojectEntities4();
                var collectedRequests = db.CollectRequests.Where(cr => cr.Status == "Collected").ToList();

                var foodDistribution = db.FoodDistributions.ToList();

                var undistributedRequests = collectedRequests.Where(cr => !foodDistribution.Any(fd => fd.CollectRequest_Id == cr.CollectRequest_Id)).ToList();

                return View(undistributedRequests);
            }
            catch
            {
                ViewBag.ErrorMessage = "An error occurred while fetching collect requests. Please try again later.";
                return View("Error"); 
            }
        }

        [HttpGet]
        public ActionResult FoodDistributionByEmployee(int id, string address)
        {
            using (var db = new MidprojectEntities4())
            {
                var collectRequest = db.CollectRequests.FirstOrDefault(cr => cr.CollectRequest_Id == id);

                if (collectRequest != null)
                {

                    var foodDistribution = new FoodDistribution
                    {
                        CollectRequest_Id = id,
                        Address = address,
                        Status = "Donated",
                        Time = DateTime.Now.ToString()
                    };

                    db.FoodDistributions.Add(foodDistribution);
                    db.SaveChanges();
                    return RedirectToAction("ViewDonateFood");
                }
                else
                {
                    return RedirectToAction("DistributeCollectedFood");
                }
            }
        }


        public ActionResult ViewDonateFood()
        {
            try
            {
                var db = new MidprojectEntities4();
                var donateFoodList = db.FoodDistributions.Where(fd => fd.Status == "Donated").ToList();
                var undistributedRequests = donateFoodList.Where(fd => !db.FoodDistributionByEmployees.Any(fde => fde.Distribution_Id== fd.Distribution_Id)).ToList();

                ViewBag.FoodDistributions = donateFoodList;

                return View(undistributedRequests);
            }
            catch
            {
                ViewBag.ErrorMessage = "An error occurred while fetching donated food distributions. Please try again later.";
                return View("Error");
            }
        }



        [HttpPost]
        public ActionResult CompleteTaskByEmploye(int Distributesid)
        {
            var db = new MidprojectEntities4();

            var Com = new FoodDistributionByEmployee();
            int empId = (int)Session["id"];
            Com.EmpId = empId;
            Com.Distribution_Id = Distributesid;
            db.FoodDistributionByEmployees.Add(Com);
            db.SaveChanges();

            return RedirectToAction("ShowTotalCompleteTask");
        }

        [HttpGet]
        public ActionResult ShowTotalCompleteTask()
        {
            int empId = (int)Session["id"];
            var db = new MidprojectEntities4();
            var AllCompleteTaskByemp = db.FoodDistributionByEmployees.Where(fd => fd.EmpId == empId).ToList();
            ViewBag.allCompleteTask = AllCompleteTaskByemp;
            return View(AllCompleteTaskByemp);
        }



    }
}
