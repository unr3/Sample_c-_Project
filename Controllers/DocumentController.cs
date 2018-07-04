using Appa_MVC.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Services;

namespace Appa_MVC.Controllers
{
    public class DocumentController : Controller
    {
        ApplicationDbContext _context = new ApplicationDbContext();
        // GET: Document
        public ActionResult Index()
        {
            return View();
        }


        [WebMethod]
        public ActionResult DeliveryNote(Array rfqids)
        { 
            string filename = "";

            GenerateExcel gn = new Models.GenerateExcel();

            filename = gn.NewDeliveryNote(rfqids); 
            

            return new JsonResult()
            {
                Data = new { FileName = filename + ".xlsx" }
            };

        }


        [HttpGet]
        public virtual ActionResult Download(string fileName)
        {
            if (fileName.Length > 0)
            {
                var file = new DirectoryInfo(HostingEnvironment.MapPath(string.Format(@"~/Files/Quotations/{0}", fileName)));

                string filestring = file.ToString();
                string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                return File(filestring, contentType, Path.GetFileName(filestring));
            }
            else
            {
                // Problem - Log the error, generate a blank file,
                //           redirect to another controller action - whatever fits with your application
                return new EmptyResult();
            }
        }

        public JsonResult GetRfqsbyFileId(int FileId)
        {
            var splitstage = Convert.ToByte(StageList.Stages.OnSplitting);
            var files = _context.ReferanceNumbers.Where(r => (r.Stage == splitstage) && r.IsDeleted == false && r.FileId == FileId).ToList();
            var list =
       new List<RfqDetail>();
            foreach (var item in files)
            {
                int itemcount = _context.LineItems.Where(l => l.ReferanceNumberId == item.Id).ToList().Count;

                StageList.Stages STG = (StageList.Stages)item.Stage;

                list.Add(new RfqDetail { RfqName = item.Name, RfqId = item.Id, Count = itemcount, StageId = item.Stage, Stage = STG.ToString() , PoNumber=item.PONumber });
            }
            return Json(list);
        }
    }
}