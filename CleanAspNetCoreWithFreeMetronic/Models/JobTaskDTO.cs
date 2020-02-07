using AutoMapper;
using CleanAspNetCoreWithFreeMetronic.Data;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleanAspNetCoreWithFreeMetronic.Models
{
    public class JobTaskDTO
    {
        public int Id { get; set; }
        public string TaskName { get; set; }
        public DateTime TaskStart { get; set; }
        public decimal EstimateHours { get; set; }
        public string TaskDescription { get; set; }
        public bool Completed { get; set; } = false;
        public int ProjectId { get; set; }
        public Project Project { get; set; }
        public string ResponsibleId { get; set; }
        public IdentityUser Responsible { get; set; }
        public IList<Log> Logs { get; set; }
        public IList<TaskFile> TaskFiles { get; set; }

        public decimal TimeSpent { get; set; }
        public JobTaskDTO()
        {
            Logs = new List<Log>();
            TaskFiles = new List<TaskFile>();
        }


    }
}
