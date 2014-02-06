using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AppraiseUtah.Client.ViewModels;
using System.Net.Mail;

namespace AppraiseUtah.Client.Utilities
{
    public static class MailUtility
    {

        #region Fields
        #endregion

        #region Properties
        #endregion

        #region Methods

        public static void SendConfirmationEmail(AppraisalViewModel appraisal)
        {
            SendConfirmationEmailToClient(appraisal);
            SendOrderEmailToAppraiser(appraisal);
        }

        #endregion

        #region Private Methods

        private static void SendConfirmationEmailToClient(AppraisalViewModel appraisal)
        {
            MailMessage message = new MailMessage();

            if (string.IsNullOrEmpty(appraisal.Appraisal.ClientPerson.Email))
            {
                throw new Exception("Client email is null or empty.  Cannot complete order");
            }

            //message.From = new MailAddress("WebOrders@appraiseutah.com", "Web Order");
            //message.To.Add(new MailAddress("orders@appraiseutah.com"));
            message.From = new MailAddress("ryan@lifferth.com", "AppraiseUtah.com");
            message.To.Add(new MailAddress(appraisal.Appraisal.ClientPerson.Email));
            message.CC.Add(new MailAddress("orders@appraiseutah.com"));

            message.IsBodyHtml = true;
            message.BodyEncoding = Encoding.UTF8;
            message.Subject = "AppraisalUtah.com Order Confirmation - ID: " + appraisal.Appraisal.Id;
            message.Body = BuildConfirmationBody(appraisal, true);

            SmtpClient smtpClient = new SmtpClient();
            smtpClient.Send(message);
        }

        private static void SendOrderEmailToAppraiser(AppraisalViewModel appraisal)
        {
            MailMessage message = new MailMessage();
            var appraiser = GetAppraiserForDisplay.GetAppraiser(appraisal.Appraisal.AppraiserId);
            if (string.IsNullOrEmpty(appraiser.Email))
            {
                throw new Exception("Appraiser email is null or empty.  Cannot complete order");
            }

            //message.From = new MailAddress("WebOrders@appraiseutah.com", "Web Order");
            //message.To.Add(new MailAddress("orders@appraiseutah.com"));
            message.From = new MailAddress("ryan@lifferth.com", "AppraiseUtah.com");
            message.To.Add(new MailAddress(appraiser.Email));
            message.CC.Add(new MailAddress("orders@appraiseutah.com"));

            message.IsBodyHtml = true;
            message.BodyEncoding = Encoding.UTF8;
            message.Subject = "New appraisal order from AppraisalUtah.com - " + appraisal.Appraisal.PropertyAddress.Address1 + ", " + appraisal.Appraisal.PropertyAddress.City + " (ID: " + appraisal.Appraisal.Id + ")";
            message.Body = BuildConfirmationBody(appraisal, false);

            SmtpClient smtpClient = new SmtpClient();
            smtpClient.EnableSsl = true;
            smtpClient.Send(message);
        }

