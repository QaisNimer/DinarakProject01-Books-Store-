using DinarakProject01.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Migrations;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DinarakProject01.Controllers
{
    public class MainPageController : Controller
    {
        readonly DinarakDbContext myDb=new DinarakDbContext();
        // GET: MainPage
        [HttpGet]
        public ActionResult getMainPage()
        {
            return View();
        }
        
        [HttpPost]
        public ActionResult getMainPage(DinarakClass dinClass/*,int?id*/)
        {
            var query = myDb.dinarakClasses.SingleOrDefault(x => x.UserName == dinClass.UserName && x.Password == dinClass.Password );
            if (query != null)
            {
                ViewBag.AdminMessage = string.Format(" Welcome Admin");
                return RedirectToAction("getBooksTable", "Books", new {id=query.Id});
            }
           
            else
            {
                ViewBag.faildLoginMessage = string.Format("You Don't Have Account , Click on Signup And Be Part Of Our Family");
                return View();
            }
        }
        /*MainPage Contoller*/

        [HttpGet]
        public ActionResult getSignUp()
        {
            return View();
        }

        [HttpPost]
        public ActionResult getSignUp(DinarakClass dinarakClass)
        {
            if (ModelState.IsValid)
            {
                var query = myDb.dinarakClasses.SingleOrDefault(x => x.UserName == dinarakClass.UserName && x.Password == dinarakClass.Password);
                if (query!=null)
                {
                    ViewBag.faildSignUpMessage = string.Format(" This Account is Valid ");
                    return View();
                }
                else {
                    myDb.dinarakClasses.Add(dinarakClass);
                    myDb.SaveChanges();
                    return RedirectToAction("getMainPage");
                }

            }
            return RedirectToAction("getMainPage");
        }

        /*SignUp Contoller*/

        // GET: Login
        public ActionResult getLogin(int?id)
        {
            List<DinarakClass> dinClass = new List<DinarakClass>();
            dinClass = (from obj in myDb.dinarakClasses select obj).ToList();
            //return View(dinClass);

            var getRole = (from obj in myDb.dinarakClasses where obj.Id == id select obj).FirstOrDefault();
            if (getRole != null)
            {
                ViewBag.nameRole = getRole.UserName;
                ViewBag.getRole = getRole.Role;
                ViewBag.getId = getRole.Id;

                return View(dinClass);
            }
            else
            {
                return View(dinClass);
            }

        }
        public ActionResult deleteData(int id)
        {
            DinarakClass dinClass = new DinarakClass();
            dinClass = (from obj in myDb.dinarakClasses where obj.Id == id select obj).FirstOrDefault();
            myDb.dinarakClasses.Remove(dinClass);
            myDb.SaveChanges();
            return RedirectToAction("getLogin");
        }
        /*Login Contoller*/


        // GET: Update
        [HttpGet]
        public ActionResult getUpdate()
        {
            return View();
        }
        [HttpPost]
        public ActionResult getUpdate(DinarakClass dinarakClass)
        {
            myDb.dinarakClasses.AddOrUpdate(dinarakClass);
            myDb.SaveChanges();
            return RedirectToAction("getLogin");
        }
        /*Update Contoller*/

    }
}