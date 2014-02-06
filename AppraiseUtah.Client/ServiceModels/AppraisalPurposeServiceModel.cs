using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using AppraiseUtah.Client.Models;
using AppraiseUtah.Client.Utilities;

namespace AppraiseUtah.Client.ServiceModels
{
    public class AppraisalPurposeServiceModel
    {

        #region Fields

        AppraisalContext _db = new AppraisalContext("AppraisalDBContext");

        #endregion

        #region Properties
        #endregion

        #region Constructor

        #endregion

        #region Methods

        public virtual List<AppraisalPurpose> Get_AppraisalPurposes()
        {
            var states = _db.GetAppraisalPurposes();
            return states;
        }

        /// <summary>
        /// Creates a list of SelectListItem to be used for dropdown controls
        /// </summary>
        /// <param name="selectedStateCode"></param>
        /// <returns></returns>
        public virtual List<SelectListItem> Get_SelectList_AppraisalPurpose(string selectedItem = "")
        {
            return SelectListUtility.CreateSelectItemList<AppraisalPurpose>(Get_AppraisalPurposes(), "AppraisalPurposeCode", "AppraisalPurposeDescription", selectedItem);
        }

        #endregion

    }
}
