using Appa_MVC.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Web;
using System.Web.Mvc;

namespace Appa_MVC.Controllers
{
    public class FileController : Controller
    {
        ApplicationDbContext _context = new ApplicationDbContext();
        // GET: File
        public ActionResult Index()
        {
            
            return View(); 
        }

        public JsonResult GetCountires()
        {
            var countries = _context.Countries.ToList();
            var list =
                new List<SelectListItem> { new SelectListItem { Text = "--Select a Country--", Value = "0" } };
            foreach (var item in countries)
            {
                list.Add(new SelectListItem { Text = item.ULKEADI, Value = item.ID });
            }
            return Json(new SelectList(list, "Value", "Text", JsonRequestBehavior.AllowGet));
        }

        public JsonResult GetFilesbyVesselId(string vesselid)
        {
            var files = _context.Files.Where(f => f.VesselId == vesselid).ToList();

            var list =
                new List<File>();
            foreach (var item in files)
            {
              
                list.Add(new File { FileNumber = item.FileNumber,Department=item.Department, IsClosed = item.IsClosed, VadePesin = item.VadePesin, Company = item.Company, Createduser=item.Createduser, CreatedDate=item.CreatedDate});
            }
            return Json(list);
        }

        public ActionResult AddNewFile(string vesselid,string dep, string vadepesin, string company)
        {
            if (vesselid!="0")
            {
                var file = new File();
                file.VesselId = vesselid;
                file.Company = company;
                file.VadePesin = vadepesin;

                string department = "";

                switch (dep)
                {
                    case "T":
                        department = "Technical";
                        break;
                    case "P":
                        department = "Provision";
                        break;
                    case "BN":
                        department = "Bonded";
                        break;
                    case "CM":
                        department = "Cash To Master";
                        break;
                    case "SP":
                        department = "Spare Part";
                        break;
                }
                file.Department = department;

                var file_number = _context.Files.Count() + 1;
                var filename = "ADMN18-" + file_number.ToString().PadLeft(5, '0') + "-" + dep;

                file.FileNumber = filename;

                file.CreatedDate = DateTime.Now;
                file.Createduser = User.Identity.GetUserName();

                _context.Files.Add(file);
                _context.SaveChanges();


                return Json(file);
            }
            else
            {
                return Json(false);
            }

          
        }


        public ActionResult AddVessel(string ulkekodu, string cari_isim)
        {
            if (ulkekodu.Length==1)
            {
                return Json(new
                {
                    success = false,

                });
            }

            try
            {
                string carikod = "";
                string querry = string.Format(@"Select top 1  CAST(S_YEDEK2 AS INT) as S_YEDEK2 From  {0}..TBLCASABIT 
                                            where S_YEDEK2 is not null
                                            order BY  CAST(S_YEDEK2 AS INT) desc", Netsis.database);

                int carisira = _context.Database.SqlQuery<int>(querry).FirstOrDefault();


                if (carisira == 0)
                {
                    carikod = "120-99-0001 ";
                    carisira = 1;
                }
                else
                {
                    carisira = carisira + 1;
                    carikod = "120-99-" + carisira.ToString("D4");
                }


                string sqlcommand = string.Format(@"INSERT INTO {4}..TBLCASABIT
                                            (SUBE_KODU, ISLETME_KODU, CARI_KOD,ULKE_KODU, CARI_ISIM, KAYITYAPANKUL,KAYITTARIHI,S_YEDEK2,CARI_TIP , HESAPTUTMASEKLI , UPDATE_KODU ,C_YEDEK1 , B_YEDEK1)
                                            VALUES
                                            (-1,1,'{0}', '{1}' , '{2}' , 'APPA',GETDATE(), {3},'A','Y','X','A','0')  
 
                                            INSERT INTO {4}..TBLCASABITEK
                                            (CARI_KOD)
                                            VALUES
                                            ('{0}')"

                                , carikod, ulkekodu, cari_isim.ToUpper(), carisira, Netsis.database);

                _context.Database.ExecuteSqlCommand(sqlcommand);

                return Json(new
                {
                    success = true,
                   
                });
            }
            catch (Exception e)
            {

                return Json(new
                {
                    success = false,

                });
            }
           
        }
    }
}