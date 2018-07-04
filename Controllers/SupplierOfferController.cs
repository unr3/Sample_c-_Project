using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Services;
using Appa_MVC.Models;
using DataTables;
using Inspinia_MVC5_SeedProject.Email;
using Inspinia_MVC5_SeedProject.Email.Templates;
using Inspinia_MVC5_SeedProject.Models;
using Microsoft.AspNet.Identity;

namespace Appa_MVC.Controllers
{
    public class SupplierOfferController : Controller
    {
        readonly ApplicationDbContext _context = new ApplicationDbContext();
        readonly Emailer _emailer = new Emailer();
        // GET: SupplierOffer

        [AllowAnonymous]
        public ActionResult Offer(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var masterdetail = _context.OfferMasters.FirstOrDefault(m => m.OfferId == id);
                if (masterdetail!=null)
                {
                    var supplier = _context.Suppliers.FirstOrDefault(s => s.Id == masterdetail.SupplierId);

                    var lineitems = _context.SupplierPriceLineItems.Include("LineItem")
                        .Include("LineItem.ReferanceNumber")
                        .Where(l =>
                        l.OfferMasterId == masterdetail.Id && l.OfferMaster.SupplierId == supplier.Id && l.LineItem.ReferanceNumber.Stage<5).ToList();

                    if (lineitems.Count>0)
                    {
                        var refid = lineitems[0].LineItem.ReferanceNumberId;

                        ViewBag.SupplierName = supplier.SupplierName;

                        if (lineitems.Count > 0)
                        {
                            var refno = _context.ReferanceNumbers.FirstOrDefault(r =>
                                r.Id == refid);
                            var file = _context.Files.FirstOrDefault(f => f.Id == refno.FileId);

                            ViewBag.IsComplated = masterdetail.IsComplated;
                            ViewBag.ComplatedDate = masterdetail.ComplatedDate;
                            ViewBag.UserName = masterdetail.NameOfPriceGiven;
                            ViewBag.OrderId = id;
                            ViewBag.FileNumber = file.FileNumber;
                            ViewBag.OfferId = id;
                        }
                        return View(lineitems);
                    }
                    else
                    {
                        return  new HttpStatusCodeResult(404,"Fiyat vermek istediginiz ürünlerin fiyatlandirilma süresi geçmistir.");
                    }

                   
                }
                
            }
            return new HttpStatusCodeResult(HttpStatusCode.Forbidden, "No Matching Result");
        }

