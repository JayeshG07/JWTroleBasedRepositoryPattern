//using DGMarket.Domain.Dto;
//using DGMarket.Domain.Entities;
//using DGMarket.Persistent;
//using Microsoft.AspNetCore.Hosting;
//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

using JWTroleBased.Authorisation;
using JWTroleBased.Authorisation.Authorization;
using JWTroleBased.Configuratios;
using JWTroleBased.Context;
using JWTroleBased.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
string conection = builder.Configuration.GetConnectionString("localString");
builder.Services.AddDbContext<AppDbContext>(p => p.UseSqlServer(conection));
builder.Services.AddAutoMapper(typeof(MapConfig));
builder.Services.AddScoped<IAuthManager, AuthManager>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//        .AddJwtBearer(options =>
//        {
//            options.RequireHttpsMetadata = false;
//            options.SaveToken = true;
//            options.TokenValidationParameters = new TokenValidationParameters
//            {
//                ValidateIssuer = true,
//                ValidIssuer = builder.Configuration["JWTSettings:Issuer"],
//                ValidateAudience = true,
//                ValidAudience = builder.Configuration["JWTSettings:Audience"],
//                ValidateLifetime = true,
//                ValidateIssuerSigningKey = true,
//                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWTSettings:Key"]))
//            };
//        });

IConfiguration configuration = builder.Configuration;
builder.Services.JwtService(builder.Configuration);



var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
