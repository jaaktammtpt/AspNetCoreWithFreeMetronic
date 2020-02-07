using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace CleanAspNetCore.IntegrationTests
{
    [Collection("Sequential")]
    public class ProjectsControllerTests : TestBase
    {
        public ProjectsControllerTests(TestApplicationFactory<FakeStartup> factory) : base(factory)
        {
        }

        [Theory]
        [InlineData("/Projects")]
        [InlineData("/Projects/Details")]
        [InlineData("/Projects/Details/1")]
        [InlineData("/Projects/Edit")]
        [InlineData("/Projects/Edit/1")]
        public async Task Get_EndpointsReturnFailToAnonymousUserForRestrictedUrls(string url)
        {
            //Arrange
           var client = Factory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });

            //Act
           var response = await client.GetAsync(url);

            //Assert
            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
        }

        [Theory]
        [InlineData("/Projects")]
        [InlineData("/Projects/Details/1")]
        public async Task Get_EndPointsReturnsSuccessForRegularUser(string url)
        {
            var provider = TestClaimsProvider.WithUserClaims();
            var client = Factory.CreateClientWithTestAuth(provider);

            var response = await client.GetAsync(url);

            response.EnsureSuccessStatusCode();
            Assert.Equal("text/html; charset=utf-8", response.Content.Headers.ContentType.ToString());
        }

        [Theory]
        [InlineData("/Projects/Edit/")]
        [InlineData("/Projects/Edit/1")]
        public async Task Get_EditReturnsFailToRegularUser(string url)
        {
            var provider = TestClaimsProvider.WithUserClaims();
            var client = Factory.CreateClientWithTestAuth(provider);

            var response = await client.GetAsync(url);

            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Theory]
        [InlineData("/Projects")]
        [InlineData("/Projects/Details/1")]
        [InlineData("/Projects/Edit/1")]
        public async Task Get_EndPointsReturnsSuccessForAdmin(string url)
        {
            var provider = TestClaimsProvider.WithAdminClaims();
            var client = Factory.CreateClientWithTestAuth(provider);

            var response = await client.GetAsync(url);

            response.EnsureSuccessStatusCode();
            Assert.Equal("text/html; charset=utf-8", response.Content.Headers.ContentType.ToString());
        }

    }
}
