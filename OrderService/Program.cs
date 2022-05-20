using Messaging;
using Microsoft.AspNetCore.Mvc;
using OrderService;
var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables()
    .AddUserSecrets<Program>()
    .AddCommandLine(args)
    .Build();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.SetUpRabbitMq(builder.Configuration);
builder.Services.AddSingleton<RabbitSender>();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var orderIdSeed = 1;
app.MapPost("/waffleOrder", (RabbitSender rabbitSender, [FromBody] Order order) =>
{
    if (order.Id is 0)
    {
        order = new Order().Seed(orderIdSeed);
        orderIdSeed++;
    }
    rabbitSender.PublishMessage<Order>(order, "order.cookwaffle");
});

app.Run();

