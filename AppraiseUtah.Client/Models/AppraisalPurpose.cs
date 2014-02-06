using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace AppraiseUtah.Client.Models
{
    public class AppraisalPurpose
    {

        #region Properites

        [Key]
        [DisplayName("Appraisal Purpose Code")]
        [StringLength(2)]
        public string AppraisalPurposeCode { get; set; }

        [DisplayName("Appraisal Purpose")]
        [StringLength(50)]
        public string AppraisalPurposeDescription { get; set; }
        
        public int DisplayOrder { get; set; }

        #endregion

    }
}
