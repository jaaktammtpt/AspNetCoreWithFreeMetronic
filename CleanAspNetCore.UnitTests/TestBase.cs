using AutoMapper;
using CleanAspNetCoreWithFreeMetronic.Data;
using CleanAspNetCoreWithFreeMetronic.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Moq;
using System;
using Xunit;

namespace CleanAspNetCore.UnitTests
{
    public abstract class TestBase
    {
        protected IMapper Mapper { get; private set; }

        public TestBase()
        {
            //https://stackoverflow.com/questions/49708895/how-to-write-xunit-test-for-net-core-2-0-service-that-uses-automapper-and-depen?rq=1
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMapping());
            });

            Mapper = config.CreateMapper();
        }

        public ILogger<T> GetLogget<T>()
        {
            return NullLogger<T>.Instance;
        }
    }
}
