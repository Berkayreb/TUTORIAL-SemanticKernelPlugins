#pragma warning disable SKEXP0070

using Microsoft.SemanticKernel;
using SemanticKernelPlugins.Server.Plugins;


var builder = WebApplication.CreateBuilder(args);

var modelId = "llama3.2:latest";
var endpoint = new Uri("http://localhost:11434");

builder.Services.AddTransient(sp =>
{
    var kernelBuilder = Kernel.CreateBuilder().AddOllamaChatCompletion(modelId, endpoint);
    kernelBuilder.Plugins.AddFromType<HomePlugin>();
    kernelBuilder.Services.AddHttpClient();
    return kernelBuilder.Build();
});
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseCors();
app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();
