using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AppraiseUtah.Client.Models
{
    public class AppraisalOrderResult 
    {

        #region Properties

        public int AppraisalOrderId { get; set; }

        public int ClientPersonId { get; set; }
        
        public int ClientAddressId { get; set; }
        
        public int OccupantPersonId { get; set; }
        
        public int PropertyAddressId { get; set; }

        #endregion


    }
}
