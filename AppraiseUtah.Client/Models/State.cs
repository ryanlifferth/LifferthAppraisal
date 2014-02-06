using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace AppraiseUtah.Client.Models
{
    public class State
    {

        #region Properties

        [DisplayName("State Code")]
        [Key]
        [StringLength(2)]
        public string StateCode { get; set; }

        [DisplayName("State")]
        [StringLength(50)]
        public string StateName { get; set; }

        #endregion

    }
}
