//using Api.Filters;
using Api.Converters;
using Api.Middlewares;
using Api.Services;
using Application;
using Application.Common.Interfaces;
using FluentValidation.AspNetCore;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true)
    .AddEnvironmentVariables();

builder.Host.UseSerilog((ctx, lc) => lc
     .ReadFrom.Configuration(ctx.Configuration));

builder.WebHost.ConfigureKestrel(options => options.AddServerHeader = false);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers()
    .AddFluentValidation(x => x.AutomaticValidationEnabled = false)
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new DateTimeConverter());
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    });
builder.Services.AddRouting(r => r.LowercaseUrls = true);
builder.Services.AddSwaggerGen();

builder.Services.AddMemoryCache();

builder.Services.AddHttpContextAccessor();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddSingleton<ICurrentUserService, CurrentUserService>();

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;

    //options.InvalidModelStateResponseFactory = actionContext =>
    //{
    //    //ValidationProblemDetails error = actionContext.ModelState
    //        //.Where(e => e.Value.Errors.Count > 0)
    //        //.Select(e => new ValidationProblemDetails(actionContext.ModelState)).FirstOrDefault();

    //    // Here you can add logging to you log file or to your Application Insights.
    //    // For example, using Serilog:
    //    // Log.Error($"{{@RequestPath}} received invalid message format: {{@Exception}}", 
    //    //   actionContext.HttpContext.Request.Path.Value, 
    //    //   error.Errors.Values);
    //    return new BadRequestObjectResult(error);
    //};
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{

}

//app.UseRequestMiddleware();

app.UseExceptionMiddleware();

app.UseHttpHeadersMiddleware();

app.UseSwagger();
app.UseSwaggerUI(c => { 
    c.DefaultModelsExpandDepth(-1);
});

app.MapControllers();

app.Run();