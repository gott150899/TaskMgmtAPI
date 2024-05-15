using Microsoft.EntityFrameworkCore;
using TaskMgmtApi.Context;
using TaskMgmtApi.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Midewares
builder.Services.AddControllers(options =>
{
    options.Filters.Add(typeof(ReturnResponseAttribute));
});

//DbContext
builder.Services.AddDbContext<TaskMgmtContext>(option =>
{
    option.UseSqlServer(
        builder.Configuration.GetConnectionString("DbConnection"),
        providerOptions => providerOptions.EnableRetryOnFailure(5, TimeSpan.FromSeconds(30), null)
    );
});
//CORS
builder.Services.AddCors(
                options =>
                    options.AddPolicy(
                        "AllowAllBrowser",
                        policy =>
                        {
                            policy
                                .AllowAnyOrigin()
                                .AllowAnyHeader()
                                .AllowAnyMethod();
                        }));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowAllBrowser");

app.UseAuthorization();

app.MapControllers();

app.Run();
