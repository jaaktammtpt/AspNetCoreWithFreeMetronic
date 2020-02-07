using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace CleanAspNetCoreWithFreeMetronic.Data
{
    public class JobTask
    {
        [Key]
        public int Id { get; set; }

        [StringLength(50, MinimumLength = 8)]
        public string TaskName { get; set; }

        [Required]
        [Display(Name = "Task Start")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime TaskStart { get; set; }

        [Required]
        [Display(Name = "Estimate Hours")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal EstimateHours { get; set; }

        [Required]
        [Display(Name = "Task Description")]
        [StringLength(50, MinimumLength = 8)]
        public string TaskDescription { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]

        [Display(Name = "Completion")]
        public bool Completed { get; set; } = false;

        [Required]
        public int ProjectId { get; set; }
        public Project Project { get; set; }        

        public string ResponsibleId { get; set; }
        public IdentityUser Responsible { get; set; }
        public IList<Log> Logs { get; set; }
        public IList<TaskFile> TaskFiles { get; set; }


        public JobTask()
        {
            Logs = new List<Log>();
            TaskFiles = new List<TaskFile>();
        }
        
        public decimal TimeSpent()
        {
            return Logs.Sum(i => i.TimeSpent);
        }

    }
}