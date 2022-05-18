using Microsoft.AspNetCore.Mvc;
using OrderService;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var orderIdSeed = 1;
app.MapPost("/waffleOrder", ([FromBody] Order order) =>
{
    if (order.Id is 0)
    {
        order = new Order().Seed(orderIdSeed);
        orderIdSeed++;
    }
     // do something with the incoming order
});

app.Run();

