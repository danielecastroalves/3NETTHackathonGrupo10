using System.Diagnostics.CodeAnalysis;
using FluentValidation;
using HealthMed.Application;
using HealthMed.Application.Common.Auth.Token;
using HealthMed.Application.Common.Behavior;
using HealthMed.Application.Common.Repositories;
using HealthMed.Domain.Entities;
using HealthMed.Infrastructure.Auth.Token;
using HealthMed.Infrastructure.Mongo.Contexts;
using HealthMed.Infrastructure.Mongo.Contexts.Interfaces;
using HealthMed.Infrastructure.Mongo.Repositories;
using HealthMed.Infrastructure.Mongo.Utils;
using HealthMed.Infrastructure.Mongo.Utils.Interfaces;
using MediatR;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using Serilog;
using Serilog.Events;
using ILogger = Serilog.ILogger;

namespace HealthMed.WebApi.DependencyInjection;

/// <summary>
/// Configuring Dependency Injections
/// </summary>
[ExcludeFromCodeCoverage]
public static class ConfigureBindingsDependencyInjection
{
    /// <summary>
    /// Register Bindings
    /// </summary>
    /// <param name="services">IServiceCollection</param>
    /// <param name="configuration">IConfiguration</param>
    public static void RegisterBindings
    (
        IServiceCollection services,
        IConfiguration configuration
    )
    {
        ConfigureBindingsMediatR(services);
        ConfigureBindingsMongo(services, configuration);
        ConfigureBindingsSerilog(services);
        ConfigureBindingsValidators(services);

        // Services
        services.AddScoped<ITokenService, TokenService>();
        services.AddSingleton(provider => new EmailService(
           smtpServer: "smtp.gmail.com",
           smtpPort: 587,
           smtpUser: "healthmed953@gmail.com",
           smtpPass: "Health&MedFiapGrupo10"
       ));
    }

    private static void ConfigureBindingsMediatR(IServiceCollection services)
    {
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddMediatR(new AssemblyReference().GetAssembly());
    }

    private static void ConfigureBindingsMongo(IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<MongoConnectionOptions>(c =>
        {
            int defaultTtlDays = configuration.GetValue<int>("Mongo:DefaultTtlDays");
            c.DefaultTtlDays = defaultTtlDays == default ? 30 : defaultTtlDays;

            c.ConnectionString = configuration.GetValue<string>("Mongo:ConnectionString");

            c.Schema = configuration.GetValue<string>("Mongo:Schema");
        });

        services.AddSingleton<IMongoConnection, MongoConnection>();
        services.AddSingleton<IMongoContext, MongoContext>();

        //Configure Mongo Repositories
        services.AddScoped<IRepository<PersonEntity>, GenericRepository<PersonEntity>>();
        services.AddScoped<IRepository<AppointmentSchedulingEntity>, GenericRepository<AppointmentSchedulingEntity>>();
       
        services.AddScoped<IUserRepository, UserRepository>();

        //Configure Mongo Serializer
        BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));

        #pragma warning disable 618
        BsonDefaults.GuidRepresentation = GuidRepresentation.Standard;
        BsonDefaults.GuidRepresentationMode = GuidRepresentationMode.V3;
        #pragma warning restore

        #pragma warning disable CS8602
        var objectSerializer = new ObjectSerializer
        (
           type =>
                   ObjectSerializer.DefaultAllowedTypes(type) ||
                   type.FullName.StartsWith("Health&Med.Domain")
        );
        #pragma warning restore CS8602

        BsonSerializer.RegisterSerializer(objectSerializer);
    }

    private static void ConfigureBindingsSerilog(IServiceCollection services)
    {
        const string path = "logs";
        var shortDate = DateTime.Now.ToString("yyyy-MM-dd_HH");
        var filename = $@"{path}\{shortDate}.log";

        var logConfig = new LoggerConfiguration()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .MinimumLevel.Override("System", LogEventLevel.Warning)
            .MinimumLevel.Override("Host.Startup", LogEventLevel.Warning)
            .MinimumLevel.Override("Host.Aggregator", LogEventLevel.Warning)
            .MinimumLevel.Override("Host.Executor", LogEventLevel.Fatal)
            .MinimumLevel.Override("Host.Results", LogEventLevel.Fatal)
            .WriteTo.Console()
            .WriteTo.File(filename, rollingInterval: RollingInterval.Day);

        ILogger logger = logConfig.CreateLogger();

        services.AddLogging(configure: x => x.AddSerilog(logger));
    }

    private static void ConfigureBindingsValidators(IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(new AssemblyReference().GetAssembly());
    }

    
}
