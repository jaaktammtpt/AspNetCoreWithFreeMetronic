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
    public class ProjectManagerTests : TestBase
    {
        [Fact]
        public async Task GetAllAsync_should_return_list_of_projects()
        {            
            var dbContext = GetDbContext();
            var mapper = GetMapper();

            dbContext.Projects.Add(new Project {
                Name = "Alfa",
                Begin = DateTime.Now.Date,
                Finish = DateTime.Today.AddDays(90),
                Budget = 10000.0M,
                Rate = 100.0M
            });

            dbContext.Projects.Add(new Project
            {
                Name = "Beeta",
                Begin = DateTime.Now.Date,
                Finish = DateTime.Today.AddDays(90),
                Budget = 20000.0M,
                Rate = 120.0M
            });

            dbContext.Projects.Add(new Project
            {
                Name = "Gamma",
                Begin = DateTime.Now.Date,
                Finish = DateTime.Today.AddDays(90),
                Budget = 30000.0M,
                Rate = 130.0M
            });

            await dbContext.SaveChangesAsync();

            var service = new ProjectManager(dbContext, mapper);
            var result = await service.GetAllAsync();

            Assert.NotNull(result);
            Assert.Equal(3, result.Count());
        }

        [Fact]
        public async Task GetByIdAsync_should_return_null_for_missing_project()
        {
            var dbContext = GetDbContext();
            var mapper = GetMapper();

            var id = -1;
            var service = new ProjectManager(dbContext, mapper);
            var result = await service.GetByIdAsync(id);

            Assert.Null(result);
        }

        [Fact]
        public async Task GetByIdAsync_should_return_model_for_existing_customer()
        {
            var dbContext = GetDbContext();
            var mapper = GetMapper();
            var id = 2;

            dbContext.Projects.Add(new Project
            {
                Name = "Alfa",
                Begin = DateTime.Now.Date,
                Finish = DateTime.Today.AddDays(90),
                Budget = 10000.0M,
                Rate = 100.0M
            });

            dbContext.Projects.Add(new Project
            {
                Name = "Beeta",
                Begin = DateTime.Now.Date,
                Finish = DateTime.Today.AddDays(90),
                Budget = 20000.0M,
                Rate = 120.0M
            });

            dbContext.Projects.Add(new Project
            {
                Name = "Gamma",
                Begin = DateTime.Now.Date,
                Finish = DateTime.Today.AddDays(90),
                Budget = 30000.0M,
                Rate = 130.0M
            });

            await dbContext.SaveChangesAsync();
            var service = new ProjectManager(dbContext, mapper);
            var result = await service.GetByIdAsync(id);

            Assert.NotNull(result);
            Assert.Equal(id, result.Id);
        }

        [Fact]
        public void ExistAsync_should_return_false_for_missing_project()
        {
            var dbContext = GetDbContext();
            var mapper = GetMapper();

            var id = -1;
            var service = new ProjectManager(dbContext, mapper);
            var result = service.ExistAsync(id);

            Assert.False(result);
        }

        [Fact]
        public async Task ExistAsync_should_return_true_for_missing_project()
        {
            var dbContext = GetDbContext();
            var mapper = GetMapper();
            var id = 2;

            dbContext.Projects.Add(new Project
            {
                Name = "Alfa",
                Begin = DateTime.Now.Date,
                Finish = DateTime.Today.AddDays(90),
                Budget = 10000.0M,
                Rate = 100.0M
            });

            dbContext.Projects.Add(new Project
            {
                Name = "Beeta",
                Begin = DateTime.Now.Date,
                Finish = DateTime.Today.AddDays(90),
                Budget = 20000.0M,
                Rate = 120.0M
            });

            dbContext.Projects.Add(new Project
            {
                Name = "Gamma",
                Begin = DateTime.Now.Date,
                Finish = DateTime.Today.AddDays(90),
                Budget = 30000.0M,
                Rate = 130.0M
            });

            await dbContext.SaveChangesAsync();
            var service = new ProjectManager(dbContext, mapper);
            var result = service.ExistAsync(id);

            Assert.True(result);
        }

        [Fact]
        public async Task DeleteAsync_should_return_true_for_deleted_project()
        {
            var dbContext = GetDbContext();
            var mapper = GetMapper();
            var id = 2;

            dbContext.Projects.Add(new Project
            {
                Name = "Alfa",
                Begin = DateTime.Now.Date,
                Finish = DateTime.Today.AddDays(90),
                Budget = 10000.0M,
                Rate = 100.0M
            });

            dbContext.Projects.Add(new Project
            {
                Name = "Beeta",
                Begin = DateTime.Now.Date,
                Finish = DateTime.Today.AddDays(90),
                Budget = 20000.0M,
                Rate = 120.0M
            });

            dbContext.Projects.Add(new Project
            {
                Name = "Gamma",
                Begin = DateTime.Now.Date,
                Finish = DateTime.Today.AddDays(90),
                Budget = 30000.0M,
                Rate = 130.0M
            });

            await dbContext.SaveChangesAsync();

            var service = new ProjectManager(dbContext, mapper);
            var data = await service.DeleteAsync(id);

            Assert.True(data);
        }


        [Fact]
        public async Task DeleteAsync_should_return_false_for_deleted_project()
        {
            var dbContext = GetDbContext();
            var mapper = GetMapper();
            var id = 4;

            dbContext.Projects.Add(new Project
            {
                Name = "Alfa",
                Begin = DateTime.Now.Date,
                Finish = DateTime.Today.AddDays(90),
                Budget = 10000.0M,
                Rate = 100.0M
            });

            dbContext.Projects.Add(new Project
            {
                Name = "Beeta",
                Begin = DateTime.Now.Date,
                Finish = DateTime.Today.AddDays(90),
                Budget = 20000.0M,
                Rate = 120.0M
            });

            dbContext.Projects.Add(new Project
            {
                Name = "Gamma",
                Begin = DateTime.Now.Date,
                Finish = DateTime.Today.AddDays(90),
                Budget = 30000.0M,
                Rate = 130.0M
            });

            await dbContext.SaveChangesAsync();

            var service = new ProjectManager(dbContext, mapper);
            //var data = await service.DeleteAsync(id);

            var exeption = await Assert.ThrowsAsync<ArgumentNullException>(async () => await service.DeleteAsync(id));
            Assert.Equal("Pask", exeption.Message);
        }

        [Fact]
        public async Task UpdateAsync_should_return_updated_model()
        {
            var dbContext = GetDbContext();
            var mapper = GetMapper();
            var id = 2;
            dbContext.Projects.Add(new Project
            {
                Name = "Alfa",
                Begin = DateTime.Now.Date,
                Finish = DateTime.Today.AddDays(90),
                Budget = 10000.0M,
                Rate = 100.0M
            });

            dbContext.Projects.Add(new Project
            {
                Name = "Beeta",
                Begin = DateTime.Now.Date,
                Finish = DateTime.Today.AddDays(90),
                Budget = 20000.0M,
                Rate = 120.0M
            });

            dbContext.Projects.Add(new Project
            {
                Name = "Gamma",
                Begin = DateTime.Now.Date,
                Finish = DateTime.Today.AddDays(90),
                Budget = 30000.0M,
                Rate = 130.0M
            });            
                        
            await dbContext.SaveChangesAsync();

            var service = new ProjectManager(dbContext, mapper);
            var project = await service.GetByIdAsync(id);

            var update = new ProjectDTO();
            update.Name = "Updated";
            update.Begin = project.Begin;
            update.Finish = project.Finish;
            update.Budget = project.Budget;
            update.Rate = project.Rate;

            var updatedData = await service.UpdateAsync(update);

            Assert.Equal("Updated", updatedData.Name);
        }

        [Fact]
        public async Task CreateAsync_should_return_created_object()
        {
            var dbContext = GetDbContext();
            var mapper = GetMapper();

            var service = new ProjectManager(dbContext, mapper);

            var projectDTO = new ProjectDTO();
            projectDTO.Name = "Updated";
            projectDTO.Begin = DateTime.Now.Date;
            projectDTO.Finish = DateTime.Today.AddDays(90);
            projectDTO.Budget = 10000.0M;
            projectDTO.Rate = 100.0M;

            var result = await service.CreateAsync(projectDTO);

            Assert.NotNull(result);
            Assert.IsType<ProjectDTO>(result);
        }
    }
}