        public static string BuildConfirmationBody(AppraisalViewModel appraisal, bool clientConfirmation)
        {
            var appraiser = GetAppraiserForDisplay.GetAppraiser(appraisal.Appraisal.AppraiserId);       // gets the AppraiserModel for the selected appraiser
            var professionalDesignations = (!string.IsNullOrEmpty(appraiser.ProfessionalDesignations)) ? " " + appraiser.ProfessionalDesignations : "";
            var propertyType = DropDownData.GetPropertyTypeList().Where(x => x.PropertyTypeCode == appraisal.Appraisal.PropertyTypeCode).FirstOrDefault();
            var appraisalPurpose = DropDownData.GetAppraisalPurposeList().Where(x => x.AppraisalPurposeCode == appraisal.Appraisal.AppraisalPurposeCode).FirstOrDefault();
            var propertyAddress = appraisal.Appraisal.PropertyAddress;
            var property = appraisal.Appraisal;
            var occupant = appraisal.Appraisal.OccupantPerson;
            bool legalSalesData = ((property.SalesContractPrice != null) || (!string.IsNullOrEmpty(property.LegalDescription))) ? true : false;

            var body = new StringBuilder();

            body.Append(@"<div style=""font-family:Tahoma, Calibri, Arial, sans-serif;font-size:15px;margin:10px 20px;padding:0"">");

            // Header
            if (clientConfirmation)
            {
                body.Append(@"<h2 style=""line-height:1.1;font-size:32px;margin-top:21px;margin-bottom:5px;color:#6abe59;font-weight:normal"">Thank you for your order at <em>AppraiseUtah.com</em></h2>");
                body.Append(@"<h4 style=""line-height:1.1;font-size:24px;margin-top:0;margin-bottom:10.5px;color:#6abe59;font-weight:normal"">Your appraisal order number is:  <strong style=""margin:0 5px"">" + appraisal.Appraisal.Id + "</strong></h4>");
            }
            else
            {
                body.Append(@"<h2 style=""line-height:1.1;font-size:32px;margin-top:21px;margin-bottom:5px;color:#6abe59;font-weight:normal"">You have received a new Appraisal Order through <em>AppraiseUtah.com</em></h2>");
                body.Append(@"<h4 style=""line-height:1.1;font-size:24px;margin-top:0;margin-bottom:10.5px;color:#6abe59;font-weight:normal"">The appraisal order number is:  <strong style=""margin:0 5px"">" + appraisal.Appraisal.Id + "</strong></h4>");
            }

            body.Append(@"<fieldset id=""ConfirmationInfo"" style=""border:none;border-bottom:1px dashed #a3a3a3;border-top:1px dashed #a3a3a3;margin-top:15px;min-height:200px;padding:0 10px 20px 0"">");
            body.Append(@"<legend style=""display:block;line-height:inherit;color:#333;border:none;font-size:22px;margin-bottom:5px;margin-left:15px;width:auto;padding:0 4px 4px"">Appraisal Order Information</legend>");

            // Build the APPRAISER section
            body.Append(@"<div style=""font-size:13px;overflow:auto;margin:0 0 8px;padding:5px 0"">");
            body.Append(@"<div class=""col-xs-2"" style=""color:#19aacf;float:left;font-weight:700;vertical-align:top;width:150px"">Appraiser</div>");
            body.Append(@"<div id=""appraiserDisplay"" style=""float:left"">");
            body.Append(@"<h4 style=""line-height:1.1;color:inherit;font-size:19px;margin-top:0;margin-bottom:10.5px;font-weight:normal"">" + appraiser.CompanyName + @"<span class=""designation"">" + professionalDesignations + "</span></h4>");
            body.Append(@"<div style=""clear:both;font-size:13px;line-height:normal;margin-top:0;overflow:auto"">");
            body.Append(@"<address style=""display:block;font-style:normal;width:180px;float:left!important"">");
            body.Append(appraiser.Address.Address1).Append("<br />");
            if (!string.IsNullOrEmpty(appraiser.Address.Address2))
            {
                body.Append(appraiser.Address.Address2).Append("<br />");
            }
            body.Append(appraiser.Address.City).Append(",");
            body.Append(appraiser.Address.StateCode).Append("  ");
            body.Append(appraiser.Address.PostalCode);
            body.Append(@"</address>");
            body.Append(@"<div class=""contact"" style=""float:left!important"">");
            body.Append(buildContactItems(appraiser));
            body.Append(@"</div>");
            body.Append(@"</div>");
            body.Append(@"</div>");
            body.Append(@"</div>");

            // Build the CLIENT section
            body.Append(@"<div style=""font-size:13px;overflow:auto;margin:0 0 8px;padding:5px 0"">");
            body.Append(@"<div class=""col-xs-2"" style=""color:#19aacf;float:left;font-weight:700;vertical-align:top;width:150px"">");
            if (clientConfirmation)
            {
                body.Append("Your Info");
            }
            else
            {
                body.Append("Client Info");
            }            
            body.Append(@"</div>");
            body.Append(@"<div style=""float:left"">");
            body.Append(@"<div style=""font-size:16px"">");
            body.Append(appraisal.Appraisal.ClientPerson.FirstName).Append(" ").Append(appraisal.Appraisal.ClientPerson.LastName);
            if (!string.IsNullOrEmpty(appraisal.Appraisal.ClientPerson.CompanyName))
            {
                body.Append(@" - <em>" + appraisal.Appraisal.ClientPerson.CompanyName + "</em>");
            }
            body.Append("</div>"); 
            body.Append(@"<address style=""display:block;font-style:normal;width:180px;float:left!important"">");
            body.Append(appraisal.Appraisal.ClientAddress.Address1).Append("<br />");
            if (!string.IsNullOrEmpty(appraisal.Appraisal.ClientAddress.Address2))
            {
                body.Append(appraisal.Appraisal.ClientAddress.Address2).Append("<br />");
            }
            body.Append(appraisal.Appraisal.ClientAddress.City).Append(",");
            body.Append(appraisal.Appraisal.ClientAddress.StateCode).Append("  ");
            body.Append(appraisal.Appraisal.ClientAddress.PostalCode);
            body.Append(@"</address>");
            body.Append(@"<div style=""float:left"">");
            body.Append(@"<abbr title=""phone"" style=""cursor:help;border-bottom:1px dotted #999;margin-right:6px;"">P:</abbr>" + appraisal.Appraisal.ClientPerson.Phone + "<br />");
            body.Append(@"<a href=""mailto:" + appraisal.Appraisal.ClientPerson.Email + @""" class=""email"" style=""color:#428bca;text-decoration:none"">" + appraisal.Appraisal.ClientPerson.Email + "</a>");
            body.Append(@"</div>");
            body.Append(@"</div>");
            body.Append(@"</div>");

            // Build the PROPERTY section
            body.Append(@"<div style=""font-size:13px;overflow:auto;margin:0 0 8px;padding:5px 0"">");
            body.Append(@"<div class=""col-xs-2"" style=""color:#19aacf;float:left;font-weight:700;vertical-align:top;width:150px"">Property Info</div>");
            body.Append(@"<div style=""float:left"">");

            body.Append(@"<address style=""display:block;font-style:normal;width:180px;float:left!important"">");
            body.Append(propertyType.PropertyTypeDescription).Append("<br />");
            body.Append(propertyAddress.Address1).Append("<br />");
            if (!string.IsNullOrEmpty(propertyAddress.Address2))
            {
                body.Append(propertyAddress.Address2).Append("<br />");
            }
            body.Append(propertyAddress.City).Append(", ").Append(propertyAddress.StateCode).Append(" ").Append(propertyAddress.PostalCode);
            body.Append("</address>");

            body.Append(@"<div style=""float:left"">");

            var dataPresent = false;
            if (OccupantDataPresent(appraisal))
            {
                body.Append(@"<div style=""float:left"">");
                body.Append(@"<span style=""color:#707070;font-style:italic;float:left;margin-right:10px;vertical-align:top"">Occupant:</span>");
                body.Append(@"</div>");
                body.Append(@"<div style=""float:left"">");
                if (!string.IsNullOrEmpty(occupant.FirstName) || !string.IsNullOrEmpty(occupant.LastName))
                {
                    body.Append(@"<span class=""occupant"">").Append(occupant.FirstName).Append(" ").Append(occupant.LastName).Append("</span>");
                    dataPresent = true;
                }
                if (!string.IsNullOrEmpty(occupant.Phone))
                {
                    if (dataPresent)
                    {
                        body.Append(@"<br />");
                    }
                    body.Append(@"<abbr title=""phone"" style=""cursor:help;border-bottom:1px dotted #999;margin-right:6px;"">P:</abbr>").Append(occupant.Phone);
                    dataPresent = true;
                }

                if (!string.IsNullOrEmpty(occupant.Email))
                {
                    if (dataPresent)
                    {
                        body.Append(@"<br />");
                    }
                    body.Append(@"<a href=""mailto:").Append(occupant.Email).Append(@""" class=""email"" style=""color:#428bca;text-decoration:none"">").Append(occupant.Email).Append("</a>");
                }
                body.Append(@"</div>");
            }
            body.Append(@"</div>");

