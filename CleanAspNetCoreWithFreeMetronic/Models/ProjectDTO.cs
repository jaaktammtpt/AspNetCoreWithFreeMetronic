using CleanAspNetCoreWithFreeMetronic.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleanAspNetCoreWithFreeMetronic.Models
{
    public class ProjectDTO
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Begin { get; set; }
        public DateTime Finish { get; set; }
        public decimal Budget { get; set; }
        public decimal Rate { get; set; }
        public IList<Team> Teams { get; set; }
        public IList<JobTask> JobTasks { get; set; }

        public ProjectDTO()
        {
            Teams = new List<Team>();
            JobTasks = new List<JobTask>();
        }
    }
}
