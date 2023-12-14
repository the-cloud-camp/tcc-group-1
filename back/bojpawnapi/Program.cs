using bojpawnapi.DataAccess;
using bojpawnapi.DTO;
using Microsoft.EntityFrameworkCore;
using bojpawnapi.Service;
using HealthChecks.NpgSql;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using bojpawnapi.Service.Auth;


//FOR AUTHEN
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using bojpawnapi.Entities.Auth;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddAutoMapper(typeof(Program));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Required for new Instance for Every Request
builder.Services.AddTransient<IAuthService, AuthService>();

// Add services to the container.
builder.Services.AddScoped<ICollateralTxService, CollateralTxService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();

builder.Services.AddDbContext<PawnDBContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("BojPawnDbConnection")));

string connString = builder.Configuration.GetConnectionString("BojPawnDbConnection");
builder.Services.AddHealthChecks().AddNpgSql(connString, tags: new[] { "startup" });

//FOR AUTHEN
// For Identity  
builder.Services.AddIdentity<ApplicationUserEntities, IdentityRole>()
                .AddEntityFrameworkStores<PawnDBContext>()
                .AddDefaultTokenProviders();
// Adding Authentication  
builder.Services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                // Adding Jwt Bearer  
                .AddJwtBearer(options =>
                {
                    options.SaveToken = true;
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidAudience = builder.Configuration["JWTKey:ValidAudience"],
                        ValidIssuer = builder.Configuration["JWTKey:ValidIssuer"],
                        ClockSkew = TimeSpan.Zero,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWTKey:Secret"]))
                    };
                });


builder.Services.AddCors(options =>
{
    options.AddPolicy("Open", builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});

var app = builder.Build();
app.Logger.LogInformation("Connection String: " + connString);
app.Logger.LogInformation("Key String: " + builder.Configuration["JWTKey:Secret"]);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//FOR AUTHEN
app.UseAuthentication();

app.UseAuthorization();
app.UseCors("Open");
app.MapControllers();

app.MapHealthChecks("/health/ready");
app.MapHealthChecks("/health/startup", new HealthCheckOptions { Predicate = x => x.Tags.Contains("startup") });        
//กรณีที่ microservice อื่นๆ ให้ทำ Custom HealhCheck Class 

app.Run();
