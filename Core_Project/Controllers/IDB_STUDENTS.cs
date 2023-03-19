using Core_Project.Data;
using Core_Project.DataBase;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FastReport.Data;
using FastReport.Web;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Reflection;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace Core_Project.Controllers
{
    public class IDB_STUDENTS : Controller
    {

        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IConfiguration _configuration;


        private CoreContext db;
        private IHostingEnvironment HostEnvironment;
        public IDB_STUDENTS(CoreContext _db, IHostingEnvironment _HostEnvironment, SignInManager<IdentityUser> signInManager, IConfiguration configuration)
        {
            db = _db;
            HostEnvironment = _HostEnvironment;
            _signInManager = signInManager;
            _configuration = configuration;

        }
        [Route("Myroute")]
        public IActionResult Index()
        {

            List<SelectListItem> c = new List<SelectListItem>();

            List<IDB_COURS> ic = (from d in db.IDB_COURS select d).ToList();
            foreach (var s in ic)
            {
                c.Add(new SelectListItem { Value = s.SUB_ID.ToString(), Text = s.SUB_ID.ToString() });
            }

            ViewBag.idb_course = c;

            ViewBag.Parentid = GenerateCode();

            if (_signInManager.IsSignedIn(User)) //verify if it's logged
            {
                return View();// LocalRedirect("~/Page");
            }
            else
                return LocalRedirect("~/");

           // return View();
        }


        public JsonResult GetCF(string id)
        {
            var a = (from d in db.CF where d.STUDENT_ID == id select new { d.STUDENT_ID, d.BATCH_ID, });
            return Json(a);
        }
        public JsonResult GetCORE_STUDENTS(string id)
        {
            var a = (from d in db.CORE_STUDENTS where d.STUDENT_ID == id select new { d.SL_NO, d.NAME,  d.SUB_ID, d.IDB_SUBJECT, d.ACTIVE, d.CORE_DATE, d.PICTURE, });
            return Json(a);
        }

        public record CF2
        {
            //primary/master table
            public string STUDENT_ID { get; set; }
            public string BATCH_ID { get; set; }
            //child/details table
            public CORE_STUDENTS[] CORE_STUDENTS { get; set; }
        }
        [HttpPost]
        public JsonResult AddMasterDetails([FromBody] CF2 b)
        {
            using var transaction = db.Database.BeginTransaction();
            try
            {
                CF a = new CF() { STUDENT_ID = b.STUDENT_ID, BATCH_ID = b.BATCH_ID };
                db.CF.Add(a);

                foreach (var c in b.CORE_STUDENTS)
                {
                    CORE_STUDENTS a1 = new CORE_STUDENTS() { SL_NO = c.SL_NO, STUDENT_ID = b.STUDENT_ID, NAME = c.NAME, SUB_ID = c.SUB_ID, IDB_SUBJECT = c.IDB_SUBJECT, ACTIVE = c.ACTIVE, CORE_DATE = DateTime.Parse(c.CORE_DATE.ToShortDateString()), PICTURE = c.PICTURE };
                    db.CORE_STUDENTS.Add(a1);
                }
                db.SaveChanges();
                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return Json("Error: No record added");
                // Other steps for handling failures
            }
            return Json(b);
        }
        public JsonResult DeleteAll(string id)
        {
            using var transaction = db.Database.BeginTransaction();
            try
            {
                List<CORE_STUDENTS> st5 = db.CORE_STUDENTS.Where(xx => xx.STUDENT_ID == id).ToList();
            db.CORE_STUDENTS.RemoveRange(st5);

            CF st6 = db.CF.Find(id);
            if (st6 != null)
            {
                db.CF.Remove(st6);
            }
            db.SaveChanges();
                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return Json("Error: No record added");
                // Other steps for handling failures
            }
            return Json("OK");
        }
        [HttpPost]
        public ActionResult UploadFiles()
        {
            if (Request.Form.Files.Count > 0)
            {
                try
                {
                    var files = Request.Form.Files;
                    for (int i = 0; i < files.Count; i++)
                    {
                        IFormFile file = files[i];
                        string fname;

                        fname = file.FileName;
                        string webRootPath = HostEnvironment.WebRootPath;
                        string fname1 = Path.Combine(webRootPath, "Uploads/" + fname);
                        file.CopyTo(new FileStream(fname1, FileMode.Create));
                    }
                    return Json("File Uploaded Successfully!");
                }
                catch (Exception ex)
                {
                    return Json("Error occurred. Error details: " + ex.Message);
                }
            }
            else
            {
                return Json("No files selected.");
            }
        }
        public string GenerateCode()
        {
            string a1 = "";
            string b1 = "";

            try
            {
                var a = (from det in db.CF select det.STUDENT_ID.Substring(3)).ToList().Max();
                if (a == null)
                    a = "0";
                int b = int.Parse(a.ToString()) + 1;
                if (b < 10)
                {
                    b1 = "000" + b.ToString();
                }
                else if (b < 100)
                {
                    b1 = "00" + b.ToString();
                }
                else if (b < 1000)
                {
                    b1 = "0" + b.ToString();
                }
                else
                {
                    b1 = b.ToString();
                }
                a1 = "ID-" + b1.ToString();
            }
            catch (Exception)
            {
                a1 = "ID-0001";
            }
            return a1;
        }
        public JsonResult GetItem2(string id)
        {
            var a = (from p in db.IDB_COURS where p.SUB_ID == id select new { p.IDB_SUBJECT }).FirstOrDefault();
            return Json(a);
        }
        public string Core_Students_Exists(string id)
        {
            var a = (from p in db.CORE_STUDENTS where p.SL_NO == id select new { p.SL_NO, }).FirstOrDefault();
            if (a != null)
                return "1";
            else
                return "0";
        }



        public IActionResult Report()
        {
            if (!_signInManager.IsSignedIn(User)) //verify if it's logged
            {
                return LocalRedirect("~/");
            }
            var webReport = new WebReport();
            var mssqlDataConnection = new MsSqlDataConnection();
            mssqlDataConnection.ConnectionString = _configuration.GetConnectionString("CoreContext");
            webReport.Report.Dictionary.Connections.Add(mssqlDataConnection);
            webReport.Report.Load(Path.Combine(HostEnvironment.ContentRootPath,"Report","Report1.frx"));
            var CF = GetTable<CF>(db.CF, "mechanic");
            webReport.Report.RegisterData(CF, "mechanic");
            return View(webReport);
        }

        static DataTable GetTable<TEntity>(IEnumerable<TEntity> table, string name) where TEntity : class
        {
            var offset = 78;
            DataTable result = new DataTable(name);
            PropertyInfo[] infos = typeof(TEntity).GetProperties();
            foreach (PropertyInfo info in infos)
            {
                if (info.PropertyType.IsGenericType
                && info.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    result.Columns.Add(new DataColumn(info.Name, Nullable.GetUnderlyingType(info.PropertyType)));
                }
                else
                {
                    result.Columns.Add(new DataColumn(info.Name, info.PropertyType));
                }
            }
            foreach (var el in table)
            {
                DataRow row = result.NewRow();
                foreach (PropertyInfo info in infos)
                {
                    if (info.PropertyType.IsGenericType &&
                    info.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        object t = info.GetValue(el);
                        if (t == null)
                        {
                            t = Activator.CreateInstance(Nullable.GetUnderlyingType(info.PropertyType));
                        }

                        row[info.Name] = t;
                    }
                    else
                    {
                        if (info.PropertyType == typeof(byte[]))
                        {
                            //Fix for Image issue.
                            var imageData = (byte[])info.GetValue(el);
                            var bytes = new byte[imageData.Length - offset];
                            Array.Copy(imageData, offset, bytes, 0, bytes.Length);
                            row[info.Name] = bytes;
                        }
                        else
                        {
                            row[info.Name] = info.GetValue(el);
                        }
                    }
                }
                result.Rows.Add(row);
            }

            return result;
        }


    }
}
