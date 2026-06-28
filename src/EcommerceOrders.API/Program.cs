using EcommerceOrders.Infrastructure;
using EcommerceOrders.Application;
using EcommerceOrders.API.Endpoints;
using Microsoft.AspNetCore.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() 
    { 
        Title = "Ecommerce Orders API", 
        Version = "v1",
        Description = "API para gerenciamento de pedidos."
    });
});

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();

builder.Services.AddMemoryCache();
builder.Services.AddProblemDetails();
builder.Services.AddHealthChecks();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Orders API v1"));
}

app.UseExceptionHandler(); 

app.UseHttpsRedirection();

app.MapHealthChecks("/health");

app.MapOrderEndpoints(); 

app.Run();