using Appa_MVC.Models;
using DataTables;
using Inspinia_MVC5_SeedProject.Email;
using Inspinia_MVC5_SeedProject.Email.Templates;
using Inspinia_MVC5_SeedProject.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Services;

namespace Appa_MVC.Controllers
{
    public class TechnicalSplitController : Controller
    {
        ApplicationDbContext _context = new ApplicationDbContext();
        readonly Emailer _emailer = new Emailer();

        // GET: TechnicalSplit
        public ActionResult Index(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var rfqs = _context.ReferanceNumbers.Where(r => (r.Stage == 6) && r.IsDeleted == false && r.FileId == id).ToList();
            var file = _context.Files.FirstOrDefault(f => f.Id == id);
            ViewBag.filename = file.FileNumber;

            SalesFileViewModel model = new SalesFileViewModel
            {
                ReferanceNumbers = rfqs,
                File = file
            };

            return View(model);
        }

        [WebMethod]
        public ActionResult SplitSelected(Array ids)
        {
            string QueryString = "ids=";  //?columns[]=firstname&columns[]=lastname&columns[]=age 

            foreach (var item in ids)
            {
                // QueryString = QueryString + "ids[]=" + item.ToString()+"&";
                QueryString = QueryString +  item.ToString() + ",";
            }
            if (QueryString.Length>2)
            {
                QueryString = QueryString.Remove(QueryString.Length - 1);
            }

            return new JsonResult()
            {
                Data = new { QueryString }
            };
        }

