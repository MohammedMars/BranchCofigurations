using MVC_First_Task.Data;
using MVC_First_Task.Log;
using MVC_First_Task.Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
namespace MVC_First_Task.Controllers
{
    public class CounterController : Controller
    {
        private const int cSUCCESS = 1;
        private const int cFAIL = 0;
        private const int cEXCEPTION = -1;
        private const int CEXIST = -2;
        private const int cNotenough = -3;
        DatabaseOperations Operations = new DatabaseOperations();
        public ActionResult Index()
        {
            try
            {
                var id = (int)Session["id"];
                List<Counter> counters = Operations.getCounters(id);
                if (counters == null)
                {
                    return View("Error");
                }
                return View(counters);
            }catch(Exception ex)
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
            }catch(Exception ex)
            {
                clsLogging.ErrorLogging(ex);
                return View("Error");
            }
        }

        [HttpPost]
        public ActionResult Create([Bind(Include = "CounterID,Name,Number,BranchID")] Counter counter)
        {
            try
            {
                var id = (int)Session["id"];
                counter.BranchID = id;
                int result = Operations.addCounter(counter);
                if (result == cSUCCESS)
                {
                    return RedirectToAction("Index");
                }
                else if (result == cFAIL)
                {
                    ModelState.AddModelError("", "Counter Number Must Be Uniqe In The Same Branch");
                    return View("Create", counter);
                }
                else if (result == cNotenough)
                {
                    ModelState.AddModelError("", "You Have Already The Maximum Number Of Counters");
                    return View(counter);
                }
                else
                {
                    return View(counter);
                }
            }
            catch (Exception ex)
            {
                clsLogging.ErrorLogging(ex);
                return View("Error");
            }

        }

        public ActionResult Delete(int counterId)
        {
            try
            {
                int result = Operations.deleteCounter(counterId);
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

        public ActionResult Edit(int counterId, int branchId)
        {
            try
            {
                Counter counter = Operations.getCounter(counterId);
                if (counter == null)
                {
                    return View("Error");
                }
                return View(counter);
            }
            catch (Exception ex)
            {
                clsLogging.ErrorLogging(ex);
                return View("Error");
            }
        }

        [HttpPost]
        public ActionResult Edit([Bind(Include = "CounterID,Name,Number,BranchID")] Counter counter)
        {
            try
            {
                int result = Operations.editCounter(counter);
                if (result == cSUCCESS)
                {
                    return RedirectToAction("Index");
                }
                else if (result == cFAIL)
                {
                    ModelState.AddModelError("", "Counter Number Must Be Uniqe In The Same Branch");
                    return View(counter);
                }
                else
                {
                    return View();
                }
            }
            catch (Exception ex)
            {
                clsLogging.ErrorLogging(ex);
                return View("Error");
            }
        }

        public ActionResult Details(int counterId, int branchId)
        {
            try
            {
                Counter counter = Operations.getCounter(counterId);
                if (counter == null)
                {
                    return View("Error");
                }
                return View(counter);
            }
            catch (Exception ex)
            {
                clsLogging.ErrorLogging(ex);
                return View("Error");
            }
        }
    }

}