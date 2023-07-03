using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using DataAnnotationsExtensions;

namespace AccentureWatch.Models
{
    public class Watch
    {
        [Key]
        public int ID { get; set; }

        [Display(Name = "Item Name")]
        [Required(ErrorMessage = "Required")]
        [StringLength(50)]
        public string? ItemName { get; set; }

        [Display(Name = "Item Number")]
        [Required(ErrorMessage = "Required")]
        [StringLength(50)]
        public string? ItemNumber { get; set; }

        [Display(Name = "Short Description")]
        [Required(ErrorMessage = "Required")]
        [StringLength(50)]
        public string? ShortDescription { get; set; }

        [Display(Name = "Full Description")]
        [Required(ErrorMessage = "Required")]
        [StringLength(100)]
        public string? FullDescription { get; set; }

        [RegularExpression(@"-?\d+(?:\.\d+)?", ErrorMessage = "Please enter valid price (e.g. 20.00)")]
        [Required(ErrorMessage = "Required")]
        public decimal? Price { get; set; }

        [Required(ErrorMessage = "Required")]
        [StringLength(50)]
        public string? Caliber { get; set; }

        [Required(ErrorMessage = "Required")]
        [StringLength(50)]
        public string? Movement { get; set; }

        [Required(ErrorMessage = "Required")]
        [StringLength(50)]
        public string? Chronograph { get; set; }

        [Display(Name = "Weight (mm)")]
        [RegularExpression(@"-?\d+(?:\.\d+)?", ErrorMessage = "Please enter valid weight (e.g. 20.00)")]
        [Required(ErrorMessage = "Required")]
        public decimal? Weight { get; set; }

        [Display(Name = "Height (mm)")]
        [RegularExpression(@"-?\d+(?:\.\d+)?", ErrorMessage = "Please enter valid height (e.g. 20.00)")]
        [Required(ErrorMessage = "Required")]
        public decimal? Height { get; set; }

        [Display(Name = "Diameter (mm)")]
        [RegularExpression(@"-?\d+(?:\.\d+)?", ErrorMessage = "Please enter valid diameter (e.g. 20.00)")]
        [Required(ErrorMessage = "Required")]
        public decimal? Diameter { get; set; }

        [Display(Name = "Thickness (mm)")]
        [RegularExpression(@"-?\d+(?:\.\d+)?", ErrorMessage = "Please enter valid thickness (e.g. 20.00)")]
        [Required(ErrorMessage = "Required")]
        public decimal? Thickness { get; set; }

        [Integer(ErrorMessage = "Please enter numbers only")]
        [Required(ErrorMessage = "Required")]
        public int? Jewel { get; set; }

        [Display(Name = "Case Material")]
        [Required(ErrorMessage = "Required")]
        [StringLength(30)]
        public string? CaseMaterial { get; set; }

        [Display(Name = "Strap Material")]
        [Required(ErrorMessage = "Required")]
        [StringLength(30)]
        public string? StrapMaterial { get; set; }

        //[NotMapped]
        [Display(Name = "File")]
        public IFormFile? FormFile { get; set; }

        public string? URL { get; set; }
    }
}
