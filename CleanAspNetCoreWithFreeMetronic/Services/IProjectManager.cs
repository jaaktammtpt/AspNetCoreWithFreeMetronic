using CleanAspNetCoreWithFreeMetronic.Data;
using CleanAspNetCoreWithFreeMetronic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleanAspNetCoreWithFreeMetronic.Services
{
    public interface IProjectManager : IRepository<ProjectDTO>
    {
    }
}
