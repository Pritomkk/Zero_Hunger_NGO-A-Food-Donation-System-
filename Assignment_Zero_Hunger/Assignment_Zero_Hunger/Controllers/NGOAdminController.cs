using Assignment_Zero_Hunger.DTO;
using Assignment_Zero_Hunger.EF;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Xml.Linq;

namespace Assignment_Zero_Hunger.Controllers
{
    public class NGOAdminController : Controller
    {
        [HttpGet]
        public ActionResult NGOAdminLogin()
        {
            return View();
        }

        [HttpPost]
        public ActionResult NGOAdminLogin(NGOAdminLoginDTO LogIn)
        {
            if (ModelState.IsValid)
            {
                var db = new MidprojectEntities4();
                try
                {
                    var verify = db.NGOAdmins.FirstOrDefault(u => u.Email == LogIn.Email && u.Password == LogIn.Password);

                    if (verify != null)
                    {
                        Session["id"] = verify.Id;
                        Session["Name"] = verify.Username;
                        Session["Email"] = verify.Email;
                        Session["password"] = verify.Password.Trim();
                        Session["type"] = "NGOAdmin";

                        return RedirectToAction("NGOAdminDash");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Invalid login credentials.");
                    }
                }
                catch (SqlException)
                {
                    ViewBag.ErrorMessage = "An error occurred while accessing the database. Please try again later.";
                    return View("Error");
                }
            }

            return View(LogIn);
        }

        [HttpGet]
        public ActionResult NGOAdminDash()
        {
            return View();
        }

        [HttpGet]
        public ActionResult NGOAdminAddEmployee()
        {
            return View();
        }

        [HttpPost]
        public ActionResult NGOAdminAddEmployee(EmployeeDTO E)
        {
            if (ModelState.IsValid)
            {
                var db = new MidprojectEntities4();
                db.Employees.Add(Convert(E));
                db.SaveChanges();

                TempData["Message1"] = "Employee Assign successfully";
                return RedirectToAction("NGOAdminDash");
            }

            return View(E);
        }


        public static List<EmployeeDTO> Convert(List<Employee> data)
        {
            var list = new List<EmployeeDTO>();
            foreach (var item in data)
            {
                list.Add(Convert(item));
            }
            return list;
        }
        public static EmployeeDTO Convert(Employee e)
        {
            return new EmployeeDTO()
            {
                EmpId = e.EmpId,
                Email = e.Email,
                Password = e.Password,
                Name = e.Name,
                NGOId = e.NGOId,
                

        };
        }
        public static Employee Convert(EmployeeDTO e)
        {
            return new Employee()
            {
               EmpId=e.EmpId,
               Email=e.Email,
               Password=e.Password,
               Name=e.Name,
               NGOId=e.NGOId,
            };
        }

        [HttpGet]
        public ActionResult ViewCollectedFood()
        {
            try
            {
                var db = new MidprojectEntities4();
                var collectedRequests = db.CollectRequests.Where(cr => cr.Status == "Collected").ToList();
                return View(collectedRequests);
            }
            catch
            {
                ViewBag.ErrorMessage = "An error occurred while fetching collect requests. Please try again later.";
                return View("Error");
            }

        }

        [HttpGet]
        public ActionResult ViewPendingFood()
        {
            try
            {
                var db = new MidprojectEntities4();
                var collectedRequests = db.CollectRequests.Where(cr => cr.Status == "Pending").ToList();
                return View(collectedRequests);
            }
            catch
            {
                ViewBag.ErrorMessage = "An error occurred while fetching collect requests. Please try again later.";
                return View("Error");
            }

        }


        [HttpGet]
        public ActionResult ViewDistriButedFood()
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


        [HttpGet]
        public ActionResult ViewDistributionFoodByEmploye()
        {
            try
            {
                var db = new MidprojectEntities4();
                var empCompleteTask = db.FoodDistributionByEmployees.ToList();
                return View(empCompleteTask);
            }
            catch
            {
                ViewBag.ErrorMessage = "An error occurred while fetching collect requests. Please try again later.";
                return View("Error");
            }

        }
        [HttpGet]
        public ActionResult EmplyeeList()
        {

            try
            {
                var db = new MidprojectEntities4();
                var emp= db.Employees.ToList();
                return View(emp);
            }
            catch
            {
                ViewBag.ErrorMessage = "An error occurred while fetching collect requests. Please try again later.";
                return View("Error");
            }
        }



    }
}
