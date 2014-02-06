using System;
using System.Collections.Generic;
using System.Data.Entity;
using AppraiseUtah.Client.Common.Constants;
using AppraiseUtah.Client.ViewModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace AppraiseUtah.Client.Models
{

    public class Appraisal
    {

        #region Fields

        
        #endregion

        #region Properties

        public int Id { get; set; }

        public string StatusCode { get; set; }

        [DisplayName("Appraiser")]
        [Required(ErrorMessage = "Please select an appraiser.")]
        //[StringLength(160)]
        public int AppraiserId { get; set; }

        public Person ClientPerson { get; set; }

        public Address ClientAddress { get; set; }

        public Person Client2Person { get; set; }

        public Address Client2Address { get; set; }

        public Person OccupantPerson { get; set; }

        public Address PropertyAddress { get; set; }

        [DisplayName("Sales Contract Price")]
        [DisplayFormat(DataFormatString = "{0:c}")]
        public decimal? SalesContractPrice { get; set; }

        [DisplayName("Property Type")]
        [Required(ErrorMessage = "Please select a Property Type")]
        public string PropertyTypeCode { get; set; }

        [DisplayName("Appraisal Type")]
        public string AppraisalTypeCode { get; set; }

        [DisplayName("Appraisal Purpose")]
        public string AppraisalPurposeCode { get; set; }

        [DisplayName("Intended report users?")]
        public string ReportUsers { get; set; }

        [DisplayName("Deliver report copies to")]
        public string DeliverReportTo { get; set; }

        [DisplayName("Contact owner/occupant for access?")]
        public bool ContactForAccess { get; set; }

        [DisplayName("Legal Description")]
        public string LegalDescription { get; set; }

        public string Comments { get; set; }

        #endregion

        #region Methods

        

        #endregion

    }

}
