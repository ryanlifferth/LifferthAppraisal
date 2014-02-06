using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using AppraiseUtah.Client.Models;
using AppraiseUtah.Client.Utilities;

namespace AppraiseUtah.Client.ServiceModels
{
    public class PropertyTypeServiceModel
    {

        #region Fields

        AppraisalContext _db = new AppraisalContext("AppraisalDBContext");

        #endregion

        #region Properties
        #endregion

        #region Constructor

        #endregion

        #region Methods

        public virtual List<PropertyType> Get_PropertyTypes()
        {
            var propertyTypes = _db.GetPropertyTypes();
            return propertyTypes;
        }

        /// <summary>
        /// Creates a list of SelectListItem to be used for dropdown controls
        /// </summary>
        /// <param name="selectedStateCode"></param>
        /// <returns></returns>
        public virtual List<SelectListItem> Get_SelectList_PropertyType(string selectedItem = "")
        {

            return SelectListUtility.CreateSelectItemList<PropertyType>(Get_PropertyTypes(), "PropertyTypeCode", "PropertyTypeDescription", selectedItem);
            
        }


        #endregion

    }
}
