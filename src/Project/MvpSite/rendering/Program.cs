using Microsoft.AspNetCore.Builder;
using Mvp.Project.MvpSite.Rendering;

var builder = WebApplication.CreateBuilder(args);

var startup = new Startup(builder.Configuration);

startup.ConfigureServices(builder.Services);

var app = builder.Build();

startup.Configure(app, app.Environment);

app.Run();