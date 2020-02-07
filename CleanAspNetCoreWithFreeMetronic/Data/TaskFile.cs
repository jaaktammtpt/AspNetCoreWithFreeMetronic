using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CleanAspNetCoreWithFreeMetronic.Data
{
    public class TaskFile
    {
        [Key]
        public int Id { get; set; }
        public string TaskFileName { get; set; }

        [Required]
        [Display(Name = "Job Task")]
        public int JobTaskId { get; set; }
        public virtual JobTask JobTask { get; set; }

        public TaskFile()
        {

        }
    }
}
