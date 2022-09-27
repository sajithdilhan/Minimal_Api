using MinimalApi.Services;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<MathService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.MapGet("/add", (int a, int b, ILogger<Program> logger) =>
{
    try
    {
        using (var scope = app.Services.CreateScope())
        {
            var mathService = scope.ServiceProvider.GetRequiredService<MathService>();
            logger.LogInformation($"Adding {a} and {b}");
            return Results.Ok(mathService.Add(a, b));
        }
    }
    catch (Exception ex)
    {
        logger.LogError(ex.Message);
        return Results.Problem(ex.Message, statusCode: (int)HttpStatusCode.InternalServerError);
    }
})
.WithName("AddTwoNumbers");

app.MapGet("/divide", (int a, int b, ILogger<Program> logger) =>
{
    try
    {
        using (var scope = app.Services.CreateScope())
        {
            var mathService = scope.ServiceProvider.GetRequiredService<MathService>();
            logger.LogInformation($"Dividing {a} by {b}");
            return Results.Ok(mathService.Divide(a, b));
        }
    }
    catch (Exception ex)
    {
        logger.LogError(ex.Message);
        return Results.Problem(ex.Message, statusCode: (int)HttpStatusCode.InternalServerError);
    }
})
.WithName("Divide");

app.Run();

