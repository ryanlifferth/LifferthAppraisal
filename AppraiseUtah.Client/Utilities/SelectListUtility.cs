using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace AppraiseUtah.Client.Utilities
{
    public static class SelectListUtility
    {

        #region Methods

        /// <summary>
        /// Uses inflection to get the values from the list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="textPropertyName"></param>
        /// <param name="valuePropertyName"></param>
        /// <param name="selectedItem"></param>
        /// <returns></returns>
        public static List<SelectListItem> CreateSelectItemList<T>(List<T> list, string valuePropertyName, string textPropertyName, string selectedItem = "")
        {
            var selectList = new List<SelectListItem>();
            var selectListItem = new SelectListItem();

            var type = typeof(T);
            var textInfo = type.GetProperty(textPropertyName);
            var valInfo = type.GetProperty(valuePropertyName);

            if (textInfo == null || valInfo == null)
            {
                return selectList;  // empty list
            }

            foreach (var item in list)
            {
                string text = textInfo.GetValue(item, null) != null ? textInfo.GetValue(item, null).ToString() : string.Empty;
                string val = (valInfo.GetValue(item, null) != null) ? valInfo.GetValue(item, null).ToString() : string.Empty;

                if (text != "" && val != "")
                {
                    selectListItem = new SelectListItem() { Text = text, Value = val };

                    if (selectedItem != "" && selectedItem == selectListItem.Value)    // Add the selected value
                    {
                        selectListItem.Selected = true;
                    }

                    selectList.Add(selectListItem);
                }
            }

            return selectList;
        }

        #endregion

    }
}
