using IcMarketsTestTask.API;
using IcMarketsTestTask.API.Application.Services.Blockchains;
using IcMarketsTestTask.API.Infrastructure.Data;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using FluentValidation;
using IcMarketsTestTask.API.Application.Behaviors;
using MediatR;

string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers();

builder.Services.AddRouting(options => options.LowercaseUrls = true);

builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")),
    ServiceLifetime.Transient
);

builder.Services
    .AddHealthChecks()
    .AddDbContextCheck<AppDbContext>(
        name: "database",
        tags: new[] { "ready" });

builder.Services.AddHttpClient<IBlockcypherClient, BlockcypherClient>(client =>
{
    var baseUrl = builder.Configuration["Blockcypher:BaseUrl"];
    if (baseUrl != null)
        client.BaseAddress = new Uri(baseUrl);
    
    var timeoutSeconds = builder.Configuration["Blockcypher:TimeoutSeconds"];
    if (timeoutSeconds != null) 
        client.Timeout = TimeSpan.FromSeconds(double.Parse(timeoutSeconds));
});

builder.Services.AddMediatR(p =>
{
    p.Lifetime = ServiceLifetime.Scoped;
    p.RegisterServicesFromAssembly(typeof(AssemblyInfo).Assembly);
});

builder.Services.AddCors(options => options.AddPolicy(MyAllowSpecificOrigins, builder =>
{
    builder
        .AllowAnyMethod()
        .AllowAnyHeader();

    var listOfUrl = new List<string>();
    listOfUrl.Add("http://127.0.0.1:8080");
    builder.WithOrigins(listOfUrl.ToArray());
}));

builder.Services.AddAutoMapper(typeof(AssemblyInfo).Assembly);

ValidatorOptions.Global.LanguageManager.Enabled = false;
builder.Services.AddValidatorsFromAssemblyContaining<AssemblyInfo>(filter: discoveredType =>
    discoveredType.ValidatorType.GetConstructors()
        .Any(x => x is { IsPublic: true, IsStatic: false } && !x.GetParameters().Any()));

builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

var app = builder.Build();
app.UseRouting();
app.UseCors();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.MapHealthChecks("/health/live", new HealthCheckOptions
{
    Predicate = _ => false
});

app.MapHealthChecks("/health/ready", new HealthCheckOptions
{
    Predicate = check => check.Tags.Contains("ready")
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();
