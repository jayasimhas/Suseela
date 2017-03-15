using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Informa.Library.ComponentModel.DataAnnotations;

namespace Informa.Web.Areas.Account.Models.User.Management
{
    public class ContactInformationUpdateRequestV2
    {
        [Required(ErrorMessage = ManagementValidationReasons.Required)]
        public string FirstName { get; set; }
        [Required(ErrorMessage = ManagementValidationReasons.Required)]
        public string LastName { get; set; }
        public string MiddleInitial { get; set; }
        public string Salutation { get; set; }

        public string ShipCountry { get; set; }

        public string ShipAddress1 { get; set; }

        public string ShipCity { get; set; }
        public string ShipPostalCode { get; set; }
        public string ShipState { get; set; }
        public string Phone { get; set; }
        public string Mobile { get; set; }
        [Required(ErrorMessage = ManagementValidationReasons.Required)]
        public string Company { get; set; }
        public string JobFunction { get; set; }

        public string JobIndustry { get; set; }

        public string JobTitle { get; set; }
    }
}