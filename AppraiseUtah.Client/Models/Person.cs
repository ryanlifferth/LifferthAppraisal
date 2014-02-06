using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace AppraiseUtah.Client.Models
{
    public class Person
    {

        #region Properties
        public int PersonId { get; set; }

        public string PersonType { get; set; }

        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [DisplayName("Last Name")]
        public string LastName { get; set; }

        [DisplayName("Company Name")]
        public string CompanyName { get; set; }

        [DisplayName("Email")]
        public string Email { get; set; }

        [DisplayName("Phone Number")]
        [DisplayFormat(DataFormatString = "{0:(###) ###-####}")]
        public string Phone { get; set; }

        #endregion

    }
}
