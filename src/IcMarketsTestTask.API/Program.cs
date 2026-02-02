
using IcMarketsTestTask.API;
using IcMarketsTestTask.API.Application.Services.Blockchains;
using IcMarketsTestTask.API.Infrastructure.Data;
using IcMarketsTestTask.API.Infrastructure.Mappings;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;

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

builder.Services.AddHttpClient<IBlockcypherClient, BlockcypherClient>(client =>
{
    var uriString = builder.Configuration["Blockcypher:BaseUrl"];
    if (uriString != null)
        client.BaseAddress = new Uri(uriString);
    client.Timeout = TimeSpan.FromSeconds(10);
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
        .AllowAnyHeader()
        .AllowCredentials();

    var listOfUrl = new List<string>();
    listOfUrl.Add("http://127.0.0.1:8080");
    builder.WithOrigins(listOfUrl.ToArray());
}));

builder.Services.AddAutoMapper(typeof(BlockcypherProfile).Assembly);

var app = builder.Build();
app.UseRouting();
app.UseCors();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();
