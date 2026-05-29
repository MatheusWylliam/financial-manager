using FinancialGoalManager.API.Filters;
using FinancialGoalManager.API.Jobs;
using FinancialGoalManager.API.Middleware;
using FinancialGoalManager.Application.Commands.FinancialGoals.RegisterGoal;
using FinancialGoalManager.Application.Validators.FinancialGoal;
using FinancialGoalManager.Core.Repositories;
using FinancialGoalManager.Infrastructure.Persistence;
using FinancialGoalManager.Infrastructure.Persistence.Repositories;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace FinancialGoalManager.API.Extensions
{
    public static class ServiceConfiguration
    {
        public static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder)
        {
            builder.ConfigureDependencyInjection()
                   .ConfigureOthersServices();

            builder.Services.AddControllers();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(s =>
            {
                s.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "FinancialManager.API",
                    Version = "v1",
                    Contact = new OpenApiContact
                    {
                        Name = "Matheus Wylliam",
                        Email = "matheus.wylliam@edu.pucrs.br",
                        Url = new Uri("https://github.com/MatheusWylliam")
                    }
                });
            });

            return builder;
        }

        public static WebApplicationBuilder ConfigureDependencyInjection(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IFinancialGoalRepository, FinancialGoalRepository>();
            builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
            builder.Services.AddScoped<IReportsRepository, ReportsRepository>();

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            builder.Services.AddTransient<GlobalExceptionHandler>();

            return builder;
        }

        public static WebApplicationBuilder ConfigureOthersServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddMediatR(config =>
            {
                config.RegisterServicesFromAssembly(typeof(RegisterGoalCommand).Assembly);
            });

            builder.Services.AddControllers(options => options.Filters.Add(typeof(ValidationFilter)))
                .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<RegisterGoalValidator>());

            builder.Services.AddDbContext<FinancialGoalManagerDbContext>(options
                => options.UseSqlServer(builder.Configuration.GetConnectionString("FinancialGoalManagerDb")));

            builder.Services.AddHostedService<RememberDaysLeftJob>();

            return builder;
        }
    }
}