        public ActionResult Split(string ids)
        {
            try
            {
                List<int> RfqIds = ids.Split(',').Select(int.Parse).ToList();
                //var test = "";
                //foreach (var item in RfqIds)
                //{
                //    test = test + item.ToString() + " / ";
                //}

                if (RfqIds.Count > 0)
                {
                    int rfqid1 = Convert.ToInt32(RfqIds[0]);

                    var rfq = _context.ReferanceNumbers.FirstOrDefault(r => r.Id == rfqid1);

                    var reflist = _context.ReferanceNumbers.Where(r => RfqIds.Contains(r.Id)).AsEnumerable();

                    var file = _context.Files.FirstOrDefault(f => f.Id == rfq.FileId);

                    var itemlist = _context.OrderLineItems.Include("ReferanceNumber").Where(l => reflist.Contains(l.ReferanceNumber)).Where(l => l.IsRemoved == false).AsEnumerable();

                    List<RfqTotal> RfqTotals = new List<RfqTotal>();

                    foreach (var item in reflist)
                    {
                        RfqTotals.Add(GetRfqTotal(item.Id));
                    }


                    var splitOrderModel = new SplitOrderModel { ReferanceNumbers = reflist, OrderLineItems = itemlist, File = file, RfqTotals = RfqTotals, Querry = ids };


                    //ViewBag.test = test;
                    return View(splitOrderModel);
                }
                else
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
            }
            catch (Exception)
            {

                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
           
            
            
        }

        public RfqTotal GetRfqTotal(int RfqId)
        {


            RfqTotal rfqTotal = new RfqTotal();
            if (RfqId != 0)
            {
                var items = _context.OrderLineItems.Where(l => l.ReferanceNumberId == RfqId && l.IsRemoved == false).ToList();
                var referance = _context.ReferanceNumbers.Include("File").FirstOrDefault(r => r.Id == RfqId);

                decimal costtotal = 0;
                decimal total = 0;
                decimal antrepototal = 0;
                decimal discount = referance.File.Discount;

                foreach (var item in items)
                {
                    //burada alternatif kontrolü yapılacak
                    total = total + Convert.ToDecimal((item.AltPrice == null ? item.Price : item.AltPrice) * (item.AltPrice == null ? item.Qtty : item.AltQtty));
                    costtotal = costtotal + Convert.ToDecimal(item.SelectedSupplierCalculatedPrice * (item.AltPrice == null ? item.Qtty : item.AltQtty));

                    if (item.IsWareHouseSelected && item.SelectedSupplierId == "4")
                    {
                        antrepototal = antrepototal + Convert.ToDecimal(item.SelectedSupplierCalculatedPrice * item.Qtty);
                    }
                }

                discount = total * (discount / 100);
                decimal grandtotal = total - discount;

                rfqTotal.AntrepoTotal = decimal.Round(antrepototal, 2, MidpointRounding.AwayFromZero);
                rfqTotal.CostTotal = decimal.Round(costtotal, 2, MidpointRounding.AwayFromZero);
                rfqTotal.Discount = decimal.Round(discount, 2, MidpointRounding.AwayFromZero);
                rfqTotal.GrandTotal = decimal.Round(grandtotal, 2, MidpointRounding.AwayFromZero);
                rfqTotal.Total = decimal.Round(total, 2, MidpointRounding.AwayFromZero);
                rfqTotal.RfqId = RfqId;
            }

            return rfqTotal;
        }



        public ActionResult Remove(int lineitemid)
        {
            try
            {
                var lineitem = _context.OrderLineItems.FirstOrDefault(l => l.Id == lineitemid);
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

        private String Number2String(int number, bool isCaps)

        {

            Char c = (Char)((isCaps ? 65 : 97) + (number - 1));

            return c.ToString();

        }

        public void AddExistingSuppliers(string impa, int itemid, int fileid)
        {
            try
            {


                var suppliers = _context.LineItemSuppliers.Where(x => x.LineItem.Impa == impa && x.ImpaChanged == false).GroupBy(x => x.SupplierId)
            .Select(g => new
            {
                g.FirstOrDefault().SupplierId,
                g.FirstOrDefault().SupplierName
            })
            .ToList();


                foreach (var sup in suppliers)
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


                    var lastpricelist = _context.SupplierPriceLineItems.Include("OfferMaster").Include("LineItem").Where(l =>
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

        public ActionResult Alternative(int lineitemid, string description, string impa, string unit, string qtty, bool isalternative, string number)
        {
            try
            {
                var lineitem = _context.OrderLineItems.FirstOrDefault(l => l.Id == lineitemid);
                int referanceno = lineitem.ReferanceNumberId;
                var referance = _context.ReferanceNumbers.FirstOrDefault(r => r.Id == referanceno);

                var pricinglineitem = new LineItem();
                if (impa.Length > 2)
                {
                    pricinglineitem.Impa = impa;
                }
                else
                {
                    pricinglineitem.Impa = lineitem.Impa;
                }

                if (unit.Length > 0)
                {
                    pricinglineitem.Unit = unit;
                }
                else
                {
                    pricinglineitem.Unit = lineitem.Unit;
                }


                if (qtty.Length > 0)
                {
                    pricinglineitem.Qtty = Convert.ToDecimal(qtty);
                }
                else
                {
                    pricinglineitem.Qtty = lineitem.Qtty;
                }

                pricinglineitem.IsRemoved = true;
                pricinglineitem.Remark = "Added on Split Stage";
                pricinglineitem.ReferanceNumberId = referanceno; 
                pricinglineitem.Description = description;
                pricinglineitem.Number = lineitem.Number;
                _context.LineItems.Add(pricinglineitem);
                _context.SaveChanges();

                AddExistingSuppliers(pricinglineitem.Impa, pricinglineitem.Id, referance.FileId);

                if (isalternative)
                {

                    var countofalternatives = _context.OrderLineItems.Where(l => l.MasterItemId == lineitemid).ToList();

                    lineitem.IsAlternative = true;
                    lineitem.MasterNumber = lineitem.Number;
                    lineitem.Number = lineitem.Number + "-" + Number2String((countofalternatives.Count + 1), true);
                    lineitem.MasterItemId = lineitem.Id;
                    lineitem.LineitemId = pricinglineitem.Id;



                    if (impa.Length > 2)
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
                    lineitem = new OrderLineItem();
                    lineitem.Qtty = Convert.ToDecimal(qtty);
                    lineitem.Number = number;
                    lineitem.Impa = impa;
                    lineitem.Description = description;
                    lineitem.Unit = unit;
                    lineitem.ReferanceNumberId = referanceno;
                    lineitem.LineitemId = pricinglineitem.Id;
                }


                _context.OrderLineItems.Add(lineitem);
                _context.SaveChanges();

 

                return Json(true);


            }
            catch (Exception e)
            {
                return Json(false);
            }
        }


        public ActionResult UpdatePo(int refid, string ponumber)
        {
            try
            {
                var rfq = _context.ReferanceNumbers.FirstOrDefault(r => r.Id == refid); 
                rfq.PONumber = ponumber;
                _context.SaveChanges();

                return Json(true);
            }
            catch (Exception)
            {

                return Json(false); 
            }
        }

        public ActionResult SendMailButton(int fileid, string querry)
        {
            try
            {
                
                List<int> RfqIds = querry.Split(',').Select(int.Parse).ToList();

                foreach (var item in RfqIds)
                {
                    var rfq = _context.ReferanceNumbers.FirstOrDefault(r => r.Id == item);
                    rfq.IsSplitted = true;
                    _context.SaveChanges();
                }
                 

                return Json(true);
            }
            catch (Exception)
            {

                return Json(false);
            }
        }

        public ActionResult SendMails(string ids)
        {
            try
            {
                List<int> RfqIds = ids.Split(',').Select(int.Parse).ToList();
                int rfqid1 = Convert.ToInt32(RfqIds[0]);

                var rfq = _context.ReferanceNumbers.FirstOrDefault(r => r.Id == rfqid1);
                var reflist = _context.ReferanceNumbers.Where(r => RfqIds.Contains(r.Id)).AsEnumerable();
                var file = _context.Files.FirstOrDefault(f => f.Id == rfq.FileId);
                var itemlist = _context.OrderLineItems.Include("ReferanceNumber").Where(l => reflist.Contains(l.ReferanceNumber)).Where(l => l.IsRemoved == false).AsEnumerable();

                var suppliers = itemlist.Where(i=>i.IsWareHouseSelected==false).GroupBy(x => x.SelectedSupplierId)
                .Select(g => new
                {
                    g.FirstOrDefault().SelectedSupplierId,
                    g.FirstOrDefault().SelectedSupplierName  
                })
               .ToList();

                var selectedWareHouses = itemlist.Where(i=>i.IsWareHouseSelected==true).GroupBy(x => x.SelectedSupplierId)
                .Select(g => new
                {
                    g.FirstOrDefault().SelectedSupplierId,
                    g.FirstOrDefault().SelectedSupplierName  
                })
               .ToList();

                var allsuppliers = _context.SuppliersCities.ToList();

                List<SupplierCityViewModel> selectedsupplierlist = new List<SupplierCityViewModel>();
                List<SupplierCityViewModel> selectedWarehouselist = new List<SupplierCityViewModel>();

                List<MailLogView> rfqMailLoglist = new List<MailLogView>();
                List<Supplier> ReceiverIds = new List<Supplier>();

                foreach (var supplier in suppliers)
                {
                    SupplierCityViewModel supcity = new SupplierCityViewModel
                    {
                        SupplierId = supplier.SelectedSupplierId,
                        SupplierName = supplier.SelectedSupplierName
                    };

                    if (supplier.SelectedSupplierId!=null)
                    {
                        var emailsup = _context.Suppliers.FirstOrDefault(x => x.Id == supplier.SelectedSupplierId);
                        supcity.Email = emailsup.Mail;

                        var durum = allsuppliers.FirstOrDefault(s => s.SupplierId.Contains(supplier.SelectedSupplierId));

                       if (durum!=null)
                       {
                            supcity.City = durum.SupplierCity;
                       }
                    }

                 

                    var sup = new Supplier
                    {
                        Id = supplier.SelectedSupplierId,
                        SupplierName = supplier.SelectedSupplierName
                    };

                    ReceiverIds.Add(sup);

                    selectedsupplierlist.Add(supcity);

                  //  
                }

                foreach (var warehouse in selectedWareHouses)
                {
                    SupplierCityViewModel supcity = new SupplierCityViewModel
                    {
                        SupplierId = warehouse.SelectedSupplierId,
                        SupplierName = warehouse.SelectedSupplierName
                    };
                    selectedWarehouselist.Add(supcity);

                    var sup = new Supplier
                    {
                        Id = warehouse.SelectedSupplierId,
                        SupplierName = warehouse.SelectedSupplierName
                    };

                    ReceiverIds.Add(sup);
                }


                var cities = selectedsupplierlist.Where(x => x.City != 0).GroupBy(x => x.City)
                  .Select(g => new
                  {
                      g.FirstOrDefault().City
                  })
                 .ToList();

                foreach (var city in cities)
                {
                    CityList.Cities curr = (CityList.Cities)city.City;
                    var sup = new Supplier
                    {
                        Id = city.City.ToString(),
                        SupplierName = curr.ToString()
                    };

                    ReceiverIds.Add(sup);
                }

                foreach (var receiver in ReceiverIds)
                {
                    MailLogView mailLogView = new MailLogView();
                    bool state =true;
                    foreach (var rfqid in RfqIds)
                    {
                        var maillog = _context.RfqMailLogs.FirstOrDefault(m => m.ReceiverId == receiver.Id && m.RfqId == rfqid);

                        if (maillog!=null)
                        {
                            state = state && true;
                        }
                        else
                        {
                            state = false;
                        }
                    }

                    mailLogView.MailSent = state.ToString();
                    mailLogView.Name = receiver.SupplierName;

                    rfqMailLoglist.Add(mailLogView);
                    
                }

                var TechnicalSplitSendMailViewModel = new TechnicalSplitSendMailViewModel
                {
                    LineItemSuppliers = selectedsupplierlist,
                    ReferanceNumbers = reflist,
                    File = file,
                    OrderLineItems = itemlist,
                    Querry = ids,
                    SelectedWarehouses = selectedWarehouselist,
                    RfqMailLogs = rfqMailLoglist
                };



                ViewBag.ids = ids;
                return View(TechnicalSplitSendMailViewModel);
            }
            catch (Exception)
            {

                throw;
            }
     
        }


        public ActionResult Edit(int? id, int? fileid, string querry)
        {
            if (id == null || fileid == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Line item id and File id should be selected !!!");
            }

            var orderlineitem = _context.OrderLineItems.Include("ReferanceNumber").FirstOrDefault(l => l.Id == id);
            var lineItem = _context.LineItems.Include("ReferanceNumber").FirstOrDefault(l => l.Id == orderlineitem.LineitemId); 
            var file = _context.Files.FirstOrDefault(f => f.Id == orderlineitem.ReferanceNumber.FileId);
            List<ViewSupplierPriceLineItem> lineitemsuppliers = new List<ViewSupplierPriceLineItem>();

            if (lineItem!=null)
            {
                lineitemsuppliers= _context.ViewSupplierPriceLineItems.Where(s => s.LineItemId == lineItem.Id).ToList();
            }
                

            var viewModel = new OrderedLineItemEditViewModel
            {
                LineItemSuppliers = lineitemsuppliers,
                LineItem = lineItem,
                File = file,
                OrderLineItem=orderlineitem,
                Querry=querry
            };


            return View(viewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(OrderedLineItemEditViewModel viewModel)
        {
            var orderlineitem = _context.OrderLineItems.Include("ReferanceNumber").FirstOrDefault(l => l.Id == viewModel.OrderLineItem.Id);
            var lineItem2 = _context.LineItems.Include("ReferanceNumber").FirstOrDefault(l => l.Id == viewModel.OrderLineItem.LineitemId);
            var file2 = _context.Files.FirstOrDefault(f => f.Id == viewModel.File.Id);
            var lineitemsuppliers2 = _context.ViewSupplierPriceLineItems.Where(s => s.LineItemId == viewModel.OrderLineItem.LineitemId).ToList();

            var viewModel2 = new OrderedLineItemEditViewModel
            {
                LineItemSuppliers = lineitemsuppliers2,
                LineItem = lineItem2,
                File = file2,
                OrderLineItem = orderlineitem,
                Querry = viewModel.Querry
            };


            if (ModelState.IsValid)
            {

                var EditedItem = _context.OrderLineItems.First(x => x.Id == viewModel.OrderLineItem.Id);

                string newimpa = viewModel.OrderLineItem.Impa;
                string oldimpa = EditedItem.Impa;

                EditedItem.Impa = viewModel.OrderLineItem.Impa;
                EditedItem.Description = viewModel.OrderLineItem.Description;
                EditedItem.Number = viewModel.OrderLineItem.Number;
                EditedItem.Qtty = viewModel.OrderLineItem.Qtty;
                EditedItem.Unit = viewModel.OrderLineItem.Unit;
                EditedItem.AltQtty = viewModel.OrderLineItem.AltQtty;
                EditedItem.AltUnit = viewModel.OrderLineItem.AltUnit;
                EditedItem.AltPrice = viewModel.OrderLineItem.AltPrice;
                EditedItem.Remark = viewModel.OrderLineItem.Remark;
                EditedItem.Price = viewModel.OrderLineItem.Price;

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

                  //  AddExistingSuppliers(EditedItem.Impa, EditedItem.Id, referance.FileId);

                    // AddDefinedSuppliers(EditedItem.Impa, EditedItem.Id,referance.FileId).GetAwaiter();


                }


                //  return RedirectToAction("Index", new { id = viewModel.LineItem.ReferanceNumber.FileId });


            

                return View("Edit", viewModel2);
            }


            return View("Edit", viewModel2);



        }

        public decimal GetPriceByWarehouseAndImpa(string warehouse, string impa, int fileid)
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


        public decimal GetQttyByWarehouseAndImpa(string warehouse, string impa)
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


                price = _context.Database.SqlQuery<int>(querry).FirstOrDefault();

                return price;

            }
            catch (Exception)
            {

                throw;
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

                var orderedlineitem = _context.OrderLineItems.FirstOrDefault(o => o.LineitemId == lineitem.Id);

                if (orderedlineitem != null)
                {
                    orderedlineitem.SelectedSupplierName = lineitemsupplier.SupplierName;
                    orderedlineitem.SelectedSupplierCurrency = lineitemsupplier.Currency;
                    orderedlineitem.SelectedSupplierId = lineitemsupplier.OfferMaster.SupplierId;
                    orderedlineitem.SelectedSupplierPrice = lineitemsupplier.SupplierPrice;
                    orderedlineitem.SelectedSupplierRemark = lineitemsupplier.Comment;
                    orderedlineitem.SelectedSupplierCalculatedPrice = lineitemsupplier.RealPrice;
                    orderedlineitem.IsWareHouseSelected = false;

                    _context.SaveChanges();
                }


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

        public ActionResult SelectSupplier(int id, int lineitemid, int fileid,int modelid,string querry)
        {
            if (id != 0 || lineitemid != 0 || fileid != 0)
            {
                try
                {
                    var orderlineitem = _context.OrderLineItems.FirstOrDefault(l => l.Id == modelid);
                    var lineitem = _context.LineItems.FirstOrDefault(l => l.Id == lineitemid);
                    var lineitemsupplier = _context.ViewSupplierPriceLineItems.Include("OfferMaster").FirstOrDefault(s => s.Id == id);


                    if (lineitem != null)
                    {
                        if (lineitemsupplier != null)
                        {
                            orderlineitem.SelectedSupplierName = lineitemsupplier.SupplierName;
                            orderlineitem.SelectedSupplierCurrency = lineitemsupplier.Currency;
                            orderlineitem.SelectedSupplierId = lineitemsupplier.OfferMaster.SupplierId;
                            orderlineitem.SelectedSupplierPrice = lineitemsupplier.SupplierPrice;
                            orderlineitem.SelectedSupplierRemark = lineitemsupplier.Comment;
                            orderlineitem.SelectedSupplierCalculatedPrice = lineitemsupplier.RealPrice;
                            orderlineitem.IsWareHouseSelected = false;
                            

                            if (orderlineitem.MailSent)
                            {
                                orderlineitem.OnRevision = true;
                            }

                            _context.SaveChanges();
                        }
                    }


                    
                    var lineItem = _context.LineItems.Include("ReferanceNumber").FirstOrDefault(l => l.Id == orderlineitem.LineitemId);
                    var file = _context.Files.FirstOrDefault(f => f.Id == lineItem.ReferanceNumber.FileId);
                    var lineitemsuppliers = _context.ViewSupplierPriceLineItems.Where(s => s.LineItemId == lineItem.Id).ToList();

                    var viewModel = new OrderedLineItemEditViewModel
                    {
                        LineItemSuppliers = lineitemsuppliers,
                        LineItem = lineItem,
                        File = file,
                        OrderLineItem = orderlineitem,
                        Querry=querry
                    };

                    return View("Edit", viewModel);
                   
                }
                catch (Exception e)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Supplier Selection ERROR !!!" + e.Message);
                }



            }

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Supplier Selection ERROR !!!");
        }

        public ActionResult SelectWarehouse(string warehouse, int lineitemid, int fileid, int orderedid, string querry)
        {
            if (warehouse.Length != 0 || lineitemid != 0 || fileid != 0)
            {
                try
                {
                    var orderlineitem = _context.OrderLineItems.FirstOrDefault(o => o.Id == orderedid);
                    var lineitem = _context.LineItems.FirstOrDefault(l => l.Id == lineitemid);

                    string warehousename = "";

                    switch (warehouse)
                    {
                        case "4": warehousename = "Antrepo(Izmir)"; break;

                        case "2":
                            warehousename = "Warehouse Izmir";
                            break;
                        case "5":
                            warehousename = "Warehouse Istabul";
                            break;
                    }

                    var warehouseprice = GetPriceByWarehouseAndImpa(warehouse, lineitem.Impa, fileid);

                    if (orderlineitem != null)
                    {

                        orderlineitem.SelectedSupplierName = warehousename;
                        orderlineitem.SelectedSupplierId = warehouse;
                        orderlineitem.SelectedSupplierPrice = warehouseprice;
                        orderlineitem.SelectedSupplierCalculatedPrice = warehouseprice;
                        orderlineitem.SelectedSupplierRemark = "";
                        orderlineitem.IsWareHouseSelected = true;


                        if (orderlineitem.MailSent)
                        {
                            orderlineitem.OnRevision = true;
                        }

                        _context.SaveChanges();

                    }



                    var lineItem = _context.LineItems.Include("ReferanceNumber").FirstOrDefault(l => l.Id == orderlineitem.LineitemId);
                    var file = _context.Files.FirstOrDefault(f => f.Id == lineItem.ReferanceNumber.FileId);
                    var lineitemsuppliers = _context.ViewSupplierPriceLineItems.Where(s => s.LineItemId == lineItem.Id).ToList();

                    var viewModel = new OrderedLineItemEditViewModel
                    {
                        LineItemSuppliers = lineitemsuppliers,
                        LineItem = lineItem,
                        File = file,
                        OrderLineItem = orderlineitem,
                        Querry=querry
                    };

                    return View("Edit", viewModel);
                }
                catch (Exception e)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Supplier Selection ERROR !!!" + e.Message);
                }



            }

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Supplier Selection ERROR !!!");
        }


        public ActionResult EditRfq(int id, string querry)
        {
            if (id != 0)
            {
                var referance = _context.ReferanceNumbers.FirstOrDefault(r => r.Id == id);
                ViewBag.querry = querry;
                return View(referance);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        public ActionResult GetRfqItems(int id)
        {

            var formData = HttpContext.Request.Form;

            using (var db = new DataTables.Database("sqlserver", @"Data Source=192.168.2.4;Initial Catalog=AppaMVC; User=PLANET; Password=PLANET74110;"))
            {
                var response = new Editor(db, "OrderLineItems")
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
                        .Validator(Validation.Numeric())
                        .SetFormatter(Format.IfEmpty(null))
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

                    .Where("ReferanceNumberId", id)
                    .Where("IsRemoved","0")
                    .Process(formData)
                    .Data();

                return Json(response, JsonRequestBehavior.AllowGet);

            }
        }


        public ActionResult UpdateCity(string supplier, int city)
        {
            try
            {
                var supp = _context.SuppliersCities.FirstOrDefault(r => r.SupplierId== supplier);

                if (supp!=null)
                {
                    supp.SupplierCity = city;
                }
                else
                {
                    SuppliersCity supnew = new SuppliersCity();
                    supnew.SupplierCity = city;
                    supnew.SupplierId = supplier;
                    _context.SuppliersCities.Add(supnew);
                }
               
                _context.SaveChanges();

                return Json(true);
            }
            catch (Exception)
            {

                return Json(false);
            }
        }

        public ActionResult Send_mail_toCity(int cityid, string querry, string note, bool shipyards)
        {
            try
            {
                List<int> RfqIds = querry.Split(',').Select(int.Parse).ToList();

                var reflist = _context.ReferanceNumbers.Where(r => RfqIds.Contains(r.Id)).AsEnumerable();
                var itemlist = _context.OrderLineItems.Include("ReferanceNumber").Where(l => reflist.Contains(l.ReferanceNumber)).Where(l => l.IsRemoved == false).AsEnumerable();
                var suppliers = itemlist.Where(i => i.IsWareHouseSelected == false).GroupBy(x => x.SelectedSupplierId)
                .Select(g => new
                {
                    g.FirstOrDefault().SelectedSupplierId,
                    g.FirstOrDefault().SelectedSupplierName
                })
                .ToList();

                List<SupplierCityViewModel> selectedsupplierlist = new List<SupplierCityViewModel>();
                var allsuppliers = _context.SuppliersCities.ToList();

                foreach (var supplier in suppliers)
                {
                    SupplierCityViewModel supcity = new SupplierCityViewModel
                    {
                        SupplierId = supplier.SelectedSupplierId,
                        SupplierName = supplier.SelectedSupplierName
                    };

                    if (supplier.SelectedSupplierId!=null)
                    {
                        var durum = allsuppliers.FirstOrDefault(s => s.SupplierId.Contains(supplier.SelectedSupplierId));

                        if (durum != null)
                        {
                            supcity.City = durum.SupplierCity;
                        }
                    } 

                    selectedsupplierlist.Add(supcity);

                    //  
                }

                // supplier in that city 

                var suppliersincity = selectedsupplierlist.Where(s => s.City == cityid).GroupBy(x => x.SupplierId)
              .Select(g => new Supplier
              {
                  Id = g.FirstOrDefault().SupplierId
              })
             .ToList();

                List<string> supids = new List<string>();

                foreach (var supid in suppliersincity)
                {
                    supids.Add(supid.Id);
                }

                var items = itemlist.Where(p => supids.Contains(p.SelectedSupplierId)).OrderBy(l => l.ReferanceNumberId).ThenBy(l => l.MasterNumber == "0" ? int.Parse(l.Number) : int.Parse(l.MasterNumber)).ToList();



                // mail sending 

                int logid = 0;

                var fileid = reflist.First().FileId;
             
                var file = _context.Files.FirstOrDefault(f => f.Id == fileid);
                var vessel = _context.Vessels.FirstOrDefault(v => v.Id == file.VesselId);

                var temp = new EmailTemplates();
                var eMailbody =
                    temp.e_mailbody_technical_split_city(items, file, note, User.Identity.GetUserName(), vessel);

                var toadress = new MailAddress("technicalmailgroup@adamarine.com");

                var emaillog = new EmailLog
                {
                    Description = "OFfice Mail " + cityid,
                    FileId = file.Id,
                    Name = "rfqs:" + querry,
                    ToAdress = toadress.ToString(),
                    State = "Sending"
                };
                _context.EmailLogs.Add(emaillog);
                _context.SaveChanges();
                logid = emaillog.Id;

              
                string warehousename = "";

                switch (cityid)
                {
                    case 34: warehousename = "Istanbul Ofis"; break;

                    case 35:
                        warehousename = "Izmir Ofis";
                        break;
                    case 31:
                        warehousename = "Iskenderun Ofis";
                        break;
                }

                string subject = vessel.VesselName + " " + file.FileNumber + " " + warehousename + " Siparis";

                string header = "ADAMAR SİPARİŞ / " + file.FileNumber;

                header = subject;



                List<MailAddress> ccadress = new List<MailAddress>
                {
                     new MailAddress("info@adamarine.com")
                };

                if (cityid==34)
                {
                    ccadress.Add(new MailAddress( "istanbul@adamarine.com"));
                    ccadress.Add(new MailAddress( "istanbul1@adamarine.com"));
                    ccadress.Add(new MailAddress( "tuzla.operation@adamarine.com"));
                    ccadress.Add(new MailAddress( "istanbul.muhasebe@adamarine.com")); 
                    ccadress.Add(new MailAddress( "thakyildiz@adamarine.com"));
                    ccadress.Add(new MailAddress( "operation@adamarine.com"));
                    ccadress.Add(new MailAddress( "operation1@adamarine.com"));
                    ccadress.Add(new MailAddress( "tuzla.operation1@adamarine.com"));
                    ccadress.Add(new MailAddress( "istanbul.muhasebe1@adamarine.com"));
                }

                if (cityid==35)
                {
                    ccadress.Add(new MailAddress( "warehouse@adamarine.com"));
                    ccadress.Add(new MailAddress("warehouse1@adamarine.com"));
                    ccadress.Add(new MailAddress( "freezone1@adamarine.com"));
                    ccadress.Add(new MailAddress( "freezone@adamarine.com"));
                    ccadress.Add(new MailAddress( "customs@adamarine.com"));
                    ccadress.Add(new MailAddress( "customs1@adamarine.com"));
                    ccadress.Add(new MailAddress( "operation@adamarine.com"));
                    ccadress.Add(new MailAddress( "operation1@adamarine.com")); 
                    ccadress.Add(new MailAddress( "carrier@adamarine.com"));
                }

                if (cityid == 31)
                {
                    ccadress.Add(new MailAddress("iskenderun@adamarine.com"));
                    
                }


                if (shipyards)
                {
                    ccadress.Add(new MailAddress("shipyards@adamarine.com"));
                }

                //ccadress.Clear();
                //toadress = new MailAddress("bkardas@adamarine.com");


                var t = Task.Run(async () =>
                {
                    await _emailer.SendEmail(header, eMailbody, toadress, ccadress, logid);
                });

                emaillog.State = "Sent";
                _context.SaveChanges();

                // mail sending 


                //mail sent and on revision false
                foreach (var item in items)
                {
                    item.MailSent = true;
                    item.OnRevision = false;
                }
                //mail sent and on revision false


                //mail log 

                foreach (var rfqid in RfqIds)
                {
                    var rfqmaillog = new RfqMailLog
                    {
                        ReceiverId = cityid.ToString(),
                        RfqId = rfqid,
                        User = User.Identity.GetUserName()
                    };

                    _context.RfqMailLogs.Add(rfqmaillog);
                }

                //mail log 

                _context.SaveChanges();

                return Json(true);
            }
            catch (Exception)
            {

                return Json(false);  
            }
            
        }


        public ActionResult Send_mail_toWarehouse(string warehouseid, string querry, string note)
        {
            try
            {
                List<int> RfqIds = querry.Split(',').Select(int.Parse).ToList();

                var reflist = _context.ReferanceNumbers.Where(r => RfqIds.Contains(r.Id)).AsEnumerable();
                var itemlist = _context.OrderLineItems.Include("ReferanceNumber").Where(l => reflist.Contains(l.ReferanceNumber)).Where(l => l.IsRemoved == false).AsEnumerable();
          
                var items = itemlist.Where(p => p.SelectedSupplierId==warehouseid).OrderBy(l=>l.ReferanceNumberId).ThenBy(l => l.MasterNumber == "0" ? int.Parse(l.Number) : int.Parse(l.MasterNumber)).ToList();


                var fileid = reflist.First().FileId;

                // mail sending 

                int logid = 0;

            
                var file = _context.Files.FirstOrDefault(f => f.Id == fileid);
                var vessel = _context.Vessels.FirstOrDefault(v => v.Id == file.VesselId);

                var temp = new EmailTemplates();
                var eMailbody =
                    temp.e_mailbody_technical_split_warehouse(items, file, note, User.Identity.GetUserName(),vessel);

                var toadress = new MailAddress("technicalmailgroup@adamarine.com");

                var emaillog = new EmailLog
                {
                    Description = "Split WareHouse" + warehouseid,
                    FileId = file.Id,
                    Name = "rfqs:" + querry,
                    ToAdress = toadress.ToString(),
                    State = "Sending"
                };
                _context.EmailLogs.Add(emaillog);
                _context.SaveChanges();
                logid = emaillog.Id;

              

                string warehousename = "";

                switch (warehouseid)
                {
                    case "4": warehousename = "Antrepo(Izmir)"; break;

                    case "2":
                        warehousename = "Warehouse Izmir";
                        break;
                    case "5":
                        warehousename = "Warehouse Istabul";
                        break;
                }

                string subject = vessel.VesselName + " " + file.FileNumber + " " +warehousename+" Siparis";

                string header = "ADAMAR SİPARİŞ / " + file.FileNumber;

                header = subject;

                List<MailAddress> ccadress = new List<MailAddress>
                {
                      new MailAddress("info@adamarine.com")
                };


                if (warehouseid=="4")
                {
                    ccadress.Add(new MailAddress( "warehouse@adamarine.com"));
                    ccadress.Add(new MailAddress( "freezone1@adamarine.com"));
                    ccadress.Add(new MailAddress( "freezone@adamarine.com"));
                    ccadress.Add(new MailAddress( "customs@adamarine.com"));
                    ccadress.Add(new MailAddress( "operation@adamarine.com"));
                    ccadress.Add(new MailAddress( "operation1@adamarine.com")); 
                    ccadress.Add(new MailAddress( "istanbul@adamarine.com"));
                    ccadress.Add(new MailAddress("warehouse2@adamarine.com"));

                }

                if (warehouseid=="2")
                {
                    ccadress.Add(new MailAddress( "warehouse@adamarine.com"));
                    ccadress.Add(new MailAddress( "freezone1@adamarine.com"));
                    ccadress.Add(new MailAddress( "freezone@adamarine.com"));
                    ccadress.Add(new MailAddress( "customs@adamarine.com"));
                    ccadress.Add(new MailAddress( "customs1@adamarine.com"));
                    ccadress.Add(new MailAddress( "operation@adamarine.com"));
                    ccadress.Add(new MailAddress( "operation1@adamarine.com"));
                    ccadress.Add(new MailAddress( "fozmen@adamarine.com"));
                    ccadress.Add(new MailAddress( "warehouse1@adamarine.com"));
                }

                if (warehouseid == "5")
                {
                    ccadress.Add(new MailAddress("istanbul@adamarine.com"));
                    ccadress.Add(new MailAddress("istanbul1@adamarine.com"));
                    ccadress.Add(new MailAddress("tuzla.operation@adamarine.com"));
                    ccadress.Add(new MailAddress("istanbul.muhasebe@adamarine.com"));
                    ccadress.Add(new MailAddress("thakyildiz@adamarine.com"));
                    ccadress.Add(new MailAddress("operation@adamarine.com"));
                    ccadress.Add(new MailAddress("operation1@adamarine.com"));
                    ccadress.Add(new MailAddress("tuzla.operation1@adamarine.com"));
                    ccadress.Add(new MailAddress("istanbul.muhasebe1@adamarine.com"));

                }


                //if (shipyards)
                //{
                //    ccadress.Add(new MailAddress("shipyards@adamarine.com"));
                //}

                //ccadress.Clear();
                //toadress= new MailAddress("bkardas@adamarine.com");

                var t = Task.Run(async () =>
                {
                    await _emailer.SendEmail(header, eMailbody, toadress, ccadress, logid);
                });

                emaillog.State = "Sent";
                _context.SaveChanges();

                // mail sending 


                //mail sent and on revision false
                foreach (var item in items)
                {
                    item.MailSent = true;
                    item.OnRevision = false;
                }
                //mail sent and on revision false


                //mail log 

                foreach (var rfqid in RfqIds)
                {
                    var rfqmaillog = new RfqMailLog
                    {
                        ReceiverId = warehouseid,
                        RfqId = rfqid,
                        User = User.Identity.GetUserName()
                    };

                    _context.RfqMailLogs.Add(rfqmaillog);
                }

                //mail log 

                _context.SaveChanges();

                return Json(true);
            }
            catch (Exception)
            {

                return Json(false);
            }

        }


        public ActionResult Send_mail_toSuppliere(string suppliereid, string querry,string note,string email)
        {
            try
            {

                
                    string tablename = Netsis.database + "..tblcasabit";

                    string sqlcommand = string.Format(@"update {2} set email='{0}' where cari_kod='{1}'", email, suppliereid, tablename);

                    _context.Database.ExecuteSqlCommand(sqlcommand);
                 

                List<int> RfqIds = querry.Split(',').Select(int.Parse).ToList();

                var reflist = _context.ReferanceNumbers.Include("File").Where(r => RfqIds.Contains(r.Id)).AsEnumerable();
                var itemlist = _context.OrderLineItems.Include("ReferanceNumber").Where(l => reflist.Contains(l.ReferanceNumber)).Where(l => l.IsRemoved == false).AsEnumerable();

                var items = itemlist.Where(p => p.SelectedSupplierId == suppliereid).OrderBy(l=>l.ReferanceNumberId).ThenBy(l => l.MasterNumber == "0" ? int.Parse(l.Number) : int.Parse(l.MasterNumber)).ToList();

                var fileid = reflist.First().FileId;

                // mail sending 

                int logid = 0;

                var supplier = _context.Suppliers.FirstOrDefault(s => s.Id == suppliereid);
                var file = _context.Files.FirstOrDefault(f => f.Id == fileid);
                 
                var temp = new EmailTemplates();
                var eMailbody =
                    temp.e_mailbody_technical_split_supplier(items, file, note, supplier, User.Identity.GetUserName());

                var toadress = new MailAddress(email);

                var emaillog = new EmailLog
                {
                    Description = "Supplier" +supplier.SupplierName,
                    FileId = file.Id,
                    Name = "rfqs:" + querry,
                    ToAdress = toadress.ToString(),
                    State = "Sending"
                };
                _context.EmailLogs.Add(emaillog);
                _context.SaveChanges();
                logid = emaillog.Id;

                string header = "ADAMAR SİPARİŞ / " + file.FileNumber;

                List<MailAddress> ccadress = new List<MailAddress>
                {
                   new MailAddress("technicalmailgroup@adamarine.com"),
                    new MailAddress("info@adamarine.com")
                };


                //ccadress.Clear();
                //toadress = new MailAddress("bkardas@adamarine.com");


                var t = Task.Run(async () =>
                {
                    await _emailer.SendEmail(header, eMailbody, toadress, ccadress, logid);
                });

                emaillog.State = "Sent";
                _context.SaveChanges();

                // mail sending 


                //mail sent and on revision false
                foreach (var item in items)
                {
                    item.MailSent = true;
                    item.OnRevision = false;
                }
                //mail sent and on revision false


                //mail log 

                foreach (var rfqid in RfqIds)
                {
                    var rfqmaillog = new RfqMailLog
                    {
                        ReceiverId = suppliereid,
                        RfqId = rfqid,
                        User = User.Identity.GetUserName()
                    };

                    _context.RfqMailLogs.Add(rfqmaillog);
                }

                //mail log 

                _context.SaveChanges();

                return Json(true);
            }
            catch (Exception e)
            {

                return Json(e.Message);
            }

        }


        public ActionResult SplitExcel(int fileid, string querry)
        {
            int logid = 0;
            try
            {
                List<int> RfqIds = querry.Split(',').Select(int.Parse).ToList();

                var reflist = _context.ReferanceNumbers.Include("File").Where(r => RfqIds.Contains(r.Id)).AsEnumerable();

                string rfqs = "";


                var file = _context.Files.FirstOrDefault(f => f.Id == fileid);
                var vessel = _context.Vessels.FirstOrDefault(v => v.Id == file.VesselId);


                List<string> ids = new List<string>();

                foreach (var item in reflist)
                {
                     
                    rfqs = rfqs + item.Name + " ,";

                    ids.Add(item.Id.ToString());
                }

                if (rfqs.Length > 2)
                {
                    rfqs = rfqs.Remove(rfqs.Length - 1);
                }
                 

                // download excel

                GenerateExcel gn = new Models.GenerateExcel();

                string filename = gn.SplitQuotation(ids.ToArray(), true);

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