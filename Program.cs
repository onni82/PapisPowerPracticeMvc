using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using PapisPowerPracticeMvc.Data.Services;
using PapisPowerPracticeMvc.Data.Services.IService;
using System.Security.Claims;
using System.Text;

namespace PapisPowerPracticeMvc
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.
			builder.Services.AddControllersWithViews();
			builder.Services.AddHttpClient();
			builder.Services.AddSession();
			builder.Services.AddScoped<IWorkoutLogServices, WorkoutLogServices>();
			builder.Services.AddCors(options =>
			{
				options.AddPolicy("AllowBackend", policy =>
				{
					policy.WithOrigins("https://localhost:7202")
						  .AllowAnyHeader()
						  .AllowAnyMethod();
				});
			});
			builder.Services.AddHttpContextAccessor();
			builder.Services.AddTransient<JwtHandler>();

			// Registrerar Http-klienter
			builder.Services.AddHttpClient<IAuthService, AuthService>(a =>
			{
				a.BaseAddress = new Uri(builder.Configuration["AuthApi:BaseURL"]);
			})
			.AddHttpMessageHandler<JwtHandler>();

			builder.Services.AddHttpClient<IMuscleGroupService, MuscleGroupService>(client =>
			{
				client.BaseAddress = new Uri(builder.Configuration["AuthApi:BaseURL"]);
			})
			.AddHttpMessageHandler<JwtHandler>();

			builder.Services.AddHttpClient<IExerciseService, ExerciseService>(client =>
			{
				client.BaseAddress = new Uri(builder.Configuration["AuthApi:BaseURL"]);
			})
			.AddHttpMessageHandler<JwtHandler>();

			// JWT-autentisering
			builder.Services.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			})
			.AddJwtBearer(options =>
			{
				options.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuer = false,
					ValidateAudience = false,
					ValidateLifetime = false,
					ValidateIssuerSigningKey = false,
					IssuerSigningKey = new SymmetricSecurityKey(
						Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]!)
					)
				};

				// Få ASP.NET att läsa JWT från cookien som heter jwt
				options.Events = new JwtBearerEvents
				{
					OnMessageReceived = context =>
					{
						var token = context.HttpContext.Request.Cookies["jwt"];
						if (!string.IsNullOrEmpty(token))
						{
							context.Token = token;
						}
						return Task.CompletedTask;
					}
				};
			});

			builder.Services.AddAuthorization();

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (!app.Environment.IsDevelopment())
			{
				app.UseExceptionHandler("/Home/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();
			app.UseCors("AllowBackend");

			app.Use(async (context, next) =>
			{
				var token = context.Request.Cookies["jwt"];

				if (!string.IsNullOrEmpty(token))
				{
					var tokenHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
					try
					{
						var jwtToken = tokenHandler.ReadJwtToken(token);
						var identity = new ClaimsIdentity(jwtToken.Claims, "jwt");
						context.User = new ClaimsPrincipal(identity);
					}
					catch
					{
						// Ignorera om token inte är giltig
					}
				}
				await next();
			});

			app.UseAuthentication();
			app.UseSession();
			app.UseAuthorization();

			// Lägger till area routing for admin
			app.MapControllerRoute(
				name: "areas",
				pattern: "{area:exists}/{controller=home}/{action=Index}/{id?}"
			);
			app.MapControllerRoute(
				name: "default",
				pattern: "{controller=Home}/{action=Index}/{id?}");

			app.Run();
		}
	}
}
