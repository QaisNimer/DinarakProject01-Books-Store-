using DinarakProject01.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;

namespace DinarakProject01.Controllers
{
    public class BooksController : Controller
    {
        readonly DinarakDbContext myDb = new DinarakDbContext();

        // GET: Books
        public ActionResult getBooksTable(int? id)
        {
            List<BookClass> books = new List<BookClass>();
            books = (from obj in myDb.bookClasses select obj).ToList();
            var getRole= (from obj in myDb.dinarakClasses where obj.Id==id select obj).FirstOrDefault();
            if (getRole != null)
            {
                ViewBag.nameRole = getRole.UserName;
                ViewBag.getRole = getRole.Role;
                ViewBag.getId = getRole.Id;
                return View(books);
            }
            else
            {
                return View(books);
            }
        } //Done  
      
        [HttpGet]
        public ActionResult getCreateBook(int? id)
        {
            var getRole = (from obj in myDb.dinarakClasses where obj.Id == id select obj).FirstOrDefault();
            if (getRole != null)
            {
                ViewBag.nameRole = getRole.UserName;
                ViewBag.getRole = getRole.Role;
                ViewBag.getId = getRole.Id;
                return View(new BookClass());
            }
            else
                return View(new BookClass()); 
        } //Done
        [HttpPost]
        public ActionResult getCreateBook(int?id,BookClass book) 
        {
            var getRole = (from obj in myDb.dinarakClasses where obj.Id == id select obj).FirstOrDefault();
            if (getRole != null)
            {
                if (ModelState.IsValid)
                {
                    myDb.bookClasses.Add(book);
                    myDb.SaveChanges();
                    return RedirectToAction("getBooksTable", new { id = getRole.Id });
                }
            }
            return View();
        }//Done

        public ActionResult getView(int? id, int? id2)
        {
            BookClass obj = myDb.bookClasses.FirstOrDefault(data => data.SerialNumber == id);
            var getRole = (from item in myDb.dinarakClasses where item.Id == id2 select item).FirstOrDefault();
            if (getRole != null)
            {
                ViewBag.nameRole = getRole.UserName;
                ViewBag.getRole = getRole.Role;
                ViewBag.getId = getRole.Id;
            }
            return View(obj);



        }//Done

        public ActionResult getDelete(int? id,int? id2)
        {
            // Find the book by SerialNumber
            BookClass obj = myDb.bookClasses.FirstOrDefault(item => item.SerialNumber == id2);
            var getRole = (from item in myDb.dinarakClasses where item.Id == id select item).FirstOrDefault();
            if (getRole != null)
            {
                ViewBag.nameRole = getRole.UserName;
                ViewBag.getRole = getRole.Role;
                ViewBag.getId = getRole.Id;
                myDb.bookClasses.Remove(obj);
                myDb.SaveChanges();
            }
            return RedirectToAction("getBooksTable", new { id = getRole.Id });
        }//Done

        [HttpGet]
        public ActionResult getEditBooks(int? id, int?id2)
        {
            BookClass obj = new BookClass();
            obj = (from data in myDb.bookClasses where data.SerialNumber == id select data).FirstOrDefault();
            var getRole = (from item in myDb.dinarakClasses where item.Id == id2 select item).FirstOrDefault();
            if (getRole != null)
            {
                ViewBag.nameRole = getRole.UserName;
                ViewBag.getRole = getRole.Role;
                ViewBag.getId = getRole.Id;
                return View(obj);
            }
            else
                return View(obj);



        }//Done
        [HttpPost]
        public ActionResult getEditBooks(int?id2,BookClass bks)
        {
            var getRole = (from obj in myDb.dinarakClasses where obj.Id == id2 select obj).FirstOrDefault();
            if (getRole != null)
            {
                if (ModelState.IsValid)
                {
                    myDb.bookClasses.AddOrUpdate(bks);
                    myDb.SaveChanges();
                    return RedirectToAction("getBooksTable", new { id = getRole.Id });
                }
            }
            return View();
            //return RedirectToAction("getBooksTable");
        }//Done

        [HttpGet]
        public ActionResult getBuy(int? id , int? id2)
        {
            BookClass buy = new BookClass();
            buy = (from obj in myDb.bookClasses where obj.SerialNumber == id select obj).FirstOrDefault();
            var getRole = (from item in myDb.dinarakClasses where item.Id == id2 select item).FirstOrDefault();
            if (getRole != null)
            {
                ViewBag.nameRole = getRole.UserName;
                ViewBag.getRole = getRole.Role;
                ViewBag.getId = getRole.Id;
                return View(buy);
            }
            else
                return View(buy);
        }

        [HttpPost]
        public ActionResult getBuy(int? id2,int numOfBooks)
        {
            BookClass book = myDb.bookClasses.FirstOrDefault();
            if (book != null)
            {
                var getRole = (from obj in myDb.dinarakClasses where obj.Id == id2 select obj).FirstOrDefault();
                if (getRole != null)
                {
                    if (ModelState.IsValid)
                    {
                        book.NumOfBooks -= numOfBooks;
                        myDb.SaveChanges();
                        return RedirectToAction("getBooksTable", new { id = getRole.Id });
                    }
                }
            }
            //return RedirectToAction("getBooksTable");            
            return View();
        }
        }
    }
