using AutoMapper;
using CleanAspNetCoreWithFreeMetronic.Data;
using CleanAspNetCoreWithFreeMetronic.Data.Services;
using CleanAspNetCoreWithFreeMetronic.Helpers;
//using CleanAspNetCoreWithFreeMetronic.Helpers.Extensions;
using CleanAspNetCoreWithFreeMetronic.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CleanAspNetCoreWithFreeMetronic
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        //virtual muutmine
        public virtual void ConfigureServices(IServiceCollection services)
        {

            //Service lisamine
            //services.AddServicesInAssembly(Configuration);
            services.AddTransient<ILogManager, LogManager>();
            services.AddTransient<IJobTaskManager, JobTaskManager>();
            //services.AddTransient<IProjectManager, ProjectManager>();
            services.AddScoped<IProjectManager, ProjectManager>();

            services.AddDbContext<ApplicationDbContext>(options => 
            {
                options.UseSqlite(Configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddDefaultIdentity<IdentityUser>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;

            }).AddRoles<IdentityRole>()
              .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddControllersWithViews();
            services.AddRazorPages();

            //Lisatud
            //services.AddSingleton<IFileProvider>(
            //new PhysicalFileProvider(
                //Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "task")));


            services.AddMvc().AddRazorRuntimeCompilation();

            //Automapper
            services.AddAutoMapper(typeof(AutoMapping));


        }

        public virtual void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}"
                );
                endpoints.MapRazorPages();
            });

            var serviceScopeFactory = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>();
            using (var serviceScope = serviceScopeFactory.CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();
                dbContext.Database.EnsureCreated();

                var _userManager = serviceScope.ServiceProvider.GetService<UserManager<IdentityUser>>();
                var _roleManager = serviceScope.ServiceProvider.GetService<RoleManager<IdentityRole>>();

                string[] userRoles =
                {
                    "Admin",
                    "User",
                };

                string[,] userData =
                {
                    {"bonakas@live.com", "Porgand1!", "User" },
                    { "jaaktamm@live.com", "Porgand1!", "Admin"},
                    {"madisliiv@live.com","Porgand1!", "User"},
                };

                for (int i = 0; i < userData.GetLength(0); i++)
                {
                    if (!dbContext.Users.Any(usr => usr.UserName == userData[i, 0]))
                    {
                        var user = new IdentityUser()
                        {
                            UserName = userData[i, 0],
                            Email = userData[i, 0],
                            NormalizedEmail = userData[i, 0].ToUpper(),
                            NormalizedUserName = userData[i, 0].ToUpper(),
                        };

                        var userResult = _userManager.CreateAsync(user, userData[i, 1]).Result;
                    }
                }

                for (int i = 0; i < userRoles.Length; i++)
                {
                    if (!_roleManager.RoleExistsAsync(userRoles[i]).Result)
                    {
                        var role = _roleManager.CreateAsync
                                   (new IdentityRole { Name = userRoles[i] }).Result;
                    }
                }

                for (int i = 0; i < userData.GetLength(0); i++)
                {
                    var userId = _userManager.FindByNameAsync(userData[i, 0]).Result;
                    var userRole = _userManager.AddToRoleAsync(userId, userData[i, 2]).Result;
                }

                if (dbContext.Projects.Count() == 0)
                {

                    var useridee = _userManager.FindByNameAsync("bonakas@live.com").Result;

                    var project = new Project()
                    {
                        Name = "Alfa",
                        Begin = DateTime.Now.Date,
                        Finish = DateTime.Today.AddDays(90),
                        Budget = 10000.0M,
                        Rate = 100.0M,

                        JobTasks = new List<JobTask>()
                        {
                            new JobTask {TaskName = "Alfa Task 1", TaskStart = DateTime.Today.AddDays(10), EstimateHours = 90.0M, TaskDescription = "Alfa Task 1 kirjeldus", Responsible = useridee,
                            Logs = new List<Log>()
                            {
                                new Log {LogDate = DateTime.Now, LogDescription = "Alfa Task 1 Log1", TimeSpent = 35,  DoneBy = useridee},
                                new Log {LogDate = DateTime.Now, LogDescription = "Alfa Task 1 Log2", TimeSpent = 12,  DoneBy = useridee}
                            }
                            },
                            new JobTask {TaskName = "Alfa Task 2", TaskStart = DateTime.Today.AddDays(11), EstimateHours = 10.0M, TaskDescription = "Alfa Task 2 kirjeldus", Responsible = useridee,
                            Logs = new List<Log>()
                            {
                                new Log {LogDate = DateTime.Now, LogDescription = "Alfa Task 2 Log1", TimeSpent = 8,  DoneBy = useridee},
                                new Log {LogDate = DateTime.Now, LogDescription = "Alfa Task 2 Log2", TimeSpent = 5,  DoneBy = useridee}
                            }
                            }
                        }
                    };

                    dbContext.Projects.Add(project);

                    project = new Project()
                    {
                        Name = "Beeta",
                        Begin = DateTime.Now.Date,
                        Finish = DateTime.Today.AddDays(90),
                        Budget = 10000.0M,
                        Rate = 200.0M,

                        JobTasks = new List<JobTask>()
                        {
                            new JobTask {TaskName = "Beeta Task 1", TaskStart = DateTime.Today.AddDays(10), EstimateHours = 20M, TaskDescription = "Beeta Task 1 kirjeldus", Responsible = useridee,
                            Logs = new List<Log>()
                            {
                                new Log {LogDate = DateTime.Now, LogDescription = "Beeta Task 1 Log1", TimeSpent = 18,  DoneBy = useridee},
                                new Log {LogDate = DateTime.Now, LogDescription = "Beeta Task 1 Log2", TimeSpent = 3,  DoneBy = useridee}
                            }
                            },
                            new JobTask {TaskName = "Beeta Task 2", TaskStart = DateTime.Today.AddDays(11), EstimateHours = 30.0M, TaskDescription = "Beeta Task 2 kirjeldus", Responsible = useridee,
                            Logs = new List<Log>()
                            {
                                new Log {LogDate = DateTime.Now, LogDescription = "Beeta Task 2 Log1", TimeSpent = 14,  DoneBy = useridee},
                                new Log {LogDate = DateTime.Now, LogDescription = "Beeta Task 2 Log2", TimeSpent = 11,  DoneBy = useridee}
                            }
                            }
                        }
                    };

                    dbContext.Projects.Add(project);
                    dbContext.SaveChanges();

                }

                dbContext.SaveChanges();

            }

            
        }
    }
}