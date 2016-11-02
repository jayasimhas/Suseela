using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Informa.Library.ComponentModel.DataAnnotations;

namespace Informa.Web.Areas.Account.Models.User.Management
{
    public class ContactInformationUpdateRequest
    {
        public string Id { get; set; }
        [Required(ErrorMessage = ManagementValidationReasons.Required)]
        public string FirstName { get; set; }
        [Required(ErrorMessage = ManagementValidationReasons.Required)]
        public string LastName { get; set; }
        public string MiddleInitial { get; set; }
        public string NameSuffix { get; set; }
        public string Salutation { get; set; }
        [Required(ErrorMessage = ManagementValidationReasons.Required)]
        public string BillCountry { get; set; }
        [Required(ErrorMessage = ManagementValidationReasons.Required)]
        public string BillAddress1 { get; set; }
        public string BillAddress2 { get; set; }
        [Required(ErrorMessage = ManagementValidationReasons.Required)]
        public string BillCity { get; set; }
        public string BillPostalCode { get; set; }
        public string BillState { get; set; }
        [Required(ErrorMessage = ManagementValidationReasons.Required)]
        public string ShipCountry { get; set; }
        [Required(ErrorMessage = ManagementValidationReasons.Required)]
        public string ShipAddress1 { get; set; }
        public string ShipAddress2 { get; set; }
        [Required(ErrorMessage = ManagementValidationReasons.Required)]
        public string ShipCity { get; set; }
        public string ShipPostalCode { get; set; }
        public string ShipState { get; set; }
        public string CountryCode { get; set; }
        public string Fax { get; set; }
        public string PhoneExtension { get; set; }
        [Required(ErrorMessage = ManagementValidationReasons.Required)]
        public string Phone { get; set; }
        [Required(ErrorMessage = ManagementValidationReasons.Required)]
        public string PhoneType { get; set; }
        [Required(ErrorMessage = ManagementValidationReasons.Required)]
        public string Company { get; set; }
        public string JobFunction { get; set; }
        [Required(ErrorMessage = ManagementValidationReasons.Required)]
        public string JobIndustry { get; set; }
        [Required(ErrorMessage = ManagementValidationReasons.Required)]
        public string JobTitle { get; set; }
    }
}