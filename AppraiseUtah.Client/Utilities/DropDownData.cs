using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using AppraiseUtah.Client.Models;
using AppraiseUtah.Client.ServiceModels;

namespace AppraiseUtah.Client.Utilities
{
    public static class DropDownData
    {

        #region Methods
        
        /// <summary>
        /// Gets the list of states for a dropdown
        /// </summary>
        /// <returns></returns>
        public static List<SelectListItem> GetStates(string selectedStateCode = "")
        {
            // Get the states
            StateServiceModel stateServiceModel = new StateServiceModel();
            var states = stateServiceModel.Get_SelectList_States(selectedStateCode);
            return states;
        }

        /// <summary>
        /// Gets the list of PropertyTypes for a dropdown
        /// </summary>
        /// <returns></returns>
        public static List<SelectListItem> GetPropertyTypes(string selectedPropertyType = "")
        {
            // Get the property types
            PropertyTypeServiceModel propertyTypeServiceModel = new PropertyTypeServiceModel();
            return propertyTypeServiceModel.Get_SelectList_PropertyType();
        }

        /// <summary>
        /// Gets a list of PropertyTypes from the service model
        /// </summary>
        /// <returns></returns>
        public static List<PropertyType> GetPropertyTypeList()
        {
            // Get the property types
            PropertyTypeServiceModel propertyTypeServiceModel = new PropertyTypeServiceModel();
            return propertyTypeServiceModel.Get_PropertyTypes();
        }



        /// <summary>
        /// Gets the list of AppraisalPurposes for a dropdown
        /// </summary>
        /// <returns></returns>
        public static List<SelectListItem> GetAppraisalPurposes(string selectedAppraisalPurpose = "")
        {
            // Get the appraisal purposes
            AppraisalPurposeServiceModel appraisalPurposeServiceModel = new AppraisalPurposeServiceModel();
            return appraisalPurposeServiceModel.Get_SelectList_AppraisalPurpose(selectedAppraisalPurpose);
        }

        /// <summary>
        /// Gets the list of AppraisalPurposes
        /// </summary>
        /// <returns></returns>
        public static List<AppraisalPurpose> GetAppraisalPurposeList()
        {
            // Get the appraisal purposes
            AppraisalPurposeServiceModel appraisalPurposeServiceModel = new AppraisalPurposeServiceModel();
            return appraisalPurposeServiceModel.Get_AppraisalPurposes();
        }

        #endregion

    }
}
