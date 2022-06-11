using System.Reflection;
using Sonar.UserProfile.Core;
using Sonar.UserProfile.Data;
using Sonar.UserProfile.Web.Filters;
using Sonar.UserProfile.Web.Tools;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options => options.Filters.Add<ExceptionFilter>())
    .ConfigureApiBehaviorOptions(options =>
    {
        options.InvalidModelStateResponseFactory = ModelStateValidator.ValidateModelState;
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

var configuration = builder.Configuration;

builder.Services.AddCore();
builder.Services.AddData(configuration);

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();

app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
app.Run();