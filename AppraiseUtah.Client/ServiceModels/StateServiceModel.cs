using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using AppraiseUtah.Client.Models;
using AppraiseUtah.Client.Utilities;

namespace AppraiseUtah.Client.ServiceModels
{
    public class StateServiceModel
    {

        #region Fields

        AppraisalContext _db = new AppraisalContext("AppraisalDBContext");

        #endregion

        #region Properties
        #endregion

        #region Constructor

        #endregion

        #region Methods

        public virtual List<State> Get_States()
        {
            var states = _db.GetStates();
            return states;
        }

        /// <summary>
        /// Creates a list of SelectListItem to be used for dropdown controls
        /// </summary>
        /// <param name="selectedStateCode"></param>
        /// <returns></returns>
        public virtual List<SelectListItem> Get_SelectList_States(string selectedStateCode = "")
        {
            //var states = Get_States();
            //var stateList = new List<SelectListItem>();
            //var stateItem = new SelectListItem();

            //foreach (var state in states)
            //{
            //    stateItem = new SelectListItem() { Text = state.StateName, Value = state.StateCode };   // create the new state list item
            //    if (selectedStateCode != "" && selectedStateCode == state.StateCode)    // Add the selected value
            //    {
            //        stateItem.Selected = true;
            //    }

            //    stateList.Add(stateItem);
            //}

            //return stateList;

            return SelectListUtility.CreateSelectItemList<State>(Get_States(), "StateCode", "StateName", selectedStateCode);
        }

        #endregion

    }
}
