using Cep.Application.Services;
using Cep.Application.Services.Interfaces;
using Cep.Infra.Data;
using Cep.Infra.Repositories;
using Cep.Infra.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }
    public IConfiguration Configuration { get; }
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "CepAPI",
                Version = "v1",
                Description = "API para consultas de Cep.",
                Contact = new OpenApiContact
                {
                    Name = "Diego Amorim",
                    Email = "diegoamorim03152004@gmail.com"
                },
            });
        });

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseInMemoryDatabase("CepDb"));

        services.AddScoped<ICepRepository, CepRepository>();
        services.AddScoped<ICepService, CepService>();
        services.AddHttpClient();

    }
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {

        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "CepAPI v1");
                options.RoutePrefix = string.Empty;
            });
        }

        app.UseHttpsRedirection();
        app.UseRouting();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
