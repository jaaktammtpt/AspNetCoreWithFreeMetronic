using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CleanAspNetCoreWithFreeMetronic.Data
{
    public class Log
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Log Date")]
        public DateTime LogDate { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Time Spent")]
        public decimal TimeSpent { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 6)]
        [Display(Name = "Log Description")]
        public string LogDescription { get; set; }

        [Required]
        [Display(Name = "Job Task")]
        public int JobTaskId { get; set; }
        public virtual JobTask JobTask { get; set; }

        [Required]
        public string DoneById { get; set; }
        [Display(Name = "Done By")]
        public virtual IdentityUser DoneBy { get; set; }

        public Log()
        {

        }
    }
}