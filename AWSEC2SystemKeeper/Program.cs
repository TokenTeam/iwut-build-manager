using AWSEC2SystemKeeper.Extension;
using AWSEC2SystemKeeper.Model;
using AWSEC2SystemKeeper.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAWSService((config) => { 
    config.AccessKey = builder.Configuration["AWS:AccessKey"];
    config.SecretKey = builder.Configuration["AWS:SecretKey"];
    config.Region = builder.Configuration["AWS:Region"];
    config.InstanceID = builder.Configuration["AWS:InstanceID"];
});
builder.Services.AddSingleton<TaskSchedulerService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
