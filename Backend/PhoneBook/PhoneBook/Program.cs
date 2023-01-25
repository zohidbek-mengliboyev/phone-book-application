using PhoneBook.Application;
using PhoneBook.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

#region ConfigureServices

var services = builder.Services;

// Add services to the container.
services.AddApplicationServices(builder.Configuration);
services.AddInfrastructureServices(builder.Configuration);

services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

services.AddCors(options =>
{
    options.AddPolicy("AllowOrigin", policy =>
    {
        policy.WithOrigins(
            "localhost:4200",
            "localhost:4202"
            );

        policy.AllowAnyHeader();

        policy.AllowAnyMethod();

    });
});
#endregion

var app = builder.Build();

#region Configure
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles();
app.Services.GetRequiredService<ILoggerFactory>().AddFile("Logs/mylog-{Date}.txt");

app.UseHttpsRedirection();

app.UseCors("AllowOrigin");

app.UseAuthorization();

app.MapControllers();
#endregion

app.Run();
