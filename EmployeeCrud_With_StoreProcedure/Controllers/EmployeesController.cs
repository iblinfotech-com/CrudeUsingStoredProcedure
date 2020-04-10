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
                return RedirectToAction("Index");
            }

            return View(employee);
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
