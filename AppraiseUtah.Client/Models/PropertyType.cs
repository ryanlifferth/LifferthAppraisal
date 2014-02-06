using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace AppraiseUtah.Client.Models
{
    public class PropertyType
    {

        #region Properites

        [Key]
        [DisplayName("Property Type Code")]
        [StringLength(2)]
        public string PropertyTypeCode { get; set; }

        [DisplayName("Property Type")]
        [StringLength(50)]
        public string PropertyTypeDescription { get; set; }

        public int DisplayOrder { get; set; }

        #endregion

    }
}
