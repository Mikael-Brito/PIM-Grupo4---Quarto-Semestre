using Microsoft.EntityFrameworkCore;
using MyProject.Data;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace MyProject
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // Este método é chamado pelo runtime. Use este método para adicionar serviços ao container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Adicionar suporte a Controllers com Views
            services.AddControllersWithViews();

            // Configurar Entity Framework com SQL Server
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            // Adicionar suporte a sessões
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            // Configurar autenticação por Cookies
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Login/Login"; // Define a página de login
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                    options.SlidingExpiration = true;
                });

            // Adicionar outros serviços conforme necessário
            // services.AddScoped<IUsuarioService, UsuarioService>();
            // services.AddScoped<IChamadoService, ChamadoService>();
        }

        // Este método é chamado pelo runtime. Use este método para configurar o pipeline de requisições HTTP.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Configurar pipeline para desenvolvimento
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // O valor padrão do HSTS é 30 dias. Você pode querer alterar isso para cenários de produção.
                app.UseHsts();
            }

            // Configurar middlewares
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            // Adicionar middleware de sessão
            app.UseSession();

            // Adicionar middleware de autenticação (DEVE vir antes de UseAuthorization)
            app.UseAuthentication();

            app.UseAuthorization();

            // Configurar rotas
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
