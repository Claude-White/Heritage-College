using HFA.Models;
using HFA.Models.ViewModels;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using HFA.Mappings;
using HFA.Services;

var builder = WebApplication.CreateBuilder(args);

var initialScopes = builder.Configuration["DownstreamApi:Scopes"]?.Split(' ') ?? builder.Configuration["MicrosoftGraph:Scopes"]?.Split(' ');

builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAd"))
        .EnableTokenAcquisitionToCallDownstreamApi(initialScopes)
            .AddMicrosoftGraph(builder.Configuration.GetSection("MicrosoftGraph"))
            .AddInMemoryTokenCaches();

builder.Services.AddAutoMapper(typeof(ArticleViewModelProfile));
builder.Services.AddAutoMapper(typeof(ArticleSummaryViewModelProfile));

// Add roles
builder.Services.AddAuthorizationBuilder()
    .AddPolicy("HasAccessToMemberContent", policy =>
        policy.RequireAssertion(context =>
            context.User.HasClaim(c => (c.Type == "groups" && c.Value == builder.Configuration.GetSection("AzureGroups").GetSection("HFAMemberGroupId").Value)) ||
            context.User.HasClaim(c => (c.Type == "groups" && c.Value == builder.Configuration.GetSection("AzureGroups").GetSection("HFADeveloperGroupId").Value))))
    .AddPolicy("HasEditPermissions", policy =>
        policy.RequireAssertion(context =>
            context.User.HasClaim(c => (c.Type == "groups" && c.Value == builder.Configuration.GetSection("AzureGroups").GetSection("HFAExecutiveGroupId").Value)) ||
            context.User.HasClaim(c => (c.Type == "groups" && c.Value == builder.Configuration.GetSection("AzureGroups").GetSection("HFADeveloperGroupId").Value))))
    .AddPolicy("HasDeveloperPermissions", policy =>
        policy.RequireAssertion(context =>
            context.User.HasClaim(c => (c.Type == "groups" && c.Value == builder.Configuration.GetSection("AzureGroups").GetSection("HFADeveloperGroupId").Value))));

// Add services to the container.
builder.Services.AddControllersWithViews(options =>
{
    var policy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
    options.Filters.Add(new AuthorizeFilter(policy));
});
builder.Services.AddRazorPages()
    .AddMicrosoftIdentityUI();

builder.Services.AddDbContext<HFAContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("HFA_KeeperConnection")));
builder.Services.AddScoped<IDbAccess, DbAccess>();
builder.Services.AddScoped<IImageService, ImageService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseStaticFiles();

app.UseStaticFiles(new StaticFileOptions()
{
    FileProvider = new PhysicalFileProvider(
      Path.Combine(Directory.GetCurrentDirectory(), @"node_modules")),
    RequestPath = new PathString("/lib")
});

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllers();

app.MapRazorPages();

app.Run();
