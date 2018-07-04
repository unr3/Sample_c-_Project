using Appa_MVC.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Inspinia_MVC5_SeedProject.Email.Templates
{
    public class EmailTemplates
    {
        public string e_mailbody_fiyat_isteme(string header, string body, string link, string userName, string userMail)
        {
            var bodyTemplate = (@"
<!DOCTYPE html PUBLIC ""-//W3C//DTD XHTML 1.0 Transitional//EN"" ""http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd"">
<html xmlns=""http://www.w3.org/1999/xhtml"" style=""font-family: 'Helvetica Neue', Helvetica, Arial, sans-serif; box-sizing: border-box; font-size: 14px; margin: 0;"">
<head>
<meta name=""viewport"" content=""width=device-width"" />
<meta http-equiv=""Content-Type"" content=""text/html; charset=UTF-8"" />
<title>Alerts e.g. approaching your limit</title>


<style type=""text/css"">
img {
max-width: 100%;
}
body {
-webkit-font-smoothing: antialiased; -webkit-text-size-adjust: none; width: 100% !important; height: 100%; line-height: 1.6em;
}
body {
background-color: #f6f6f6;
}
@media only screen and (max-width: 640px) {
  body {
	padding: 0 !important;
  }
  h1 {
	font-weight: 800 !important; margin: 20px 0 5px !important;
  }
  h2 {
	font-weight: 800 !important; margin: 20px 0 5px !important;
  }
  h3 {
	font-weight: 800 !important; margin: 20px 0 5px !important;
  }
  h4 {
	font-weight: 800 !important; margin: 20px 0 5px !important;
  }
  h1 {
	font-size: 22px !important;
  }
  h2 {
	font-size: 18px !important;
  }
  h3 {
	font-size: 16px !important;
  }
  .container {
	padding: 0 !important; width: 100% !important;
  }
  .content {
	padding: 0 !important;
  }
  .content-wrap {
	padding: 10px !important;
  }
  .invoice {
	width: 100% !important;
  }
}
</style>
</head>

");

            bodyTemplate = bodyTemplate + string.Format(@"


<body itemscope itemtype=""http://schema.org/EmailMessage"" style=""font-family: 'Helvetica Neue',Helvetica,Arial,sans-serif; box-sizing: border-box; font-size: 14px; -webkit-font-smoothing: antialiased; -webkit-text-size-adjust: none; width: 100% !important; height: 100%; line-height: 1.6em; background-color: #f6f6f6; margin: 0;"" bgcolor=""#f6f6f6"">

<table class=""body-wrap"" style=""font-family: 'Helvetica Neue',Helvetica,Arial,sans-serif; box-sizing: border-box; font-size: 14px; width: 100%; background-color: #f6f6f6; margin: 0;"" bgcolor=""#f6f6f6""><tr style=""font-family: 'Helvetica Neue',Helvetica,Arial,sans-serif; box-sizing: border-box; font-size: 14px; margin: 0;""><td style=""font-family: 'Helvetica Neue',Helvetica,Arial,sans-serif; box-sizing: border-box; font-size: 14px; vertical-align: top; margin: 0;"" valign=""top""></td>
		<td class=""container"" width=""600"" style=""font-family: 'Helvetica Neue',Helvetica,Arial,sans-serif; box-sizing: border-box; font-size: 14px; vertical-align: top; display: block !important; max-width: 600px !important; clear: both !important; margin: 0 auto;"" valign=""top"">
			<div class=""content"" style=""font-family: 'Helvetica Neue',Helvetica,Arial,sans-serif; box-sizing: border-box; font-size: 14px; max-width: 600px; display: block; margin: 0 auto; padding: 20px;"">
				<table class=""main"" width=""100%"" cellpadding=""0"" cellspacing=""0"" style=""font-family: 'Helvetica Neue',Helvetica,Arial,sans-serif; box-sizing: border-box; font-size: 14px; border-radius: 3px; background-color: #fff; margin: 0; border: 1px solid #e9e9e9;"" bgcolor=""#fff""><tr style=""font-family: 'Helvetica Neue',Helvetica,Arial,sans-serif; box-sizing: border-box; font-size: 14px; margin: 0;""><td class=""alert alert-warning"" style=""font-family: 'Helvetica Neue',Helvetica,Arial,sans-serif; box-sizing: border-box; font-size: 16px; vertical-align: top; color: #fff; font-weight: 500; text-align: center; border-radius: 3px 3px 0 0; background-color: #FF9F00; margin: 0; padding: 20px;"" align=""center"" bgcolor=""#FF9F00"" valign=""top"">
							{0}
						</td>
					</tr><tr style=""font-family: 'Helvetica Neue',Helvetica,Arial,sans-serif; box-sizing: border-box; font-size: 14px; margin: 0;""><td class=""content-wrap"" style=""font-family: 'Helvetica Neue',Helvetica,Arial,sans-serif; box-sizing: border-box; font-size: 14px; vertical-align: top; margin: 0; padding: 20px;"" valign=""top"">
							<table width=""100%"" cellpadding=""0"" cellspacing=""0"" style=""font-family: 'Helvetica Neue',Helvetica,Arial,sans-serif; box-sizing: border-box; font-size: 14px; margin: 0;""><tr style=""font-family: 'Helvetica Neue',Helvetica,Arial,sans-serif; box-sizing: border-box; font-size: 14px; margin: 0;""><td class=""content-block"" style=""font-family: 'Helvetica Neue',Helvetica,Arial,sans-serif; box-sizing: border-box; font-size: 14px; vertical-align: top; margin: 0; padding: 0 0 20px;"" valign=""top"">
										
									</td>
								</tr><tr style=""font-family: 'Helvetica Neue',Helvetica,Arial,sans-serif; box-sizing: border-box; font-size: 14px; margin: 0;""><td class=""content-block"" style=""font-family: 'Helvetica Neue',Helvetica,Arial,sans-serif; box-sizing: border-box; font-size: 14px; vertical-align: top; margin: 0; padding: 0 0 20px;"" valign=""top"">
										{1}
									</td>
								</tr><tr style=""font-family: 'Helvetica Neue',Helvetica,Arial,sans-serif; box-sizing: border-box; font-size: 14px; margin: 0;""><td class=""content-block"" style=""font-family: 'Helvetica Neue',Helvetica,Arial,sans-serif; box-sizing: border-box; font-size: 14px; vertical-align: top; margin: 0; padding: 0 0 20px;"" valign=""top"">
										<a href=""{2}"" class=""btn-primary"" style=""font-family: 'Helvetica Neue',Helvetica,Arial,sans-serif; box-sizing: border-box; font-size: 14px; color: #FFF; text-decoration: none; line-height: 2em; font-weight: bold; text-align: center; cursor: pointer; display: inline-block; border-radius: 5px; text-transform: capitalize; background-color: #348eda; margin: 0; border-color: #348eda; border-style: solid; border-width: 10px 20px;"">Sayfayı Ziyaret Et.</a>
									</td>
								</tr>

		   <tr>
							<td>
										<div style=""text-size-adjust: none !important; -ms-text-size-adjust: none !important; -webkit-text-size-adjust: none !important;margin-left: 33%;
																"">

												 
																	<p style=""font-family: Helvetica, Arial, sans-serif; font-size: 13px; line-height: 15px; color: rgb(33, 33, 33); margin-bottom: 10px;""><span style=""font-weight: bold; color: rgb(33, 33, 33); display: inline;"">{4}</span>
																	   <span style=""color: #212121;""></span>
																	  <span style=""display: inline;""><br></span>
																	  <a href=""mailto:{3}"" style=""color: rgb(71, 124, 204); text-decoration: none; display: inline;"">{3}</a><span style=""display: inline;""> / </span><span style=""color: rgb(33, 33, 33); display: inline;""> + 90 232 616 17 19 </span>
																	</p>

																	  <p style=""font-family: Helvetica, Arial, sans-serif; font-size: 13px; line-height: 15px; margin-bottom: 10px;"">
																		<span style=""font-weight: bold; color: rgb(33, 33, 33); display: inline;"">ADAMAR INTERNATIONAL SHIP SUPPLY CO.</span>
																		<span style=""display: inline;""><br></span>
																		 <span style=""color: #212121;""></span>
																		 <span style=""color: #212121;""></span>
																		<span></span> <span style=""color: #212121;""></span>
																		<span></span> <span style=""color: #212121;""></span>
																		<span></span>
																		<a href=""http://adamarine.com"" style=""color: rgb(71, 124, 204); text-decoration: none; display: inline;"">adamarine.com</a>
																	  </p>
																	  </div>
							</td>
							</tr>

							</table></td>
					</tr></table><div class=""footer"" style=""font-family: 'Helvetica Neue',Helvetica,Arial,sans-serif; box-sizing: border-box; font-size: 14px; width: 100%; clear: both; color: #999; margin: 0; padding: 20px;"">
					<table width=""100%"" style=""font-family: 'Helvetica Neue',Helvetica,Arial,sans-serif; box-sizing: border-box; font-size: 14px; margin: 0;""><tr style=""font-family: 'Helvetica Neue',Helvetica,Arial,sans-serif; box-sizing: border-box; font-size: 14px; margin: 0;""><td class=""aligncenter content-block"" style=""font-family: 'Helvetica Neue',Helvetica,Arial,sans-serif; box-sizing: border-box; font-size: 12px; vertical-align: top; color: #999; text-align: center; margin: 0; padding: 0 0 20px;"" align=""center"" valign=""top""><a href=""http://www.mailgun.com"" style=""font-family: 'Helvetica Neue',Helvetica,Arial,sans-serif; box-sizing: border-box; font-size: 12px; color: #999; text-decoration: underline; margin: 0;"">Sent From Appa</a> </td>
						</tr>

				 

</table></div></div>
		</td>
		<td style=""font-family: 'Helvetica Neue',Helvetica,Arial,sans-serif; box-sizing: border-box; font-size: 14px; vertical-align: top; margin: 0;"" valign=""top""></td>
	</tr></table>

 

</body>
</html>
			", header, body, link, userMail, userName);

            return bodyTemplate;
        }


        public decimal isaltqtty(decimal qtty, decimal? altqtty)
        {
            if(altqtty==null)
            { return qtty; }
            else
            {
                return (decimal)altqtty;
            }
        }
        public decimal isaltprice(decimal price, decimal? altprice)
        {
            if (altprice == null)
            { return price; }
            else
            {
                return (decimal)altprice;
            }
        }
        public decimal isaltunit(decimal unit, decimal? altunit)
        {
            if (altunit == null)
            { return unit; }
            else
            {
                return (decimal)altunit;
            }
        }

        public string e_mailbody_technical_split_supplier(IEnumerable<OrderLineItem> lineitems, File file, string note, Supplier supplier,string UserName)
        {
            decimal total = 0;

            foreach (var item in lineitems)
            {
                
                if (item.AltQtty==null)
                {

                }
                total = total + (item.SelectedSupplierPrice * isaltqtty(item.Qtty,item.AltQtty));
            }

            string body = string.Format(@"
<table>
<tbody><tr>
<td style=""height:30pt;background-color:#13B5EA;padding:0 0 0 15pt;""><span style=""background-color:#13B5EA;"">
<div><font face=""arial,sans-serif"" size=""3"" color=""white""><span style=""font-size:11.5pt;""><b>ADAMAR INTERNATIONAL SHIP SUPPLY CO.</b></span></font></div>
</span></td>
</tr>
<tr>
<td valign=""top"" style=""padding:11.25pt 0 0 0;"">
<table width=""100%"" border=""0"" cellspacing=""0"" cellpadding=""0"" style=""width:100%;"">
<tbody><tr height=""47"" style=""height:28.5pt;"">
<td style=""height:28.5pt;background-color:#D6DCE4;padding:0 0 0 15pt;border-style:none none solid none;border-bottom-width:1.5pt;border-bottom-color:#13B5EA;"">
<span style=""background-color:#D6DCE4;"">
<div><font face=""arial,sans-serif"" size=""3"" color=""#525C64""><span style=""font-size:11.5pt;"">Üst Bilgiler:</span></font></div>
</span></td>
</tr>
<tr>
<td style=""padding:9pt;border:1pt solid #D6DCE4;"">
<table width=""100%"" border=""0"" cellspacing=""0"" cellpadding=""0"" style=""width:100%;"">
<tbody><tr>
<td width=""311"" valign=""top"" style=""width:311px;padding:0;"">
<table width=""100%"" border=""0"" cellspacing=""0"" cellpadding=""0"" style=""width:100%;"">
<tbody><tr>
<td width=""125"" style=""width:75pt;padding:0;"">
<div><font face=""arial,sans-serif"" size=""2"" color=""#525C64""><span style=""font-size:9pt;""><b>Firma</b></span></font></div>
</td>
<td style=""padding:0;"">
<div><font face=""arial,sans-serif"" size=""2"" color=""#008B00""><span style=""font-size:9pt;""><b>{0}</b></span></font></div>
</td>
</tr>
<tr>
<td width=""125"" style=""width:75pt;padding:0;"">
<div><font face=""arial,sans-serif"" size=""2"" color=""#525C64""><span style=""font-size:9pt;""><b>Dosya No:</b></span></font></div>
</td>
<td style=""padding:0;"">
<div><font face=""arial,sans-serif"" size=""2"" color=""#E81E24""><span style=""font-size:9pt;""><b>{1}</b></span></font></div>
</td>
</tr> 
<tr>
<td width=""208"" style=""width:125pt;padding:0;"">
<div><font face=""arial,sans-serif"" size=""2"" color=""#525C64""><span style=""font-size:9pt;""><b>Tarih</b></span></font></div>
</td>
<td style=""padding:0;"">
<div><font face=""arial,sans-serif"" size=""2"" color=""#2E3235""><span style=""font-size:9pt;""><b>{2}</b></span></font></div>
</td>
</tr>
<tr>
<td width=""208"" style=""width:125pt;padding:0;"">
<div><font face=""arial,sans-serif"" size=""2"" color=""#525C64""><span style=""font-size:9pt;""><b>Total</b></span></font></div>
</td>
<td style=""padding:0;"">
<div><font face=""arial,sans-serif"" size=""2"" color=""#2E3235""><span style=""font-size:9pt;""><b>{3}</b></span></font></div>
</td>
</tr>
</tbody></table>
</td>
</tr>
</tbody></table>
</td>
</tr>
</tbody></table>
</td>
</tr>
<tr>
<td valign=""top"" style=""padding:11.25pt 0 0 0;"">
<table width=""100%"" border=""0"" cellspacing=""0"" cellpadding=""0"" style=""width:100%;"">
<tbody><tr height=""47"" style=""height:28.5pt;"">
<td style=""height:28.5pt;background-color:#D6DCE4;padding:0 0 0 15pt;border-style:none none solid none;border-bottom-width:1.5pt;border-bottom-color:#13B5EA;"">
<span style=""background-color:#D6DCE4;"">
<div><font face=""arial,sans-serif"" size=""3"" color=""#525C64""><span style=""font-size:11.5pt;"">Not:</span></font></div>
</span></td>
</tr>
<tr>
<td style=""padding:9pt;border:1pt solid #D6DCE4;"">
<table width=""100%"" border=""0"" cellspacing=""0"" cellpadding=""0"" style=""width:100%;"">
<tbody><tr>
<td valign=""top"" style=""padding:0;"">
<div><font face=""arial,sans-serif"" size=""2"" color=""#525C64""><span style=""font-size:9pt;"">{4}</span></font></div>
<div style=""margin-top:14pt;margin-bottom:14pt;"">&nbsp;</div>
</td>
</tr>
</tbody></table>
</td>
</tr>
</tbody></table>
</td>
</tr>
<tr>
<td valign=""top"" style=""padding:11.25pt 0;"">
<table width=""100%"" border=""0"" cellspacing=""0"" cellpadding=""0"" style=""width:100%;"">
<tbody><tr height=""47"" style=""height:28.5pt;"">
<td style=""height:28.5pt;background-color:#D6DCE4;padding:0 15pt;border-style:none none solid none;border-bottom-width:1.5pt;border-bottom-color:#13B5EA;"">
<span style=""background-color:#D6DCE4;"">
<table width=""100%"" border=""0"" cellspacing=""0"" cellpadding=""0"" style=""width:100%;"">
<tbody><tr>
<td style=""padding:0;"">
<div><font face=""arial,sans-serif"" size=""3"" color=""#525C64""><span style=""font-size:11.5pt;"">Ürünler :</span></font></div>
</td>
<td style=""padding:0;"">
<div align=""right"" style="""">&nbsp;</div>
</td>
</tr>
</tbody></table>
</span></td>
</tr>
<tr>
<td style=""padding:0;border:1pt solid #D6DCE4;"">
<table width=""100%"" border=""0"" cellspacing=""0"" cellpadding=""0"" style=""width:100%;"">
<tbody><tr>
<td style=""background-color:#D6DCE4;padding:8.25pt 5.25pt;border-style:none solid solid none;border-right-width:1pt;border-bottom-width:1pt;border-right-color:#BEC4CB;border-bottom-color:#BEC4CB;"">
<span style=""background-color:#D6DCE4;"">
<div><font face=""arial,sans-serif"" size=""1"" color=""#2E3235""><span style=""font-size:7.5pt;""><b>Ref No.</b></span></font></div>
</span></td>
<td style=""background-color:#D6DCE4;padding:8.25pt 5.25pt;border-style:none solid solid none;border-right-width:1pt;border-bottom-width:1pt;border-right-color:#BEC4CB;border-bottom-color:#BEC4CB;"">
<span style=""background-color:#D6DCE4;"">
<div><font face=""arial,sans-serif"" size=""1"" color=""#2E3235""><span style=""font-size:7.5pt;""><b>Nr.</b></span></font></div>
</span></td>
<td style=""background-color:#D6DCE4;padding:8.25pt 5.25pt;border-style:none solid solid none;border-right-width:1pt;border-bottom-width:1pt;border-right-color:#BEC4CB;border-bottom-color:#BEC4CB;"">
<span style=""background-color:#D6DCE4;"">
<div><font face=""arial,sans-serif"" size=""1"" color=""#2E3235""><span style=""font-size:7.5pt;""><b>Impa</b></span></font></div>
</span></td>
<td style=""background-color:#D6DCE4;padding:8.25pt 5.25pt;border-style:none none solid none;border-bottom-width:1pt;border-bottom-color:#BEC4CB;"">
<span style=""background-color:#D6DCE4;"">
<div><font face=""arial,sans-serif"" size=""1"" color=""#2E3235""><span style=""font-size:7.5pt;""><b>Description</b></span></font></div>
</span></td>
<td style=""background-color:#D6DCE4;padding:8.25pt 5.25pt;border-style:none none solid none;border-bottom-width:1pt;border-bottom-color:#BEC4CB;"">
<span style=""background-color:#D6DCE4;"">
<div><font face=""arial,sans-serif"" size=""1"" color=""#2E3235""><span style=""font-size:7.5pt;""><b>Unit</b></span></font></div>
</span></td>
<td style=""background-color:#D6DCE4;padding:8.25pt 5.25pt;border-style:none none solid none;border-bottom-width:1pt;border-bottom-color:#BEC4CB;"">
<span style=""background-color:#D6DCE4;"">
<div><font face=""arial,sans-serif"" size=""1"" color=""#2E3235""><span style=""font-size:7.5pt;""><b>Qtty</b></span></font></div>
</span></td>
<td style=""background-color:#D6DCE4;padding:8.25pt 5.25pt;border-style:none none solid none;border-bottom-width:1pt;border-bottom-color:#BEC4CB;"">
<span style=""background-color:#D6DCE4;"">
<div><font face=""arial,sans-serif"" size=""1"" color=""#2E3235""><span style=""font-size:7.5pt;""><b>Price</b></span></font></div>
</span></td>
<td style=""background-color:#D6DCE4;padding:8.25pt 5.25pt;border-style:none none solid none;border-bottom-width:1pt;border-bottom-color:#BEC4CB;"">
<span style=""background-color:#D6DCE4;"">
<div><font face=""arial,sans-serif"" size=""1"" color=""#2E3235""><span style=""font-size:7.5pt;""><b>Total</b></span></font></div>
</span></td>
<td style=""background-color:#D6DCE4;padding:8.25pt 5.25pt;border-style:none none solid none;border-bottom-width:1pt;border-bottom-color:#BEC4CB;"">
<span style=""background-color:#D6DCE4;"">
<div><font face=""arial,sans-serif"" size=""1"" color=""#2E3235""><span style=""font-size:7.5pt;""><b>Remark</b></span></font></div>
</span></td>
</tr>
", supplier.SupplierName, file.FileNumber, DateTime.Now.Date.ToString("dd-MM-yyyy"), string.Format("{0:0.00}", total), note);

            foreach (var item in lineitems)
            {
                body = body + string.Format(@"
<tr>
<td style=""padding:8.25pt 5.25pt;border-style:none solid solid none;border-right-width:1pt;border-bottom-width:1pt;border-right-color:#BEC4CB;border-bottom-color:#BEC4CB;"">
<div><font face=""arial,sans-serif"" size=""1"" color=""#2E3235""><span style=""font-size:7.5pt;"">{1}</span></font></div>
</td>
<td style=""padding:8.25pt 5.25pt;border-style:none solid solid none;border-right-width:1pt;border-bottom-width:1pt;border-right-color:#BEC4CB;border-bottom-color:#BEC4CB;"">
<div><font face=""arial,sans-serif"" size=""1"" color=""#2E3235""><span style=""font-size:7.5pt;"">{2}</span></font></div>
</td>
<td style=""padding:8.25pt 5.25pt;border-style:none solid solid none;border-right-width:1pt;border-bottom-width:1pt;border-right-color:#BEC4CB;border-bottom-color:#BEC4CB;"">
<div><font face=""arial,sans-serif"" size=""1"" color=""#2E3235""><span style=""font-size:7.5pt;"">{3}</span></font></div>
</td>
<td style=""padding:8.25pt 5.25pt;border-style:none solid solid none;border-right-width:1pt;border-bottom-width:1pt;border-right-color:#BEC4CB;border-bottom-color:#BEC4CB;"">
<div><font face=""arial,sans-serif"" size=""1"" color=""#2E3235""><span style=""font-size:7.5pt;"">{4}</span></font></div>
</td>
<td style=""padding:8.25pt 5.25pt;border-style:none solid solid none;border-right-width:1pt;border-bottom-width:1pt;border-right-color:#BEC4CB;border-bottom-color:#BEC4CB;"">
<div><font face=""arial,sans-serif"" size=""1"" color=""#2E3235""><span style=""font-size:7.5pt;"">{5}</span></font></div>
</td>
<td style=""padding:8.25pt 5.25pt;border-style:none none solid none;border-bottom-width:1pt;border-bottom-color:#BEC4CB;"">
<div><font face=""arial,sans-serif"" size=""1"" color=""#2E3235""><span style=""font-size:7.5pt;"">{6}</span></font></div>
</td>
<td style=""padding:8.25pt 5.25pt;border-style:none none solid solid;border-bottom-width:1pt;border-left-width:1pt;border-bottom-color:#BEC4CB;border-left-color:#BEC4CB;"">
<div><font face=""arial,sans-serif"" size=""1"" color=""#2E3235""><span style=""font-size:7.5pt;"">{7} {8}</span></font></div>
</td>
<td style=""padding:8.25pt 5.25pt;border-style:none none solid solid;border-bottom-width:1pt;border-left-width:1pt;border-bottom-color:#BEC4CB;border-left-color:#BEC4CB;"">
<div><font face=""arial,sans-serif"" size=""1"" color=""#2E3235""><span style=""font-size:7.5pt;"">{9}</span></font></div>
</td>
<td style=""padding:8.25pt 5.25pt;border-style:none none solid solid;border-bottom-width:1pt;border-left-width:1pt;border-bottom-color:#BEC4CB;border-left-color:#BEC4CB;"">
<div><font face=""arial,sans-serif"" size=""1"" color=""#2E3235""><span style=""font-size:7.5pt;"">{10}</span></font></div>
</td>
</tr>", "",item.ReferanceNumber.Name,item.Number,item.Impa,item.Description + "(" + item.SelectedSupplierTurkishDefinition + ")", item.Unit,item.AltQtty==null? item.Qtty :item.AltQtty,item.SelectedSupplierPrice,item.SelectedSupplierCurrency, string.Format("{0:0.00}", (item.SelectedSupplierPrice* isaltqtty(item.Qtty, item.AltQtty) )) , item.SelectedSupplierRemark);
            }

           


            body = body + string.Format(@"
</tbody></table>
</td>
</tr>
<tr>
<td><br>

<br>

<div style=""margin-left:33%;"">
<div style=""margin-top:14pt;margin-bottom:10px;""><font face=""Helvetica,arial,sans-serif"" size=""1"" color=""#212121""><span style=""font-size:13px;""><font color=""#212121""><b>{0}</b></font> <br>

<a href=""mailto:technicalmailgroup@adamarine.com"" target=""_blank"" rel=""noopener noreferrer"" style=""text-decoration:none;""><font color=""#477CCC""><span style=""display:inline;"">technicalmailgroup@adamarine.com</span></font></a> / <font color=""#212121"">+ 90 232 616 17 19 </font></span></font></div>
<div style=""margin-top:14pt;margin-bottom:10px;""><font face=""Helvetica,arial,sans-serif"" size=""1""><span style=""font-size:13px;""><font color=""#212121""><b>ADAMAR INTERNATIONAL SHIP SUPPLY CO.</b></font> <br>

<a href=""http://adamarine.com"" target=""_blank"" rel=""noopener noreferrer"" style=""text-decoration:none;""><font color=""#477CCC""><span style=""display:inline;"">adamarine.com</span></font></a> </span></font></div>
</div>
</td>
</tr>
<tr>
<td valign=""top"" style=""width:100%;"">
<div align=""center"" style=""text-align:center;margin-top:9px;""><font size=""1""><b>Adamar International Ship Supply</b></font></div>
<div align=""center"" style=""text-align:center;margin-top:14pt;margin-bottom:14pt;"">
<font size=""1""><b>CORAKLAR MAH. 5003 SOK. NO:26 ALIAGA ORGANIZE SANAYI BOLGESI</b></font></div>
<div align=""center"" style=""text-align:center;margin-top:14pt;margin-bottom:14pt;"">
<font size=""1""><b>Aliaga-Izmir ,&#8203; TURKEY</b></font></div>
</td>
</tr>
</tbody></table>
</td>
</tr>
</tbody></table>
", UserName);
  

 

            

            return body;
        }


        public string e_mailbody_technical_split_city(IEnumerable<OrderLineItem> lineitems, File file, string note, string UserName,Vessel vessel)
        {
            decimal total = 0;

            foreach (var item in lineitems)
            {

                if (item.AltQtty == null)
                {

                }
                total = total + (item.SelectedSupplierPrice * isaltqtty(item.Qtty, item.AltQtty));
            }

            string body = string.Format(@"
<table>
<tbody><tr>
<td style=""height:30pt;background-color:#13B5EA;padding:0 0 0 15pt;""><span style=""background-color:#13B5EA;"">
<div><font face=""arial,sans-serif"" size=""3"" color=""white""><span style=""font-size:11.5pt;""><b>ADAMAR INTERNATIONAL SHIP SUPPLY CO.</b></span></font></div>
</span></td>
</tr>
<tr>
<td valign=""top"" style=""padding:11.25pt 0 0 0;"">
<table width=""100%"" border=""0"" cellspacing=""0"" cellpadding=""0"" style=""width:100%;"">
<tbody><tr height=""47"" style=""height:28.5pt;"">
<td style=""height:28.5pt;background-color:#D6DCE4;padding:0 0 0 15pt;border-style:none none solid none;border-bottom-width:1.5pt;border-bottom-color:#13B5EA;"">
<span style=""background-color:#D6DCE4;"">
<div><font face=""arial,sans-serif"" size=""3"" color=""#525C64""><span style=""font-size:11.5pt;"">Üst Bilgiler:</span></font></div>
</span></td>
</tr>
<tr>
<td style=""padding:9pt;border:1pt solid #D6DCE4;"">
<table width=""100%"" border=""0"" cellspacing=""0"" cellpadding=""0"" style=""width:100%;"">
<tbody><tr>
<td width=""311"" valign=""top"" style=""width:311px;padding:0;"">
<table width=""100%"" border=""0"" cellspacing=""0"" cellpadding=""0"" style=""width:100%;"">
<tbody><tr>
<td width=""125"" style=""width:75pt;padding:0;"">
<div><font face=""arial,sans-serif"" size=""2"" color=""#525C64""><span style=""font-size:9pt;""><b>Firma</b></span></font></div>
</td>
<td style=""padding:0;"">
<div><font face=""arial,sans-serif"" size=""2"" color=""#008B00""><span style=""font-size:9pt;""><b>{0}</b></span></font></div>
</td>
</tr>
<tr>
<td width=""125"" style=""width:75pt;padding:0;"">
<div><font face=""arial,sans-serif"" size=""2"" color=""#525C64""><span style=""font-size:9pt;""><b>Dosya No:</b></span></font></div>
</td>
<td style=""padding:0;"">
<div><font face=""arial,sans-serif"" size=""2"" color=""#E81E24""><span style=""font-size:9pt;""><b>{1}</b></span></font></div>
</td>
</tr> 
<tr>
<td width=""208"" style=""width:125pt;padding:0;"">
<div><font face=""arial,sans-serif"" size=""2"" color=""#525C64""><span style=""font-size:9pt;""><b>Tarih</b></span></font></div>
</td>
<td style=""padding:0;"">
<div><font face=""arial,sans-serif"" size=""2"" color=""#2E3235""><span style=""font-size:9pt;""><b>{2}</b></span></font></div>
</td>
</tr>
<tr>
<td width=""208"" style=""width:125pt;padding:0;"">
<div><font face=""arial,sans-serif"" size=""2"" color=""#525C64""><span style=""font-size:9pt;""><b>Total</b></span></font></div>
</td>
<td style=""padding:0;"">
<div><font face=""arial,sans-serif"" size=""2"" color=""#2E3235""><span style=""font-size:9pt;""><b>{3}</b></span></font></div>
</td>
</tr>
</tbody></table>
</td>
</tr>
</tbody></table>
</td>
</tr>
</tbody></table>
</td>
</tr>
<tr>
<td valign=""top"" style=""padding:11.25pt 0 0 0;"">
<table width=""100%"" border=""0"" cellspacing=""0"" cellpadding=""0"" style=""width:100%;"">
<tbody><tr height=""47"" style=""height:28.5pt;"">
<td style=""height:28.5pt;background-color:#D6DCE4;padding:0 0 0 15pt;border-style:none none solid none;border-bottom-width:1.5pt;border-bottom-color:#13B5EA;"">
<span style=""background-color:#D6DCE4;"">
<div><font face=""arial,sans-serif"" size=""3"" color=""#525C64""><span style=""font-size:11.5pt;"">Not:</span></font></div>
</span></td>
</tr>
<tr>
<td style=""padding:9pt;border:1pt solid #D6DCE4;"">
<table width=""100%"" border=""0"" cellspacing=""0"" cellpadding=""0"" style=""width:100%;"">
<tbody><tr>
<td valign=""top"" style=""padding:0;"">
<div><font face=""arial,sans-serif"" size=""2"" color=""#525C64""><span style=""font-size:9pt;"">{4}</span></font></div>
<div style=""margin-top:14pt;margin-bottom:14pt;"">&nbsp;</div>
</td>
</tr>
</tbody></table>
</td>
</tr>
</tbody></table>
</td>
</tr>
<tr>
<td valign=""top"" style=""padding:11.25pt 0;"">
<table width=""100%"" border=""0"" cellspacing=""0"" cellpadding=""0"" style=""width:100%;"">
<tbody><tr height=""47"" style=""height:28.5pt;"">
<td style=""height:28.5pt;background-color:#D6DCE4;padding:0 15pt;border-style:none none solid none;border-bottom-width:1.5pt;border-bottom-color:#13B5EA;"">
<span style=""background-color:#D6DCE4;"">
<table width=""100%"" border=""0"" cellspacing=""0"" cellpadding=""0"" style=""width:100%;"">
<tbody><tr>
<td style=""padding:0;"">
<div><font face=""arial,sans-serif"" size=""3"" color=""#525C64""><span style=""font-size:11.5pt;"">Ürünler :</span></font></div>
</td>
<td style=""padding:0;"">
<div align=""right"" style="""">&nbsp;</div>
</td>
</tr>
</tbody></table>
</span></td>
</tr>
<tr>
<td style=""padding:0;border:1pt solid #D6DCE4;"">
<table width=""100%"" border=""0"" cellspacing=""0"" cellpadding=""0"" style=""width:100%;"">
<tbody><tr>
<td style=""background-color:#D6DCE4;padding:8.25pt 5.25pt;border-style:none solid solid none;border-right-width:1pt;border-bottom-width:1pt;border-right-color:#BEC4CB;border-bottom-color:#BEC4CB;"">
<span style=""background-color:#D6DCE4;"">
<div><font face=""arial,sans-serif"" size=""1"" color=""#2E3235""><span style=""font-size:7.5pt;""><b>Ref No.</b></span></font></div>
</span></td>
<td style=""background-color:#D6DCE4;padding:8.25pt 5.25pt;border-style:none solid solid none;border-right-width:1pt;border-bottom-width:1pt;border-right-color:#BEC4CB;border-bottom-color:#BEC4CB;"">
<span style=""background-color:#D6DCE4;"">
<div><font face=""arial,sans-serif"" size=""1"" color=""#2E3235""><span style=""font-size:7.5pt;""><b>Nr.</b></span></font></div>
</span></td>
<td style=""background-color:#D6DCE4;padding:8.25pt 5.25pt;border-style:none solid solid none;border-right-width:1pt;border-bottom-width:1pt;border-right-color:#BEC4CB;border-bottom-color:#BEC4CB;"">
<span style=""background-color:#D6DCE4;"">
<div><font face=""arial,sans-serif"" size=""1"" color=""#2E3235""><span style=""font-size:7.5pt;""><b>Impa</b></span></font></div>
</span></td>
<td style=""background-color:#D6DCE4;padding:8.25pt 5.25pt;border-style:none none solid none;border-bottom-width:1pt;border-bottom-color:#BEC4CB;"">
<span style=""background-color:#D6DCE4;"">
<div><font face=""arial,sans-serif"" size=""1"" color=""#2E3235""><span style=""font-size:7.5pt;""><b>Description</b></span></font></div>
</span></td>
<td style=""background-color:#D6DCE4;padding:8.25pt 5.25pt;border-style:none none solid none;border-bottom-width:1pt;border-bottom-color:#BEC4CB;"">
<span style=""background-color:#D6DCE4;"">
<div><font face=""arial,sans-serif"" size=""1"" color=""#2E3235""><span style=""font-size:7.5pt;""><b>Unit</b></span></font></div>
</span></td>
<td style=""background-color:#D6DCE4;padding:8.25pt 5.25pt;border-style:none none solid none;border-bottom-width:1pt;border-bottom-color:#BEC4CB;"">
<span style=""background-color:#D6DCE4;"">
<div><font face=""arial,sans-serif"" size=""1"" color=""#2E3235""><span style=""font-size:7.5pt;""><b>Qtty</b></span></font></div>
</span></td>
<td style=""background-color:#D6DCE4;padding:8.25pt 5.25pt;border-style:none none solid none;border-bottom-width:1pt;border-bottom-color:#BEC4CB;"">
<span style=""background-color:#D6DCE4;"">
<div><font face=""arial,sans-serif"" size=""1"" color=""#2E3235""><span style=""font-size:7.5pt;""><b>Supplier</b></span></font></div>
</span></td>
<td style=""background-color:#D6DCE4;padding:8.25pt 5.25pt;border-style:none none solid none;border-bottom-width:1pt;border-bottom-color:#BEC4CB;"">
<span style=""background-color:#D6DCE4;"">
<div><font face=""arial,sans-serif"" size=""1"" color=""#2E3235""><span style=""font-size:7.5pt;""><b>Price</b></span></font></div>
</span></td>
<td style=""background-color:#D6DCE4;padding:8.25pt 5.25pt;border-style:none none solid none;border-bottom-width:1pt;border-bottom-color:#BEC4CB;"">
<span style=""background-color:#D6DCE4;"">
<div><font face=""arial,sans-serif"" size=""1"" color=""#2E3235""><span style=""font-size:7.5pt;""><b>Supplier R</b></span></font></div>
</span></td>
<td style=""background-color:#D6DCE4;padding:8.25pt 5.25pt;border-style:none none solid none;border-bottom-width:1pt;border-bottom-color:#BEC4CB;"">
<span style=""background-color:#D6DCE4;"">
<div><font face=""arial,sans-serif"" size=""1"" color=""#2E3235""><span style=""font-size:7.5pt;""><b>Total</b></span></font></div>
</span></td>
</tr>
", "ADAMAR SİPARİŞ", file.FileNumber + " / " + vessel.VesselName, DateTime.Now.Date.ToString("dd-MM-yyyy"), string.Format("{0:0.00}", total), note);

            foreach (var item in lineitems)
            {
                body = body + string.Format(@"
<tr>
<td style=""padding:8.25pt 5.25pt;border-style:none solid solid none;border-right-width:1pt;border-bottom-width:1pt;border-right-color:#BEC4CB;border-bottom-color:#BEC4CB;"">
<div><font face=""arial,sans-serif"" size=""1"" color=""#2E3235""><span style=""font-size:7.5pt;"">{1}</span></font></div>
</td>
<td style=""padding:8.25pt 5.25pt;border-style:none solid solid none;border-right-width:1pt;border-bottom-width:1pt;border-right-color:#BEC4CB;border-bottom-color:#BEC4CB;"">
<div><font face=""arial,sans-serif"" size=""1"" color=""#2E3235""><span style=""font-size:7.5pt;"">{2}</span></font></div>
</td>
<td style=""padding:8.25pt 5.25pt;border-style:none solid solid none;border-right-width:1pt;border-bottom-width:1pt;border-right-color:#BEC4CB;border-bottom-color:#BEC4CB;"">
<div><font face=""arial,sans-serif"" size=""1"" color=""#2E3235""><span style=""font-size:7.5pt;"">{3}</span></font></div>
</td>
<td style=""padding:8.25pt 5.25pt;border-style:none solid solid none;border-right-width:1pt;border-bottom-width:1pt;border-right-color:#BEC4CB;border-bottom-color:#BEC4CB;"">
<div><font face=""arial,sans-serif"" size=""1"" color=""#2E3235""><span style=""font-size:7.5pt;"">{4}</span></font></div>
</td>
<td style=""padding:8.25pt 5.25pt;border-style:none solid solid none;border-right-width:1pt;border-bottom-width:1pt;border-right-color:#BEC4CB;border-bottom-color:#BEC4CB;"">
<div><font face=""arial,sans-serif"" size=""1"" color=""#2E3235""><span style=""font-size:7.5pt;"">{5}</span></font></div>
</td>
<td style=""padding:8.25pt 5.25pt;border-style:none solid solid none;border-right-width:1pt;border-bottom-width:1pt;border-right-color:#BEC4CB;border-bottom-color:#BEC4CB;"">
<div><font face=""arial,sans-serif"" size=""1"" color=""#2E3235""><span style=""font-size:7.5pt;"">{6}</span></font></div>
</td>
<td style=""padding:8.25pt 5.25pt;border-style:none solid solid none;border-right-width:1pt;border-bottom-width:1pt;border-right-color:#BEC4CB;border-bottom-color:#BEC4CB;"">
<div><font face=""arial,sans-serif"" size=""1"" color=""#2E3235""><span style=""font-size:7.5pt;"">{10}</span></font></div>
</td>
<td style=""padding:8.25pt 5.25pt;border-style:none solid solid none;border-right-width:1pt;border-bottom-width:1pt;border-right-color:#BEC4CB;border-bottom-color:#BEC4CB;"">
<div><font face=""arial,sans-serif"" size=""1"" color=""#2E3235""><span style=""font-size:7.5pt;"">{7} {8}</span></font></div>
</td>
<td style=""padding:8.25pt 5.25pt;border-style:none solid solid none;border-right-width:1pt;border-bottom-width:1pt;border-right-color:#BEC4CB;border-bottom-color:#BEC4CB;"">
<div><font face=""arial,sans-serif"" size=""1"" color=""#2E3235""><span style=""font-size:7.5pt;"">{11}</span></font></div>
</td>
<td style=""padding:8.25pt 5.25pt;border-style:none none solid solid;border-bottom-width:1pt;border-left-width:1pt;border-bottom-color:#BEC4CB;border-left-color:#BEC4CB;"">
<div><font face=""arial,sans-serif"" size=""1"" color=""#2E3235""><span style=""font-size:7.5pt;"">{9}</span></font></div>
</td>
</tr>", "", item.ReferanceNumber.Name, item.Number, item.Impa, item.Description + " R " + item.Remark, item.Unit,(item.AltQtty==null)? item.Qtty :item.AltQtty, item.SelectedSupplierPrice, item.SelectedSupplierCurrency, string.Format("{0:0.00}", (item.SelectedSupplierPrice * isaltqtty(item.Qtty, item.AltQtty))),item.SelectedSupplierName,item.SelectedSupplierRemark);
            }




            body = body + string.Format(@"
</tbody></table>
</td>
</tr>
<tr>
<td><br>

<br>

<div style=""margin-left:33%;"">
<div style=""margin-top:14pt;margin-bottom:10px;""><font face=""Helvetica,arial,sans-serif"" size=""1"" color=""#212121""><span style=""font-size:13px;""><font color=""#212121""><b>{0}</b></font> <br>

<a href=""mailto:technicalmailgroup@adamarine.com"" target=""_blank"" rel=""noopener noreferrer"" style=""text-decoration:none;""><font color=""#477CCC""><span style=""display:inline;"">technicalmailgroup@adamarine.com</span></font></a> / <font color=""#212121"">+ 90 232 616 17 19 </font></span></font></div>
<div style=""margin-top:14pt;margin-bottom:10px;""><font face=""Helvetica,arial,sans-serif"" size=""1""><span style=""font-size:13px;""><font color=""#212121""><b>ADAMAR INTERNATIONAL SHIP SUPPLY CO.</b></font> <br>

<a href=""http://adamarine.com"" target=""_blank"" rel=""noopener noreferrer"" style=""text-decoration:none;""><font color=""#477CCC""><span style=""display:inline;"">adamarine.com</span></font></a> </span></font></div>
</div>
</td>
</tr>
<tr>
<td valign=""top"" style=""width:100%;"">
<div align=""center"" style=""text-align:center;margin-top:9px;""><font size=""1""><b>Adamar International Ship Supply</b></font></div>
<div align=""center"" style=""text-align:center;margin-top:14pt;margin-bottom:14pt;"">
<font size=""1""><b>CORAKLAR MAH. 5003 SOK. NO:26 ALIAGA ORGANIZE SANAYI BOLGESI</b></font></div>
<div align=""center"" style=""text-align:center;margin-top:14pt;margin-bottom:14pt;"">
<font size=""1""><b>Aliaga-Izmir ,&#8203; TURKEY</b></font></div>
</td>
</tr>
</tbody></table>
</td>
</tr>
</tbody></table>
", UserName);






            return body;
        }

        public string e_mailbody_technical_split_warehouse(IEnumerable<OrderLineItem> lineitems, File file, string note, string UserName, Vessel vessel)
        {
            decimal total = 0;

            CurrencyList.Currencies curr = (CurrencyList.Currencies)file.Currency;
             
            foreach (var item in lineitems)
            {

                if (item.AltQtty == null)
                {

                }
                total = total + (isaltprice((decimal)item.Price, item.AltPrice) * isaltqtty(item.Qtty, item.AltQtty));
            }

            string body = string.Format(@"
<table>
<tbody><tr>
<td style=""height:30pt;background-color:#13B5EA;padding:0 0 0 15pt;""><span style=""background-color:#13B5EA;"">
<div><font face=""arial,sans-serif"" size=""3"" color=""white""><span style=""font-size:11.5pt;""><b>ADAMAR INTERNATIONAL SHIP SUPPLY CO.</b></span></font></div>
</span></td>
</tr>
<tr>
<td valign=""top"" style=""padding:11.25pt 0 0 0;"">
<table width=""100%"" border=""0"" cellspacing=""0"" cellpadding=""0"" style=""width:100%;"">
<tbody><tr height=""47"" style=""height:28.5pt;"">
<td style=""height:28.5pt;background-color:#D6DCE4;padding:0 0 0 15pt;border-style:none none solid none;border-bottom-width:1.5pt;border-bottom-color:#13B5EA;"">
<span style=""background-color:#D6DCE4;"">
<div><font face=""arial,sans-serif"" size=""3"" color=""#525C64""><span style=""font-size:11.5pt;"">Üst Bilgiler:</span></font></div>
</span></td>
</tr>
<tr>
<td style=""padding:9pt;border:1pt solid #D6DCE4;"">
<table width=""100%"" border=""0"" cellspacing=""0"" cellpadding=""0"" style=""width:100%;"">
<tbody><tr>
<td width=""311"" valign=""top"" style=""width:311px;padding:0;"">
<table width=""100%"" border=""0"" cellspacing=""0"" cellpadding=""0"" style=""width:100%;"">
<tbody><tr>
<td width=""125"" style=""width:75pt;padding:0;"">
<div><font face=""arial,sans-serif"" size=""2"" color=""#525C64""><span style=""font-size:9pt;""><b>Firma</b></span></font></div>
</td>
<td style=""padding:0;"">
<div><font face=""arial,sans-serif"" size=""2"" color=""#008B00""><span style=""font-size:9pt;""><b>{0}</b></span></font></div>
</td>
</tr>
<tr>
<td width=""125"" style=""width:75pt;padding:0;"">
<div><font face=""arial,sans-serif"" size=""2"" color=""#525C64""><span style=""font-size:9pt;""><b>Dosya No:</b></span></font></div>
</td>
<td style=""padding:0;"">
<div><font face=""arial,sans-serif"" size=""2"" color=""#E81E24""><span style=""font-size:9pt;""><b>{1}</b></span></font></div>
</td>
</tr> 

<tr>
<td width=""125"" style=""width:75pt;padding:0;"">
<div><font face=""arial,sans-serif"" size=""2"" color=""#525C64""><span style=""font-size:9pt;""><b>Döviz:</b></span></font></div>
</td>
<td style=""padding:0;"">
<div><font face=""arial,sans-serif"" size=""2"" color=""#E81E24""><span style=""font-size:9pt;""><b>{5}</b></span></font></div>
</td>
</tr> 

<tr>
<td width=""125"" style=""width:75pt;padding:0;"">
<div><font face=""arial,sans-serif"" size=""2"" color=""#525C64""><span style=""font-size:9pt;""><b>İskonto:</b></span></font></div>
</td>
<td style=""padding:0;"">
<div><font face=""arial,sans-serif"" size=""2"" color=""#E81E24""><span style=""font-size:9pt;""><b>{6}</b></span></font></div>
</td>
</tr> 

<tr>
<td width=""125"" style=""width:75pt;padding:0;"">
<div><font face=""arial,sans-serif"" size=""2"" color=""#525C64""><span style=""font-size:9pt;""><b>Liman:</b></span></font></div>
</td>
<td style=""padding:0;"">
<div><font face=""arial,sans-serif"" size=""2"" color=""#E81E24""><span style=""font-size:9pt;""><b>{7}</b></span></font></div>
</td>
</tr> 

<tr>
<td width=""208"" style=""width:125pt;padding:0;"">
<div><font face=""arial,sans-serif"" size=""2"" color=""#525C64""><span style=""font-size:9pt;""><b>Tarih</b></span></font></div>
</td>
<td style=""padding:0;"">
<div><font face=""arial,sans-serif"" size=""2"" color=""#2E3235""><span style=""font-size:9pt;""><b>{2}</b></span></font></div>
</td>
</tr>
<tr>
<td width=""208"" style=""width:125pt;padding:0;"">
<div><font face=""arial,sans-serif"" size=""2"" color=""#525C64""><span style=""font-size:9pt;""><b>Total</b></span></font></div>
</td>
<td style=""padding:0;"">
<div><font face=""arial,sans-serif"" size=""2"" color=""#2E3235""><span style=""font-size:9pt;""><b>{3}</b></span></font></div>
</td>
</tr>
</tbody></table>
</td>
</tr>
</tbody></table>
</td>
</tr>
</tbody></table>
</td>
</tr>
<tr>
<td valign=""top"" style=""padding:11.25pt 0 0 0;"">
<table width=""100%"" border=""0"" cellspacing=""0"" cellpadding=""0"" style=""width:100%;"">
<tbody><tr height=""47"" style=""height:28.5pt;"">
<td style=""height:28.5pt;background-color:#D6DCE4;padding:0 0 0 15pt;border-style:none none solid none;border-bottom-width:1.5pt;border-bottom-color:#13B5EA;"">
<span style=""background-color:#D6DCE4;"">
<div><font face=""arial,sans-serif"" size=""3"" color=""#525C64""><span style=""font-size:11.5pt;"">Not:</span></font></div>
</span></td>
</tr>
<tr>
<td style=""padding:9pt;border:1pt solid #D6DCE4;"">
<table width=""100%"" border=""0"" cellspacing=""0"" cellpadding=""0"" style=""width:100%;"">
<tbody><tr>
<td valign=""top"" style=""padding:0;"">
<div><font face=""arial,sans-serif"" size=""2"" color=""#525C64""><span style=""font-size:9pt;"">{4}</span></font></div>
<div style=""margin-top:14pt;margin-bottom:14pt;"">&nbsp;</div>
</td>
</tr>
</tbody></table>
</td>
</tr>
</tbody></table>
</td>
</tr>
<tr>
<td valign=""top"" style=""padding:11.25pt 0;"">
<table width=""100%"" border=""0"" cellspacing=""0"" cellpadding=""0"" style=""width:100%;"">
<tbody><tr height=""47"" style=""height:28.5pt;"">
<td style=""height:28.5pt;background-color:#D6DCE4;padding:0 15pt;border-style:none none solid none;border-bottom-width:1.5pt;border-bottom-color:#13B5EA;"">
<span style=""background-color:#D6DCE4;"">
<table width=""100%"" border=""0"" cellspacing=""0"" cellpadding=""0"" style=""width:100%;"">
<tbody><tr>
<td style=""padding:0;"">
<div><font face=""arial,sans-serif"" size=""3"" color=""#525C64""><span style=""font-size:11.5pt;"">Ürünler :</span></font></div>
</td>
<td style=""padding:0;"">
<div align=""right"" style="""">&nbsp;</div>
</td>
</tr>
</tbody></table>
</span></td>
</tr>
<tr>
<td style=""padding:0;border:1pt solid #D6DCE4;"">
<table width=""100%"" border=""0"" cellspacing=""0"" cellpadding=""0"" style=""width:100%;"">
<tbody><tr>
<td style=""background-color:#D6DCE4;padding:8.25pt 5.25pt;border-style:none solid solid none;border-right-width:1pt;border-bottom-width:1pt;border-right-color:#BEC4CB;border-bottom-color:#BEC4CB;"">
<span style=""background-color:#D6DCE4;"">
<div><font face=""arial,sans-serif"" size=""1"" color=""#2E3235""><span style=""font-size:7.5pt;""><b>Ref No.</b></span></font></div>
</span></td>
<td style=""background-color:#D6DCE4;padding:8.25pt 5.25pt;border-style:none solid solid none;border-right-width:1pt;border-bottom-width:1pt;border-right-color:#BEC4CB;border-bottom-color:#BEC4CB;"">
<span style=""background-color:#D6DCE4;"">
<div><font face=""arial,sans-serif"" size=""1"" color=""#2E3235""><span style=""font-size:7.5pt;""><b>Nr.</b></span></font></div>
</span></td>
<td style=""background-color:#D6DCE4;padding:8.25pt 5.25pt;border-style:none solid solid none;border-right-width:1pt;border-bottom-width:1pt;border-right-color:#BEC4CB;border-bottom-color:#BEC4CB;"">
<span style=""background-color:#D6DCE4;"">
<div><font face=""arial,sans-serif"" size=""1"" color=""#2E3235""><span style=""font-size:7.5pt;""><b>Impa</b></span></font></div>
</span></td>
<td style=""background-color:#D6DCE4;padding:8.25pt 5.25pt;border-style:none none solid none;border-bottom-width:1pt;border-bottom-color:#BEC4CB;"">
<span style=""background-color:#D6DCE4;"">
<div><font face=""arial,sans-serif"" size=""1"" color=""#2E3235""><span style=""font-size:7.5pt;""><b>Description</b></span></font></div>
</span></td>
<td style=""background-color:#D6DCE4;padding:8.25pt 5.25pt;border-style:none none solid none;border-bottom-width:1pt;border-bottom-color:#BEC4CB;"">
<span style=""background-color:#D6DCE4;"">
<div><font face=""arial,sans-serif"" size=""1"" color=""#2E3235""><span style=""font-size:7.5pt;""><b>Unit</b></span></font></div>
</span></td>
<td style=""background-color:#D6DCE4;padding:8.25pt 5.25pt;border-style:none none solid none;border-bottom-width:1pt;border-bottom-color:#BEC4CB;"">
<span style=""background-color:#D6DCE4;"">
<div><font face=""arial,sans-serif"" size=""1"" color=""#2E3235""><span style=""font-size:7.5pt;""><b>Qtty</b></span></font></div>
</span></td>
<td style=""background-color:#D6DCE4;padding:8.25pt 5.25pt;border-style:none none solid none;border-bottom-width:1pt;border-bottom-color:#BEC4CB;"">
<span style=""background-color:#D6DCE4;"">
<div><font face=""arial,sans-serif"" size=""1"" color=""#2E3235""><span style=""font-size:7.5pt;""><b>Price</b></span></font></div>
</span></td>
<td style=""background-color:#D6DCE4;padding:8.25pt 5.25pt;border-style:none none solid none;border-bottom-width:1pt;border-bottom-color:#BEC4CB;"">
<span style=""background-color:#D6DCE4;"">
<div><font face=""arial,sans-serif"" size=""1"" color=""#2E3235""><span style=""font-size:7.5pt;""><b>A. Unit</b></span></font></div>
</span></td>
<td style=""background-color:#D6DCE4;padding:8.25pt 5.25pt;border-style:none none solid none;border-bottom-width:1pt;border-bottom-color:#BEC4CB;"">
<span style=""background-color:#D6DCE4;"">
<div><font face=""arial,sans-serif"" size=""1"" color=""#2E3235""><span style=""font-size:7.5pt;""><b>A. Qtty</b></span></font></div>
</span></td>
<td style=""background-color:#D6DCE4;padding:8.25pt 5.25pt;border-style:none none solid none;border-bottom-width:1pt;border-bottom-color:#BEC4CB;"">
<span style=""background-color:#D6DCE4;"">A
<div><font face=""arial,sans-serif"" size=""1"" color=""#2E3235""><span style=""font-size:7.5pt;""><b>A. Price</b></span></font></div>
</span></td>
<td style=""background-color:#D6DCE4;padding:8.25pt 5.25pt;border-style:none none solid none;border-bottom-width:1pt;border-bottom-color:#BEC4CB;"">
<span style=""background-color:#D6DCE4;"">
<div><font face=""arial,sans-serif"" size=""1"" color=""#2E3235""><span style=""font-size:7.5pt;""><b>Total</b></span></font></div>
</span></td>
</tr>
", "ADAMAR SİPARİŞ", file.FileNumber + " / " + vessel.VesselName, DateTime.Now.Date.ToString("dd-MM-yyyy"), string.Format("{0:0.00}", total), note, curr,file.Discount,file.DeliveryPlace);

            foreach (var item in lineitems)
            {
                body = body + string.Format(@"
<tr>
<td style=""padding:8.25pt 5.25pt;border-style:none solid solid none;border-right-width:1pt;border-bottom-width:1pt;border-right-color:#BEC4CB;border-bottom-color:#BEC4CB;"">
<div><font face=""arial,sans-serif"" size=""1"" color=""#2E3235""><span style=""font-size:7.5pt;"">{1}</span></font></div>
</td>
<td style=""padding:8.25pt 5.25pt;border-style:none solid solid none;border-right-width:1pt;border-bottom-width:1pt;border-right-color:#BEC4CB;border-bottom-color:#BEC4CB;"">
<div><font face=""arial,sans-serif"" size=""1"" color=""#2E3235""><span style=""font-size:7.5pt;"">{2}</span></font></div>
</td>
<td style=""padding:8.25pt 5.25pt;border-style:none solid solid none;border-right-width:1pt;border-bottom-width:1pt;border-right-color:#BEC4CB;border-bottom-color:#BEC4CB;"">
<div><font face=""arial,sans-serif"" size=""1"" color=""#2E3235""><span style=""font-size:7.5pt;"">{3}</span></font></div>
</td>
<td style=""padding:8.25pt 5.25pt;border-style:none solid solid none;border-right-width:1pt;border-bottom-width:1pt;border-right-color:#BEC4CB;border-bottom-color:#BEC4CB;"">
<div><font face=""arial,sans-serif"" size=""1"" color=""#2E3235""><span style=""font-size:7.5pt;"">{4}</span></font></div>
</td>
<td style=""padding:8.25pt 5.25pt;border-style:none solid solid none;border-right-width:1pt;border-bottom-width:1pt;border-right-color:#BEC4CB;border-bottom-color:#BEC4CB;"">
<div><font face=""arial,sans-serif"" size=""1"" color=""#2E3235""><span style=""font-size:7.5pt;"">{5}</span></font></div>
</td>
<td style=""padding:8.25pt 5.25pt;border-style:none none solid none;border-bottom-width:1pt;border-bottom-color:#BEC4CB;"">
<div><font face=""arial,sans-serif"" size=""1"" color=""#2E3235""><span style=""font-size:7.5pt;"">{6}</span></font></div>
</td>
<td style=""padding:8.25pt 5.25pt;border-style:none none solid solid;border-bottom-width:1pt;border-left-width:1pt;border-bottom-color:#BEC4CB;border-left-color:#BEC4CB;"">
<div><font face=""arial,sans-serif"" size=""1"" color=""#2E3235""><span style=""font-size:7.5pt;"">{7} {8}</span></font></div>
</td>
<td style=""padding:8.25pt 5.25pt;border-style:none none solid solid;border-bottom-width:1pt;border-left-width:1pt;border-bottom-color:#BEC4CB;border-left-color:#BEC4CB;"">
<div><font face=""arial,sans-serif"" size=""1"" color=""#2E3235""><span style=""font-size:7.5pt;"">{10}</span></font></div>
</td>
<td style=""padding:8.25pt 5.25pt;border-style:none none solid solid;border-bottom-width:1pt;border-left-width:1pt;border-bottom-color:#BEC4CB;border-left-color:#BEC4CB;"">
<div><font face=""arial,sans-serif"" size=""1"" color=""#2E3235""><span style=""font-size:7.5pt;"">{11}</span></font></div>
</td>
<td style=""padding:8.25pt 5.25pt;border-style:none none solid solid;border-bottom-width:1pt;border-left-width:1pt;border-bottom-color:#BEC4CB;border-left-color:#BEC4CB;"">
<div><font face=""arial,sans-serif"" size=""1"" color=""#2E3235""><span style=""font-size:7.5pt;"">{12}</span></font></div>
</td>
<td style=""padding:8.25pt 5.25pt;border-style:none none solid solid;border-bottom-width:1pt;border-left-width:1pt;border-bottom-color:#BEC4CB;border-left-color:#BEC4CB;"">
<div><font face=""arial,sans-serif"" size=""1"" color=""#2E3235""><span style=""font-size:7.5pt;"">{9}</span></font></div>
</td>
</tr>", "", item.ReferanceNumber.Name, item.Number, item.Impa, item.Description, item.Unit, item.Qtty, item.Price, curr, string.Format("{0:0.00}", (isaltprice((decimal)item.Price, item.AltPrice) * isaltqtty(item.Qtty, item.AltQtty))), item.AltUnit, item.AltQtty, item.AltPrice == null ? "":item.AltPrice.ToString() + " " + curr);
            }




            body = body + string.Format(@"
</tbody></table>
</td>
</tr>
<tr>
<td><br>

<br>

<div style=""margin-left:33%;"">
<div style=""margin-top:14pt;margin-bottom:10px;""><font face=""Helvetica,arial,sans-serif"" size=""1"" color=""#212121""><span style=""font-size:13px;""><font color=""#212121""><b>{0}</b></font> <br>

<a href=""mailto:technicalmailgroup@adamarine.com"" target=""_blank"" rel=""noopener noreferrer"" style=""text-decoration:none;""><font color=""#477CCC""><span style=""display:inline;"">technicalmailgroup@adamarine.com</span></font></a> / <font color=""#212121"">+ 90 232 616 17 19 </font></span></font></div>
<div style=""margin-top:14pt;margin-bottom:10px;""><font face=""Helvetica,arial,sans-serif"" size=""1""><span style=""font-size:13px;""><font color=""#212121""><b>ADAMAR INTERNATIONAL SHIP SUPPLY CO.</b></font> <br>

<a href=""http://adamarine.com"" target=""_blank"" rel=""noopener noreferrer"" style=""text-decoration:none;""><font color=""#477CCC""><span style=""display:inline;"">adamarine.com</span></font></a> </span></font></div>
</div>
</td>
</tr>
<tr>
<td valign=""top"" style=""width:100%;"">
<div align=""center"" style=""text-align:center;margin-top:9px;""><font size=""1""><b>Adamar International Ship Supply</b></font></div>
<div align=""center"" style=""text-align:center;margin-top:14pt;margin-bottom:14pt;"">
<font size=""1""><b>CORAKLAR MAH. 5003 SOK. NO:26 ALIAGA ORGANIZE SANAYI BOLGESI</b></font></div>
<div align=""center"" style=""text-align:center;margin-top:14pt;margin-bottom:14pt;"">
<font size=""1""><b>Aliaga-Izmir ,&#8203; TURKEY</b></font></div>
</td>
</tr>
</tbody></table>
</td>
</tr>
</tbody></table>
", UserName);






            return body;
        }


        public string e_mailbody_suppliercomplated(IEnumerable<SupplierPriceLineItem> lineitems, File file, Supplier supplier,string Name)
        {
            decimal total = 0;

            foreach (var item in lineitems)
            {
 
                total = total + (item.SupplierPrice * item.LineItem.Qtty);
            }

            string body = string.Format(@"
<table>
<tbody><tr>
<td style=""height:30pt;background-color:#13B5EA;padding:0 0 0 15pt;""><span style=""background-color:#13B5EA;"">
<div><font face=""arial,sans-serif"" size=""3"" color=""white""><span style=""font-size:11.5pt;""><b>ADAMAR INTERNATIONAL SHIP SUPPLY CO.</b></span></font></div>
</span></td>
</tr>
<tr>
<td valign=""top"" style=""padding:11.25pt 0 0 0;"">
<table width=""100%"" border=""0"" cellspacing=""0"" cellpadding=""0"" style=""width:100%;"">
<tbody><tr height=""47"" style=""height:28.5pt;"">
<td style=""height:28.5pt;background-color:#D6DCE4;padding:0 0 0 15pt;border-style:none none solid none;border-bottom-width:1.5pt;border-bottom-color:#13B5EA;"">
<span style=""background-color:#D6DCE4;"">
<div><font face=""arial,sans-serif"" size=""3"" color=""#525C64""><span style=""font-size:11.5pt;"">Üst Bilgiler:</span></font></div>
</span></td>
</tr>
<tr>
<td style=""padding:9pt;border:1pt solid #D6DCE4;"">
<table width=""100%"" border=""0"" cellspacing=""0"" cellpadding=""0"" style=""width:100%;"">
<tbody><tr>
<td width=""311"" valign=""top"" style=""width:311px;padding:0;"">
<table width=""100%"" border=""0"" cellspacing=""0"" cellpadding=""0"" style=""width:100%;"">
<tbody><tr>
<td width=""125"" style=""width:75pt;padding:0;"">
<div><font face=""arial,sans-serif"" size=""2"" color=""#525C64""><span style=""font-size:9pt;""><b>Firma</b></span></font></div>
</td>
<td style=""padding:0;"">
<div><font face=""arial,sans-serif"" size=""2"" color=""#008B00""><span style=""font-size:9pt;""><b>{0}</b></span></font></div>
</td>
</tr>
<tr>
<td width=""125"" style=""width:75pt;padding:0;"">
<div><font face=""arial,sans-serif"" size=""2"" color=""#525C64""><span style=""font-size:9pt;""><b>Dosya No:</b></span></font></div>
</td>
<td style=""padding:0;"">
<div><font face=""arial,sans-serif"" size=""2"" color=""#E81E24""><span style=""font-size:9pt;""><b>{1}</b></span></font></div>
</td>
</tr> 
<tr>
<td width=""208"" style=""width:125pt;padding:0;"">
<div><font face=""arial,sans-serif"" size=""2"" color=""#525C64""><span style=""font-size:9pt;""><b>Tarih</b></span></font></div>
</td>
<td style=""padding:0;"">
<div><font face=""arial,sans-serif"" size=""2"" color=""#2E3235""><span style=""font-size:9pt;""><b>{2}</b></span></font></div>
</td>
</tr>
<tr>
<td width=""208"" style=""width:125pt;padding:0;"">
<div><font face=""arial,sans-serif"" size=""2"" color=""#525C64""><span style=""font-size:9pt;""><b>Total</b></span></font></div>
</td>
<td style=""padding:0;"">
<div><font face=""arial,sans-serif"" size=""2"" color=""#2E3235""><span style=""font-size:9pt;""><b>{3}</b></span></font></div>
</td>
</tr>
</tbody></table>
</td>
</tr>
</tbody></table>
</td>
</tr>
</tbody></table>
</td>
</tr>
<tr>
<td valign=""top"" style=""padding:11.25pt 0 0 0;"">
<table width=""100%"" border=""0"" cellspacing=""0"" cellpadding=""0"" style=""width:100%;"">
<tbody><tr height=""47"" style=""height:28.5pt;"">
<td style=""height:28.5pt;background-color:#D6DCE4;padding:0 0 0 15pt;border-style:none none solid none;border-bottom-width:1.5pt;border-bottom-color:#13B5EA;"">
<span style=""background-color:#D6DCE4;"">
<div><font face=""arial,sans-serif"" size=""3"" color=""#525C64""><span style=""font-size:11.5pt;"">Not:</span></font></div>
</span></td>
</tr>
<tr>
<td style=""padding:9pt;border:1pt solid #D6DCE4;"">
<table width=""100%"" border=""0"" cellspacing=""0"" cellpadding=""0"" style=""width:100%;"">
<tbody><tr>
<td valign=""top"" style=""padding:0;"">
<div><font face=""arial,sans-serif"" size=""2"" color=""#525C64""><span style=""font-size:9pt;"">{4}</span></font></div>
<div style=""margin-top:14pt;margin-bottom:14pt;"">&nbsp;</div>
</td>
</tr>
</tbody></table>
</td>
</tr>
</tbody></table>
</td>
</tr>
<tr>
<td valign=""top"" style=""padding:11.25pt 0;"">
<table width=""100%"" border=""0"" cellspacing=""0"" cellpadding=""0"" style=""width:100%;"">
<tbody><tr height=""47"" style=""height:28.5pt;"">
<td style=""height:28.5pt;background-color:#D6DCE4;padding:0 15pt;border-style:none none solid none;border-bottom-width:1.5pt;border-bottom-color:#13B5EA;"">
<span style=""background-color:#D6DCE4;"">
<table width=""100%"" border=""0"" cellspacing=""0"" cellpadding=""0"" style=""width:100%;"">
<tbody><tr>
<td style=""padding:0;"">
<div><font face=""arial,sans-serif"" size=""3"" color=""#525C64""><span style=""font-size:11.5pt;"">Ürünler :</span></font></div>
</td>
<td style=""padding:0;"">
<div align=""right"" style="""">&nbsp;</div>
</td>
</tr>
</tbody></table>
</span></td>
</tr>
<tr>
<td style=""padding:0;border:1pt solid #D6DCE4;"">
<table width=""100%"" border=""0"" cellspacing=""0"" cellpadding=""0"" style=""width:100%;"">
<tbody><tr>
<td style=""background-color:#D6DCE4;padding:8.25pt 5.25pt;border-style:none solid solid none;border-right-width:1pt;border-bottom-width:1pt;border-right-color:#BEC4CB;border-bottom-color:#BEC4CB;"">
<span style=""background-color:#D6DCE4;"">
<div><font face=""arial,sans-serif"" size=""1"" color=""#2E3235""><span style=""font-size:7.5pt;""><b>Ref Id.</b></span></font></div>
</span></td>
<td style=""background-color:#D6DCE4;padding:8.25pt 5.25pt;border-style:none solid solid none;border-right-width:1pt;border-bottom-width:1pt;border-right-color:#BEC4CB;border-bottom-color:#BEC4CB;"">
<span style=""background-color:#D6DCE4;"">
<div><font face=""arial,sans-serif"" size=""1"" color=""#2E3235""><span style=""font-size:7.5pt;""><b>Nr.</b></span></font></div>
</span></td>
<td style=""background-color:#D6DCE4;padding:8.25pt 5.25pt;border-style:none solid solid none;border-right-width:1pt;border-bottom-width:1pt;border-right-color:#BEC4CB;border-bottom-color:#BEC4CB;"">
<span style=""background-color:#D6DCE4;"">
<div><font face=""arial,sans-serif"" size=""1"" color=""#2E3235""><span style=""font-size:7.5pt;""><b>Impa</b></span></font></div>
</span></td>
<td style=""background-color:#D6DCE4;padding:8.25pt 5.25pt;border-style:none none solid none;border-bottom-width:1pt;border-bottom-color:#BEC4CB;"">
<span style=""background-color:#D6DCE4;"">
<div><font face=""arial,sans-serif"" size=""1"" color=""#2E3235""><span style=""font-size:7.5pt;""><b>Description</b></span></font></div>
</span></td>
<td style=""background-color:#D6DCE4;padding:8.25pt 5.25pt;border-style:none none solid none;border-bottom-width:1pt;border-bottom-color:#BEC4CB;"">
<span style=""background-color:#D6DCE4;"">
<div><font face=""arial,sans-serif"" size=""1"" color=""#2E3235""><span style=""font-size:7.5pt;""><b>Unit</b></span></font></div>
</span></td>
<td style=""background-color:#D6DCE4;padding:8.25pt 5.25pt;border-style:none none solid none;border-bottom-width:1pt;border-bottom-color:#BEC4CB;"">
<span style=""background-color:#D6DCE4;"">
<div><font face=""arial,sans-serif"" size=""1"" color=""#2E3235""><span style=""font-size:7.5pt;""><b>Qtty</b></span></font></div>
</span></td>
<td style=""background-color:#D6DCE4;padding:8.25pt 5.25pt;border-style:none none solid none;border-bottom-width:1pt;border-bottom-color:#BEC4CB;"">
<span style=""background-color:#D6DCE4;"">
<div><font face=""arial,sans-serif"" size=""1"" color=""#2E3235""><span style=""font-size:7.5pt;""><b>Price</b></span></font></div>
</span></td>
<td style=""background-color:#D6DCE4;padding:8.25pt 5.25pt;border-style:none none solid none;border-bottom-width:1pt;border-bottom-color:#BEC4CB;"">
<span style=""background-color:#D6DCE4;"">
<div><font face=""arial,sans-serif"" size=""1"" color=""#2E3235""><span style=""font-size:7.5pt;""><b>Total</b></span></font></div>
</span></td>
</tr>
", supplier.SupplierName, file.FileNumber, DateTime.Now.Date.ToString("dd-MM-yyyy"), string.Format("{0:0.00}", total), "ONAYLAYAN: " +Name);

            foreach (var item in lineitems)
            {
                body = body + string.Format(@"
<tr>
<td style=""padding:8.25pt 5.25pt;border-style:none solid solid none;border-right-width:1pt;border-bottom-width:1pt;border-right-color:#BEC4CB;border-bottom-color:#BEC4CB;"">
<div><font face=""arial,sans-serif"" size=""1"" color=""#2E3235""><span style=""font-size:7.5pt;"">{1}</span></font></div>
</td>
<td style=""padding:8.25pt 5.25pt;border-style:none solid solid none;border-right-width:1pt;border-bottom-width:1pt;border-right-color:#BEC4CB;border-bottom-color:#BEC4CB;"">
<div><font face=""arial,sans-serif"" size=""1"" color=""#2E3235""><span style=""font-size:7.5pt;"">{2}</span></font></div>
</td>
<td style=""padding:8.25pt 5.25pt;border-style:none solid solid none;border-right-width:1pt;border-bottom-width:1pt;border-right-color:#BEC4CB;border-bottom-color:#BEC4CB;"">
<div><font face=""arial,sans-serif"" size=""1"" color=""#2E3235""><span style=""font-size:7.5pt;"">{3}</span></font></div>
</td>
<td style=""padding:8.25pt 5.25pt;border-style:none solid solid none;border-right-width:1pt;border-bottom-width:1pt;border-right-color:#BEC4CB;border-bottom-color:#BEC4CB;"">
<div><font face=""arial,sans-serif"" size=""1"" color=""#2E3235""><span style=""font-size:7.5pt;"">{4}</span></font></div>
</td>
<td style=""padding:8.25pt 5.25pt;border-style:none solid solid none;border-right-width:1pt;border-bottom-width:1pt;border-right-color:#BEC4CB;border-bottom-color:#BEC4CB;"">
<div><font face=""arial,sans-serif"" size=""1"" color=""#2E3235""><span style=""font-size:7.5pt;"">{5}</span></font></div>
</td>
<td style=""padding:8.25pt 5.25pt;border-style:none none solid none;border-bottom-width:1pt;border-bottom-color:#BEC4CB;"">
<div><font face=""arial,sans-serif"" size=""1"" color=""#2E3235""><span style=""font-size:7.5pt;"">{6}</span></font></div>
</td>
<td style=""padding:8.25pt 5.25pt;border-style:none none solid solid;border-bottom-width:1pt;border-left-width:1pt;border-bottom-color:#BEC4CB;border-left-color:#BEC4CB;"">
<div><font face=""arial,sans-serif"" size=""1"" color=""#2E3235""><span style=""font-size:7.5pt;"">{7} {8}</span></font></div>
</td>
<td style=""padding:8.25pt 5.25pt;border-style:none none solid solid;border-bottom-width:1pt;border-left-width:1pt;border-bottom-color:#BEC4CB;border-left-color:#BEC4CB;"">
<div><font face=""arial,sans-serif"" size=""1"" color=""#2E3235""><span style=""font-size:7.5pt;"">{9}</span></font></div>
</td>
</tr>", "", item.LineItem.ReferanceNumberId, item.LineItem.Number, item.LineItem.Impa, item.LineItem.Description + "(" + item.TurkishDescription + ")", item.LineItem.Unit, item.LineItem.Qtty, item.SupplierPrice, item.Currency, string.Format("{0:0.00}", (item.SupplierPrice * item.LineItem.Qtty)));
            }




            body = body + string.Format(@"
</tbody></table>
</td>
</tr>
<tr>
<td><br>

<br>

<div style=""margin-left:33%;"">
<div style=""margin-top:14pt;margin-bottom:10px;""><font face=""Helvetica,arial,sans-serif"" size=""1"" color=""#212121""><span style=""font-size:13px;""><font color=""#212121""><b> APPA </b></font> <br>

<a href=""mailto:technicalmailgroup@adamarine.com"" target=""_blank"" rel=""noopener noreferrer"" style=""text-decoration:none;""><font color=""#477CCC""><span style=""display:inline;"">technicalmailgroup@adamarine.com</span></font></a> / <font color=""#212121"">+ 90 232 616 17 19 </font></span></font></div>
<div style=""margin-top:14pt;margin-bottom:10px;""><font face=""Helvetica,arial,sans-serif"" size=""1""><span style=""font-size:13px;""><font color=""#212121""><b>ADAMAR INTERNATIONAL SHIP SUPPLY CO.</b></font> <br>

<a href=""http://adamarine.com"" target=""_blank"" rel=""noopener noreferrer"" style=""text-decoration:none;""><font color=""#477CCC""><span style=""display:inline;"">adamarine.com</span></font></a> </span></font></div>
</div>
</td>
</tr>
<tr>
<td valign=""top"" style=""width:100%;"">
<div align=""center"" style=""text-align:center;margin-top:9px;""><font size=""1""><b>Adamar International Ship Supply</b></font></div>
<div align=""center"" style=""text-align:center;margin-top:14pt;margin-bottom:14pt;"">
<font size=""1""><b>CORAKLAR MAH. 5003 SOK. NO:26 ALIAGA ORGANIZE SANAYI BOLGESI</b></font></div>
<div align=""center"" style=""text-align:center;margin-top:14pt;margin-bottom:14pt;"">
<font size=""1""><b>Aliaga-Izmir ,&#8203; TURKEY</b></font></div>
</td>
</tr>
</tbody></table>
</td>
</tr>
</tbody></table>
");






            return body;

        }


    }
}