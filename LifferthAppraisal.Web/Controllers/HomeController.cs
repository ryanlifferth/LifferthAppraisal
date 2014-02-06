using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;

namespace LifferthAppraisal.Controllers
{
    public class HomeController : Controller
    {

        //
        // GET: /Home/

        public ActionResult Index()
        {

            return View();
        }

        //
        // GET: /Home/ServicesAndFees/

        public ActionResult ServicesAndFees()
        {

            return View();
        }

        //
        // GET: /Home/ServiceArea/

        public ActionResult ServiceArea()
        {

            return View();
        }

        //
        // GET: /Home/AboutUs/

        public ActionResult AboutUs()
        {
            XElement staffXml = XElement.Load(Server.MapPath("~/App_Data/Staff.xml"));
            var staffData = from data in staffXml.Elements("Appraiser")
                            select data;
            ViewData["staffData"] = staffData;

            return View();
        }

        //
        // GET: /Home/Staff/ID/

        public ActionResult Staff(String id)
        {
            XElement staffXml = XElement.Load(Server.MapPath("~/App_Data/Staff.xml"));
            var staffData = (from data in staffXml.Elements("Appraiser")
                             where data.Attribute("id").Value == id
                             select new
                             {
                                 ID = data.Attribute("id").Value,
                                 Name = data.Element("Name").Value,
                                 Certifications = data.Element("Certifications").Value,
                                 ImgId = data.Element("ImgId").Value,
                                 Description = data.Element("Description").Value
                             }).SingleOrDefault();

            // If no match was made then redirect to the "AboutUs" view
            if (staffData == null)
            {
                return View("AboutUs");
            }

            ViewData["ID"] = staffData.ID;
            ViewData["Name"] = staffData.Name;
            ViewData["Certifications"] = staffData.Certifications;
            ViewData["ImgId"] = staffData.ImgId;
            ViewData["Description"] = staffData.Description;

            return View();
        }

    }
}
