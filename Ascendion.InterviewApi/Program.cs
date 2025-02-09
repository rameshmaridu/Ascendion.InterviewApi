using Ascendion.InterviewApi.ApplicationParameters;
using Ascendion.InterviewApi.Repository;
using Ascendion.InterviewApi.Service;
using Ascendion.InterviewApi.Service.Contract;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<AppParams>(builder.Configuration.GetSection("AppSettings"));
builder.Services.AddMemoryCache();
builder.Services.AddSingleton<IHttpRequestProcessor, HttpRequestProcessor>();
builder.Services.AddSingleton<IStoryService, StoryService>();
builder.Services.AddSingleton<IStoryRepository, StoryRepository>();

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
