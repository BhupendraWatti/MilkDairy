using System.ComponentModel.DataAnnotations;

namespace MilkDairy.Model.Models
{
    public class Details
    {
        [Key]
        public int ID { get; set; }
        [Required]
        [MaxLength(10)]
        public string? Items { get; set; }
        [Required]
        public DateTime MFD { get; set; }
        [Required]
        public int Price { get; set; }
        [Required]
        public DateTime EXD{ get; set; }
        [Required]
        public int Quantity { get; set; } 

    }
}
