using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using CovidAPI.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Octokit;

namespace CovidAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            Thread updateThread = new Thread(AsyncCheckDatasetUpdate);
            updateThread.Start();
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private async void AsyncCheckDatasetUpdate(object obj)
        {
            var github = new GitHubClient(new ProductHeaderValue("Covid-API"));
            var repository = await github.Repository.Get("datasets", "covid-19");

            if (repository.UpdatedAt.UtcDateTime.Hour > DateTime.UtcNow.Hour)
            {
                Console.WriteLine($"Atualizando dataset, ultimo update: {repository.UpdatedAt.UtcDateTime.ToLongDateString()}");

                WebClient client = new WebClient();
                client.DownloadFile(new Uri(CovidData.DATASET_URL), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "time-series-19-covid-combined.csv"));

                Console.WriteLine("Dataset atualizado!");
            }

            Thread.Sleep(30 * 60 * 1000);
        }
    }
}
