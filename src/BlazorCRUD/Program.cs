using BlazorCRUD.Components;
using Microsoft.EntityFrameworkCore;
using System.IO;

using BlazorCRUD.Interfaces;
using BlazorCRUD.Services;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddQuickGridEntityFrameworkAdapter();

builder.Services.AddHttpClient("Gateway", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ApiGateway:BaseUrl"]!);
});

builder.Services.AddScoped<ProtectedLocalStorage>();

builder.Services.AddScoped<IAuthService, AuthApiService>();
builder.Services.AddScoped<IStudentService, StudentApiService>();
builder.Services.AddScoped<AuthTokenStore>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(BlazorCRUD.Client._Imports).Assembly);

app.Run();
