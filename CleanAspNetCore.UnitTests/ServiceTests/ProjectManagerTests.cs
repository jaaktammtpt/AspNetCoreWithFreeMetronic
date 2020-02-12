using AutoMapper;
using CleanAspNetCoreWithFreeMetronic.Data;
using CleanAspNetCoreWithFreeMetronic.Models;
using CleanAspNetCoreWithFreeMetronic.Data.Services;
using CleanAspNetCoreWithFreeMetronic.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CleanAspNetCore.UnitTests.ServiceTests
{
    public class ProjectManagerTests : ServiceTestBase
    {
        private readonly ProjectManager ProjectService;

        public ProjectManagerTests()
        {
            ProjectService = new ProjectManager(DbContext, Mapper);
        }

        [Fact]
        public async Task GetAllAsync_should_return_list_of_projects()
        {
            await InitProjectListAsync();

            var result = await ProjectService.GetAllAsync();

            Assert.NotNull(result);
            Assert.Equal(3, result.Count());
        }

        [Fact]
        public async Task GetByIdAsync_should_return_null_for_missing_project()
        {
            await InitProjectListAsync();
            var id = -1;

            var result = await ProjectService.GetByIdAsync(id);

            Assert.Null(result);
        }

        [Fact]
        public async Task GetByIdAsync_should_return_model_for_existing_customer()
        {
            await InitProjectListAsync();
            var id = 2;

            var result = await ProjectService.GetByIdAsync(id);

            Assert.NotNull(result);
            Assert.Equal(id, result.Id);
        }

        [Fact]
        public async Task ExistAsync_should_return_false_for_missing_projectAsync()
        {
            await InitProjectListAsync();
            var id = -1;

            var result = ProjectService.ExistAsync(id);

            Assert.False(result);
        }

        [Fact]
        public async Task ExistAsync_should_return_true_for_missing_project()
        {
            await InitProjectListAsync();
            var id = 2;

            var result = ProjectService.ExistAsync(id);

            Assert.True(result);
        }

        [Fact]
        public async Task DeleteAsync_should_return_true_for_deleted_project()
        {
            await InitProjectListAsync();
            var id = 2;

            var data = await ProjectService.DeleteAsync(id);

            Assert.True(data);
        }


        [Fact]
        public async Task DeleteAsync_should_return_false_for_deleted_project()
        {
            await InitProjectListAsync();
            var id = 4;

            var exeption = await Assert.ThrowsAsync<ArgumentNullException>(async () => await ProjectService.DeleteAsync(id));
            Assert.Equal("Pask", exeption.Message);
        }

        [Fact]
        public async Task UpdateAsync_should_return_updated_model()
        {
            await InitProjectListAsync();
            var id = 2;

            var project = await ProjectService.GetByIdAsync(id);

            var update = new ProjectDTO();
            update.Name = "Updated";
            update.Begin = project.Begin;
            update.Finish = project.Finish;
            update.Budget = project.Budget;
            update.Rate = project.Rate;

            var updatedData = await ProjectService.UpdateAsync(update);

            Assert.Equal("Updated", updatedData.Name);
        }

        [Fact]
        public async Task CreateAsync_should_return_created_object()
        {
            await InitProjectListAsync();

            var projectDTO = new ProjectDTO();
            projectDTO.Name = "Updated";
            projectDTO.Begin = DateTime.Now.Date;
            projectDTO.Finish = DateTime.Today.AddDays(90);
            projectDTO.Budget = 10000.0M;
            projectDTO.Rate = 100.0M;

            var result = await ProjectService.CreateAsync(projectDTO);

            Assert.NotNull(result);
            Assert.IsType<ProjectDTO>(result);
        }

        private async Task InitProjectListAsync()
        {
            DbContext.Projects.Add(new Project
            {
                Name = "Alfa",
                Begin = DateTime.Now.Date,
                Finish = DateTime.Today.AddDays(90),
                Budget = 10000.0M,
                Rate = 100.0M
            });

            DbContext.Projects.Add(new Project
            {
                Name = "Beeta",
                Begin = DateTime.Now.Date,
                Finish = DateTime.Today.AddDays(90),
                Budget = 20000.0M,
                Rate = 120.0M
            });

            DbContext.Projects.Add(new Project
            {
                Name = "Gamma",
                Begin = DateTime.Now.Date,
                Finish = DateTime.Today.AddDays(90),
                Budget = 30000.0M,
                Rate = 130.0M
            });

            await DbContext.SaveChangesAsync();
        }
    }
}
