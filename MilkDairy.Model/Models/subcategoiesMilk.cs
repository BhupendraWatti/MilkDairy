using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkDairy.Model.Models
{
    public class subcategoiesMilk
    {
        [Key]
        public int MilkId { get; set; }  // Unique ID  
        [Required]
        public string? MilkType { get; set; } // Cow, Buffalo, Goat, etc.  
        [Required]
        public string? MilkName { get; set; } // Full Cream, Toned, Skimmed, etc.  

        // Additional Properties  
        [Required]
        public string? FatContent { get; set; } // Example: "4.5%", "3.5
        [Required]
        public string? ProteinContent { get; set; } // Example: "3.2g per 100ml"  
        [Required]
        public bool IsOrganic { get; set; } // True/False  
       
        [Required]
        public string? PackagingType { get; set; } // Bottle, Pouch, Tetra pack 

    }
}
