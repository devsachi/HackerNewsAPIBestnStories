using HackerNewsAPI.Services;
using HackerNewsAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddResponseCaching();
builder.Services.AddControllers(options =>
{
    options.CacheProfiles.Add("OneMinuteCacheProfile",
        new CacheProfile()
        {
            Duration = 60
        });
})
.AddNewtonsoftJson(setupAction =>
    {
        setupAction.SerializerSettings.ContractResolver =
            new CamelCasePropertyNamesContractResolver();
    });
    


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();

builder.Services.AddMemoryCache();
//Services Registration
builder.Services.AddTransient<IExternalDataService, ExternalDataService>();
builder.Services.AddScoped<IStoriesCache, StoriesCache>();
builder.Services.AddScoped<IStoriesService, StoriesService>();

builder.Services.AddResponseCaching();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler(appBuilder =>
    {
        appBuilder.Run(async context =>
        {
            context.Response.StatusCode = 500;
            await context.Response.WriteAsync(
                "An unexpected fault happened. Try again later.");
        });
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
