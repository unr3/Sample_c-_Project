using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using Appa_MVC.Models;

namespace Inspinia_MVC5_SeedProject.Email
{
    public class Emailer
    {
        readonly ApplicationDbContext _context = new ApplicationDbContext();
        public async Task SendEmail(string subject, string body, MailAddress toadress, List<MailAddress> ccadress,int logid)
        {
          

            var fromAddress = new MailAddress("info@xxxx.com", "Adamar");
            const string fromPassword = "xxxxx";
            var smtp = new SmtpClient
            {
                Host = "smtp.office365.com",
                UseDefaultCredentials = false,
                Port = 587,
                EnableSsl = true, 
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword),
                Timeout = 20000
                
            };

           

            using (var message = new MailMessage(fromAddress, toadress)
            {
                Subject = subject,
                Body = body
            })
            {
                foreach (var item in ccadress)
                {
                     message.CC.Add(item);
                }

                
              

                message.IsBodyHtml = true;
                
                try
                {
                    await smtp.SendMailAsync(message);
                    //log e-mail sent
                    smtp.Dispose();
                }
                catch (Exception e)
                {
                    //log e-mail send error 
                    var emaillog = _context.EmailLogs.FirstOrDefault(x => x.Id == logid);
                    if (emaillog != null) emaillog.State = "Error: "+e.Message;
                    _context.SaveChanges();
                }




            }
        }
    }
}