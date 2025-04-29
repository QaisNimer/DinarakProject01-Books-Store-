using ClosedXML.Excel;
using DinarakProject01.Models;
using ExcelDataReader;
using LinqToExcel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity.Migrations;
using System.Data.Entity.Validation;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Net;
using System.Data.Entity;
using ICSharpCode.SharpZipLib.Zip;
using DocumentFormat.OpenXml.Office2010.Excel;
using System.Runtime.InteropServices;
using DinarakProject01.ViewModel;

namespace DinarakProject01.Controllers
{
    public class MainPageController : Controller
    {
        readonly DinarakDbContext myDb = new DinarakDbContext();
        // GET: MainPage
        [HttpGet]
        public ActionResult getMainPage()
        {
            return View();
        }
        [HttpPost]
        public ActionResult getMainPage(DinarakClass dinClass)
        {
            var query = myDb.dinarakClasses.SingleOrDefault(x => x.UserName == dinClass.UserName && x.Password == dinClass.Password);

            if (query != null)
            {
                return RedirectToAction("getBooksTable", "Books", new { id = query.Id });
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
                if (query != null)
                {
                    ViewBag.faildSignUpMessage = string.Format(" This Account is Valid ");
                    return View();
                }
                else
                {
                    ViewBag.SignUpMessage = "OK";
                    myDb.dinarakClasses.Add(dinarakClass);
                    myDb.SaveChanges();
                    //return RedirectToAction("getMainPage");
                    return View();
                }
            }
            return RedirectToAction("getMainPage");
        }
        /*SignUp Contoller*/

        // GET: Login
        public ActionResult getLogin(int? id , int page = 1, int pageSize = 10)
        {
            List<DinarakClass> dinClass = new List<DinarakClass>();
            dinClass = (from obj in myDb.dinarakClasses select obj).ToList();
            var items = myDb.dinarakClasses.OrderBy(i => i.Id).Skip((page - 1) * pageSize).Take(pageSize).ToList();
            int totalItems = myDb.dinarakClasses.Count();
            var getRole = (from obj in myDb.dinarakClasses where obj.Id == id select obj).FirstOrDefault();
            if (getRole != null)
            {
                var viewModel = new ItemsViewModel
                {
                    Items = items,
                    CurrentPage = page,
                    PageSize = pageSize,
                    TotalItems = totalItems
                };

                ViewBag.nameRole = getRole.UserName;
                ViewBag.getRole = getRole.Role;
                ViewBag.getId = getRole.Id;

                return View(viewModel);
            }
            else
            {
                return View(dinClass);
            }
        }
        public ActionResult deleteData(int? id, int? id2)
        {
            DinarakClass dinClass = new DinarakClass();
            dinClass = (from obj in myDb.dinarakClasses where obj.Id == id select obj).FirstOrDefault();
            var getRole = (from item in myDb.dinarakClasses where item.Id == id2 select item).FirstOrDefault();
            if (getRole != null)
            {
                ViewBag.nameRole = getRole.UserName;
                ViewBag.getRole = getRole.Role;
                ViewBag.getId = getRole.Id;
                myDb.dinarakClasses.Remove(dinClass);
                myDb.SaveChanges();
            }
            return RedirectToAction("getLogin", new { id = getRole.Id });
        }
        /*Login Contoller*/

        // GET: Update
        [HttpGet]
        public ActionResult getUpdate(int? id, int? id2)
        {
            DinarakClass obj = new DinarakClass();
            obj = (from data in myDb.dinarakClasses where data.Id == id select data).FirstOrDefault();
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
        }
        [HttpPost]
        public ActionResult getUpdate(int? id2, DinarakClass dinarakClass)
        {
            var getRole = (from obj in myDb.dinarakClasses where obj.Id == id2 select obj).FirstOrDefault();
            if (getRole != null)
            {
                if (ModelState.IsValid)
                {
                    myDb.dinarakClasses.AddOrUpdate(dinarakClass);
                    myDb.SaveChanges();
                    return RedirectToAction("getLogin", new { id = getRole.Id });
                }
            }
            return View();
        }
        /*Update Contoller*/

        /*Export*/
        [HttpPost]
        public FileResult Export()
        {

            DataTable dt = new DataTable("DinarakClasses");
            dt.Columns.AddRange(new DataColumn[4] { new DataColumn("Id"),
            new DataColumn("UserName"),
            new DataColumn("Password"),
            new DataColumn("Role")});

            var dinInfos = from dinClass in myDb.dinarakClasses select dinClass;

            foreach (var item in dinInfos)
            {
                dt.Rows.Add(item.Id, item.UserName, item.Password, item.Role);
            }
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "DinarakClass.xlsx");
                }
            }
        }

        public List<DinarakClass> ImportExcel(Stream fileStream)
        {
            var myDinList = new List<DinarakClass>();
            fileStream.Position = 0;
            using (var workbook = new XSSFWorkbook(fileStream))
            {
                var sheet = workbook.GetSheetAt(0);
                var rowCount = sheet.LastRowNum;
                for (int i = 1; i <= rowCount; i++)
                {
                    var row = sheet.GetRow(i);
                    if (row == null || row.Cells.All(c => c.CellType == CellType.Blank))
                    {
                        // Skip empty rows
                        continue;
                    }

                    var idCell = row.GetCell(0);
                    var userNameCell = row.GetCell(1);
                    var passwordCell = row.GetCell(2);
                    var roleCell = row.GetCell(3);

                    int id = idCell == null ? 0 : (idCell.CellType == CellType.Numeric ? (int)idCell.NumericCellValue : int.TryParse(idCell.ToString(), out int result) ? result : 0);
                    string userName = userNameCell == null ? string.Empty : (userNameCell.CellType == CellType.String ? userNameCell.StringCellValue : userNameCell.NumericCellValue.ToString());
                    string password = passwordCell == null ? string.Empty : (passwordCell.CellType == CellType.String ? passwordCell.StringCellValue : passwordCell.NumericCellValue.ToString());
                    string role = roleCell == null ? string.Empty : (roleCell.CellType == CellType.String ? roleCell.StringCellValue : roleCell.NumericCellValue.ToString());

                    if (id == 0 || string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(role))
                    {
                        // Skip rows where Id, UserName, Password, or Role are empty or zero
                        continue;
                    }

                    var dinarakClass = new DinarakClass
                    {
                        Id = id,
                        UserName = userName,
                        Password = password,
                        Role = role
                    };
                    myDinList.Add(dinarakClass);
                }
            }
            return myDinList;
        }

        [HttpPost]
        public ActionResult Index(HttpPostedFileBase file, int? id)
        {
            if (file == null || file.ContentLength == 0)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "No file selected");

            try
            {
                using (var stream = new MemoryStream())
                {
                    file.InputStream.CopyTo(stream);
                    var dinClass = ImportExcel(stream);
                    var query = myDb.dinarakClasses.FirstOrDefault(obj => obj.Id == id);
                    if (query != null)
                    {
                        foreach (var item in dinClass)
                        {
                            if (item.Id == query.Id)
                            {
                                myDb.Entry(query).CurrentValues.SetValues(item);
                                myDb.Entry(query).State = EntityState.Modified;
                            }
                            else
                            {
                                myDb.dinarakClasses.AddOrUpdate(item);
                            }
                        }
                        myDb.SaveChanges();
                        return RedirectToAction("getLogin", new { id = query.Id });
                    }
                    else
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.NotFound, "Record not found");
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, ex.Message);
            }

        }
    }
}
