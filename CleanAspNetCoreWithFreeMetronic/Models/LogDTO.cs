using CleanAspNetCoreWithFreeMetronic.Data;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CleanAspNetCoreWithFreeMetronic.Models
{
    public class LogDTO
    {        
        public int Id { get; set; }
        public DateTime LogDate { get; set; }
        public decimal TimeSpent { get; set; }
        public string LogDescription { get; set; }
        public int JobTaskId { get; set; }
        public virtual JobTask JobTask { get; set; }
        public string DoneById { get; set; }
        public virtual IdentityUser DoneBy { get; set; }

        
        public LogDTO()
        {

        }

    }
}
