﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerUI;
using TestProject.Data.Models;

namespace TestProject
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
			services.AddDbContext<ApplicationDbContext>(options =>
				options
					.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"))
					.EnableSensitiveDataLogging(),
				ServiceLifetime.Transient, ServiceLifetime.Singleton);

			services.AddMvc(config =>
			{
				config.InputFormatters.Add(new XmlSerializerInputFormatter(config));
			})
				//.AddXmlDataContractSerializerFormatters()
				.SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

			services.AddSwaggerGen(options =>
			{
				options.SwaggerDoc("v1",
					new Info
					{
						Title = "API V1",
						Version = "v1",
					}
				 );
			});

		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseMvc();

			app.UseSwagger(c =>
			{
				c.PreSerializeFilters.Add((swagger, httpReq) => swagger.Host = httpReq.Host.Value);
			});

			app.UseSwaggerUI(c =>
			{
				c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
				c.DocExpansion(DocExpansion.None);
			});
		}
	}
}
