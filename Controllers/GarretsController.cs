using Appa_MVC.Models;
using ExcelDataReader;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;

namespace Inspinia_MVC5_SeedProject.Controllers
{
    public class GarretsController : Controller
    {

        readonly ApplicationDbContext _context = new ApplicationDbContext();
        // GET: Garrets
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult SaveUploadedFile()
        {
            bool isSavedSuccessfully = true;
            string fName = "";
            string filename = "";


            try
            {
                foreach (string fileName in Request.Files)
                {
                    HttpPostedFileBase file = Request.Files[fileName];
                    //Save file content goes here
                    fName = file.FileName;
                    string identifier = Guid.NewGuid().ToString();
                    filename = "Appa_Garrets" + identifier;

                    if (file != null && file.ContentLength > 0)
                    {
                        var originalDirectory = new DirectoryInfo(HostingEnvironment.MapPath(string.Format(@"~/Files/TechnicalFiles")));
                        

                        string pathString = originalDirectory.ToString();

                        var fileName1 = Path.GetFileName(file.FileName);

                        bool isExists = System.IO.Directory.Exists(pathString);

                        if (!isExists)
                            System.IO.Directory.CreateDirectory(pathString);

                        var path = string.Format("{0}\\{1}", pathString, file.FileName);
                        file.SaveAs(path);


                        //exceli aç düzenle
                        var fileinfo = new FileInfo(path);
                        if (fileinfo.Exists)
                        {
                            using (ExcelPackage p = new ExcelPackage(fileinfo))
                            {
                                //using (stream = new FileStream(path, FileMode.Open, FileAccess.ReadWrite))
                                {
                                    //p.Load(stream);

                                   

                                    ExcelWorksheet ws = p.Workbook.Worksheets[1];
                                    //ws.Protection.IsProtected = false;

                                    int sira = 24;

                                    while (true)
                                    {
                                        if (ws.Cells["C" + sira].Value==null)
                                        {

                                            break;
                                        }

                                        var linenumber = ws.Cells["C" + sira].Value.ToString();

                                       

                                       

                                        var contract = _context.GarretsContracts.FirstOrDefault(g => g.Id == linenumber);
                                       // var contract = _context.GarretsContracts.FirstOrDefault(g => g.id == linenumber);

                                        if (contract!=null)
                                        {
                                             ws.Cells["J" + sira].Value = contract.grossprice;
                                             ws.Cells["L" + sira].Value = contract.remark;
                                            ws.Cells["S" + sira].Value = Convert.ToDecimal(contract.maliyet.ToString().Replace('.',','));

                                            //var qtty = Convert.ToDecimal(ws.Cells["F" + sira].Value.ToString().Replace(".", ","));



                                        }
                                        else
                                        {
                                            ws.Cells["S" + sira].Value = 0;
                                        }

                                        ws.Cells["V" + sira].Formula = string.Format(@"SUBSTITUTE(S{0},""."","","")*SUBSTITUTE(F{0},""."","","")", sira.ToString());

                                        sira++;
                                    }

                                    ws.Cells["V" + (sira+1)].Formula= string.Format(@"SUM(V{0}:V{1})", 24, sira-1);

                                    p.Workbook.Calculate();

                                    FileInfo excelFile = new FileInfo(HostingEnvironment.MapPath(string.Format(@"~/Files/Quotations/{0}.xlsx", filename)));
                                    p.SaveAs(excelFile);
                                }

                            }

                        }

                        // exceli   aç düzenle


                        //exceli indir

                        //exceli indir

                    }


                }



            }
            catch (Exception ex)
            {
                isSavedSuccessfully = false;
            }


            if (isSavedSuccessfully)
            {
                return Json(new { Message = filename });
            }
            else
            {
                return Json(new { Message = "Error in saving file" });
            }
        }
    }
}