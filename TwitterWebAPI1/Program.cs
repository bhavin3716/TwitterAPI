using Microsoft.EntityFrameworkCore;
using TwitterWebAPI1.Data;
using TwitterWebAPI1.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Add Automapper
builder.Services.AddAutoMapper(typeof(Program).Assembly);

//SQL context
builder.Services.AddDbContextPool<TwitterAppDataContext>(Options =>
{
    Options.UseSqlServer(builder.Configuration.GetConnectionString("DbConnection"));
});

// Services
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITweetsService, TweetsService>();


//Token authantocation serivice registration
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(option =>
    {
        option.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(builder.Configuration.GetSection("AppSettings:SecretToken").Value)),
            ValidateIssuer = false,
            ValidateActor = false
        };
    });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
