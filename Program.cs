//using Microsoft.AspNetCore.Authentication.JwtBearer;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.IdentityModel.Tokens;
//using System.Text;
//using TheBookNookApi.AppDbContext;
//using TheBookNookApi.Service;
//using TheBookNookApi.Services;
//using TheBookNookApi.Services.Interfaces;

////var builder = WebApplication.CreateBuilder(args);
////// Enable CORS globally
//////builder.Services.AddCors(options =>
//////{
//////    options.AddPolicy("AllowAll",
//////        builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
//////});
////// Add CORS
////builder.Services.AddCors(options =>
////{
////    options.AddPolicy("AllowAngularApp",
////        policy => policy
////            .WithOrigins("http://localhost:4200") // Frontend origin
////            .AllowAnyHeader()
////            .AllowAnyMethod());
////});
////// Add services to the container.

////builder.Services.AddControllers();
////builder.Services.AddDbContext<BookNookDbContext>(options =>
////    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
////// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
////builder.Services.AddEndpointsApiExplorer();
////builder.Services.AddSwaggerGen();
////// Registers the UserService to be injected wherever IUserService is required.
////// Scoped lifetime ensures a new instance per request.
////builder.Services.AddScoped<IUserService, UserService>();
////var app = builder.Build();

////// Configure the HTTP request pipeline.
////if (app.Environment.IsDevelopment())
////{
////    app.UseSwagger();
////    app.UseSwaggerUI();
////}

////app.UseHttpsRedirection();
////app.UseCors("AllowAngularApp");
////app.UseAuthorization();

////app.MapControllers();

////app.Run();

//var builder = WebApplication.CreateBuilder(args);

//#region Services Configuration

//// Configure CORS to allow requests from the Angular frontend
//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("AllowAngularApp", policy =>
//        policy.WithOrigins("http://localhost:4200") // Frontend origin
//              .AllowAnyHeader()
//              .AllowAnyMethod());
//});

//// Add controllers support
//builder.Services.AddControllers();

//// Register the database context with SQL Server
//builder.Services.AddDbContext<BookNookDbContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//// Register Swagger/OpenAPI for API documentation
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

//// Registers the UserService to be injected wherever IUserService is required.
//// Scoped lifetime ensures a new instance per request.
//// Register application services
//builder.Services.AddScoped<IUserService, UserService>();
//builder.Services.AddScoped<ITokenService, TokenService>();

//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//    .AddJwtBearer(options =>
//    {
//        options.TokenValidationParameters = new TokenValidationParameters
//        {
//            ValidateIssuer = true,
//            ValidateAudience = true,
//            ValidateLifetime = true,
//            ValidateIssuerSigningKey = true,
//            ValidIssuer = builder.Configuration["Jwt:Issuer"],
//            ValidAudience = builder.Configuration["Jwt:Audience"],
//            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
//        };
//    });

//builder.Services.AddAuthorization();


//#endregion

//var app = builder.Build();

//#region Middleware Configuration

//// Enable Swagger only in development
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

//// Use HTTPS redirection
//app.UseHttpsRedirection();

//// Enable CORS for allowed frontend
//app.UseCors("AllowAngularApp");

//app.UseAuthentication();
//// Enable authorization middleware
//app.UseAuthorization();


//// Map controller endpoints
//app.MapControllers();

//#endregion

//// Run the application
//app.Run();


#region Using Directives

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TheBookNookApi.AppDbContext;
using TheBookNookApi.Service;
using TheBookNookApi.Services;
using TheBookNookApi.Services.Interfaces;

#endregion

var builder = WebApplication.CreateBuilder(args);

#region Services Configuration

// Enable CORS to allow requests from the Angular frontend
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp", policy =>
        policy.WithOrigins("http://localhost:4200") // Angular frontend origin
              .AllowAnyHeader()
              .AllowAnyMethod());
});

// Add controller services
builder.Services.AddControllers();

// Register EF Core DbContext with SQL Server
builder.Services.AddDbContext<BookNookDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add Swagger for API documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register application-specific services
// Scoped lifetime ensures a new instance per request.
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<ICartService, CartService>();

// Register AutoMapper for object mapping
var jwtKey = builder.Configuration["Jwt:Key"];
if (Encoding.UTF8.GetBytes(jwtKey).Length < 32)
{
    throw new Exception("JWT key must be at least 256 bits (32 bytes) long.");
}
// Configure JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

// Add authorization support
builder.Services.AddAuthorization();

#endregion

var app = builder.Build();

#region Middleware Configuration

// Enable Swagger UI in development environment
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Redirect HTTP requests to HTTPS
app.UseHttpsRedirection();

// Apply CORS policy for frontend access
app.UseCors("AllowAngularApp");

// Enable authentication & authorization middlewares
app.UseAuthentication();
app.UseAuthorization();

// Map controller endpoints
app.MapControllers();

#endregion

// Run the application
app.Run();

