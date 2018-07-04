using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace Appa_MVC.Models
{
    public class GenerateExcel
    {
        ApplicationDbContext _context = new ApplicationDbContext();

        public string NewQuotation(Array rfqids,bool withprice=false)
        {
            string filename = "";

            if (rfqids.Length>0)
            {
              using (ExcelPackage excel = new ExcelPackage())
                    {
                        string identifier = Guid.NewGuid().ToString();

                    // FileInfo newFile = new FileInfo("D:\\Sample\\sample1.xlsx");

                        FileInfo newFile = null;

                    if (!withprice)
                    {
                         newFile = new FileInfo(HostingEnvironment.MapPath("~/Files/Samples/Quotation.xlsx"));
                    }

                    if (withprice)
                    {
                        newFile = new FileInfo(HostingEnvironment.MapPath("~/Files/Samples/QuotationWithPrice.xlsx"));
                    }
                        

                        ExcelPackage pck = new ExcelPackage(newFile);
                        var sourceWorksheet = pck.Workbook.Worksheets["Quotation"];

                            //file detail
                            int rfqfirst = Convert.ToInt32(rfqids.GetValue(0));
                            var rfqdetail = _context.ReferanceNumbers.Include("File").FirstOrDefault(r => r.Id == rfqfirst);
                            var vessel = _context.Vessels.FirstOrDefault(v => v.Id == rfqdetail.File.VesselId);

                            //üst bilgi
                            sourceWorksheet.Cells["C8"].Value = rfqdetail.File.FileNumber;
                            sourceWorksheet.Cells["C4"].Value = rfqdetail.File.Company;
                            sourceWorksheet.Cells["C6"].Value = vessel.VesselName;
                            sourceWorksheet.Cells["C7"].Value = rfqdetail.File.DeliveryPlace;

                            int curid = rfqdetail.File.Currency ?? default(int);
                            CurrencyList.Currencies curr = (CurrencyList.Currencies)curid;

                            sourceWorksheet.Cells["C10"].Value = curr.ToString();
                            sourceWorksheet.Cells["C9"].Value = DateTime.Now.Date.ToString("dd-MM-yyyy");
                            sourceWorksheet.Cells["C12"].Value = rfqdetail.File.PaymentTerm; 
                            sourceWorksheet.Cells["C11"].Value = rfqdetail.File.Discount;

                            //file detail

                    int discount = rfqdetail.File.Discount;


                    // Target a worksheet 


                    sourceWorksheet.Cells[1, 1].Value = "Appa -" + identifier;

                        //constant members for each RFQ
                        var rangerfqname = "C16:C17";
                        var rangeheader1 = "A19:R19";
                        var rangeheader2 = "A20:R20";
                        var rangelineitem = "A21:R21";



                    //worksheet.Cells[headerRange].AutoFitColumns();

                    int sira = 19; // first line item will be on 21


                    List<string> reftoplamhucreler = new List<string>();

                    for (int j = 0; j < rfqids.Length; j++)
                    {
                        int rfqid = Convert.ToInt32(rfqids.GetValue(j));
                        var rfq = _context.ReferanceNumbers.FirstOrDefault(r => r.Id == rfqid);

                        if (j == 0)
                        {
                            sourceWorksheet.Cells["C16"].Value = "RFQ NO: " + rfq.Name;
                        }
                        else
                        {
                            sira = sira + 5;

                            sourceWorksheet.Cells[rangerfqname].Copy(sourceWorksheet.Cells["C" + sira + ":C" + (sira + 1)]);
                            sourceWorksheet.Cells["C" + sira].Value = "RFQ NO: " + rfq.Name;

                            sira = sira + 1; 
                            sira = sira + 1; 
                            sira = sira - 4;
                        }

                        if (j!=0)
                        {
                            sira = sira + 5;
                            sourceWorksheet.Cells[rangeheader1].Copy(sourceWorksheet.Cells["A" + sira + ":R" + sira]);
                            sira++;
                            sourceWorksheet.Cells[rangeheader2].Copy(sourceWorksheet.Cells["A" + sira + ":R" + sira]);
                        }
                        else
                        {
                            sira = sira + 1;
                        }

                        //line items 
                        var lineitemlist = _context.LineItems.Where(l => l.ReferanceNumberId == rfqid && l.IsRemoved == false).ToList();

                       


                        var lineitems = lineitemlist.OrderBy(l => l.MasterNumber == "0" ? int.Parse(l.Number) : int.Parse(l.MasterNumber)).ToList();

                        foreach (var item in lineitems)
                        {
                            sira = sira + 1;
                            //first design
                            sourceWorksheet.Cells[rangelineitem].Copy(sourceWorksheet.Cells["A" + sira + ":R" + sira]);

                            sourceWorksheet.Cells["A" + sira].Value = item.Number;
                            sourceWorksheet.Cells["B" + sira].Value = item.Impa;
                            sourceWorksheet.Cells["C" + sira].Value = item.Description;
                            sourceWorksheet.Cells["D" + sira].Value = item.Qtty;
                            sourceWorksheet.Cells["E" + sira].Value = item.Unit;
                            sourceWorksheet.Cells["F" + sira].Value = item.Price;
                            //Total will be on G cell
                            sourceWorksheet.Cells["H" + sira].Value = item.AltQtty;
                            sourceWorksheet.Cells["I" + sira].Value = item.AltUnit;
                            sourceWorksheet.Cells["J" + sira].Value = item.AltPrice;

                            sourceWorksheet.Cells["K" + sira].Value = item.Remark;

                            //Total
                            //  sourceWorksheet.Cells["G" + sira].Formula = string.Format(@"IF(J{0}>0;(H{0}*J{0});(D{0}*F{0}))", sira.ToString());

                            if (item.AltPrice==null)
                            {
                                sourceWorksheet.Cells["G" + sira].Formula = string.Format(@"D{0}*F{0}", sira.ToString());
                            }
                            else
                            {
                                sourceWorksheet.Cells["G" + sira].Formula = string.Format(@"H{0}*J{0}", sira.ToString());
                            }

                            if (item.Number.Contains('-'))
                            {
                                sourceWorksheet.Cells["G" + sira].Value = 0;
                            }

                            //quitation with price

                            if (withprice)
                            {
                                sourceWorksheet.Cells["P" + sira].Value = item.SelectedSupplierName;
                                sourceWorksheet.Cells["Q" + sira].Value = item.SelectedSupplierCalculatedPrice;

                                if (item.AltPrice==null)
                                {
                                    sourceWorksheet.Cells["R" + sira].Formula = string.Format(@"Q{0}*D{0}", sira.ToString());
                                }
                                else
                                {
                                    sourceWorksheet.Cells["R" + sira].Formula = string.Format(@"Q{0}*H{0}", sira.ToString());
                                }
                               
                            }
                           
                        }


                        sira = sira + 1;
                 

                        sourceWorksheet.Cells["E" + (sira + 1) + ":F" + (sira + 1)].Merge = true;
                        sourceWorksheet.Cells["E" + (sira + 1) + ":F" + (sira + 1)].Value = "SUBTOTAL :";
                        sourceWorksheet.Cells["E" + (sira + 1) + ":F" + (sira + 1)].Style.Font.Bold = true;
                        sourceWorksheet.Cells["E" + (sira + 1) + ":F" + (sira + 1)].Style.Font.Size = 12;
                        sourceWorksheet.Cells["G" + (sira + 1)].Style.Font.Bold = true;

                        int son = sira + 1 - 3 + 2;
                        int baslangic = sira + 1 - 3 -lineitems.Count + 2;
                        string formulsubtotal = string.Format(@"SUM(G{0}:G{1})", baslangic, son);
                        sourceWorksheet.Cells["G" + (sira + 1)].Formula = formulsubtotal;
                        sourceWorksheet.Cells["G" + (sira + 1)].Style.Numberformat.Format = "#,##0.00";


                        sourceWorksheet.Cells["E" + (sira + 2) + ":F" + (sira + 2)].Merge = true;
                        sourceWorksheet.Cells["E" + (sira + 2) + ":F" + (sira + 2)].Value = "DISC AMOUNT :";
                        sourceWorksheet.Cells["E" + (sira + 2) + ":F" + (sira + 2)].Style.Font.Bold = true;
                        sourceWorksheet.Cells["E" + (sira + 2) + ":F" + (sira + 2)].Style.Font.Size = 12;
                        sourceWorksheet.Cells["G" + (sira + 2)].Style.Font.Bold = true;

                        string discformula = string.Format(@"((G{0}*({1}/100)))", sira + 1, discount);
                        sourceWorksheet.Cells["G" + (sira + 2)].Formula = discformula;
                        sourceWorksheet.Cells["G" + (sira + 2)].Style.Numberformat.Format = "#,##0.00";

                          
                        sourceWorksheet.Cells["E" + (sira + 3) + ":F" + (sira + 3)].Merge = true;
                        sourceWorksheet.Cells["E" + (sira + 3) + ":F" + (sira + 3)].Value = "TOTAL :";
                        sourceWorksheet.Cells["E" + (sira + 3) + ":F" + (sira + 3)].Style.Font.Bold = true;
                        sourceWorksheet.Cells["E" + (sira + 3) + ":F" + (sira + 3)].Style.Font.Size = 12;
                        sourceWorksheet.Cells["G" + (sira + 3)].Style.Font.Bold = true;
 

                        sourceWorksheet.Cells["G" + (sira + 3)].Formula = string.Format(@"G{0}-G{1}", sira + 1, sira + 2);
                        sourceWorksheet.Cells["G" + (sira + 3)].Style.Numberformat.Format = "#,##0.00";


                        if (withprice)
                        {
                            sourceWorksheet.Cells["E" + (sira + 4) + ":F" + (sira + 4)].Merge = true;
                            sourceWorksheet.Cells["E" + (sira + 4) + ":F" + (sira + 4)].Value = "Cost Total :";
                            sourceWorksheet.Cells["E" + (sira + 4) + ":F" + (sira + 4)].Style.Font.Bold = true;
                            sourceWorksheet.Cells["E" + (sira + 4) + ":F" + (sira + 4)].Style.Font.Size = 12;
                            sourceWorksheet.Cells["G" + (sira + 4)].Style.Font.Bold = true;
 
                            string formulcosttotal = string.Format(@"SUM(R{0}:R{1})", baslangic, son);
                            sourceWorksheet.Cells["G" + (sira + 4)].Formula = formulcosttotal;
                            sourceWorksheet.Cells["G" + (sira + 4)].Style.Numberformat.Format = "#,##0.00";
                        }


                        reftoplamhucreler.Add("G" + (sira + 3));

                    }

                    // Grand Total &  Expense 

                    string geneltoplamformula = "";
                    foreach (var item in reftoplamhucreler)
                    {
                        geneltoplamformula = geneltoplamformula + item + "+";
                    }

                    geneltoplamformula = geneltoplamformula.Remove(geneltoplamformula.Length - 1);

                    sira = sira + 7;


                    Color colFromHex = System.Drawing.ColorTranslator.FromHtml("#DAEEF3");

                    sourceWorksheet.Cells["D" + (sira + 1) + ":F" + (sira +1)].Merge = true;
                    sourceWorksheet.Cells["D" + (sira + 1) + ":F" + (sira + 1)].Value = "DELIVERY EXPENSE :";
                    sourceWorksheet.Cells["D" + (sira + 1) + ":F" + (sira + 1)].Style.Font.Bold = true;
                    sourceWorksheet.Cells["D" + (sira + 1) + ":F" + (sira + 1)].Style.Font.Size = 12;


                    sourceWorksheet.Cells["G" + (sira + 1)].Value = rfqdetail.File.DeliveryCost;
                    sourceWorksheet.Cells["G" + (sira + 1)].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    sourceWorksheet.Cells["G" + (sira + 1)].Style.Numberformat.Format = "#,##0.00";

                    sourceWorksheet.Cells["G" + (sira + 1)].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    sourceWorksheet.Cells["G" + (sira + 1)].Style.Fill.BackgroundColor.SetColor(colFromHex);

                    
              
                     
                   
                    sourceWorksheet.Cells["D"+ (sira + 2)+":F"+ (sira + 2)].Merge = true;
                    sourceWorksheet.Cells["D" + (sira + 2) + ":F" + (sira + 2)].Value= "GRAND TOTAL :";
                    sourceWorksheet.Cells["D" + (sira + 2) + ":F" + (sira + 2)].Style.Font.Bold = true;
                    sourceWorksheet.Cells["D" + (sira + 2) + ":F" + (sira + 2)].Style.Font.Size = 12;
                  



                    geneltoplamformula = geneltoplamformula + ("+G" + (sira + 1));
                    sourceWorksheet.Cells["G" + (sira + 2)].Formula = geneltoplamformula; 
                    sourceWorksheet.Cells["G" + (sira + 2)].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    sourceWorksheet.Cells["G" + (sira + 2)].Style.Numberformat.Format = "#,##0.00";

                    sourceWorksheet.Cells["G" + (sira + 2)].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    sourceWorksheet.Cells["G" + (sira + 2)].Style.Fill.BackgroundColor.SetColor(colFromHex);

                    if (vessel!=null)
                    {
                        filename = vessel.VesselName.ToString().Replace(@"/", "_");
                    }

                    filename = filename + "_" + rfqdetail.File.FileNumber.ToString()+"_";

                    filename = "Appa_"+ filename + identifier;

                    // calculate all formulas in the workbook
                    pck.Workbook.Calculate();

                    FileInfo excelFile = new FileInfo(HostingEnvironment.MapPath(string.Format(@"~/Files/Quotations/{0}.xlsx", filename)));
                        pck.SaveAs(excelFile);
                    }
            }

      

            return filename;
        }

        public string NewDeliveryNote(Array rfqids, bool withprice = false) {

            string filename = "";

            int wssira = 1;
            if (rfqids.Length > 0)
            {
                using (ExcelPackage excel = new ExcelPackage())
                {
                    string identifier = Guid.NewGuid().ToString();


                    FileInfo newFile = null;

                    if (!withprice)
                    {
                        newFile = new FileInfo(HostingEnvironment.MapPath("~/Files/Samples/DeliveryNoteSample.xlsx"));
                    }

                   

                    ExcelPackage pck = new ExcelPackage(newFile);
                    var sourceWorksheet = pck.Workbook.Worksheets["01"];

                    ReferanceNumber rfq = new ReferanceNumber();

                    int filesira = 1;
                    for (int j = 0; j < rfqids.Length; j++)
                    {
                        int rfqid = Convert.ToInt32(rfqids.GetValue(j));
                         rfq = _context.ReferanceNumbers.FirstOrDefault(r => r.Id == rfqid);
                        var file = _context.Files.FirstOrDefault(f => f.Id == rfq.FileId);
                        var vessel = _context.Vessels.FirstOrDefault(v => v.Id == file.FileNumber);


                        //line items
                        
                        var orderlineitems =  _context.OrderLineItems.Include("ReferanceNumber").Where(l =>l.ReferanceNumberId==rfqid).Where(l => l.IsRemoved == false).AsEnumerable();
                        var lineitems = orderlineitems.OrderBy(l => l.MasterNumber == "0" ? int.Parse(l.Number) : int.Parse(l.MasterNumber)).ToList();
                        int sirano = 1;

                        int sondeger = lineitems.Count();
                        int baslangic = 0;
                        int wsayisi = 1;

                        if (sondeger % 15 != 0)
                        {
                            wsayisi = (sondeger / 15) + 1;
                        }
                        else
                        {
                            wsayisi = sondeger / 15;
                        }

                        for (int w = 0; w < wsayisi; w++)
                        {
                            //ws oluştur 1
                            if (wssira != 1)
                            {
                                pck.Workbook.Worksheets.Copy("01",wssira.ToString()); 
                            }

                            var worksheet = pck.Workbook.Worksheets[wssira];
                        
                            List<char> columns = new List<char>
                            {
                                'A',
                                'B',
                                'C',
                                'D'
                            }; 

                            for (int i = 21; i < 58; i++)
                            {
                                foreach (var item in columns)
                                {
                                    worksheet.Cells[item.ToString() + i].Value = "";
                                }
                            }

                            wssira++; 

                            var sonuc = lineitems.Count;

                            sonuc = ((w + 1) * 15 > sonuc ? lineitems.Count : (w + 1) * 15);

                            int sira = 21;

                          

                            // üst bilgiler
                            worksheet.Cells["D16"].Value = DateTime.Now.Date.ToShortDateString();
                            worksheet.Cells["D17"].Value = file.FileNumber + "-" + filesira;
                            worksheet.Cells["C18"].Value = "REF: " +rfq.Name;

                            worksheet.Cells["B15"].Value = file.FileNumber;
                            worksheet.Cells["B17"].Value = file.DeliveryPlace;
                            // üst bilgiler
                            filesira++;

                            for (int k = baslangic; k < sonuc; k++)
                            {
                                worksheet.Cells["A" + sira].Value = sirano;
                                worksheet.Cells["B" + sira].Value = lineitems[k].Description;
                                worksheet.Cells["C" + sira].Value = lineitems[k].Qtty;
                                worksheet.Cells["D" + sira].Value = lineitems[k].Unit;

                                sira++;
                                sirano++;
                            }
                            baslangic = baslangic + 15; 
                        }

                        if (vessel != null)
                        {
                            filename = vessel.VesselName.Replace(@"/", "_"); ;
                        }

                        
                    }

                    filename = filename + "_" + rfq.File.FileNumber.ToString() + "_";

                    filename = "Appa_Delivery" + filename + identifier;

                    // calculate all formulas in the workbook
                    pck.Workbook.Calculate();

                    FileInfo excelFile = new FileInfo(HostingEnvironment.MapPath(string.Format(@"~/Files/Quotations/{0}.xlsx", filename)));
                    pck.SaveAs(excelFile);
                }
            }

            return filename;
        }

        public string SplitQuotation(Array rfqids, bool withprice = false)
        {
            string filename = "";


            try
            {
                if (rfqids.Length > 0)
                {
                    using (ExcelPackage excel = new ExcelPackage())
                    {
                        string identifier = Guid.NewGuid().ToString();

                        // FileInfo newFile = new FileInfo("D:\\Sample\\sample1.xlsx");

                        FileInfo newFile = null;

                        if (!withprice)
                        {
                            newFile = new FileInfo(HostingEnvironment.MapPath("~/Files/Samples/Quotation.xlsx"));
                        }

                        if (withprice)
                        {
                            newFile = new FileInfo(HostingEnvironment.MapPath("~/Files/Samples/QuotationWithPrice.xlsx"));
                        }


                        ExcelPackage pck = new ExcelPackage(newFile);
                        var sourceWorksheet = pck.Workbook.Worksheets["Quotation"];

                        //file detail
                        int rfqfirst = Convert.ToInt32(rfqids.GetValue(0));
                        var rfqdetail = _context.ReferanceNumbers.Include("File").FirstOrDefault(r => r.Id == rfqfirst);
                        var vessel = _context.Vessels.FirstOrDefault(v => v.Id == rfqdetail.File.VesselId);

                        //üst bilgi
                        sourceWorksheet.Cells["C8"].Value = rfqdetail.File.FileNumber;
                        sourceWorksheet.Cells["C4"].Value = rfqdetail.File.Company;
                        sourceWorksheet.Cells["C6"].Value = vessel.VesselName;
                        sourceWorksheet.Cells["C7"].Value = rfqdetail.File.DeliveryPlace;

                        int curid = rfqdetail.File.Currency ?? default(int);
                        CurrencyList.Currencies curr = (CurrencyList.Currencies)curid;

                        sourceWorksheet.Cells["C10"].Value = curr.ToString();
                        sourceWorksheet.Cells["C9"].Value = DateTime.Now.Date.ToString("dd-MM-yyyy");
                        sourceWorksheet.Cells["C12"].Value = rfqdetail.File.PaymentTerm;
                        sourceWorksheet.Cells["C11"].Value = rfqdetail.File.Discount;

                        //file detail

                        int discount = rfqdetail.File.Discount;


                        // Target a worksheet 


                        sourceWorksheet.Cells[1, 1].Value = "Appa -" + identifier;

                        //constant members for each RFQ
                        var rangerfqname = "C16:C17";
                        var rangeheader1 = "A19:R19";
                        var rangeheader2 = "A20:R20";
                        var rangelineitem = "A21:R21";



                        //worksheet.Cells[headerRange].AutoFitColumns();

                        int sira = 19; // first line item will be on 21


                        List<string> reftoplamhucreler = new List<string>();

                        for (int j = 0; j < rfqids.Length; j++)
                        {
                            int rfqid = Convert.ToInt32(rfqids.GetValue(j));
                            var rfq = _context.ReferanceNumbers.FirstOrDefault(r => r.Id == rfqid);

                            if (j == 0)
                            {
                                sourceWorksheet.Cells["C16"].Value = "RFQ NO: " + rfq.Name;
                            }
                            else
                            {
                                sira = sira + 5;

                                sourceWorksheet.Cells[rangerfqname].Copy(sourceWorksheet.Cells["C" + sira + ":C" + (sira + 1)]);
                                sourceWorksheet.Cells["C" + sira].Value = "RFQ NO: " + rfq.Name;

                                sira = sira + 1;
                                sira = sira + 1;
                                sira = sira - 4;
                            }

                            if (j != 0)
                            {
                                sira = sira + 5;
                                sourceWorksheet.Cells[rangeheader1].Copy(sourceWorksheet.Cells["A" + sira + ":R" + sira]);
                                sira++;
                                sourceWorksheet.Cells[rangeheader2].Copy(sourceWorksheet.Cells["A" + sira + ":R" + sira]);
                            }
                            else
                            {
                                sira = sira + 1;
                            }

                            //line items 
                            var lineitemlist = _context.OrderLineItems.Where(l => l.ReferanceNumberId == rfqid && l.IsRemoved == false).ToList();


                            var lineitems = lineitemlist.OrderBy(l => l.MasterNumber == "0" ? int.Parse(l.Number) : int.Parse(l.MasterNumber)).ToList();

                            int i = 1;
                            foreach (var item in lineitems)
                            {
                                sira = sira + 1;
                                //first design
                                sourceWorksheet.Cells[rangelineitem].Copy(sourceWorksheet.Cells["A" + sira + ":R" + sira]);

                                sourceWorksheet.Cells["A" + sira].Value = i;
                                sourceWorksheet.Cells["B" + sira].Value = item.Impa;
                                sourceWorksheet.Cells["C" + sira].Value = item.Description + (!string.IsNullOrEmpty(item.Remark) ? "R :" + item.Remark : "");
                                sourceWorksheet.Cells["D" + sira].Value = item.AltQtty == null ? item.Qtty : item.AltQtty;
                                sourceWorksheet.Cells["E" + sira].Value = item.AltUnit == null ? item.Unit : item.AltUnit;
                                sourceWorksheet.Cells["F" + sira].Value = item.AltPrice == null ? item.Price : item.AltPrice;
                                //Total will be on G cell
                                sourceWorksheet.Cells["H" + sira].Value = item.AltQtty;
                                sourceWorksheet.Cells["I" + sira].Value = item.AltUnit;
                                sourceWorksheet.Cells["J" + sira].Value = item.AltPrice;

                                sourceWorksheet.Cells["K" + sira].Value = item.Remark;

                                //Total
                                //  sourceWorksheet.Cells["G" + sira].Formula = string.Format(@"IF(J{0}>0;(H{0}*J{0});(D{0}*F{0}))", sira.ToString());

                                if (item.AltPrice == null)
                                {
                                    sourceWorksheet.Cells["G" + sira].Formula = string.Format(@"D{0}*F{0}", sira.ToString());
                                }
                                else
                                {
                                    sourceWorksheet.Cells["G" + sira].Formula = string.Format(@"H{0}*J{0}", sira.ToString());
                                }

                                if (item.Number.Contains('-'))
                                {
                                    sourceWorksheet.Cells["G" + sira].Value = 0;
                                }

                                //quitation with price

                                if (withprice)
                                {
                                    sourceWorksheet.Cells["P" + sira].Value = item.SelectedSupplierName;
                                    sourceWorksheet.Cells["Q" + sira].Value = item.SelectedSupplierCalculatedPrice;
                                    //  sourceWorksheet.Cells["R" + sira].Formula = string.Format(@"Q{0}*D{0}", sira.ToString());


                                    if (item.AltPrice == null)
                                    {
                                        sourceWorksheet.Cells["R" + sira].Formula = string.Format(@"Q{0}*D{0}", sira.ToString());
                                    }
                                    else
                                    {
                                        sourceWorksheet.Cells["R" + sira].Formula = string.Format(@"Q{0}*H{0}", sira.ToString());
                                    }
                                }
                                i++;
                            }


                            sira = sira + 1;


                            sourceWorksheet.Cells["E" + (sira + 1) + ":F" + (sira + 1)].Merge = true;
                            sourceWorksheet.Cells["E" + (sira + 1) + ":F" + (sira + 1)].Value = "SUBTOTAL :";
                            sourceWorksheet.Cells["E" + (sira + 1) + ":F" + (sira + 1)].Style.Font.Bold = true;
                            sourceWorksheet.Cells["E" + (sira + 1) + ":F" + (sira + 1)].Style.Font.Size = 12;
                            sourceWorksheet.Cells["G" + (sira + 1)].Style.Font.Bold = true;

                            int son = sira + 1 - 3 + 2;
                            int baslangic = sira + 1 - 3 - lineitems.Count + 2;
                            string formulsubtotal = string.Format(@"SUM(G{0}:G{1})", baslangic, son);
                            sourceWorksheet.Cells["G" + (sira + 1)].Formula = formulsubtotal;
                            sourceWorksheet.Cells["G" + (sira + 1)].Style.Numberformat.Format = "#,##0.00";


                            sourceWorksheet.Cells["E" + (sira + 2) + ":F" + (sira + 2)].Merge = true;
                            sourceWorksheet.Cells["E" + (sira + 2) + ":F" + (sira + 2)].Value = "DISC AMOUNT :";
                            sourceWorksheet.Cells["E" + (sira + 2) + ":F" + (sira + 2)].Style.Font.Bold = true;
                            sourceWorksheet.Cells["E" + (sira + 2) + ":F" + (sira + 2)].Style.Font.Size = 12;
                            sourceWorksheet.Cells["G" + (sira + 2)].Style.Font.Bold = true;

                            string discformula = string.Format(@"((G{0}*({1}/100)))", sira + 1, discount);
                            sourceWorksheet.Cells["G" + (sira + 2)].Formula = discformula;
                            sourceWorksheet.Cells["G" + (sira + 2)].Style.Numberformat.Format = "#,##0.00";


                            sourceWorksheet.Cells["E" + (sira + 3) + ":F" + (sira + 3)].Merge = true;
                            sourceWorksheet.Cells["E" + (sira + 3) + ":F" + (sira + 3)].Value = "TOTAL :";
                            sourceWorksheet.Cells["E" + (sira + 3) + ":F" + (sira + 3)].Style.Font.Bold = true;
                            sourceWorksheet.Cells["E" + (sira + 3) + ":F" + (sira + 3)].Style.Font.Size = 12;
                            sourceWorksheet.Cells["G" + (sira + 3)].Style.Font.Bold = true;


                            sourceWorksheet.Cells["G" + (sira + 3)].Formula = string.Format(@"G{0}-G{1}", sira + 1, sira + 2);
                            sourceWorksheet.Cells["G" + (sira + 3)].Style.Numberformat.Format = "#,##0.00";


                            if (withprice)
                            {
                                sourceWorksheet.Cells["E" + (sira + 4) + ":F" + (sira + 4)].Merge = true;
                                sourceWorksheet.Cells["E" + (sira + 4) + ":F" + (sira + 4)].Value = "Cost Total :";
                                sourceWorksheet.Cells["E" + (sira + 4) + ":F" + (sira + 4)].Style.Font.Bold = true;
                                sourceWorksheet.Cells["E" + (sira + 4) + ":F" + (sira + 4)].Style.Font.Size = 12;
                                sourceWorksheet.Cells["G" + (sira + 4)].Style.Font.Bold = true;

                                string formulcosttotal = string.Format(@"SUM(R{0}:R{1})", baslangic, son);
                                sourceWorksheet.Cells["G" + (sira + 4)].Formula = formulcosttotal;
                                sourceWorksheet.Cells["G" + (sira + 4)].Style.Numberformat.Format = "#,##0.00";
                            }


                            reftoplamhucreler.Add("G" + (sira + 3));

                        }

                        // Grand Total &  Expense 

                        string geneltoplamformula = "";
                        foreach (var item in reftoplamhucreler)
                        {
                            geneltoplamformula = geneltoplamformula + item + "+";
                        }

                        geneltoplamformula = geneltoplamformula.Remove(geneltoplamformula.Length - 1);

                        sira = sira + 7;


                        Color colFromHex = System.Drawing.ColorTranslator.FromHtml("#DAEEF3");

                        sourceWorksheet.Cells["D" + (sira + 1) + ":F" + (sira + 1)].Merge = true;
                        sourceWorksheet.Cells["D" + (sira + 1) + ":F" + (sira + 1)].Value = "DELIVERY EXPENSE :";
                        sourceWorksheet.Cells["D" + (sira + 1) + ":F" + (sira + 1)].Style.Font.Bold = true;
                        sourceWorksheet.Cells["D" + (sira + 1) + ":F" + (sira + 1)].Style.Font.Size = 12;


                        sourceWorksheet.Cells["G" + (sira + 1)].Value = rfqdetail.File.DeliveryCost;
                        sourceWorksheet.Cells["G" + (sira + 1)].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        sourceWorksheet.Cells["G" + (sira + 1)].Style.Numberformat.Format = "#,##0.00";

                        sourceWorksheet.Cells["G" + (sira + 1)].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        sourceWorksheet.Cells["G" + (sira + 1)].Style.Fill.BackgroundColor.SetColor(colFromHex);





                        sourceWorksheet.Cells["D" + (sira + 2) + ":F" + (sira + 2)].Merge = true;
                        sourceWorksheet.Cells["D" + (sira + 2) + ":F" + (sira + 2)].Value = "GRAND TOTAL :";
                        sourceWorksheet.Cells["D" + (sira + 2) + ":F" + (sira + 2)].Style.Font.Bold = true;
                        sourceWorksheet.Cells["D" + (sira + 2) + ":F" + (sira + 2)].Style.Font.Size = 12;




                        geneltoplamformula = geneltoplamformula + ("+G" + (sira + 1));
                        sourceWorksheet.Cells["G" + (sira + 2)].Formula = geneltoplamformula;
                        sourceWorksheet.Cells["G" + (sira + 2)].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        sourceWorksheet.Cells["G" + (sira + 2)].Style.Numberformat.Format = "#,##0.00";

                        sourceWorksheet.Cells["G" + (sira + 2)].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        sourceWorksheet.Cells["G" + (sira + 2)].Style.Fill.BackgroundColor.SetColor(colFromHex);

                        if (vessel != null)
                        {
                            filename = vessel.VesselName.ToString().Replace(@"/", "_");
                        }

                        filename = filename + "_" + rfqdetail.File.FileNumber.ToString() + "_";

                        filename = "Appa_" + filename + identifier;

                        // calculate all formulas in the workbook
                        pck.Workbook.Calculate();

                        FileInfo excelFile = new FileInfo(HostingEnvironment.MapPath(string.Format(@"~/Files/Quotations/{0}.xlsx", filename)));
                        pck.SaveAs(excelFile);
                    }
                }



                return filename;
            }
            catch (Exception e)
            {

                return e.Message;
            }
        
        }
    }
}