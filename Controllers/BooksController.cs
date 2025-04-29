using DinarakProject01.Models;
using DinarakProject01.ViewModel;
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
        //public ActionResult getBooksTable(int? id, int page = 1, int pageSize = 1)
        //{
        //    List<BookClass> books = new List<BookClass>();
        //    var items = myDb.bookClasses.OrderBy(i => i.SerialNumber).Skip((page - 1) * pageSize).Take(pageSize).ToList();
        //    int totalItems = myDb.bookClasses.Count();
        //    var emptyBooks = (from obj in myDb.bookClasses where obj.NumOfBooks == 0 select obj).FirstOrDefault();
        //    var getRole = (from obj in myDb.dinarakClasses where obj.Id == id select obj).FirstOrDefault();
        //    var itemsQuery = myDb.bookClasses.Where(b => b.NumOfBooks > 0).OrderBy(i => i.SerialNumber);

        //    ItemsViewModel ViewModel;

        //    while (emptyBooks != null)
        //    {
        //        if (getRole.Role == "Admin")
        //        {
        //            books = (from obj in myDb.bookClasses select obj).ToList();
        //            ViewBag.nameRole = getRole.UserName;
        //            ViewBag.getRole = getRole.Role;
        //            ViewBag.getId = getRole.Id;

        //            ViewModel = new ItemsViewModel
        //            {
        //                CurrentPage = page,
        //                PageSize = pageSize,
        //                TotalItems = totalItems,
        //                ItemsBook = items
        //            };

        //            return View(ViewModel);
        //        }
        //        else if(getRole.Role=="User")
        //        {
        //            books = (from obj in myDb.bookClasses where (obj.NumOfBooks > 0) select obj).ToList();
        //            ViewBag.nameRole = getRole.UserName;
        //            ViewBag.getRole = getRole.Role;
        //            ViewBag.getId = getRole.Id;
        //            var sol = totalItems - emptyBooks.NumOfBooks;
        //            ViewModel = new ItemsViewModel
        //            {
        //                CurrentPage = page,
        //                PageSize = pageSize,
        //                TotalItems = totalItems,
        //                ItemsBook = items
        //            };
        //            return View(ViewModel);
        //        }
        //    }

        //    if (getRole != null)
        //    {
        //        ViewBag.nameRole = getRole.UserName;
        //        ViewBag.getRole = getRole.Role;
        //        ViewBag.getId = getRole.Id;

        //        ViewModel = new ItemsViewModel
        //        {
        //            CurrentPage = page,
        //            PageSize = pageSize,
        //            TotalItems = totalItems,
        //            ItemsBook = items
        //        };

        //        return View(ViewModel);
        //    }
        //    else
        //    {
        //        ViewBag.nameRole = getRole.UserName;
        //        ViewBag.getRole = getRole.Role;
        //        ViewBag.getId = getRole.Id;
        //        ViewModel = new ItemsViewModel
        //        {
        //            CurrentPage = page,
        //            PageSize = pageSize,
        //            TotalItems = totalItems,
        //            ItemsBook = items
        //        };

        //        return View(ViewModel);
        //    }
        //}
        public ActionResult getBooksTable(int? id, int page = 1, int pageSize = 1)
        {
            List<BookClass> books = new List<BookClass>();
            var items = myDb.bookClasses.OrderBy(i => i.SerialNumber).Skip((page - 1) * pageSize).Take(pageSize).ToList();
            int totalItems = myDb.bookClasses.Count();
            var emptyBooks = (from obj in myDb.bookClasses where obj.NumOfBooks == 0 select obj).FirstOrDefault();
            var getRole = (from obj in myDb.dinarakClasses where obj.Id == id select obj).FirstOrDefault();

            ItemsViewModel ViewModel;

            if (getRole != null)
            {
                ViewBag.nameRole = getRole.UserName;
                ViewBag.getRole = getRole.Role;
                ViewBag.getId = getRole.Id;

                if (getRole.Role == "Admin")
                {
                    books = (from obj in myDb.bookClasses select obj).ToList();
                }
                else if (getRole.Role == "User")
                {
                    books = (from obj in myDb.bookClasses where obj.NumOfBooks > 0 select obj).ToList();
                    totalItems = books.Count();
                    items = books.OrderBy(i => i.SerialNumber).Skip((page - 1) * pageSize).Take(pageSize).ToList();
                }

                ViewModel = new ItemsViewModel
                {
                    CurrentPage = page,
                    PageSize = pageSize,
                    TotalItems = totalItems,
                    ItemsBook = items
                };

                return View(ViewModel);
            }

            // In case getRole is null, handle accordingly
            // Assuming we return an empty view with default values
            ViewModel = new ItemsViewModel
            {
                CurrentPage = page,
                PageSize = pageSize,
                TotalItems = 0,
                ItemsBook = new List<BookClass>()
            };

            return View(ViewModel);
        }

        //Done  
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

        public ActionResult ShowComment(int? id , int? id2)
        {
            var tempComments = (from obj in myDb.commentsClasses
                                where obj.SerialNumber == id
                                select new
                                {
                                    obj.SerialNumber,
                                    obj.CommentDescription,
                                    obj.CommentId,
                                    obj.CommentedOn,
                                    obj.Rating
                                }).ToList();
            var getBook = (from bk in myDb.bookClasses where bk.SerialNumber == id select bk).FirstOrDefault();

            List<Comments> listOfCommentClass = tempComments.Select(temp => new Comments
            {
                SerialNumber = temp.SerialNumber,
                CommentDescription = temp.CommentDescription,
                CommentId = temp.CommentId,
                CommentedOn = temp.CommentedOn,
                Rating = temp.Rating,
                
            }).ToList();
            ViewBag.bookId = id;
            var getRole = (from obj in myDb.dinarakClasses where obj.Id == id2 select obj).FirstOrDefault();
            if (getRole != null)
            {
                ViewBag.nameRole = getRole.UserName;
                ViewBag.getRole = getRole.Role;
                ViewBag.getId = getRole.Id;
            }
            ViewBag.bookName = getBook.BookName;
           
            return View(listOfCommentClass);
        }

        [HttpPost]
        public ActionResult AddComment(int SerialNumber, int id2Val , int rating ,string bookCommnet)
        {
            Comments obj=new Comments();
            obj.SerialNumber = SerialNumber;
            obj.Rating = rating;
            obj.CommentDescription= bookCommnet;
            obj.CommentedOn = DateTime.Now;
            obj.GuestId = Guid.NewGuid();
            myDb.commentsClasses.Add(obj);
            myDb.SaveChanges();

            return RedirectToAction("ShowComment", new { id = SerialNumber, id2 = id2Val });
        }

        public ActionResult getDelete(int? id, int? id2)
        {
            // Find the book by SerialNumber
            BookClass obj = myDb.bookClasses.FirstOrDefault(item => item.SerialNumber == id);
            var getRole = (from item in myDb.dinarakClasses where item.Id == id2 select item).FirstOrDefault();
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
            if (bks.Price !=0 || bks.NumOfBooks != 0)
            {
                if (getRole != null)
                {
                    if (ModelState.IsValid)
                    {
                        myDb.bookClasses.AddOrUpdate(bks);
                        myDb.SaveChanges();
                        return RedirectToAction("getBooksTable", new { id = getRole.Id });
                    }
                }
            }
            else {
                if (getRole != null)
                {
                    if (ModelState.IsValid)
                    {
                        myDb.bookClasses.AddOrUpdate(bks);
                        myDb.SaveChanges();
                        return RedirectToAction("getBooksTable", new { id = getRole.Id });
                    }
                }
            }
            return View();
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
            return View();
        }
        }
    }
