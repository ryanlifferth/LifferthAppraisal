using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AppraiseUtah.Client.Models
{
    public class Appraiser
    {

        #region Properties

        public int AppraiserId { get; set; }

        public string CompanyName { get; set; }

        public Address Address { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string Fax { get; set; }

        public string AreasServed { get; set; }

        public bool StateCertified { get; set; }

        public string ProfessionalDesignations { get; set; }

        public bool Residential { get; set; }

        public bool Commercial { get; set; }

        #endregion

    }
}
