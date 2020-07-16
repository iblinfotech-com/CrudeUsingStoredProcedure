using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using EmployeeCrud_With_StoreProcedure.Models;
using EmployeeCrud_With_StoreProcedure.Repository;

namespace EmployeeCrud_With_StoreProcedure.Controllers
{
    public class EmployeesController : Controller
    {
        public ActionResult Index()
        {
            EmpRepository EmpRepo = new EmpRepository();
            return View(EmpRepo.GetAllEmployees());
        }

        [HttpPost]
        public ActionResult GetData(string searchByFromdate, string searchByTodate, string fromtime,string totime)
        {
            ////Get parameters
            var start = (Convert.ToInt32(Request["start"]));
            var Length = (Convert.ToInt32(Request["length"])) == 0 ? 10 : (Convert.ToInt32(Request["length"]));
            var searchvalue = Request["search[value]"] ?? "";
            var sortcoloumnIndex = Convert.ToInt32(Request["order[0][column]"]);
            var SortColumn = "";
            var SortOrder = "ASC";
            var sortDirection = Request["order[0][dir]"] ?? "asc";
            var totalRecords = 0;
            var draw = Request.Form.GetValues("draw").FirstOrDefault();  
            int pageSize = Length != null ? Convert.ToInt32(Length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            var pageNo = (start / Length) + 1;
            EmpRepository EmpRepo = new EmpRepository();
            switch (sortcoloumnIndex)
            {
                case 0:
                    SortColumn = "SrNo";
                    break;
                case 1:
                    SortColumn = "Name";
                    break;
                case 2:
                    SortColumn = "City";
                    break;
                case 3:
                    SortColumn = "Salary";
                    break;
                case 4:
                    SortColumn = "Gender";
                    break;
                case 5:
                    SortColumn = "JoinDate";
                    break;
                case 6:
                    SortColumn = "IsActive";
                    break;
                default:
                    SortColumn = "SrNo";
                    break;
            }
            if (sortDirection == "asc")
                SortOrder = "ASC";
            else
                SortOrder = "DESC";


            var data = EmpRepo.ShortingEmployee(searchvalue, pageNo, pageSize, SortColumn, SortOrder, searchByFromdate, searchByTodate,fromtime,totime);
            if (data.Count != 0)
            {
                totalRecords = data[0].TotalCount;
            }


            return Json(new { draw = draw, recordsFiltered = totalRecords, recordsTotal = totalRecords, data = data }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteEmployee(int id)
        {
            string result;
            try
            {
                EmpRepository EmpRepo = new EmpRepository();
                EmpRepo.DeleteEmployee(id);
                result = "Recored Deleted..!!";
            }
            catch (Exception)
            {
                result = "failed";
            }

            return Json(result, JsonRequestBehavior.AllowGet);

        }
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmpRepository EmpRepo = new EmpRepository();
            EmpModel employee = EmpRepo.GetAllEmployees().Find(emp => emp.Empid == id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(EmpModel employee)
        {
            if (ModelState.IsValid)
            {
                EmpRepository EmpRepo = new EmpRepository();

                if (EmpRepo.AddEmployee(employee))
                {
                    ModelState.Clear();
                    ViewBag.Message = "Employee details added successfully";
                }
            }

            return View();
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmpRepository EmpRepo = new EmpRepository();
            EmpModel employee = EmpRepo.GetAllEmployees().Find(emp => emp.Empid == id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EmpModel employee)
        {
            if (ModelState.IsValid)
            {
                EmpRepository EmpRepo = new EmpRepository();

                EmpRepo.UpdateEmployee(employee);
                return RedirectToAction("Index");
            }
            return View(employee);
        }

        public ActionResult Delete(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmpRepository EmpRepo = new EmpRepository();
            if (EmpRepo.DeleteEmployee(id))
            {
                return RedirectToAction("Index");
            }
            return View();
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {

            }
            base.Dispose(disposing);
        }
    }
}
