using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Appa_MVC.Models;

namespace Appa_MVC.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewData["SubTitle"] = "Welcome in Appa ";
            ViewData["Message"] = "Please Select Your Department on the left";

           // File file = new File();
           //// file.FileNumber = "ADMTEST0002";

           // ApplicationDbContext _context = new ApplicationDbContext();
           // file=  _context.Files.FirstOrDefault(c => c.FileNumber == "ADMTEST0002");

           // ReferanceNumber referanceNumber=new ReferanceNumber();
           // referanceNumber.File = file;
           // referanceNumber.Name = "test Rfq 01";
           // referanceNumber.DueDate = DateTime.Now.AddDays(5);

           // _context.ReferanceNumbers.Add(referanceNumber);
           // _context.SaveChanges();



            return View();
        }

        public ActionResult Minor()
        {
            ViewData["SubTitle"] = "Simple example of second view";
            ViewData["Message"] = "Data are passing to view by ViewData from controller";

            return View();
        }
    }
}