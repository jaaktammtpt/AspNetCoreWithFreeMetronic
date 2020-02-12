using CleanAspNetCoreWithFreeMetronic.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanAspNetCore.UnitTests.ServiceTests
{
    public abstract class ServiceTestBase : TestBase
    {
        protected ApplicationDbContext DbContext { get; private set; }

        protected ServiceTestBase()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                                  .UseInMemoryDatabase(Guid.NewGuid().ToString())
                                  .Options;

            DbContext = new ApplicationDbContext(options);
        }
    }
}
