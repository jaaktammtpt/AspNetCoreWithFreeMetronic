using CleanAspNetCoreWithFreeMetronic.Controllers;
using CleanAspNetCoreWithFreeMetronic.Data;
using CleanAspNetCoreWithFreeMetronic.Models;
using CleanAspNetCoreWithFreeMetronic.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CleanAspNetCore.UnitTests.ControllerTests
{
    public class ProjectsControllerTests
    {

        [Fact]
        public async Task Index_should_return_index_or_default_view()
        {
            var projectServiceMock = new Mock<IProjectManager>();
            var logger = new Mock<ILogger<ProjectsController>>();

            projectServiceMock.Setup(c => c.GetAllAsync())
                               .ReturnsAsync(() => new List<ProjectDTO>());

            var controller = new ProjectsController(logger.Object, projectServiceMock.Object);

            var result = await controller.Index() as ViewResult;
            var viewName = result.ViewName;

            Assert.True(string.IsNullOrEmpty(viewName) || viewName == "Index");
        }

        [Fact]
        public async Task Details_should_return_bad_request_for_missing_id()
        {
            var projectServiceMock = new Mock<IProjectManager>();
            var logger = new Mock<ILogger<ProjectsController>>();
            var controller = new ProjectsController(logger.Object, projectServiceMock.Object);
            var id = (int?)null;

            var result = await controller.Details(id);

            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task Details_should_return_not_found_for_missing_project()
        {
            var projectServiceMock = new Mock<IProjectManager>();
            var logger = new Mock<ILogger<ProjectsController>>();

            projectServiceMock.Setup(c => c.GetByIdAsync(It.IsAny<int>()))
                               .ReturnsAsync(() => null);

            var controller = new ProjectsController(logger.Object, projectServiceMock.Object);

            var id = -1;

            var result = await controller.Details(id);

            Assert.IsType<NotFoundResult>(result);

        }

        [Fact]
        public async Task Details_should_return_detailsview_for_existing_project()
        {
            var model = new ProjectDTO();
            var projectServiceMock = new Mock<IProjectManager>();
            var logger = new Mock<ILogger<ProjectsController>>();

            projectServiceMock.Setup(c => c.GetByIdAsync(It.IsAny<int>()))
                               .ReturnsAsync(() => model);

            var controller = new ProjectsController(logger.Object, projectServiceMock.Object);
            var id = 10;

            var result = await controller.Details(id) as ViewResult;
            var viewName = result.ViewName;

            Assert.True(string.IsNullOrEmpty(viewName) || viewName == "Details");
            Assert.Equal(model, result.Model);
        }

        [Fact]
        public async Task Create_ValidData_Return_OkResult()
        {
            var projectServiceMock = new Mock<IProjectManager>();
            var logger = new Mock<ILogger<ProjectsController>>();

            var controller = new ProjectsController(logger.Object, projectServiceMock.Object);

            var model = new ProjectDTO();

            model.Name = "Alfa";
            model.Begin = DateTime.Now.Date;
            model.Finish = DateTime.Today.AddDays(90);
            model.Budget = 10000.0M;
            model.Rate = 100.0M;

            var data = await controller.Create(model);
            Assert.IsType<RedirectToActionResult>(data);
        }

        [Fact]
        public void Create_return_view()
        {
            var projectServiceMock = new Mock<IProjectManager>();
            var logger = new Mock<ILogger<ProjectsController>>();
            
            var controller = new ProjectsController(logger.Object, projectServiceMock.Object);

            var result = controller.Create() as ViewResult;
            var viewName = result.ViewName;

            Assert.True(string.IsNullOrEmpty(viewName) || viewName == "Create");
        }

    }
}