            body.Append(@"<br style=""clear:both"" />");

            if (appraisal.Appraisal.ContactForAccess)
            {
                body.Append(@"<div style=""clear:both;margin-top:4px"">Contact owner/occupant for access?  <strong style=""margin:0 5px"">Yes</strong></div>");
            }

            if (legalSalesData)
            {
                body.Append(@"<div style=""clear:both;margin-top:4px"">");
                if (property.SalesContractPrice != null)
                {
                    body.Append(@"<div class=""price"">");
                    body.Append(string.Format("{0:C0}", property.SalesContractPrice));
                    body.Append(@"<span style=""color:#707070;font-style:italic"">- sales/contract price</span>");
                    body.Append(@"</div>");
                }

                if (!string.IsNullOrEmpty(property.LegalDescription))
                {
                    body.Append(@"<div style=""margin-top:4px;"">");
                    body.Append(@"<strong style=""margin:0 5px 0 0"">Legal description:</strong>");
                    body.Append(property.LegalDescription);
                    body.Append(@"</div>");
                }
                body.Append(@"</div>");
            }
            body.Append(@"</div>");
            body.Append(@"</div>");


            // Build the APPRAISAL REPORT section
            body.Append(@"<div style=""font-size:13px;overflow:auto;margin:0 0 8px;padding:5px 0"">");
            body.Append(@"<div style=""color:#19aacf;float:left;font-weight:700;vertical-align:top;width:150px"">Appraisal Report</div>");
            body.Append(@"<div style=""float:left"">");

