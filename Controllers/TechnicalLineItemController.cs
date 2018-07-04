using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Services;
using System.Xml;
using System.Xml.Linq;
using Appa_MVC.Models;
using ExcelDataReader;
using Inspinia_MVC5_SeedProject.Email;
using Inspinia_MVC5_SeedProject.Email.Templates;
using Inspinia_MVC5_SeedProject.Models;
using Microsoft.AspNet.Identity;
using OfficeOpenXml;

namespace Appa_MVC.Controllers
{
    public class TechnicalLineItemController : Controller
    {
        readonly ApplicationDbContext _context = new ApplicationDbContext();
        readonly Emailer _emailer= new Emailer();
        // GET: TechnicalLineItem
        public ActionResult Index(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest,"Id is required");
            }

            try
            {
             


                ViewBag.id = id;

                var rfq = _context.ReferanceNumbers.FirstOrDefault(x => x.Id == id);

         
                ViewBag.RfqName = rfq.Name;

                var fileDetail = _context.Files.FirstOrDefault(x => x.Id == rfq.FileId);
                ViewBag.FileName = fileDetail.FileNumber;

                switch (rfq.Stage)
                {
                    case (byte)StageList.Stages.OnApproval:
                        return RedirectToAction("Approve", new { id = fileDetail.Id });
                    case (byte)StageList.Stages.Pricing:
                        return RedirectToAction("Index", "TechnicalPricing", new { id = fileDetail.Id });
                    case (byte)StageList.Stages.OnSaleDepartment:
                        return RedirectToAction("File", "Sales", new { id = fileDetail.Id });
                    case (byte)StageList.Stages.OnSplitting:
                        return RedirectToAction("Index", "TechnicalSplit", new { id = fileDetail.Id });
                }
 
                 
                int stage = rfq.Stage;
                if (stage == 0)
                {
                    rfq.Stage = 1;
                    _context.SaveChanges();
                    stage = 1;
                }
                ViewBag.Stage = stage;

                

                var vessel = _context.Vessels.FirstOrDefault(x => x.Id == fileDetail.VesselId);
                ViewBag.VesselName = vessel.VesselName;

                var lineItems = _context.LineItems.Include("LineItemSuppliers").Where(x => x.ReferanceNumberId == id).ToList();

                return View(lineItems);
            }
            catch (Exception e)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, e.Message);
            }

             
        }


        public ActionResult Suppliers(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var lineitem = _context.LineItems.FirstOrDefault(x => x.Id == id);

            if (lineitem != null)
            {
                ViewBag.RfqId = lineitem.ReferanceNumberId;

                return View(lineitem);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }


        public ActionResult RemoveRfq(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }


            var rfq = _context.ReferanceNumbers.FirstOrDefault(x => x.Id == id);
             
             
            return View(rfq);
            
            
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Suppliers(LineItemSupplier supplier)
        {
            if (ModelState.IsValid && supplier.SupplierId!=null)
            {
                var lineitemsuppliers = _context.LineItemSuppliers.Where(x => x.LineItemId == supplier.LineItemId)
                    .Where(x => x.SupplierId == supplier.SupplierId).ToList();
                if (lineitemsuppliers.Count==0)
                {
                    _context.LineItemSuppliers.Add(supplier);
                    _context.SaveChanges();
                }
                
                return RedirectToAction("Suppliers", new { id = supplier.LineItemId });
            }

            var lineitem = _context.LineItems.FirstOrDefault(x => x.Id == supplier.LineItemId);
           
            return View(lineitem);
        }

        public ActionResult DeleteSupplier(int id, int itemid)
        {
            
            var supplier = _context.LineItemSuppliers.FirstOrDefault(x => x.Id == id);

            if (supplier != null)
            {
                _context.LineItemSuppliers.Remove(supplier);
                _context.SaveChanges();
                return RedirectToAction("Suppliers", new { id = itemid });
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var lineitem = _context.LineItems.FirstOrDefault(x => x.Id == id);

            if (lineitem != null)
            {
                ViewBag.RfqId = lineitem.ReferanceNumberId;

                return View(lineitem);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(LineItem lineItem)
        {
            if (ModelState.IsValid)
            {
                var EditedItem = _context.LineItems.First(x => x.Id == lineItem.Id);
                EditedItem.Impa = lineItem.Impa;
                EditedItem.Description = lineItem.Description;
                EditedItem.Number = lineItem.Number;
                EditedItem.Qtty = lineItem.Qtty;
                EditedItem.Unit = lineItem.Unit; 

                _context.SaveChanges();
                return RedirectToAction("Index", new { id = lineItem.ReferanceNumberId });
            }

            ViewBag.RfqId = lineItem.ReferanceNumberId;
            return View(lineItem);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var lineitem = _context.LineItems.FirstOrDefault(x => x.Id == id);

            if (lineitem != null)
            {
                ViewBag.RfqId = lineitem.ReferanceNumberId;

                return View(lineitem);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(LineItem lineItem)
        {
             
                var tobeDeletedItem = _context.LineItems.Find(lineItem.Id);
                if (tobeDeletedItem != null)
                 {
                _context.LineItems.Remove(tobeDeletedItem);
                     _context.SaveChanges();
                return RedirectToAction("Index", new { id = lineItem.ReferanceNumberId });
                  }
    
            ViewBag.RfqId = lineItem.ReferanceNumberId;
            return View(lineItem);
        }

        public ActionResult Create(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }


            ViewBag.RfqId = id;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(LineItem lineItem)
        {
            if (ModelState.IsValid)
            {
                string impachech = "ADM-" + lineItem.ReferanceNumberId + "-";
                var noimpa = _context.LineItems.Where(l => l.IsRemoved == false && l.ReferanceNumberId == lineItem.ReferanceNumberId).Where(l => l.Impa.StartsWith(impachech)).ToList().Count();

                int noimpacount = noimpa + 1;
                string impa = lineItem.Impa.Replace(" ", "").Replace(".", "").PadLeft(6, '0');
                if (impa == "000000")
                {
                    impa = "ADM-" + lineItem.ReferanceNumberId + "-" + ((noimpacount).ToString()).PadLeft(4, '0');

                    lineItem.Impa = impa;
                }

                lineItem.Description = lineItem.Description.ToUpper().Replace('İ','I');

                // lineItem.WarehouseInfo = GetQttyByWarehouseAndImpa("4",lineItem.Impa).ToString();
                _context.LineItems.Add(lineItem);
                _context.SaveChanges();
                return RedirectToAction("Index", new { id = lineItem.ReferanceNumberId });
            }

            ViewBag.RfqId = lineItem.ReferanceNumberId;
            return View(lineItem);
        }

        public ActionResult UploadXml(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.RfqId = id;
            return View();
        }

        public ActionResult SaveUploadedFile(int id)
        {
            bool isSavedSuccessfully = true;
            string fName = "";
            

            try
            {
                foreach (string fileName in Request.Files)
                {
                    HttpPostedFileBase file = Request.Files[fileName];
                    //Save file content goes here
                    fName = file.FileName;
                  
                    if (file != null && file.ContentLength > 0)
                    {
                        var originalDirectory = new DirectoryInfo(HostingEnvironment.MapPath(string.Format(@"~/Files/TechnicalFiles")));
                       // var originalDirectory = new DirectoryInfo(string.Format("{0}Files\\TechnicalFiles", Server.MapPath(@"\")));

                        string pathString = originalDirectory.ToString();

                        var fileName1 = Path.GetFileName(file.FileName);

                        bool isExists = System.IO.Directory.Exists(pathString);

                        if (!isExists)
                            System.IO.Directory.CreateDirectory(pathString);

                        var path =  string.Format("{0}\\{1}", pathString, file.FileName);
                        file.SaveAs(path);

                        string impachech = "ADM-" + id + "-";
                        var noimpa = _context.LineItems.Where(l => l.IsRemoved == false && l.ReferanceNumberId == id).Where(l => l.Impa.StartsWith(impachech)).ToList().Count();

                        int noimpacount = noimpa+1;

                        switch (file.ContentType)
                        {
                            case "text/xml":
                                var doc = XDocument.Load(path);

                                var lineitems = doc.Descendants("LineItem")
                                    .Select(u => new
                                    {
                                        Number = u.Attribute("Number")?.Value,
                                        Description = u.Attribute("Description")?.Value != null
                                            ? u.Attribute("Description")?.Value
                                            : "",
                                        MeasureUnitQualifier = u.Attribute("MeasureUnitQualifier")?.Value != null
                                            ? u.Attribute("MeasureUnitQualifier")?.Value
                                            : "",
                                        Quantity = u.Attribute("Quantity")?.Value != null ? u.Attribute("Quantity")?.Value : "",
                                        TypeCode = u.Attribute("TypeCode")?.Value != null ? u.Attribute("TypeCode")?.Value : "",
                                        Identification = u.Attribute("Identification")?.Value != null
                                            ? u.Attribute("Identification")?.Value
                                            : "",
                                        SparePartNotes = u.Element("SparePartNotes")?.Value != null ? u.Element("SparePartNotes")?.Value
                                            : "",
                                        DrawingNumber = u.Attribute("DrawingNumber")?.Value != null ? u.Attribute("DrawingNumber")?.Value
                                            : "",
                                    });

                                
                                foreach (var item in lineitems)
                                {
                                    string impa = item.Identification.Replace(" ", "").Replace(".", "").PadLeft(6, '0');

                                    int n;
                                    bool isNumeric = int.TryParse(impa, out n);

                                    if (!isNumeric)
                                    {
                                        impa = "000000";
                                    }

                                    if (impa == "000000")
                                    {
                                        impa = "ADM-" + id+"-" + ((noimpacount).ToString()).PadLeft(4, '0');
                                        noimpacount = noimpacount + 1;
                                    }

                                    LineItem lineItem = new LineItem
                                    {
                                        Impa = impa,
                                        Description = item.Description.ToUpperInvariant() + " /SPN ; " +
                                                      item.SparePartNotes.ToUpperInvariant() + " /DN; " +
                                                      item.DrawingNumber.ToUpperInvariant(),
                                        Unit = item.MeasureUnitQualifier.ToUpperInvariant(),
                                        Qtty = Convert.ToDecimal(item.Quantity),
                                        Number = (Convert.ToInt32(item.Number)).ToString(),
                                        ReferanceNumberId = id
                                       // WarehouseInfo = GetQttyByWarehouseAndImpa("4", item.Identification.Replace(" ", "").Replace(".", "").PadLeft(6, '0')).ToString()
                                };

                                    _context.LineItems.Add(lineItem);
                                    _context.SaveChanges();
                                }
                                break;
                            case "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet":

                                // read excel and save it to db
                                // ExcelDataReader works with the binary Excel file, so it needs a FileStream
                                // to get started. This is how we avoid dependencies on ACE or Interop:
                                Stream stream = file.InputStream;

                                // We return the interface, so that
                                IExcelDataReader reader = null;


                                if (file.FileName.EndsWith(".xls"))
                                {
                                  reader = ExcelReaderFactory.CreateBinaryReader(stream);
                                }
                                else if (file.FileName.EndsWith(".xlsx"))
                                {
                                    reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                                }
                                else
                                {
                                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                                }

                              
                                DataSet result = reader.AsDataSet();
                                //dataset geldi ilk satır üst bilgiler

                                DataTable dt = result.Tables[0];

                                for (int i = 1; i < dt.Rows.Count; i++)
                                {

                                    string impa = dt.Rows[i][1].ToString().PadLeft(6, '0');
                                    if (impa == "000000")
                                    {
                                        impa = impa = "ADM-" + id + "-" + ((noimpacount).ToString()).PadLeft(4, '0');
                                        noimpacount = noimpacount + 1;
                                    }

                                    LineItem lineItem = new LineItem
                                    {
                                        Number = (Convert.ToInt32(dt.Rows[i][0].ToString())).ToString(),
                                        Impa = impa,
                                        Description = dt.Rows[i][2].ToString().ToUpper().Replace('İ', 'I'),
                                        Unit = dt.Rows[i][3].ToString(),
                                        Qtty = Convert.ToDecimal(dt.Rows[i][4].ToString()),
                                        ReferanceNumberId = id
                                    };

                                    _context.LineItems.Add(lineItem);
                                    _context.SaveChanges();
                                }
                                 
                                reader.Close();  

                                break;
                        }
                    }


                }

              

            }
            catch (Exception ex)
            {
                isSavedSuccessfully = false;
            }


            if (isSavedSuccessfully)
            {
                return Json(new { Message = fName });
            }
            else
            {
                return Json(new { Message = "Error in saving file" });
            }
        }


        public ActionResult DownloadExcelSample()
        {
            var file = new DirectoryInfo(string.Format("{0}Files\\Samples\\TechnicalExcelSample.xlsx", Server.MapPath(@"\")));

            string filestring = file.ToString();
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            return File(filestring, contentType, Path.GetFileName(filestring));
        }

        public ActionResult WritingCompleted(int id)
        {
          var rfq = _context.ReferanceNumbers.FirstOrDefault(x => x.Id == id);
            if (rfq != null)
          {
              rfq.Stage = 2;
              _context.SaveChanges();

              var items = _context.LineItems.Where(x => x.ReferanceNumberId == id).ToList();
              foreach (var lineItem in items)
              {
                    // AddDefinedSuppliers(lineItem.Impa,lineItem.Id).GetAwaiter();

                    // getwarehouseqtty


                    var t1 = Task.Run(async () =>
                    {
                        await AddDefinedSuppliers(lineItem.Impa, lineItem.Id);
                        
                    });
                    t1.Wait();


                    var t2 = Task.Run(async () =>
                    {
                       
                        await GetQttyByWarehouseAndImpa("4", lineItem.Impa, lineItem.Id);
                    });
                    t2.Wait();

                    var t3 = Task.Run(async () =>
                    {

                        await GetLastRemark( lineItem.Id,lineItem.Impa);
                    });
                    t3.Wait();

                }
               
              // add suppliers to items async
          }
            return RedirectToAction("Index", new { id = id });
        }

        public async Task GetLastRemark(int itemid,string impa)
        {
            var lastitem = await _context.LineItems.OrderByDescending(l=>l.Id).FirstOrDefaultAsync(l => l.Impa == impa && l.Remark != null);
            if (lastitem != null)
            {
                var item = await _context.LineItems.FirstOrDefaultAsync(l => l.Id == itemid);
                item.Remark = lastitem.Remark;
                await _context.SaveChangesAsync();
            }
        }

        public async Task AddDefinedSuppliers(string impa,int itemid)
        {
            var supkayitlilar = await _context.LineItemSuppliers.Where(x => x.LineItemId == itemid).ToListAsync();

            var supeskiler =await _context.LineItemSuppliers.Where(x => x.LineItem.Impa == impa).GroupBy(x => x.SupplierId)
                .Select(g => new
                {
                    g.FirstOrDefault().SupplierId,
                    g.FirstOrDefault().SupplierName
                })
                .ToListAsync();

            foreach (var item in supeskiler.ToList())
            {
                foreach (var kayitli in supkayitlilar)
                {
                    if (kayitli.SupplierId==item.SupplierId)
                    {
                        supeskiler.Remove(item);
                    }
                }
            }
          

            foreach (var sup in supeskiler)
            { 

                var supplier = new LineItemSupplier
                {
                    LineItemId = itemid,
                    SupplierId = sup.SupplierId,
                    SupplierName = sup.SupplierName
                };

                _context.LineItemSuppliers.Add(supplier);
                _context.SaveChanges();
            }
             
        }

        public ActionResult AddingSuppliersCompleted(int id)
        {
            var rfq = _context.ReferanceNumbers.FirstOrDefault(x => x.Id == id);
            if (rfq != null)
            {
                rfq.Stage = 3;
                _context.SaveChanges();
            }
            return RedirectToAction("Index", new { id = id });
        }

        [HttpPost]
        public ActionResult ImportExcelFileToDatabase(HttpPostedFileBase FileUpload)
        {
            try
            {
                DataSet ds = new DataSet();
                if (Request.Files["FileUpload"].ContentLength > 0)
                {
                    string timestamp = DateTime.Now.Date.ToString();
                    string fileExtension = System.IO.Path.GetExtension(Request.Files["FileUpload"].FileName);

                    if (fileExtension == ".xls" || fileExtension == ".xlsx")
                    {
                        string fileLocation = Server.MapPath("~/Files/TechnicalExcels/") + Request.Files["FileUpload"].FileName + "_" + timestamp;
                        if (System.IO.File.Exists(fileLocation))
                        {
                            System.IO.File.Delete(fileLocation);
                        }
                        Request.Files["FileUpload"].SaveAs(fileLocation);
                        string excelConnectionString = string.Empty;
                        excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" +
                        fileLocation + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                        //connection String for xls file format.
                        if (fileExtension == ".xls")
                        {
                            excelConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" +
                            fileLocation + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
                        }
                        //connection String for xlsx file format.
                        else if (fileExtension == ".xlsx")
                        {
                            excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" +
                            fileLocation + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                        }
                        //Create Connection to Excel work book and add oledb namespace
                        var excelConnection = new OleDbConnection(excelConnectionString);
                        excelConnection.Open();
                        var dt = new DataTable();

                        dt = excelConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                        if (dt == null)
                        {
                            return null;
                        }

                        String[] excelSheets = new String[dt.Rows.Count];
                        int t = 0;
                        //excel data saves in temp file here.
                        foreach (DataRow row in dt.Rows)
                        {
                            excelSheets[t] = row["TABLE_NAME"].ToString();
                            t++;
                        }
                        OleDbConnection excelConnection1 = new OleDbConnection(excelConnectionString);


                        string query = string.Format("Select * from [{0}]", excelSheets[0]);
                        using (OleDbDataAdapter dataAdapter = new OleDbDataAdapter(query, excelConnection1))
                        {
                            dataAdapter.Fill(ds);
                        }
                    }
 
                    ViewBag.State = "Success"; ;
                }
                else
                {
                    ViewBag.State = "NoFile";
                }
                return View("Index");

            }
            catch (Exception ex)
            {
                ViewBag.State = "Error "+ex.Message;
                return View("Index");
            }
        }



        public JsonResult GetSuppliers()
        {
            var suppliers = _context.Suppliers.ToList();
            var list =
                new List<SelectListItem> { new SelectListItem { Text = @"--Select a Supplier--", Value = "0" } };
            foreach (var item in suppliers)
            {
                list.Add(new SelectListItem { Text = item.SupplierName, Value = item.Id });
            }
            return Json(new SelectList(list, "Value", "Text", JsonRequestBehavior.AllowGet));
        }


        [WebMethod]
        public ActionResult AddSuppliersMultiple(Array lineitemids,Array supplierids,int rfqId)
        {
            foreach (var lineitemid in lineitemids)
            {
                foreach (var supplierid in supplierids)
                {
                    var id = lineitemid;

                    var supplier = _context.Suppliers.FirstOrDefault(x => x.Id == supplierid.ToString());
                    if (supplier!=null)
                    {
                        var lineitemsupplier = new LineItemSupplier
                        {
                            LineItemId = Convert.ToInt32(id),
                            SupplierId = supplier.Id,
                            SupplierName = supplier.SupplierName
                        };

                        var lineitemsuppliers = _context.LineItemSuppliers.Where(x => x.LineItemId == lineitemsupplier.LineItemId)
                            .Where(x => x.SupplierId == lineitemsupplier.SupplierId).ToList();

                        if (lineitemsuppliers.Count == 0)
                        {
                            _context.LineItemSuppliers.Add(lineitemsupplier);
                            _context.SaveChanges();
                        }
                    }

                }
            }

            return Json(Url.Action("Index", "TechnicalLineItem", new { id = rfqId }));
        
        }

        [WebMethod]
        public ActionResult SendMails(Array supplierids, int fileId,Array allsuppliers)
        {

            if (supplierids.Length > 0 && fileId != 0)
            {
                var filedetail = _context.Files.FirstOrDefault(x => x.Id == fileId);
                foreach (var supplierid in supplierids)
                {
                    var supplier = _context.Suppliers.FirstOrDefault(x => x.Id == (string) supplierid);
                    int logid = 0;
                    if (supplier == null) continue;
                    {
                        try
                        {
                            var email = supplier.Mail;

                            //send mail and log (file/ref/sup-id/date/user)
                            // log e-mail sending 

                            var emaillog = new EmailLog
                            {
                                Description = "Approval Stage",
                                FileId = fileId,
                                Name = supplier.SupplierName,
                                ToAdress = email,
                                State = "Sending"
                            };
                            _context.EmailLogs.Add(emaillog);
                            _context.SaveChanges();
                            logid = emaillog.Id;

                            var subject = "ADAMAR FIYAT TALEBI / " + filedetail.FileNumber;
                            var toadress = new MailAddress("info@adamarine.com");

                            if (email.Length>2)
                            {
                                toadress = new MailAddress(email);
                            }
                           
                            var ccAddresses =
                                new List<MailAddress> {new MailAddress("technicalmailgroup@adamarine.com") };
                            var body =
                                $"<strong> Sayın {supplier.SupplierName}  Yetkilisi </strong> Aşağıda ki butona tıklayarak açılan sayfada fiyat vermeniz Rica olunur. </br> Not: Sertifikası mevcut ürünlerin sertifiklarının en kısa zamanda tarafımıza gönderilmesini rica ediyoruz.  ";
                            var link = string.Format(@"http://82.222.153.50:5559//supplieroffer/offer/{0}",
                                GetStringSha256Hash(fileId + "/-/" + supplier.Id));

                            var temp = new EmailTemplates();
                            var eMailbody =
                                temp.e_mailbody_fiyat_isteme(subject, body, link, User.Identity.GetUserName(), "technicalmailgroup@adamarine.com");
                            var t = Task.Run(async () =>
                            {
                                await _emailer.SendEmail(subject, eMailbody, toadress, ccAddresses, logid);
                            });

                            System.Threading.Thread.Sleep(2100);

                            emaillog.State = "Sent";
                            _context.SaveChanges();

                        }
                        catch (Exception e)
                        {
                            var errorMessage = e.Message;
                            var emailLog = _context.EmailLogs.FirstOrDefault(x => x.Id == logid);
                            if (emailLog != null) emailLog.State = "Error:  " + errorMessage;
                            _context.SaveChanges();

                        }
                    }
                }

                //create suppliers records with price 
                var reflist = _context.ReferanceNumbers
                    .Where(r => r.IsDeleted == false)
                    .Where(f => f.FileId == fileId)
                    .Where(f => f.Stage == 3).Select(x=>x.Id).ToList();

                int masterid;

                foreach (var allsupplier in allsuppliers)
                {
                    
                    var supplier = _context.Suppliers.FirstOrDefault(x => x.Id == (string) allsupplier);
                    if (supplier == null) continue;
                    {
                        var offerid = GetStringSha256Hash(fileId + "/-/" + supplier.Id);
                        var offer = _context.OfferMasters.FirstOrDefault(x => x.OfferId == offerid);
                        //offer master save
                        if (offer == null)
                        {
                            var offermaster = new OfferMaster
                            {
                                OfferId = offerid,
                                FileId = fileId,
                                SupplierId = supplier.Id
                            };

                            _context.OfferMasters.Add(offermaster);
                            _context.SaveChanges(); 
                        }
 
                    }
                }
             
                       
                        //supplierline item create 

                try
                {
                    foreach (var allsupplier in allsuppliers)
                    {
                        var supplier = _context.Suppliers.FirstOrDefault(x => x.Id == (string)allsupplier);

                        var lineitemssupplier = _context.LineItemSuppliers.Include("LineItem")
                            .Where(l => l.SupplierId == supplier.Id && reflist.Contains(l.LineItem.ReferanceNumber.Id)).ToList();

                        var offerid = GetStringSha256Hash(fileId + "/-/" + supplier.Id);
                        var offer = _context.OfferMasters.FirstOrDefault(x => x.OfferId == offerid);
                        masterid = offer.Id;

                        foreach (var lineItemSupplier in lineitemssupplier)
                        {
                            var masterid1 = masterid;
                            var t = Task.Run(async () =>
                            {
                                await Createsupplierlineitems(lineItemSupplier,masterid1);
                            });
                            t.Wait();
                        }


                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }



                foreach (var refid in reflist)
                  {
                      var referance = _context.ReferanceNumbers.FirstOrDefault(r => r.Id == refid);
                      if (referance!=null)
                      {
                          referance.Stage = 4;
                          _context.SaveChanges();
                      } 

                   }
            

                return Json(true);

                //stages will be set to 4 

            
            }
            return Json(false);
        }

        private async Task Createsupplierlineitems(LineItemSupplier lineItemSupplier,int masterid)
        {
            var suplineitem = await
               _context.SupplierPriceLineItems.Include("OfferMaster").Include("LineItem").FirstOrDefaultAsync(x =>
                   x.LineItemId == lineItemSupplier.LineItemId && x.OfferMasterId == masterid).ConfigureAwait(true);

            if (suplineitem == null)
            {
                //will be added if supplier has price

                var lastpricelist = await _context.SupplierPriceLineItems.Include("OfferMaster").Include("LineItem").Where(l =>
                    l.OfferMaster.SupplierId == lineItemSupplier.SupplierId &&
                    l.LineItem.Impa == lineItemSupplier.LineItem.Impa && l.SupplierPrice!=0).OrderBy(l=>l.CreatedDate).Distinct().ToListAsync().ConfigureAwait(true);


                var supplierLineItem = new SupplierPriceLineItem
                {
                    OfferMasterId = masterid,
                    LineItemId = lineItemSupplier.LineItemId,
                    CreatedDate = DateTime.Now.Date
                };

                if (lastpricelist.Count > 0)
                {
                    var lastoffer = lastpricelist[0];
                    supplierLineItem.Currency = lastoffer.Currency;
                    supplierLineItem.Vat = lastoffer.Vat;
                    supplierLineItem.Comment = lastoffer.Comment;
                    supplierLineItem.SupplierPrice = lastoffer.SupplierPrice;
                    supplierLineItem.OldPriceDate = lastoffer.CreatedDate;
                    supplierLineItem.IsOldPrice = true;
                    supplierLineItem.TurkishDescription = lastoffer.TurkishDescription;

                }

                _context.SupplierPriceLineItems.Add(supplierLineItem);
               await _context.SaveChangesAsync();
            }
             
        }

        internal static string GetStringSha256Hash(string text)
        {
            if (String.IsNullOrEmpty(text))
                return String.Empty;

            using (var sha = new System.Security.Cryptography.SHA256Managed())
            {
                byte[] textData = System.Text.Encoding.UTF8.GetBytes(text);
                byte[] hash = sha.ComputeHash(textData);
                return BitConverter.ToString(hash).Replace("-", String.Empty);
            }
        }


        [WebMethod]
        public ActionResult RemoveSuppliersMultiple(Array lineitemids, Array supplierids, int rfqId)
        {
            foreach (var lineitemid in lineitemids)
            {
                foreach (var supplierid in supplierids)
                {
                    var id = lineitemid;

                    var supplier = _context.Suppliers.FirstOrDefault(x => x.Id == supplierid.ToString());
                    if (supplier != null)
                    {
                        var lineitemsupplier = new LineItemSupplier
                        {
                            LineItemId = Convert.ToInt32(id),
                            SupplierId = supplier.Id,
                            SupplierName = supplier.SupplierName
                        };

                        var lineitemsuppliers = _context.LineItemSuppliers
                            .Where(x => x.LineItemId == lineitemsupplier.LineItemId)
                            .FirstOrDefault(x => x.SupplierId == lineitemsupplier.SupplierId);


                        if (lineitemsuppliers!=null)
                        {
                            _context.LineItemSuppliers.Remove(lineitemsuppliers);
                            _context.SaveChanges();
                        }
                    }

                }
            }

            return Json(Url.Action("Index", "TechnicalLineItem", new { id = rfqId }));

        }

        [WebMethod]
        public ActionResult RemoveAllSuppliers(int rfqId)
        {
            var lineitemsupplier = _context.LineItemSuppliers.Include("LineItem").Where(l => l.LineItem.ReferanceNumberId == rfqId);
 
            _context.LineItemSuppliers.RemoveRange(lineitemsupplier);

            _context.SaveChanges();
            
            

            return Json(Url.Action("Index", "TechnicalLineItem", new { id = rfqId }));

        }



        [WebMethod]
        public ActionResult MoveRfqToFile(int rfqId, byte fileid,string name)
        {
            var file = _context.Files.FirstOrDefault(f => f.Id == fileid);
            var rfq = _context.ReferanceNumbers.FirstOrDefault(x => x.Id == rfqId);
            if (file!=null && rfq!=null)
            {
                rfq.FileId = fileid;
                rfq.Name = name;
                _context.SaveChanges();

                return Json(Url.Action("Index", "TechnicalLineItem", new { id = rfqId }));
            }
            else
            {
                //return View("RemoveRfq", rfq);

                return Json(null);
            }

           
        }

        [WebMethod]
        public ActionResult RemoveRfqFromFile(int rfqId)
        {
            var rfq = _context.ReferanceNumbers.FirstOrDefault(x => x.Id == rfqId);
            rfq.IsDeleted = true;
            _context.SaveChanges();

            return Json(Url.Action("Index", "Technical"));
        }


        public ActionResult Approve(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }


            try
            {
                //file info
                var filedetails = _context.Files.FirstOrDefault(f => f.Id == id);

                ViewBag.FileId = id;
                ViewBag.FileName = filedetails.FileNumber;

                var reflist = _context.ReferanceNumbers
                    .Where(r => r.IsDeleted == false)
                    .Where(f => f.FileId == id)
                    .Where(f => f.Stage == 3).AsEnumerable();

                var itemlist = _context.LineItems.Where(l => reflist.Contains(l.ReferanceNumber)).AsEnumerable();

                var lineitemsupliers = _context.LineItemSuppliers.Where(l => itemlist.Contains(l.LineItem)).AsEnumerable();

                var suplist = _context.Suppliers.Where(s => lineitemsupliers.Any(l=>l.SupplierId.Contains(s.Id))).AsEnumerable();

               var approveModel = new ApproveModel {ReferanceNumbers = reflist,Suppliers = suplist };

                //list rfq numbers on approve
                //supplier list with e-mail 

                return View(approveModel);
            }
            catch (Exception e)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest,e.Message);
            }

         
        }

        public ActionResult MoveToEdit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var referanceNumber = _context.ReferanceNumbers.FirstOrDefault(x => x.Id == id);

            if (referanceNumber != null)
            {
                var rfqId = referanceNumber.Id;
                referanceNumber.Stage = 2;
                _context.SaveChanges(); 
     
                return RedirectToAction("Index", new { id = rfqId });
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }


        public ActionResult UpdateMail(string id, int? fileId)
        {
            if (id == null || fileId == null )
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var supplier = _context.Suppliers.FirstOrDefault(x => x.Id == id);

            if (supplier != null)
            {
                ViewBag.fileId = fileId;

                return View(supplier);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        [WebMethod]
        public ActionResult UpdateSupplierMail(string email,string supid, int? fileId)
        {
            if (string.IsNullOrEmpty(supid) || fileId == null ||  string.IsNullOrEmpty(email))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest,"No Supplier id defined");
            } 

            try
            {
                string tablename = Netsis.database + "..tblcasabit";

                string sqlcommand = string.Format(@"update {2} set email='{0}' where cari_kod='{1}'", email, supid, tablename);

                _context.Database.ExecuteSqlCommand(sqlcommand);

                return RedirectToAction("Approve", new { id = fileId });
            }
            catch (Exception e)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest,e.Message);
            }
  
        }


        public  decimal GetPriceByWarehouseAndImpa(string warehouse, string impa, int fileid)
        {
         
            decimal price = 0;

            try
            {
                impa = impa + "%";

                string querry = string.Format(@"SELECT TOP 1  cast(isnull(STHAR_NF,0) as decimal(18,2))  as price
         FROM  {2}..TBLSTHAR
                               WHERE        stok_kodu LIKE ( '{0}') AND STHAR_GCKOD = 'G' AND STHAR_NF <> 0 AND STHAR_HTUR != 'L' AND sube_kodu = {1} and ( PROJE_KODU='0' or PROJE_KODU is null)
                               ORDER BY STHAR_TARIH ", impa, warehouse, Netsis.database);


                price = _context.Database.SqlQuery<decimal>(querry).FirstOrDefault();


                var file = _context.Files.FirstOrDefault(f => f.Id == fileid);

                var currency = file.Currency;


                var currencyrate = _context.CurrencyRates.FirstOrDefault(c => c.CurrencyId == currency);

                price = (price / currencyrate.Rate) * (decimal)(1.18);


                return decimal.Round(price, 2, MidpointRounding.AwayFromZero);



            }
            catch (Exception)
            {

                throw;
            }

        }


        public async Task GetQttyByWarehouseAndImpa(string warehouse, string impa,int lineitemid)
        {

            int price = 0;

            try
            {
                impa = impa + "%";

                string querry = string.Format(@"
SELECT        TOP 1 (isnull
                            ((SELECT        cast(sum(STHAR_GCMIK) AS int)
                                FROM          {2}..TBLSTHAR
                                WHERE        STHAR_GCKOD = 'G' AND TBLSTHAR.STOK_KODU LIKE ('{0}') AND sthar_htur != 'L' AND SUBE_KODU = {1} ), 0)
								 - 
					isnull ((SELECT        cast(sum(STHAR_GCMIK) AS int)
                                FROM           {2}..TBLSTHAR
                                WHERE        STHAR_GCKOD = 'C' AND TBLSTHAR.STOK_KODU LIKE ('{0}') AND sthar_htur != 'L' AND SUBE_KODU = {1}), 0)) AS qtty
             FROM            {2}..TBLSTHAR
", impa, warehouse, Netsis.database);


                price = await _context.Database.SqlQuery<int>(querry).FirstOrDefaultAsync();

                LineItem lineitem = await _context.LineItems.FirstOrDefaultAsync(l => l.Id == lineitemid);

                lineitem.WarehouseInfo = price.ToString();

               await _context.SaveChangesAsync();
                 

            }
            catch (Exception e)
            {

                throw;
            }

        }

        public ActionResult ViewRfq(int id)
        {
            try
            {
                var rfq = _context.ReferanceNumbers.FirstOrDefault(r => r.Id == id);
                var lineitems = _context.LineItems.Where(l => l.ReferanceNumberId == id && l.IsRemoved==false).ToList();

                ViewBag.RfqName = rfq.Name;

                return View(lineitems);
            }
            catch (Exception e)
            {


                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, e.Message);
            }
         
        }

        public ActionResult ViewOrderedRfq(int id)
        {
            try
            {
                var rfq = _context.ReferanceNumbers.FirstOrDefault(r => r.Id == id);
                var lineitems = _context.OrderLineItems.Where(l => l.ReferanceNumberId == id && l.IsRemoved == false).ToList();

                ViewBag.RfqName = rfq.Name;

                return View(lineitems);
            }
            catch (Exception e)
            {


                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, e.Message);
            }

        }




    }
}