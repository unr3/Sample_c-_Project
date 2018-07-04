using Appa_MVC.Models;
using Inspinia_MVC5_SeedProject.Email;
using Inspinia_MVC5_SeedProject.Email.Templates;
using Inspinia_MVC5_SeedProject.Models;
using Microsoft.AspNet.Identity;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Services;

namespace Appa_MVC.Controllers
{
    public class SalesController : Controller
    {
        ApplicationDbContext _context = new ApplicationDbContext();
        readonly Emailer _emailer = new Emailer();
        // GET: Sales
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult File(int id)
        {
            var rfqs = _context.ReferanceNumbers.Where(r => (r.Stage == 5 || r.Stage==6) && r.IsDeleted == false && r.FileId == id).ToList();
            var file = _context.Files.FirstOrDefault(f => f.Id == id);
            ViewBag.filename = file.FileNumber;

            SalesFileViewModel model = new SalesFileViewModel
            {
                ReferanceNumbers = rfqs,
                File = file
            };

            return View(model);
        }

 


        public JsonResult GetVessels()
        {
  
            var vessels = _context.Vessels.ToList();
            var list =
                new List<SelectListItem> { new SelectListItem { Text = "--Select a Vessel--", Value = "0" } };
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
        public ActionResult SelectFile(int fileid)
        {
          
            return Json(Url.Action("File", "Sales", new { id = fileid }));

        }


        [WebMethod]
        public ActionResult CreateExcel(Array rfqids)
        {

            string filename = "";

            GenerateExcel gn = new Models.GenerateExcel();

            filename = gn.NewQuotation(rfqids);  

         //   var file = new DirectoryInfo(string.Format(@"D:\Sample\{0}.xlsx", filename));
             

            return new JsonResult()
            {
                Data = new { FileName = filename+".xlsx" }
            };
             
        }

        [HttpGet]
        public virtual ActionResult Download(string fileName)
        {
            if (fileName.Length>0)
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

        [WebMethod]
        public ActionResult RfqsOrdered(Array rfqids)
        {
            try
            {
                if (rfqids.Length > 0)
                {
                    int logid = 0;
                    string rfqs = "";
                    int fileid = 0;
                    foreach (var item in rfqids)
                    {
                        var id = Convert.ToInt32(item.ToString());
                        var rfq = _context.ReferanceNumbers.FirstOrDefault(r => r.Id ==id);
                        rfq.Stage = 6;
                        rfq.IsOrdered = true;
                        fileid = rfq.FileId;
                        rfqs = rfqs+ rfq.Name + ", ";

                        var existingitems = _context.OrderLineItems.Where(o => o.ReferanceNumberId == rfq.Id).ToList();

                        foreach (var existing in existingitems)
                        {
                            _context.OrderLineItems.Remove(existing);
                        }

                        _context.SaveChanges();
                        

                        // clone linteitems to lineitemordered
                        var lineitems = _context.LineItems.Where(l => l.ReferanceNumberId == rfq.Id && l.IsRemoved == false).ToList();

                        foreach (var lineitem in lineitems)
                        {
                            OrderLineItem orderLineItem = new OrderLineItem
                            {
                                Impa = lineitem.Impa,
                                IsAlternative = lineitem.IsAlternative,
                                IsWareHouseSelected = lineitem.IsWareHouseSelected,
                                MasterItemId = lineitem.MasterItemId,
                                MasterNumber = lineitem.MasterNumber,
                                Number = lineitem.Number,
                                Price = lineitem.Price,
                                Qtty = lineitem.Qtty,
                                ReferanceNumber = lineitem.ReferanceNumber,
                                ReferanceNumberId = lineitem.ReferanceNumberId,
                                Remark = lineitem.Remark,
                                SelectedSupplier = lineitem.SelectedSupplier,
                                SelectedSupplierCalculatedPrice = lineitem.SelectedSupplierCalculatedPrice,
                                SelectedSupplierCurrency = lineitem.SelectedSupplierCurrency,
                                SelectedSupplierId = lineitem.SelectedSupplierId,
                                SelectedSupplierName = lineitem.SelectedSupplierName,
                                SelectedSupplierPrice = lineitem.SelectedSupplierPrice,
                                SelectedSupplierRemark = lineitem.SelectedSupplierRemark,
                                Unit = lineitem.Unit,
                                WarehouseInfo = lineitem.WarehouseInfo,
                                AltPrice = lineitem.AltPrice,
                                AltUnit = lineitem.AltUnit,
                                AltQtty=lineitem.AltQtty,
                                Description=lineitem.Description,
                                LineitemId=lineitem.Id,
                                SelectedSupplierTurkishDefinition=lineitem.SelectedSupplierTurkishDefinition
                            };
                            _context.OrderLineItems.Add(orderLineItem);
                             
                        }

                    }
                    _context.SaveChanges();

                    var file = _context.Files.FirstOrDefault(f => f.Id == fileid);
                    var vessel = _context.Vessels.FirstOrDefault(v => v.Id == file.VesselId);

                    if (rfqs.Length > 2)
                    {
                        rfqs = rfqs.Remove(rfqs.Length - 2);
                    }

                    _context.SaveChanges();

                   




                    // sale dep mail gönderme

                    var toadress = new MailAddress("technicalmailgroup@adamarine.com");

                    var emaillog = new EmailLog
                    {
                        Description = "Sale Dpt.",
                        FileId = fileid,
                        Name = "rfqs:" + rfqs,
                        ToAdress = toadress.ToString(),
                        State = "Sending"
                    };
                    _context.EmailLogs.Add(emaillog);
                    _context.SaveChanges();
                    logid = emaillog.Id;


                    string header = string.Format("Rfqs have been Ordered {1} - ( {0} )", file.FileNumber, vessel.VesselName);
                    string body = string.Format("<strong> TECHNICAL TEAM </br> </strong> You could view details for {1} - {0} on the AppA Split Order Page. </br>   </br>  RFQs; {2}  </br>   </br> <b> Note;  {3} </b>", file.FileNumber, vessel.VesselName, rfqs, "");
                    string link = string.Format(@"http://192.168.2.4/TechnicalSplit/File/{0}", file.Id);

                    List<MailAddress> ccadress = new List<MailAddress>
                    {
                       // new MailAddress("technicalmailgroup@adamarine.com")
                    };

                    var temp = new EmailTemplates();
                    var eMailbody =
                        temp.e_mailbody_fiyat_isteme(header, body, link, User.Identity.GetUserName(), "technicalmailgroup@adamarine.com");

                     
                    var t = Task.Run(async () =>
                    {
                        await _emailer.SendEmail(header, eMailbody, toadress, ccadress, logid);
                    });

                    emaillog.State = "Sent";
                    _context.SaveChanges();

                }


                return Json(true);
            }
            catch (Exception e)
            {

                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, e.Message);
            }


        }


        public ActionResult SaleReport()
        {

            var rfqlist = _context.ReferanceNumbers.Where(r => r.IsDeleted == false && r.RemovedFromSaleReport == false).AsEnumerable();

            List<int> fileids = new List<int>();

            foreach (var item in rfqlist)
            {
                fileids.Add(item.FileId);
            }


            var filelist = _context.VFiles.Where(f => fileids.Contains(f.Id)).AsEnumerable();

            SaleReportViewModel viewModel = new SaleReportViewModel
            {
                Files = filelist,
                ReferanceNumbers = rfqlist
            };

             
            return View(viewModel);
        }


        [Authorize(Roles = "SaleManager")]
        public ActionResult removeAndSendMail(int removingid, string fileorRfq, string RemovalReason)
        {

            Vessel vessel = new Vessel();
            string filename = "";
            string rfqnames = "";

            if (fileorRfq=="File")
            {
                var rfqlist = _context.ReferanceNumbers.Include("File").Where(r => r.FileId == removingid);

                foreach (var item in rfqlist)
                {
                    item.RemovedFromSaleReport = true;
                    item.RemovedFromSaleReportReason = RemovalReason;
                    item.RemovedFromSaleReportUser = User.Identity.Name;
                    vessel = _context.Vessels.FirstOrDefault(v => v.Id == item.File.VesselId);
                    filename = item.File.FileNumber;
                    rfqnames = rfqnames + " " + item.Name+" ,";
                }

                _context.SaveChanges();
            }

            if (fileorRfq=="Rfq")
            {
                var rfq = _context.ReferanceNumbers.Include("File").FirstOrDefault(r => r.Id == removingid);

                rfq.RemovedFromSaleReport = true;
                rfq.RemovedFromSaleReportReason = RemovalReason;
                rfq.RemovedFromSaleReportUser = User.Identity.Name;

                vessel = _context.Vessels.FirstOrDefault(v => v.Id == rfq.File.VesselId);
                filename = rfq.File.FileNumber;

                rfqnames = rfq.Name;
                _context.SaveChanges();
            }

            string vesselname = "";
            if (vessel!=null)
            {
                vesselname = vessel.VesselName;
            }
          
         

            // send mail
            // sale dep mail gönderme
            int logid = 0;
            var toadress = new MailAddress("info@adamarine.com");

            var emaillog = new EmailLog
            {
                Description = "Sale Removal.",
                FileId = removingid,
                Name = "removed:" + fileorRfq,
                ToAdress = toadress.ToString(),
                State = "Sending"
            };
            _context.EmailLogs.Add(emaillog);
            _context.SaveChanges();
            logid = emaillog.Id;


            string header = string.Format("{1}-{2} have been Removed From List  - ( {0} )", fileorRfq, vesselname, filename);
            string body = string.Format("<strong> Fallowing Rfq(s)  Have Been Removed From List  </br> Vessel : {0} </br> File: {3} </br> </strong> Removal Reason {1}  </br>   </br>  RFQs; {2}  </br>   </br>", vesselname, RemovalReason, rfqnames,filename);
            string link = string.Format(@"http://192.168.2.4/sales/SaleReport");

            List<MailAddress> ccadress = new List<MailAddress>
            {
                 new MailAddress("ttopkara@adamarine.com"),
                 new MailAddress("technical@adamarine.com"),
                 new MailAddress("sales@adamarine.com") 
            };

            var temp = new EmailTemplates();
            var eMailbody =
                temp.e_mailbody_fiyat_isteme(header, body, link, User.Identity.GetUserName(), "technicalmailgroup@adamarine.com");


            var t = Task.Run(async () =>
            {
                await _emailer.SendEmail(header, eMailbody, toadress, ccadress, logid);
            });

            emaillog.State = "Sent";
            _context.SaveChanges();
            // send mail


            return Json(false);
        }


    }
}