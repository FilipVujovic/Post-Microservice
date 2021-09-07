using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using PostService.AuthorizationMock;
using PostService.Data;
using PostService.Data.BlockMockRepository;
using PostService.Data.FollowingMockRepository;
using PostService.Data.TypeOfPostRepository;
using PostService.Data.UserMockRepository;
using PostService.Entities;
using PostService.Logger;
using PostService.Services;
using PostService.Services.TypeService;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace PostService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers(setup =>
            {
                setup.ReturnHttpNotAcceptable = true;
            }
            ).AddXmlDataContractSerializerFormatters();
            services.AddDbContext<DatabaseContext>(o => o.UseSqlServer(Configuration.GetConnectionString("Database")));
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddScoped<ITypeOfPostRepository, TypeOfPostRepository>();
            services.AddScoped<ITypeOfPostService, TypeOfPostService>();
            services.AddScoped<IPostService, PostsService>();
            services.AddScoped<IAuthorizationMockService, AuthorizationMockService>();
            services.AddScoped<IUserMockRepository, UserMockRepository>();
            services.AddScoped<IPostRepository, PostRepository>();
            services.AddScoped<IBlockMockRepository, BlockMockRepository>();
            services.AddScoped<IFollowingMockRepository, FollowingMockRepository>();
            services.AddSingleton(typeof(ILoggerRepository<>), typeof(LoggerRepository<>));
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "PostService", Version = "v1",
                    Description = "This API allows CRUD operations on the post database. It also filters get requests and returns posts in accordance to the filter",
                    Contact = new Microsoft.OpenApi.Models.OpenApiContact
                    {
                        Name = "Filip Vujovic",
                        Email = "f.vujovic998@gmail.com",
                        Url = new Uri("http://www.ftn.uns.ac.rs/")
                    },
                    License = new Microsoft.OpenApi.Models.OpenApiLicense
                    {
                        Name = "FTN licence",
                        Url = new Uri("http://www.ftn.uns.ac.rs/")
                    },
                });

                var xmlComments = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlCommentsPath = Path.Combine(AppContext.BaseDirectory, xmlComments);

                c.IncludeXmlComments(xmlCommentsPath);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "PostService v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
