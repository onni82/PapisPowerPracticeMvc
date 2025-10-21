using PapisPowerPracticeMvc.Data.Services;
using PapisPowerPracticeMvc.Data.Services.IService;

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
            builder.Services.AddScoped<IWorkoutLogServices,WorkoutLogServices>();
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

            builder.Services.AddHttpClient<IAuthService, AuthService>(a =>
            {
                a.BaseAddress = new Uri(builder.Configuration["AuthApi:BaseURL"]);
            })
            .AddHttpMessageHandler<JwtHandler>();

            builder.Services.AddHttpClient<IMuscleGroupService, MuscleGroupService>(client =>
            {
                client.BaseAddress = new Uri(builder.Configuration["AuthApi:BaseURL"]);
            });

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

            app.UseAuthentication();
            app.UseSession();
            app.UseAuthorization();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
