using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Services;
using Appa_MVC.Models;

namespace Appa_MVC.Controllers
{
    public class TechnicalController : Controller
    {
        ApplicationDbContext _context = new ApplicationDbContext();
        // GET: Technical
        public ActionResult Index()
        {
           
            return View();
        }

        public JsonResult GetVessels()
        {
            var vessels = _context.Vessels.ToList();
            var list =
                new List<SelectListItem> {new SelectListItem {Text = "--Select a Vessel--", Value = "0"}};
            foreach (var item in vessels)
            {
                list.Add(new SelectListItem { Text = item.VesselName, Value = item.Id });
            }
            return Json(new SelectList(list, "Value", "Text", JsonRequestBehavior.AllowGet)); 
        }

        public JsonResult GetFilesbyVesselId(string VesselId)
        {
            var files = _context.Files.Where(f => f.VesselId == VesselId).ToList();
            var list =
                new List<SelectListItem> { new SelectListItem { Text = "--Select a File--", Value = "0" } };
            foreach (var item in files)
            {
                list.Add(new SelectListItem { Text = item.FileNumber, Value = item.Id.ToString() });
            }
            return Json(new SelectList(list, "Value", "Text", JsonRequestBehavior.AllowGet));
        }

        [WebMethod]
       
        public ActionResult AddNewRfq(int FileId, string RfqName)
        { 
            var rfq = new ReferanceNumber();
            rfq.FileId = Convert.ToByte(FileId);
            rfq.Name = RfqName;

           _context.ReferanceNumbers.Add(rfq);
            _context.SaveChanges();

            int RfqId = rfq.Id;

            //var files = _context.ReferanceNumbers.Where(f => f.FileId == FileId).ToList();
            //var list =
            //    new List<SelectListItem> { new SelectListItem { Text = "--Select a Rfq--", Value = "0" } };
            //foreach (var item in files)
            //{
            //    list.Add(new SelectListItem { Text = item.Name, Value = item.Id.ToString() });
            //}
            //return Json(new SelectList(list, "Value", "Text", JsonRequestBehavior.AllowGet));

            return RedirectToAction("Index", "TechnicalLineItem", new { id = RfqId });
        }

        public JsonResult GetRfqsbyFileId(int FileId)
        {
            var files = _context.ReferanceNumbers.Where(f => f.FileId == FileId).Where(f=>f.IsDeleted==false).ToList();
            var list =
                new List<SelectListItem> { new SelectListItem { Text = "--Select a Rfq--", Value = "0" } };
            foreach (var item in files)
            {
                list.Add(new SelectListItem { Text = item.Name, Value = item.Id.ToString() });
            }
            return Json(new SelectList(list, "Value", "Text", JsonRequestBehavior.AllowGet));
        }

        public JsonResult GetRfqsDetailbyFileId(int FileId)
        {
            var files = _context.ReferanceNumbers.Where(f => f.FileId == FileId).Where(f => f.IsDeleted == false).ToList();
            var list =
                new List<RfqDetail> ();
            foreach (var item in files)
            {
                int itemcount = _context.LineItems.Where(l => l.ReferanceNumberId == item.Id).ToList().Count;

                StageList.Stages STG = (StageList.Stages)item.Stage;

                list.Add(new RfqDetail { RfqName = item.Name, RfqId = item.Id, Count= itemcount , StageId=item.Stage,Stage= STG.ToString() });
            }
            return Json(list);
        }

        public ActionResult ViewRfq(int? Id)
        {
            return RedirectToAction("Index", "TechnicalLineItem", new { id = Id });
        }



        [HttpGet]
        public ActionResult UpdateFileDetails(int id)
        {
            var file = _context.Files.FirstOrDefault(f => f.Id == id);
            return View(file);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateFileDetails(File file_new)
        {
            try
            {
                var file = _context.Files.FirstOrDefault(f => f.Id == file_new.Id);
                file.Currency = file_new.Currency;
                file.DeliveryCost = file_new.DeliveryCost;
                file.DeliveryPlace = file_new.DeliveryPlace;
                file.Discount = file_new.Discount;
                file.Note1 = file_new.Note1;
                file.Note2 = file_new.Note2;
                file.Note3 = file_new.Note3;
                file.PaymentTerm = file_new.PaymentTerm;
                file.Company = file_new.Company;
                file.Eta = file_new.Eta;
                file.VadePesin = file_new.VadePesin;



                _context.SaveChanges();

                return View(file);
            }
            catch (Exception e)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, e.Message);
            }

        }


        [WebMethod]
        public ActionResult MoveRfqs(Array rfqids, byte stageid)
        {
            int id = 0;
            foreach (var rfqid in rfqids)
            {
                int refid = Convert.ToInt32(rfqid);
                var rfq = _context.ReferanceNumbers.FirstOrDefault(r => r.Id == refid);
                rfq.Stage = stageid;
                id = rfq.Id;
            }
            _context.SaveChanges();

            return Json(Url.Action("Index", "TechnicalLineItem", new { id = id }));

        }


        public ActionResult Reports()
        {
            var rfqs = _context.ReferanceNumbers.Include("File").Where(r => r.Stage < 5 && r.Isactive==false).ToList();

            List<FileMasterDetailsViewModel> list = new List<FileMasterDetailsViewModel>();

            foreach (var rfq in rfqs)
            {
                var vessel = _context.Vessels.FirstOrDefault(v => v.Id == rfq.File.VesselId);
                 
                StageList.Stages stag = (StageList.Stages)rfq.Stage;

                FileMasterDetailsViewModel item = new FileMasterDetailsViewModel
                {
                    File = rfq.File.FileNumber,
                    Rfq = rfq.Name,
                    RFQId = rfq.Id,
                    Stage = stag.ToString()
                };

                if (vessel!=null)
                {
                    item.Vessel = vessel.VesselName;
                }
                list.Add(item);

            }

            
            return View(list);
        }


        public ActionResult DilovasiMizan()
        {
            var kalemler = _context.DilovasiMizan.ToList();
            
            return View(kalemler);
        }

        public ActionResult AntrepoMizan()
        {
            var kalemler = _context.AntrepoMizan.ToList();

            return View(kalemler);
        }

        public ActionResult RemoveFromReport(int? Id)
        {
            var rfq = _context.ReferanceNumbers.FirstOrDefault(r => r.Id == Id);

            rfq.Isactive = true;

            _context.SaveChanges();

            return RedirectToAction("Reports", "Technical" );

        }

    }
}