        [AllowAnonymous]
        public ActionResult Edit(int id,string offerid)
        {
            var lineitem = _context.SupplierPriceLineItems.Include("LineItem").FirstOrDefault(l => l.Id == id);
            if (lineitem!=null && !string.IsNullOrEmpty(offerid))
            {
                ViewBag.OfferId = offerid;
                return View(lineitem);
            }
            

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult Edit(SupplierPriceLineItem lineItem)
        {
            try
            {
                var offer = _context.OfferMasters.FirstOrDefault(o => o.Id == lineItem.OfferMasterId);
                var EditedItem = _context.SupplierPriceLineItems.Include("LineItem").First(x => x.Id == lineItem.Id);
                if (ModelState.IsValid)
                {
                   
                    EditedItem.SupplierPrice = lineItem.SupplierPrice;
                    EditedItem.Vat = lineItem.Vat;
                    EditedItem.Currency = lineItem.Currency;
                    EditedItem.Comment = lineItem.Comment;
                    EditedItem.IsOldPrice = false;
                    EditedItem.UpdateDate=DateTime.Now;
                    EditedItem.TurkishDescription = lineItem.TurkishDescription;

                    _context.SaveChanges();

                    return RedirectToAction("Offer", new { id = offer.OfferId });
                }

                ViewBag.OfferId = offer.OfferId;
                return View(EditedItem);
            }
            catch (Exception e)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest,e.Message);
            }

        }

        [AllowAnonymous]
        [WebMethod]

        public ActionResult Complated(string orderId, string name)
        {
        
            var offermaster = _context.OfferMasters.FirstOrDefault(o => o.OfferId == orderId);
            if (offermaster!=null)
            {
                offermaster.IsComplated = true;
                offermaster.NameOfPriceGiven = name;
                offermaster.ComplatedDate=DateTime.Now;
                _context.SaveChanges();

                var itemlist = _context.SupplierPriceLineItems.Include("LineItem").Where(l => l.OfferMaster.OfferId == orderId).ToList();

                foreach (var supplierPriceLineItem in itemlist)
                {
                    supplierPriceLineItem.UpdateDate = offermaster.ComplatedDate;
                    
                }
                _context.SaveChanges();

                // mail sending
                int logid = 0;

                var fileid = offermaster.FileId;

                var file = _context.Files.FirstOrDefault(f => f.Id == fileid); 
                var sup = _context.Suppliers.FirstOrDefault(f => f.Id == offermaster.SupplierId);

                var temp = new EmailTemplates();
                var eMailbody =
                    temp.e_mailbody_suppliercomplated(itemlist,file,sup,offermaster.NameOfPriceGiven);

               

                MailAddress supmail = new MailAddress("info@adamarine.com");

                if (sup.Mail != null)
                {
                    supmail = new MailAddress(sup.Mail);
                }

                var toadress = new MailAddress("technicalmailgroup@adamarine.com");

                var emaillog = new EmailLog
                {
                    Description = "Supplier Mail " + offermaster.SupplierId,
                    FileId = file.Id,
                    Name = "Offer ID:" + offermaster.OfferId + " NAME: " + offermaster.NameOfPriceGiven,
                    ToAdress = toadress.ToString(),
                    State = "Sending"
                };
                _context.EmailLogs.Add(emaillog);
                _context.SaveChanges();
                logid = emaillog.Id;

                string header = "Fiyat Talebiniz için Teşekkür ederiz / " + file.FileNumber;

                List<MailAddress> ccadress = new List<MailAddress>
                {
                   supmail
                };


                var t = Task.Run(async () =>
                {
                    await _emailer.SendEmail(header, eMailbody, toadress, ccadress, logid);
                });

                emaillog.State = "Sent";
                _context.SaveChanges();

                // mail sending



                return RedirectToAction("Offer", "SupplierOffer", new { id = orderId });
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        [AllowAnonymous]
        public ActionResult EditRfq(string id)
        {
            if (id.Length>5)
            {
                var masterdetails = _context.OfferMasters.FirstOrDefault(o => o.OfferId == id);
               
                var supplier = _context.Suppliers.FirstOrDefault(s => s.Id == masterdetails.SupplierId);
                ViewBag.SupName = supplier.SupplierName;
                return View(masterdetails);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        [AllowAnonymous]
        public ActionResult GetRfqItems(string id)
        {
            var masterdetails = _context.OfferMasters.FirstOrDefault(o => o.OfferId == id);
            int masterid = masterdetails.Id; 
            var supplier = _context.Suppliers.FirstOrDefault(s => s.Id == masterdetails.SupplierId);

            var formData = HttpContext.Request.Form;

            using (var db = new DataTables.Database("sqlserver", @"Data Source=192.168.2.4;Initial Catalog=AppaMVC; User=PLANET; Password=PLANET74110;"))
            {
                var response = new Editor(db, "SupplierPriceLineItems")
                    .Model<SupplierEditModel>()
                     .Field(new Field("SupplierPriceLineItems.Id", "supplierlineid")
                    )
                     .Field(new Field("LineItems.Id", "lineitemid")

                    )
                    .Field(new Field("LineItems.Number")
                        .Validator(Validation.NotEmpty())
                    )
                    .Field(new Field("LineItems.Impa")
                        .Validator(Validation.NotEmpty())
                    )
                    .Field(new Field("LineItems.Description"))
                    .Field(new Field("LineItems.Unit")
                        .Validator(Validation.NotEmpty())
                    )
                    .Field(new Field("LineItems.Qtty")
                        .Validator(Validation.Numeric())
                        .SetFormatter(Format.IfEmpty(null))
                    )
                    .Field(new Field("SupplierPriceLineItems.SupplierPrice")
                    )
                    .Field(new Field("SupplierPriceLineItems.Comment"))
                    .Field(new Field("SupplierPriceLineItems.Currency"))
                    .Field(new Field("SupplierPriceLineItems.Vat"))
                    .Where("Deleted", 0)
                    .Where("OfferMasterId", masterid)
                    .LeftJoin("LineItems", "SupplierPriceLineItems.LineItemId", "=", "LineItems.id")
                    .Process(formData)
                    .Data();  

                return Json(response, JsonRequestBehavior.AllowGet);

            }
        }

        private void SupplierOfferController_PreEdit1(object sender, PreEditEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void SupplierOfferController_PreEdit(object sender, PreEditEventArgs e)
        {
            throw new NotImplementedException();
        }

        public bool checkdecimal(string value)
        {

            decimal val = 0;
            return decimal.TryParse(value, out val);

            
        }

        [AllowAnonymous]
        public ActionResult SetCurrencyAndVat(string orderid, string currency, int vat)
        {
            if (orderid != string.Empty && currency!=string.Empty && vat!=0)
            {
                var master = _context.OfferMasters.FirstOrDefault(m => m.OfferId == orderid);
                var items = _context.SupplierPriceLineItems.Where(s => s.OfferMasterId == master.Id);

                foreach (var item in items)
                {
                    item.Currency = currency;
                    item.Vat = vat;
                    item.IsOldPrice = false;
                    item.UpdateDate = DateTime.Now; 

                }
                _context.SaveChanges();

                return Json(true);
            }
            else
            {
                return Json(false);
            }


        }
    }
}