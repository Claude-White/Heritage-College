using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using OHMS;
using OHMS.Models;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// Add Cookies
builder.Services.AddScoped<CookieStorageAccessor>();

// Add Constants
builder.Services.AddSingleton(new AppConstants());

//------- Register Auth Service Here -------
builder.Services.AddScoped<HIAAAStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(sp 
    => sp.GetRequiredService<HIAAAStateProvider>());
builder.Services.AddAuthorizationCore(options => {
    options.AddPolicy("Student", policy => policy
        .RequireClaim(ClaimTypes.Role, "Student"));
    options.AddPolicy("Teacher", policy => policy
        .RequireClaim(ClaimTypes.Role, "Teacher"));
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
});
//------------------------------------------

await builder.Build().RunAsync();
