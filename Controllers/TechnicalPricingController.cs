using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using Appa_MVC.Models;
using DataTables;
using Inspinia_MVC5_SeedProject.Email;
using Inspinia_MVC5_SeedProject.Email.Templates;
using Inspinia_MVC5_SeedProject.Models;
using Microsoft.AspNet.Identity;
using static System.String;
using Format = DataTables.Format;


namespace Appa_MVC.Controllers
{
    public class TechnicalPricingController : Controller
    {
        readonly ApplicationDbContext _context = new ApplicationDbContext();
        readonly Emailer _emailer = new Emailer();
        // GET: TechnicalPricing
        public ActionResult Index(int? id)
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
                    .Where(f => f.Stage == 4).AsEnumerable();

                var itemlist = _context.LineItems.Include("ReferanceNumber").Where(  l => reflist.Contains(l.ReferanceNumber)).Where(l=>l.IsRemoved==false).AsEnumerable();

                var lineitemsupliers = _context.LineItemSuppliers.Where(l => itemlist.Contains(l.LineItem)).AsEnumerable();

                var suppliermaster = _context.SupplierMasters.Where(s => lineitemsupliers.Any(l => l.SupplierId.Contains(s.SupplierId)))
                    .Where(s => s.FileId == id).AsEnumerable();

                List<RfqTotal> RfqTotals = new List<RfqTotal>();

                foreach (var item in reflist)
                {
                    RfqTotals.Add(GetRfqTotal(item.Id));
                }

                var pricingModel = new PricingModel { ReferanceNumbers = reflist, LineItems = itemlist, SupplierOffers= suppliermaster,File = filedetails,RfqTotals=RfqTotals };

                //list rfq numbers on pricing
                //lineitems
                ViewBag.FileId = id;

