using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using MilkDairy.Model.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkDairy.Model
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? Address { get; set; }
        [Required]
        public string? City { get; set; }
        [Required]
        public string? State { get; set; }
        [Required]
        public string? Pincode { get; set; }
        public int? CompanyID { get; set; }
        [ForeignKey(nameof(CompanyID))]
        [ValidateNever]
        public Company  Company{ get; set; }
    }

}
