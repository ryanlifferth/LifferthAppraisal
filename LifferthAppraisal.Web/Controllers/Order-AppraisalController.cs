using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AppraiseUtah.Client.ServiceModels;
using AppraiseUtah.Client.Utilities;
using AppraiseUtah.Client.ViewModels;

namespace LifferthAppraisal.Controllers
{
    public class Order_AppraisalController : Controller
    {

        #region Fields

        AppraisalServiceModel _appraisalServiceModel = new AppraisalServiceModel();

        #endregion


        #region Index

        //
        // GET: /Order-Appraisal/

        [HttpGet]
        public ActionResult Index(int id = 0)
        {
            AppraisalViewModel appraisalViewModel = new AppraisalViewModel();

            if (id != 0)
            {
                appraisalViewModel.Appraisal = _appraisalServiceModel.Get_Appraisal(id);
                //var appraisers = _appraisalServiceModel.Get_Appraisers();
                //var appraiser = _appraisalServiceModel.Get_Appraiser(3);

                //TODO:  Format phone numbers
            }

            return View(appraisalViewModel);
        }

        [HttpPost]
        public ActionResult Index(AppraisalViewModel appraisalViewModel)
        {
            appraisalViewModel.Appraisal.ClientPerson.Phone = AppraiseUtah.Client.Utilities.ScrubData.RemoveNonNumeric(appraisalViewModel.Appraisal.ClientPerson.Phone);
            appraisalViewModel.Appraisal.OccupantPerson.Phone = AppraiseUtah.Client.Utilities.ScrubData.RemoveNonNumeric(appraisalViewModel.Appraisal.OccupantPerson.Phone);

            // Set appraiser id to the proper value from web.config
            appraisalViewModel.Appraisal.AppraiserId = int.Parse(ConfigurationManager.AppSettings["AppraiserId"].ToString());

            var appraisalId = _appraisalServiceModel.Save_Appraisal(appraisalViewModel);
            appraisalViewModel.Appraisal.Id = appraisalId;

            // Send the confirmation email

            MailUtility.SendConfirmationEmail(appraisalViewModel);

            return RedirectToAction("Confirmation", new { id = appraisalId });
        }

        #endregion Index

        #region Confirmation

        //
        // GET: /Order-Appraisal/Confirmation

        [HttpGet]
        public ActionResult Confirmation(int id = 0)
        {
            AppraisalViewModel appraisalViewModel = new AppraisalViewModel();

            if (id != 0)
            {
                appraisalViewModel.Appraisal = _appraisalServiceModel.Get_Appraisal(id);
                //TODO:  Format phone numbers
            }

            return View(appraisalViewModel);
        }

        #endregion Confirmation


        #region Test

        //
        // GET: /Order-Appraisal/Test

        [HttpGet]
        public ActionResult Test(int id = 0)
        {
            AppraisalViewModel appraisalViewModel = new AppraisalViewModel();

            if (id != 0)
            {
                appraisalViewModel.Appraisal = _appraisalServiceModel.Get_Appraisal(id);
                //TODO:  Format phone numbers
            }

            return View(appraisalViewModel);
        }

        #endregion Test

    }
}