                return View(pricingModel);
            }
            catch (Exception e)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, e.Message);
            }
        }
        
        public ActionResult Edit(int? id, int? fileid)
        {
            if (id==null || fileid == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Line item id and File id should be selected !!!");
            }


         
            var lineItem = _context.LineItems.Include("ReferanceNumber").FirstOrDefault(l => l.Id == id);
            var file = _context.Files.FirstOrDefault(f => f.Id == lineItem.ReferanceNumber.FileId);
            var lineitemsuppliers = _context.ViewSupplierPriceLineItems.Where(s => s.LineItemId == id).ToList();

            var viewModel = new LineItemPricingEditViewModel
            {
                LineItemSuppliers = lineitemsuppliers,
                LineItem = lineItem,
                File = file
            };


            return View(viewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(LineItemPricingEditViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
               
                var EditedItem = _context.LineItems.First(x => x.Id == viewModel.LineItem.Id);

                string newimpa = viewModel.LineItem.Impa;
                string oldimpa = EditedItem.Impa;

                EditedItem.Impa = viewModel.LineItem.Impa;
                EditedItem.Description = viewModel.LineItem.Description;
                EditedItem.Number = viewModel.LineItem.Number;
                EditedItem.Qtty = viewModel.LineItem.Qtty;
                EditedItem.Unit = viewModel.LineItem.Unit;
                EditedItem.AltQtty = viewModel.LineItem.AltQtty;
                EditedItem.AltUnit = viewModel.LineItem.AltUnit;
                EditedItem.AltPrice = viewModel.LineItem.AltPrice;
                EditedItem.Remark = viewModel.LineItem.Remark;
                EditedItem.Price = viewModel.LineItem.Price;

                _context.SaveChanges();

                //if impa has changed add definedsuppliers
                if (oldimpa != newimpa)
                {
                    var oldsuppliers = _context.SupplierPriceLineItems.Where(s => s.LineItemId == EditedItem.Id).ToList();

                    foreach (var item in oldsuppliers)
                    {
                        item.Deleted = true;
                    }

                    

                    var changedsuppliers = _context.LineItemSuppliers.Where(x => x.LineItem.Id == EditedItem.Id).ToList();

                    foreach (var item in changedsuppliers)
                    {
                        item.ImpaChanged = true;
                    }
                    _context.SaveChanges();

                    //tedarikçi kaydı

                    // master kaydı var mı kontrol edilecek 

                    // fiyatlandırma için eski fiyat kayıtları girilecek


                    var referance = _context.ReferanceNumbers.FirstOrDefault(r => r.Id == EditedItem.ReferanceNumberId);

                     AddExistingSuppliers(EditedItem.Impa, EditedItem.Id, referance.FileId);

                    // AddDefinedSuppliers(EditedItem.Impa, EditedItem.Id,referance.FileId).GetAwaiter();


                }


                return RedirectToAction("Index", new { id = viewModel.LineItem.ReferanceNumber.FileId });
            }

            var lineItem = _context.LineItems.Include("ReferanceNumber").FirstOrDefault(l => l.Id == viewModel.LineItem.Id);
            var file = _context.Files.FirstOrDefault(f => f.Id == lineItem.ReferanceNumber.FileId);
            var lineitemsuppliers = _context.ViewSupplierPriceLineItems.Where(s => s.LineItemId == viewModel.LineItem.Id).ToList();

            var viewModelorg = new LineItemPricingEditViewModel
            {
                LineItemSuppliers = lineitemsuppliers,
                LineItem = lineItem,
                File = file
            };
            return View(viewModelorg);
        }


        private async Task Createsupplierlineitems(LineItemSupplier lineItemSupplier, int masterid)
        {
            var suplineitem = await
               _context.SupplierPriceLineItems.Include("OfferMaster").Include("LineItem").FirstOrDefaultAsync(x =>
                   x.LineItemId == lineItemSupplier.LineItemId && x.OfferMasterId == masterid).ConfigureAwait(true);

            if (suplineitem == null)
            {
                //will be added if supplier has price

                var lastpricelist = await _context.SupplierPriceLineItems.Include("OfferMaster").Include("LineItem").Where(l =>
                    l.OfferMaster.SupplierId == lineItemSupplier.SupplierId &&
                    l.LineItem.Impa == lineItemSupplier.LineItem.Impa && l.SupplierPrice != 0).OrderBy(l => l.CreatedDate).Distinct().ToListAsync().ConfigureAwait(true);


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

                }

                _context.SupplierPriceLineItems.Add(supplierLineItem);
                await _context.SaveChangesAsync();
            }

        }

        public ActionResult SelectSupplier(int id, int lineitemid, int fileid)
        {
            if (id!=0 || lineitemid!=0 || fileid!=0)
            {
                try
                {
                    var lineitem = _context.LineItems.FirstOrDefault(l => l.Id == lineitemid);
                    var lineitemsupplier = _context.ViewSupplierPriceLineItems.Include("OfferMaster").FirstOrDefault(s=> s.Id == id);


                    if (lineitem != null)
                    {
                        if (lineitemsupplier != null)
                        {
                            lineitem.SelectedSupplierName = lineitemsupplier.SupplierName;
                            lineitem.SelectedSupplierCurrency = lineitemsupplier.Currency;
                            lineitem.SelectedSupplierId = lineitemsupplier.OfferMaster.SupplierId;
                            lineitem.SelectedSupplierPrice = lineitemsupplier.SupplierPrice;
                            lineitem.SelectedSupplierRemark = lineitemsupplier.Comment;
                            lineitem.SelectedSupplierCalculatedPrice = lineitemsupplier.RealPrice;
                            lineitem.SelectedSupplierTurkishDefinition = lineitemsupplier.TurkishDescription;
                            lineitem.IsWareHouseSelected = false;
                            _context.SaveChanges();
                        }
                    }


                    var lineItem = _context.LineItems.Include("ReferanceNumber").FirstOrDefault(l => l.Id == lineitemid);
                    var file = _context.Files.FirstOrDefault(f => f.Id == lineItem.ReferanceNumber.FileId);
                    var lineitemsuppliers = _context.ViewSupplierPriceLineItems.Where(s => s.LineItemId == lineitemid).ToList();

                    var viewModelorg = new LineItemPricingEditViewModel
                    {
                        LineItemSuppliers = lineitemsuppliers,
                        LineItem = lineItem,
                        File = file
                    };
                    return Edit(viewModelorg);
                }
                catch (Exception e)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Supplier Selection ERROR !!!"+e.Message);
                }


               
            }

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Supplier Selection ERROR !!!");
        }

        public ActionResult GetBestSuppliers(int id)
        {
            if (id!=0)
            {
                try
                {
                    var lineitems = _context.LineItems.Include("ReferanceNumber").Where(l => l.ReferanceNumber.FileId == id && l.ReferanceNumber.Stage == 4 && l.IsRemoved!=true).OrderBy(s => s.Number).ToList();

                    foreach (var lineitem in lineitems)
                    {
                        var supplier = _context.ViewSupplierPriceLineItems.Include("OfferMaster")
                            .Where(s => s.LineItemId == lineitem.Id && s.RealPrice != 0).OrderBy(s => s.RealPrice)
                            .FirstOrDefault();

                        if (supplier != null)
                        {
                            lineitem.SelectedSupplierName = supplier.SupplierName;
                            lineitem.SelectedSupplierCurrency = supplier.Currency;
                            lineitem.SelectedSupplierId = supplier.OfferMaster.SupplierId;
                            lineitem.SelectedSupplierPrice = supplier.SupplierPrice;
                            lineitem.SelectedSupplierRemark = supplier.Comment;
                            lineitem.SelectedSupplierCalculatedPrice = supplier.RealPrice;
                            lineitem.IsWareHouseSelected = false;
                            lineitem.SelectedSupplierTurkishDefinition = supplier.TurkishDescription;
                        }
                    }
                    _context.SaveChanges();
                    return RedirectToAction("Index", "TechnicalPricing", new { id });

                }
                catch (Exception e)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Get Best Price Error Please Try Again !!!" + e.Message);
                }
                
            }

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "GET BEST PRICE  ERROR !!!" );
        }

        public ActionResult SetPrice(int id,decimal percentage)
        {
            if (id != 0 && percentage!=0)
            {
                try
                {
                    var lineitems = _context.LineItems.Include("ReferanceNumber").Where(l => l.ReferanceNumber.FileId == id && l.ReferanceNumber.Stage == 4 && l.IsRemoved != true).ToList();

                    foreach (var lineitem in lineitems)
                    {
                         
                        if (lineitem.SelectedSupplierId != null)
                        {
                            lineitem.Price = lineitem.SelectedSupplierCalculatedPrice * (100 + percentage) / 100;
                        }
                    }
                    _context.SaveChanges();
                    return Json(true);

                }
                catch (Exception e)
                {
                    return Json(false);
                }

            }

            return Json(false);
        }

        public ActionResult Remove(int lineitemid)
        {
            try
            {
                var lineitem = _context.LineItems.FirstOrDefault(l => l.Id == lineitemid);
                lineitem.IsRemoved = true;
                lineitem.RemovedUser = User.Identity.GetUserName();

                _context.SaveChanges(); 

                return Json(true);
            }
            catch (Exception e)
            {
                return Json(false);
            }
        }


        public ActionResult Alternative(int lineitemid,string description, string impa,string unit, string qtty,bool isalternative,string number)
        {
            try
            {
                var lineitem = _context.LineItems.FirstOrDefault(l => l.Id == lineitemid);
                int referanceno = lineitem.ReferanceNumberId;

                if (isalternative)
                {
                    
                    var countofalternatives = _context.LineItems.Where(l => l.MasterItemId == lineitemid).ToList();

                    lineitem.IsAlternative = true;
                    lineitem.MasterNumber = lineitem.Number;
                    lineitem.Number = lineitem.Number + "-" + Number2String((countofalternatives.Count + 1), true);
                    lineitem.MasterItemId = lineitem.Id;

                    if (impa.Length == 6)
                    {
                        lineitem.Impa = impa;
                    }

                    if (description.Length > 2)
                    {
                        lineitem.Description = description;
                    }
                    if (unit.Length > 0)
                    {
                        lineitem.Unit = unit;
                    }
                    if (qtty.Length > 0)
                    {
                        lineitem.Qtty = Convert.ToDecimal(qtty);
                    }
                }
                else
                {
                    lineitem = new LineItem();
                    lineitem.Qtty= Convert.ToDecimal(qtty);
                    lineitem.Number = number;
                    lineitem.Impa = impa;
                    lineitem.Description = description;
                    lineitem.Unit = unit;
                    lineitem.ReferanceNumberId = referanceno;
                }

               
                _context.LineItems.Add(lineitem);
                _context.SaveChanges();


                /*tedarikçi kaydı

                master kaydı var mı kontrol edilecek 

                fiyatlandırma için eski fiyat kayıtları girilecek

                 */

                var referance = _context.ReferanceNumbers.FirstOrDefault(r => r.Id == referanceno);

                AddExistingSuppliers(lineitem.Impa, lineitem.Id, referance.FileId);


                
                return Json(true);


            }
            catch (Exception e)
            {
                return Json(false);
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

        public void AddExistingSuppliers(string impa, int itemid, int fileid)
        {
            try
            {
              

                var suppliers = _context.LineItemSuppliers.Where(x => x.LineItem.Impa == impa && x.ImpaChanged==false).GroupBy(x => x.SupplierId)
            .Select(g => new
            {
                g.FirstOrDefault().SupplierId,
                g.FirstOrDefault().SupplierName
            })
            .ToList();


                foreach (var sup in  suppliers)
                {
                    var supplier = new LineItemSupplier
                    {
                        LineItemId = itemid,
                        SupplierId = sup.SupplierId,
                        SupplierName = sup.SupplierName
                    };

                    _context.LineItemSuppliers.Add(supplier);
                    _context.SaveChanges();



                    int masterid = 0;
                    var offerid = GetStringSha256Hash(fileid + "/-/" + sup.SupplierId);
                    var offer = _context.OfferMasters.FirstOrDefault(x => x.OfferId == offerid);


                    //offer master save
                    if (offer == null)
                    {
                        var offermaster = new OfferMaster
                        {
                            OfferId = offerid,
                            FileId = fileid,
                            SupplierId = sup.SupplierId
                        };
                         
                        _context.OfferMasters.Add(offermaster);
                        _context.SaveChanges();

                        masterid = offermaster.Id;
                    }
                    else
                    {
                        masterid = offer.Id;
                    }


                    // suplineitem


                    var lastpricelist =  _context.SupplierPriceLineItems.Include("OfferMaster").Include("LineItem").Where(l =>
                        l.OfferMaster.SupplierId == supplier.SupplierId &&
                        l.LineItem.Impa == supplier.LineItem.Impa && l.SupplierPrice != 0).OrderBy(l => l.CreatedDate).Distinct().ToList();


                    var supplierLineItem = new SupplierPriceLineItem
                    {
                        OfferMasterId = masterid,
                        LineItemId = supplier.LineItemId,
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

                        _context.SupplierPriceLineItems.Add(supplierLineItem);
                        _context.SaveChanges();
                    }

                  

                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task AddDefinedSuppliers(string impa, int itemid, int fileid)
        {

            try
            {
                var suppliers = _context.LineItemSuppliers.Where(x => x.LineItem.Impa == impa).GroupBy(x => x.SupplierId)
            .Select(g => new
            {
                g.FirstOrDefault().SupplierId,
                g.FirstOrDefault().SupplierName
            })
            .ToListAsync();


                foreach (var sup in await suppliers)
                {
                    var supplier = new LineItemSupplier
                    {
                        LineItemId = itemid,
                        SupplierId = sup.SupplierId,
                        SupplierName = sup.SupplierName
                    };

                    _context.LineItemSuppliers.Add(supplier);
                    _context.SaveChanges();



                    int masterid = 0;
                    var offerid = GetStringSha256Hash(fileid + "/-/" + sup.SupplierId);
                    var offer = _context.OfferMasters.FirstOrDefault(x => x.OfferId == offerid);


                    //offer master save
                    if (offer == null)
                    {
                        var offermaster = new OfferMaster
                        {
                            OfferId = offerid,
                            FileId = fileid,
                            SupplierId = sup.SupplierId
                        };

                        masterid = offermaster.Id;
                        _context.OfferMasters.Add(offermaster);
                        _context.SaveChanges();
                    }
                    else
                    {
                        masterid = offer.Id;
                    }


                    // suplineitem


                    var lastpricelist = await _context.SupplierPriceLineItems.Include("OfferMaster").Include("LineItem").Where(l =>
                        l.OfferMaster.SupplierId == supplier.SupplierId &&
                        l.LineItem.Impa == supplier.LineItem.Impa && l.SupplierPrice != 0).OrderBy(l => l.CreatedDate).Distinct().ToListAsync().ConfigureAwait(true);


                    var supplierLineItem = new SupplierPriceLineItem
                    {
                        OfferMasterId = masterid,
                        LineItemId = supplier.LineItemId,
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

                    }

                    _context.SupplierPriceLineItems.Add(supplierLineItem);
                    await _context.SaveChangesAsync();

                }
            }
            catch (Exception)
            {

                throw;
            }

        }

        private String Number2String(int number, bool isCaps)

        {

            Char c = (Char)((isCaps ? 65 : 97) + (number - 1));

            return c.ToString();

        }

        public ActionResult EditRfq(int id)
        {
            if (id!=0)
            {
                var referance = _context.ReferanceNumbers.FirstOrDefault(r => r.Id == id);
                return View(referance);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        public ActionResult GetRfqItems(int id)
        {

            var formData = HttpContext.Request.Form;

            using (var db = new DataTables.Database("sqlserver", @"Data Source=192.168.2.4;Initial Catalog=AppaMVC; User=PLANET; Password=PLANET74110;"))
            {
                var response = new Editor(db, "LineItems")
                    .Model<LineItemEditModel>()
                    .Field(new Field("Number")
                        .Validator(Validation.NotEmpty())
                    )
                    .Field(new Field("Impa")
                        .Validator(Validation.NotEmpty())
                    )
                    .Field(new Field("Description"))
                    .Field(new Field("Unit")
                        .Validator(Validation.NotEmpty())
                    )
                    .Field(new Field("Qtty")
                        .Validator(Validation.Numeric())
                        .SetFormatter(Format.IfEmpty(null))
                    )
                    .Field(new Field("Price")
                     
                    )
                    .Field(new Field("Remark"))
                    .Field(new Field("AltUnit"))
                    .Field(new Field("AltQtty"))
                    .Field(new Field("AltPrice"))
                    .Field(new Field("SelectedSupplierName"))
                    .Field(new Field("SelectedSupplierCalculatedPrice")
                        .Validator(Validation.Numeric())
                        .SetFormatter(Format.IfEmpty(null))
                    )
                    .Field(new Field("SelectedSupplierRemark"))
                    .Where("IsRemoved",0)             
                    .Where("ReferanceNumberId", id)
                    .Process(formData)
                    .Data();

                return Json(response, JsonRequestBehavior.AllowGet);

            }
        }

        public JsonResult GetAllSuppliers()
        {
            var suppliers = _context.Suppliers.ToList();
            var list =
                new List<SelectListItem> { new SelectListItem { Text = "--Select a Supplier--", Value = "0" } };
            foreach (var item in suppliers)
            {
                list.Add(new SelectListItem { Text = item.SupplierName, Value = item.Id });
            }
            return Json(new SelectList(list, "Value", "Text", JsonRequestBehavior.AllowGet));
        }


        public ActionResult AddNewSupplier(int fileid, int lineitemid, string lineitemimpa, string supplierid, string price, int vat, string currency, string remark)
        {
            try
            {
                var lineitem = _context.LineItems.FirstOrDefault(l => l.Id == lineitemid);
                int referanceno = lineitem.ReferanceNumberId;

                var sup = _context.Suppliers.FirstOrDefault(s => s.Id == supplierid);


                //check master and add supplier

                var supplier = new LineItemSupplier
                {
                    LineItemId = lineitemid,
                    SupplierId = sup.Id,
                    SupplierName = sup.SupplierName
                };

                _context.LineItemSuppliers.Add(supplier);
                _context.SaveChanges();



                int masterid = 0;
                var offerid = GetStringSha256Hash(fileid + "/-/" + sup.Id);
                var offer = _context.OfferMasters.FirstOrDefault(x => x.OfferId == offerid);


                //offer master save
                if (offer == null)
                {
                    var offermaster = new OfferMaster
                    {
                        OfferId = offerid,
                        FileId = fileid,
                        SupplierId = sup.Id
                    };

                    _context.OfferMasters.Add(offermaster);
                    _context.SaveChanges();

                    masterid = offermaster.Id;
                }
                else
                {
                    masterid = offer.Id;
                }


                // suplineitem 
                var supplierLineItem = new SupplierPriceLineItem
                {
                    OfferMasterId = masterid,
                    LineItemId = supplier.LineItemId,
                    CreatedDate = DateTime.Now.Date,
                    Vat = vat,
                    Comment = remark,
                    SupplierPrice = Convert.ToDecimal(price), 
                    Currency = currency
                };

                _context.SupplierPriceLineItems.Add(supplierLineItem);
                _context.SaveChanges();


                // supplier select 
                var lineitemsupplier = _context.ViewSupplierPriceLineItems.Include("OfferMaster").FirstOrDefault(s => s.Id == supplierLineItem.Id);
                 
                    if (lineitemsupplier != null)
                    {
                        lineitem.SelectedSupplierName = lineitemsupplier.SupplierName;
                        lineitem.SelectedSupplierCurrency = lineitemsupplier.Currency;
                        lineitem.SelectedSupplierId = lineitemsupplier.OfferMaster.SupplierId;
                        lineitem.SelectedSupplierPrice = lineitemsupplier.SupplierPrice;
                        lineitem.SelectedSupplierRemark = lineitemsupplier.Comment;
                        lineitem.SelectedSupplierCalculatedPrice = lineitemsupplier.RealPrice;
                        lineitem.IsWareHouseSelected = false;

                    _context.SaveChanges();
                    }
                 
                     
                return Json(true);


            }
            catch (Exception e)
            {
                return Json(false);
            }
        }


        public  decimal GetPriceByWarehouseAndImpa(string warehouse, string impa,int fileid)
        {
            
            decimal price = 0;

            try
            {

                switch (warehouse)
                {
                    case "4":
                        impa = impa + "%";
                        break;
                    case "2":
                        impa= impa + "%";
                        break;
                    case "5":
                        impa = "%" + impa;
                        break; 
                }

                if (warehouse=="4")
                {
                    
                }
               

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


        public  decimal GetQttyByWarehouseAndImpa(string warehouse, string impa)
        {
            
            int price = 0;

            try
            {
                switch (warehouse)
                {
                    case "4":
                        impa = impa + "%";
                        break;
                    case "2":
                        impa = impa + "%";
                        break;
                    case "5":
                        impa = "%" + impa;
                        break;
                }

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


                price = _context.Database.SqlQuery<int>(querry).FirstOrDefault();

                return price;

            }
            catch (Exception)
            {

                throw;
            }

        }



        public ActionResult SelectWarehouse(string warehouse, int lineitemid, int fileid)
        {
            if (warehouse.Length != 0 || lineitemid != 0 || fileid != 0)
            {
                try
                {
                    var lineitem = _context.LineItems.FirstOrDefault(l => l.Id == lineitemid);

                    string warehousename = "";

                    switch (warehouse)
                    {
                        case "4":warehousename = "Antrepo(Izmir)"; break;
                            
                        case "2":
                            warehousename = "Warehouse Izmir";
                            break;
                        case "5":
                            warehousename = "Warehouse Istabul";
                            break;
                    }

                    var warehouseprice = GetPriceByWarehouseAndImpa(warehouse, lineitem.Impa, fileid);

                    if (lineitem != null)
                    {
                         
                            lineitem.SelectedSupplierName = warehousename;                     
                            lineitem.SelectedSupplierId = warehouse;
                            lineitem.SelectedSupplierPrice = warehouseprice; 
                            lineitem.SelectedSupplierCalculatedPrice = warehouseprice;
                           lineitem.SelectedSupplierRemark = "";
                            lineitem.IsWareHouseSelected = true;

                             _context.SaveChanges();

                    }


                    var lineItem = _context.LineItems.Include("ReferanceNumber").FirstOrDefault(l => l.Id == lineitemid);
                    var file = _context.Files.FirstOrDefault(f => f.Id == lineItem.ReferanceNumber.FileId);
                    var lineitemsuppliers = _context.ViewSupplierPriceLineItems.Where(s => s.LineItemId == lineitemid).ToList();

                    var viewModelorg = new LineItemPricingEditViewModel
                    {
                        LineItemSuppliers = lineitemsuppliers,
                        LineItem = lineItem,
                        File = file
                    };
                    return Edit(viewModelorg);
                }
                catch (Exception e)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Supplier Selection ERROR !!!" + e.Message);
                }



            }

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Supplier Selection ERROR !!!");
        }


        public ActionResult ClearSupplier( int lineitemid, int fileid)
        {
            if (lineitemid != 0 )
            {
                try
                {
                    var lineitem = _context.LineItems.FirstOrDefault(l => l.Id == lineitemid);
 

                    if (lineitem != null)
                    {

                        lineitem.SelectedSupplierName = "";
                        lineitem.SelectedSupplierId = "";
                        lineitem.SelectedSupplierPrice = 0;
                        lineitem.SelectedSupplierCalculatedPrice = 0;
                        lineitem.SelectedSupplierRemark = ""; 
                        lineitem.IsWareHouseSelected = false;

                        _context.SaveChanges();

                    }


                    var lineItem = _context.LineItems.Include("ReferanceNumber").FirstOrDefault(l => l.Id == lineitemid);
                    var file = _context.Files.FirstOrDefault(f => f.Id == lineItem.ReferanceNumber.FileId);
                    var lineitemsuppliers = _context.ViewSupplierPriceLineItems.Where(s => s.LineItemId == lineitemid).ToList();

                    var viewModelorg = new LineItemPricingEditViewModel
                    {
                        LineItemSuppliers = lineitemsuppliers,
                        LineItem = lineItem,
                        File = file
                    };
                    return Edit(viewModelorg);
                }
                catch (Exception e)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Supplier Selection ERROR !!!" + e.Message);
                }



            }

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Supplier Selection ERROR !!!");
        }

        public RfqTotal GetRfqTotal(int RfqId) {


            RfqTotal rfqTotal = new RfqTotal();
            if (RfqId!=0)
            {
                var items = _context.LineItems.Where(l => l.ReferanceNumberId == RfqId && l.IsRemoved == false).ToList();
                var referance = _context.ReferanceNumbers.Include("File").FirstOrDefault(r => r.Id == RfqId);

                decimal costtotal = 0;
                decimal total = 0;
                decimal antrepototal = 0;
                decimal discount = referance.File.Discount;

                List<decimal> costs = new List<decimal>();

                foreach (var item in items)
                {
                    //burada alternatif kontrolü yapılacak
                    total = total + Convert.ToDecimal((item.AltPrice == null ? item.Price : item.AltPrice) * (item.AltPrice == null ? item.Qtty : item.AltQtty));
                    costtotal=costtotal+ Convert.ToDecimal(item.SelectedSupplierCalculatedPrice * (item.AltPrice == null ? item.Qtty : item.AltQtty));

                 
                    if (item.IsWareHouseSelected && item.SelectedSupplierId=="4")
                    {
                        antrepototal=antrepototal+ Convert.ToDecimal(item.SelectedSupplierCalculatedPrice * item.Qtty);
                    }
                }

                discount = total * (discount / 100);
                decimal grandtotal = total - discount;

                rfqTotal.AntrepoTotal = decimal.Round(antrepototal, 2, MidpointRounding.AwayFromZero);  
                rfqTotal.CostTotal = decimal.Round(costtotal, 2, MidpointRounding.AwayFromZero); 
                rfqTotal.Discount = decimal.Round(discount, 2, MidpointRounding.AwayFromZero); 
                rfqTotal.GrandTotal = decimal.Round(grandtotal, 2, MidpointRounding.AwayFromZero); 
                rfqTotal.Total = decimal.Round(total, 2, MidpointRounding.AwayFromZero) ;
                rfqTotal.RfqId =   RfqId;
            }

            return rfqTotal;
        }

        public ActionResult PricingCompleted(int fileid)
        {
            int logid = 0;
            try
            {
                var reflist = _context.ReferanceNumbers
                .Where(r => r.IsDeleted == false)
                .Where(f => f.FileId == fileid)
                .Where(f => f.Stage == 4).AsEnumerable();

                string rfqs = "";
              

                var file = _context.Files.FirstOrDefault(f => f.Id == fileid);
                var vessel = _context.Vessels.FirstOrDefault(v => v.Id == file.VesselId);


                List<string> ids = new List<string>();

                foreach (var item in reflist)
                {
                    item.Stage = 5;

                    rfqs = rfqs + item.Name + " ,";

                    ids.Add(item.Id.ToString());
                }

                if (rfqs.Length >2)
                {
                    rfqs = rfqs.Remove(rfqs.Length - 1);
                }

                _context.SaveChanges();

                // sale dep mail gönderme

                var toadress = new MailAddress("sales@adamarine.com");

                var emaillog = new EmailLog
                {
                    Description = "Sale Dpt.",
                    FileId = fileid,
                    Name = "rfqs:"+rfqs,
                    ToAdress = toadress.ToString(),
                    State = "Sending"
                };
                _context.EmailLogs.Add(emaillog);
                _context.SaveChanges();
                logid = emaillog.Id;


                string header = string.Format("File is Ready {1}- ( {0} )", file.FileNumber, vessel.VesselName);
                string body = string.Format("<strong> SALE TEAM </br> </strong> You could view details for {1}- {0} on the AppA Create Document Page. </br>   </br>  RFQs; {2}  </br>   </br> <b> Note;  {3} </b>", file.FileNumber, vessel.VesselName, rfqs, "");
                string link = string.Format(@"http://192.168.2.4:5559/Sales/File/{0}",file.Id);
                 
                List<MailAddress> ccadress = new List<MailAddress>();
                ccadress.Add(new MailAddress("info@adamarine.com"));
                ccadress.Add(new MailAddress("technical5@adamarine.com"));


                var temp = new EmailTemplates();
                var eMailbody =
                    temp.e_mailbody_fiyat_isteme(header, body, link, User.Identity.GetUserName(), "sales@adamarine.com");


                var t = Task.Run(async () =>
                {
                    await _emailer.SendEmail(header, eMailbody, toadress, ccadress, logid);
                });

                emaillog.State = "Sent";
                _context.SaveChanges();

                // download excel

                GenerateExcel gn = new Models.GenerateExcel();

               string filename = gn.NewQuotation(ids.ToArray(),true);

                return new JsonResult()
                {
                    Data = new { FileName = filename + ".xlsx" }
                };
            }
            catch (Exception e)
            {
                var errorMessage = e.Message;
                var emailLog = _context.EmailLogs.FirstOrDefault(x => x.Id == logid);
                if (emailLog != null) emailLog.State = "Error:  " + errorMessage;
                _context.SaveChanges();

                return Json(false);
            }
        }

  



    }
}