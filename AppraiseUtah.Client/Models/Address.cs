using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace AppraiseUtah.Client.Models
{
    public class Address
    {

        #region Properties
        public int AddressId { get; set; }

        public string AddressType { get; set; }

        [DisplayName("Address Line 1")]
        public string Address1 { get; set; }

        [DisplayName("Address Line 2")]
        public string Address2 { get; set; }

        [DisplayName("City")]
        public string City { get; set; }

        [DisplayName("State")]
        public string StateCode { get; set; }

        [DisplayName("Zip Code")]
        public string PostalCode { get; set; }

        #endregion

    }
}
