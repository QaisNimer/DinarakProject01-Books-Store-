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
                return View(books);
            }
            else
            {
                return View(books);
            }
        } //Done  
      
        [HttpGet]
        public ActionResult getCreateBook()
        {
            return View(new BookClass());
        } //Done
        [HttpPost]
        public ActionResult getCreateBook(BookClass book) 
        {
            if (ModelState.IsValid)
            {
                myDb.bookClasses.Add(book);
                myDb.SaveChanges();
                return RedirectToAction("getBooksTable");
            }
            return View();
        }//Done

        public ActionResult getView(int? id)
        {
            BookClass obj = myDb.bookClasses.FirstOrDefault(data => data.SerialNumber == id);
            if (obj == null)
            {
                return View("getCreateBook");
            }
            return View(obj);
        }//Done

        public ActionResult getDelete(int id)
        {
            // Find the book by SerialNumber
            BookClass obj = myDb.bookClasses.FirstOrDefault(item => item.SerialNumber == id);

            // Remove the book from the database regardless of whether it's null or not
            myDb.bookClasses.Remove(obj);
            myDb.SaveChanges();

            // Redirect to the book listing page
            return RedirectToAction("getBooksTable");
        }//Done

        [HttpGet]
        public ActionResult getEditBooks(int? id)
        {
            BookClass obj = new BookClass();
            obj = (from data in myDb.bookClasses where data.SerialNumber == id select data).FirstOrDefault();
            return View(obj);
        }//Done
        [HttpPost]
        public ActionResult getEditBooks(BookClass bks)
        {
            myDb.bookClasses.AddOrUpdate(bks);
            myDb.SaveChanges();
            return RedirectToAction("getBooksTable");
        }//Done

        [HttpGet]
        public ActionResult getBuy(int? id)
        {
            BookClass buy = new BookClass();
            buy = (from obj in myDb.bookClasses where obj.SerialNumber == id select obj).FirstOrDefault();
            return View(buy); 
        }

        [HttpPost]
        public ActionResult getBuy(int numOfBooks)
        {
            BookClass book = myDb.bookClasses.FirstOrDefault();
            if (book != null)
            {
                book.NumOfBooks = numOfBooks;
                myDb.SaveChanges(); 
            }

            return View();
        }

        //[HttpPost]

        //public ActionResult UpdateNumOfBooks(int numOfBooks)

        //{

        //    BookClass book = myDb.bookClasses.FirstOrDefault();

        //    if (book != null)

        //    {

        //        book.NumOfBooks = numOfBooks;

        //        myDb.SaveChanges();

        //    }


        //    return Json(new { success = true });

        }
    }
