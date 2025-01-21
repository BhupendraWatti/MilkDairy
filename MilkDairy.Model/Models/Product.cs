using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkDairy.Model.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(40)]
        public string? Name { get; set; }
        [Required]
        [MaxLength(1800)]
        public string? Description { get; set; }
        [Required]
        [MaxLength (1200)]
        public string? Brand { get; set; }
        [Required]
        public string? WeightVolume { get; set; }
        [Required]
        [MaxLength(2000)]
        public string? Ingredients { get; set; }
        [Required]
        public float UnitPrice { get; set; }
        [Required]
        public int UnitsInStock { get; set; }
        [Required]
        public bool IsAvailable { get; set; }
        [ValidateNever]
        [Required]
        public string? ImgURL { get; set; }
        
    }
}
