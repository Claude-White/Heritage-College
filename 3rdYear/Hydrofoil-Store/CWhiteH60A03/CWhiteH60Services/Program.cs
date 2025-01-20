using CWhiteH60Services.CalculateTaxes;
using CWhiteH60Services.CheckCreditCard;
using CWhiteH60Services.DAL;
using CWhiteH60Services.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
        options.JsonSerializerOptions.WriteIndented = true;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

/* Database Context Dependency Injection */
var dbHost = Environment.GetEnvironmentVariable("DB_HOST");
var dbName = Environment.GetEnvironmentVariable("DB_NAME");
var dbPassword = Environment.GetEnvironmentVariable("DB_SA_PASSWORD");

if (dbHost == null || dbName == null || dbPassword == null) {
    var connectionString = builder.Configuration.GetConnectionString("MyConnection") ?? throw new InvalidOperationException("Connection string 'H60AssignmentDbCWContextConnection' not found.");
    builder.Services.AddDbContext<H60AssignmentDbCWContext>(options => options.UseSqlServer(connectionString));
}
else {
    var connectionString = $"Data Source={dbHost};Initial Catalog={dbName};User ID=sa;Password={dbPassword};TrustServerCertificate=True;";
    builder.Services.AddDbContext<H60AssignmentDbCWContext>(opt => opt.UseSqlServer(connectionString));
}
/* ===================================== */

builder.Services.AddScoped<IStoreRepository<Product>, ProductRepository>();
builder.Services.AddScoped<ProductRepository>();
builder.Services.AddScoped<IStoreRepository<ProductCategory>, ProdCatRepository>();
builder.Services.AddScoped<ICustomerRepository<Customer>, CustomerRepository>();
builder.Services.AddScoped<IShoppingCartRepository<ShoppingCart>, ShoppingCartRepository>();
builder.Services.AddScoped<ICartItemRepository<CartItem>, CartItemRepository>();
builder.Services.AddScoped<IOrderRepository<Order>, OrderRepository>();
builder.Services.AddScoped<CheckCreditCardSoapClient>(serviceProvider => {
    var endpointConfiguration = CheckCreditCardSoapClient.EndpointConfiguration.CheckCreditCardSoap;
    return new CheckCreditCardSoapClient(endpointConfiguration);
});
builder.Services.AddScoped<CalculateTaxesSoapClient>(serviceProvider => {
    var endpointConfiguration = CalculateTaxesSoapClient.EndpointConfiguration.CalculateTaxesSoap;
    return new CalculateTaxesSoapClient(endpointConfiguration);
});
builder.Services.AddScoped<IOrderItemRepository<OrderItem>, OrderItemRepository>();

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options => {
        options.SignIn.RequireConfirmedAccount = true;
    })
    .AddEntityFrameworkStores<H60AssignmentDbCWContext>()
    .AddDefaultTokenProviders();

builder.Services.AddScoped<UserManager<IdentityUser>>();
builder.Services.AddScoped<SignInManager<IdentityUser>>();
builder.Services.AddScoped<RoleManager<IdentityRole>>();

builder.Services.AddScoped<IUserStore<IdentityUser>, UserStore<IdentityUser, IdentityRole, H60AssignmentDbCWContext>>();
builder.Services.AddScoped<IUserEmailStore<IdentityUser>>(provider =>
    (IUserEmailStore<IdentityUser>)provider.GetRequiredService<IUserStore<IdentityUser>>());

builder.Services.AddHttpClient();

builder.Services.AddCors(options => {
    options.AddPolicy("CorsPolicy", builder => {
        builder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

builder.Services.AddProblemDetails();

var app = builder.Build();

if (app.Environment.IsDevelopment()) {
    app.UseSwagger(options => options.RouteTemplate = "openapi/{documentName}.json");
    app.MapScalarApiReference();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("CorsPolicy");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();