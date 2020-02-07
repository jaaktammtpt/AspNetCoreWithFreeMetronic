using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CleanAspNetCoreWithFreeMetronic.Data
{
    public class Team
    {
        public int Id { get; set; }
        [ForeignKey("IdentityUser")]
        public string MemberId { get; set; }
        public virtual IdentityUser Member { get; set; }
        public int ProjectId { get; set; }
        public Project Project { get; set; }

    }
}
