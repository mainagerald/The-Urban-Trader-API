using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using urban_trader_be.Data;
using urban_trader_be.Interface;
using urban_trader_be.Model;
using urban_trader_be.Repository;
using urban_trader_be.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});

builder.Services.AddControllers().AddNewtonsoftJson(options =>{ options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore; } ); //prevent object cycles

builder.Services.AddDbContext<AppDatabaseContext>(options=>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<AppUser, IdentityRole>(options=>{
    options.Password.RequireDigit=true;
    options.Password.RequireLowercase=true;
    options.Password.RequireUppercase=true;
    options.Password.RequiredLength=8;
}).AddEntityFrameworkStores<AppDatabaseContext>();

builder.Services.AddAuthentication(options=>{
    options.DefaultAuthenticateScheme=
    options.DefaultChallengeScheme=
    options.DefaultForbidScheme=
    options.DefaultScheme=
    options.DefaultSignInScheme=
    options.DefaultSignOutScheme=JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options=>{
    options.TokenValidationParameters= new TokenValidationParameters
    {
        ValidateIssuer=true,
        ValidIssuer=builder.Configuration["JWT:Issuer"],
        ValidateAudience=true,
        ValidAudience=builder.Configuration["JWT:Audience"],
        ValidateIssuerSigningKey=true,
        IssuerSigningKey=new SymmetricSecurityKey(
            System.Text.Encoding.UTF8.GetBytes(builder.Configuration["JWT:SigningKey"])
        )
    };
});

//dependency injections
builder.Services.AddScoped<iStockRepository, StockRepository >();
builder.Services.AddScoped<iCommentRepository, CommentRepository >();
builder.Services.AddScoped<iTokenService, TokenService>();
builder.Services.AddScoped<iPortfolioRespository, PortfolioRepository>();
builder.Services.AddScoped<iExternalApiService, ExternalApiService>();
builder.Services.AddHttpClient<iExternalApiService, ExternalApiService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// order matters here!

// cors configuration 
app.UseCors(x=>x
.AllowAnyMethod()
.AllowAnyHeader()
.AllowCredentials()
// .WithOrigins("https://localhost:44351")
.SetIsOriginAllowed(origin=>true));

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