            if (appraisal.Appraisal.Client2Person != null)
            {
                var orderClient = appraisal.Appraisal.Client2Person;
                var orderClientAddress = appraisal.Appraisal.Client2Address;

                body.Append(@"<div style=""color: #707070;font-style: italic;float: left;margin-right: 10px;vertical-align: top;"">");
                body.Append(@"<span style=""border-bottom: 1px dotted #999;color: #707070;font-style: italic;"">Appraisal Client:</span>");
                body.Append(@"</div>");
                body.Append(@"<div style=""margin-bottom: 10px; overflow: auto;"">");
                body.Append(@"<div>").Append(orderClient.FirstName).Append(" ").Append(orderClient.LastName).Append("</div>");
                body.Append(@"<address style=""float: left !important;font-style: normal;line-height: 1.428571429;display: block; width: 160px;"">");
                body.Append(orderClientAddress.Address1).Append("<br />");
                if (!string.IsNullOrEmpty(orderClientAddress.Address2))
                {
                    body.Append(orderClientAddress.Address2).Append("<br />");
                }
                body.Append(orderClientAddress.City).Append(", ").Append(orderClientAddress.StateCode).Append("  ").Append(orderClientAddress.PostalCode);
                body.Append(@"</address>");
                body.Append(@"<div style=""float: left;"">");
                body.Append(@"<abbr title=""phone"" style=""border-bottom: 1px dotted #999999;margin-right: 6px;"">P:</abbr>").Append(orderClient.Phone).Append("<br />");
                if (!string.IsNullOrEmpty(orderClient.Email))
                {
                    body.Append(@"<a style=""color: #2a6496;text-decoration: underline;"" href=""mailto:").Append(orderClient.Email).Append(@""" class=""email"">").Append(orderClient.Email).Append("</a>");
                }
                body.Append(@"</div>");
                body.Append(@"</div>");
            }

            if (!String.IsNullOrEmpty(appraisal.Appraisal.ReportUsers))
            {
                body.Append(@"<div>");
                body.Append(@"<strong>Intended report users:</strong>  ");
                body.Append(appraisal.Appraisal.ReportUsers);
                body.Append(@"</div>");
             }
            
            if (!String.IsNullOrEmpty(appraisal.Appraisal.DeliverReportTo))
            {
                body.Append(@"<div>");
                body.Append(@"<strong>Report should be delivered to:</strong>  ");
                body.Append(appraisal.Appraisal.DeliverReportTo);
                body.Append(@"</div>");
            }

            if (appraisalPurpose != null)
            {
                body.Append(@"<div>");
                body.Append(@"<strong>Appraisal purpose:</strong>  ");
                body.Append(appraisalPurpose.AppraisalPurposeDescription);
                body.Append(@"</div>");
            }
            if (!string.IsNullOrEmpty(appraisal.Appraisal.Comments))
            {
                body.Append(@"<div style=""margin-top:4px"">");
                body.Append(@"<strong style=""margin:0 5px 0 0"">Additional comments:</strong>  ");
                body.Append(appraisal.Appraisal.Comments);
                body.Append(@"</div>");
            }
            if (string.IsNullOrEmpty(appraisal.Appraisal.Comments) && string.IsNullOrEmpty(appraisal.Appraisal.AppraisalPurposeCode))
            {
                body.Append(@"<span>--</span>");
            }
            body.Append(@"</div>");
            body.Append(@"</div>");
            
            body.Append(@"</fieldset>");


            // Build disclaimer
            body.Append(@"<div style=""color:#959595;font-size:13px;font-style:italic;text-align:center;"">");
            if (clientConfirmation)
            {
                body.Append(BuildUserDisclaimer());
            }
            else
            {
                body.Append(BuildAppraiserDisclaimer());
            }
            body.Append(@"</div>");

            body.Append(@"</div>");

            return body.ToString();
        }

        private static string buildContactItems(AppraiseUtah.Client.Models.Appraiser appraiser)
        {
            var contact = "";

            contact = (!string.IsNullOrEmpty(appraiser.Phone)) ? @"<abbr title=""Phone"" style=""cursor:help;border-bottom:1px dotted #999;margin-right:6px;"">P:</abbr>" + appraiser.Phone : "";

            if (contact != "")
            {
                contact += (!string.IsNullOrEmpty(appraiser.Fax)) ? @"<br /><abbr title=""Fax"" style=""cursor:help;border-bottom:1px dotted #999;margin-right:6px;"">F:</abbr>" + appraiser.Fax : "";
            }
            else
            {
                contact += (!string.IsNullOrEmpty(appraiser.Fax)) ? @"<abbr title=""Fax"" style=""cursor:help;border-bottom:1px dotted #999;margin-right:6px;"">F:</abbr>" + appraiser.Fax : "";
            }

            if (contact != "")
            {
                contact += (!string.IsNullOrEmpty(appraiser.Email)) ? @"<br /><a href=""mailto:" + appraiser.Email + @""" class=""email"" style=""color:#428bca;text-decoration:none"">" + appraiser.Email + "</a>" : "";
            }
            else
            {
                contact += (!string.IsNullOrEmpty(appraiser.Email)) ? @"<a href=""mailto:" + appraiser.Email + @""" class=""email"" style=""color:#428bca;text-decoration:none"">" + appraiser.Email + "</a>" : "";
            }

            return contact;
        }

        private static bool OccupantDataPresent(AppraisalViewModel appraisal)
        {
            // Check to see if Occupant data is present
            bool occupantData = false;
            var occupant = appraisal.Appraisal.OccupantPerson;
            if (occupant != null)
            {
                if (!string.IsNullOrEmpty(occupant.FirstName) ||
                    !string.IsNullOrEmpty(occupant.LastName) ||
                    !string.IsNullOrEmpty(occupant.Phone) ||
                    !string.IsNullOrEmpty(occupant.Email))
                {
                    occupantData = true;
                }
            }

            return occupantData;
        }

        private static string BuildUserDisclaimer()
        {
            var msg = new StringBuilder("This is the only communication you will recieve from AppraiseUtah.com.  The Appraiser should contact you soon to arrange all appraisal details, including payment.  AppraiseUtah.com is not responsible for the appraisal in any way.");

            return msg.ToString();
        }

        private static string BuildAppraiserDisclaimer()
        {
            var msg = new StringBuilder("This is the only communication you will receive from AppraiseUtah.com.  As the appraiser, you should reach out to the client to arrange all appraisal details, including payment.  AppraiseUtah.com is not responsible for the appraisal in any way.");

            return msg.ToString();
        }

        #endregion

    }
}
