using Appa_MVC.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Appa_MVC.Controllers
{
    public class CurrencyController : Controller
    {
        ApplicationDbContext _context = new ApplicationDbContext();
        // GET: Currency
        public ActionResult Index()
        {
            var cur = new Currency();

            var euro = _context.CurrencyRates.Where(c=>c.CurrencyId==2).OrderByDescending(c=>c.Date).FirstOrDefault();
            var usd = _context.CurrencyRates.Where(c => c.CurrencyId == 1).OrderByDescending(c => c.Date).FirstOrDefault();

            cur.EURO = euro.Rate; 
            cur.USD = usd.Rate; 
             
            cur.Parity=decimal.Round((euro.Rate / usd.Rate), 2, MidpointRounding.AwayFromZero);

            return View(cur);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(Currency currency)
        {
            try
            {
                var usd = currency.USD;
                var euro = currency.EURO;

                var currencyeuro = new CurrencyRate
                {
                    CurrencyId = 2,
                    Currency = "EURO",
                    Date = DateTime.Now,
                    User = User.Identity.GetUserName(),
                    Rate = euro
                };

                _context.CurrencyRates.Add(currencyeuro);

                var currencyusd = new CurrencyRate
                {
                    CurrencyId = 1,
                    Currency = "USD",
                    Date = DateTime.Now,
                    User = User.Identity.GetUserName(),
                    Rate = usd
                };
                _context.CurrencyRates.Add(currencyusd);

                _context.SaveChanges();

                currency.Parity = decimal.Round((currencyeuro.Rate / currencyusd.Rate), 2, MidpointRounding.AwayFromZero);

                return RedirectToAction("Index");
            }
            catch (Exception e)
            {

                return new HttpStatusCodeResult(HttpStatusCode.BadRequest,e.Message);
            }
           
        }
    }
}