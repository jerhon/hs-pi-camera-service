using Honlsoft.Pi.CameraService.Camera;
using Honlsoft.Pi.Hardware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace Honlsoft.Pi.CameraService
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

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Honlsoft.Pi.CameraService", Version = "v1" });
            });
            services.AddHostedService<CameraPollingService>();
            services.AddHostedService<MmalInitialization>();
            services.Configure<CameraOptions>(Configuration.GetSection("Camera"));
            services.Configure<ServoOptions>(Configuration.GetSection("Servo"));
            services.AddSingleton<CameraImageCache>();
            services.AddSingleton(Unosquare.RaspberryIO.Pi.I2C.AddDevice(0x40));
            services.AddSingleton<Pca9685>();
            services.AddSingleton<Servo>();
            services.AddSingleton<ICameraCapture, MmalCameraCapture>();
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Honlsoft.Pi.CameraService v1"));
            }
            
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
