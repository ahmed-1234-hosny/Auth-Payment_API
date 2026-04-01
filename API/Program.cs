
using Application.Interfaces;
using Infrastucture.Data;
using Infrastucture.Repo;
using Infrastucture.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Stripe;
using System.Text;

namespace API
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			//DI
			// DbContext
			builder.Services.AddDbContext<AppDBContext>(options =>
				options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

			// Repositories
			builder.Services.AddScoped<IUserRepository, UserRepo>();
			builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepo>();
			builder.Services.AddScoped<IPaymentRepository, PaymentRepo>();

			// Services
			builder.Services.AddScoped<IAuthService, AuthService>();
			builder.Services.AddScoped<IPaymentService, PaymentService>();

			// Stripe
			StripeConfiguration.ApiKey = builder.Configuration["Stripe:SecretKey"];

			// JWT Authentication
			var jwtKey = builder.Configuration["Jwt:Key"]!;
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
						IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
					};
				});


			// Add services to the container.
			builder.Services.AddAuthentication();
			builder.Services.AddControllers();
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

			app.UseHttpsRedirection();
			app.UseAuthentication();
			app.UseAuthorization();


			app.MapControllers();

			app.Run();
		}
	}
}
