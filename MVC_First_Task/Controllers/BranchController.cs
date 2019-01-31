using MVC_First_Task.Data;
using MVC_First_Task.Log;
using MVC_First_Task.Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace MVC_First_Task.Controllers
{
    public class BranchController : Controller
    {
        private const int cSUCCESS = 1;
        private const int cFAIL = 0;
        private const int cEXCEPTION = -1;
        private const int CEXIST = -2;
        DatabaseOperations Operations = new DatabaseOperations();
        public ActionResult Index()
        {
            try
            {
                Operations.Start();
                List<Branch> branches = Operations.getBranches();
                if (branches == null)
                {
                    return View("Error");
                }
                else
                {
                    return View(branches);
                }
            }
            catch (Exception ex)
            {
                clsLogging.ErrorLogging(ex);
                return View("Error");
            }
        }

        public ActionResult Create()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                clsLogging.ErrorLogging(ex);
                return View("Error");
            }
        }

        [HttpPost]
        public ActionResult Create([Bind(Include = "BranchID,Name,Descriptions,ConuntersNumber,Counters")] Branch branch)
        {
            try
            {
                int result = Operations.addBranch(branch);
                if (result == cSUCCESS)
                {
                    return RedirectToAction("Index");
                }
                else if (result == CEXIST)
                {
                    ModelState.AddModelError("", "Branch Name Already Used");
                    return View(branch);
                }
                else
                {
                    return View("Error");
                }
            }
            catch (Exception ex)
            {
                clsLogging.ErrorLogging(ex);
                return View("Error");
            }
        }

        public ActionResult Delete(int id)
        {
            try
            {
                int result = Operations.deleteBranch(id);
                if (result == cSUCCESS)
                    return RedirectToAction("Index");
                else
                    return View("Error");
            }
            catch (Exception ex)
            {
                clsLogging.ErrorLogging(ex);
                return View("Error");
            }
        }

        public ActionResult Details(int id)
        {
            try
            {
                Session["id"] = id;
                Branch branch = Operations.getBranch(id);
                if (branch == null)
                {
                    return View("Error");
                }
                return View(branch);
            }
            catch (Exception ex)
            {
                clsLogging.ErrorLogging(ex);
                return View("Error");
            }
        }

        public ActionResult Edit(int id)
        {
            try
            {
                Branch branch = Operations.getBranch(id);
                if (branch == null)
                {
                    return View("Error");
                }
                return View(branch);
            }
            catch (Exception ex)
            {
                clsLogging.ErrorLogging(ex);
                return View("Error");
            }
        }

        [HttpPost]
        public ActionResult Edit([Bind(Include = "BranchID,Name,Descriptions,ConuntersNumber,Counters")] Branch branch)
        {
            try
            {
                int result = Operations.editBranch(branch);
                if (result == cSUCCESS)
                {
                    return RedirectToAction("Index");
                }
                else if (result == CEXIST)
                {
                    ModelState.AddModelError("", "Branch Name Already Used");
                    return View(branch);
                }
                else if (result == cFAIL)
                {
                    ModelState.AddModelError("", "You Have Already " + branch.Counters.Count + " Counters");
                    return View(branch);
                }
                else
                {
                    return View("Error");
                }
            }
            catch (Exception ex)
            {
                clsLogging.ErrorLogging(ex);
                return View("Error");
            }
        }
    }
}