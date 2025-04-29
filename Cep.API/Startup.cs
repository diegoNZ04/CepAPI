using System.Reflection;
using Cep.API.Middlewares;
using Cep.Application.Mappings;
using Cep.Application.Services;
using Cep.Application.Services.Interfaces;
using Cep.Application.Validators;
using Cep.Infra.Data;
using Cep.Infra.Repositories;
using Cep.Infra.Repositories.Interfaces;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

public class Startup
{
    public IConfiguration Configuration { get; }
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddAutoMapper(typeof(CepProfile));
        services.AddFluentValidationAutoValidation();
        services.AddValidatorsFromAssemblyContaining<CepRequestValidator>();

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));

        AddSwaggerConfiguration(services);
        AddHttpClients(services);
        AddDependencyInjection(services);

    }

    private void AddSwaggerConfiguration(IServiceCollection services)
    {
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
            var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
        });
    }

    private void AddHttpClients(IServiceCollection services)
    {
        services.AddHttpClient<IViaCepService, ViaCepService>();
    }

    private void AddDependencyInjection(IServiceCollection services)
    {
        services.AddScoped<ICepRepository, CepRepository>();
        services.AddScoped<ICepService, CepService>();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseMiddleware<ExceptionMiddleware>();

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
