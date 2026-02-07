using BlazorCRUD.Components;

using BlazorCRUD.Interfaces;
using BlazorCRUD.Services;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Components.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

// Add authentication and authorization
builder.Services.AddAuthentication("Cookies")
    .AddCookie("Cookies", options =>
    {
        options.LoginPath = "/login";
        options.LogoutPath = "/logout";
    });
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
builder.Services.AddAuthorization();

builder.Services.AddHttpClient("Gateway", client =>
{
    var gatewayUrl = Environment.GetEnvironmentVariable("GATEWAY_BASE_URL") ?? builder.Configuration["ApiGateway:BaseUrl"];
    
    if (string.IsNullOrEmpty(gatewayUrl))
    {
        throw new InvalidOperationException("API Gateway base URL is not configured. Please set the GATEWAY_BASE_URL environment variable or add it to appsettings.json.");
    }
    
    client.BaseAddress = new Uri(gatewayUrl!);
});

builder.Services.AddScoped<ProtectedLocalStorage>();

builder.Services.AddScoped<IAuthService, AuthApiService>();
builder.Services.AddScoped<IStudentService, StudentApiService>();
builder.Services.AddScoped<IAuthTokenStore, AuthTokenStore>();

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

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(BlazorCRUD.Client._Imports).Assembly);

app.Run();
