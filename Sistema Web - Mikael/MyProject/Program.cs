// Importação dos namespaces necessários
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using MyProject.Data; // Namespace onde está o AppDbContext (contexto do banco)

// Criação do builder da aplicação
var builder = WebApplication.CreateBuilder(args);

// ======================================================
//                CONFIGURAÇÃO DE SERVIÇOS
// ======================================================

// Adiciona o suporte a Controllers e Views (MVC)
builder.Services.AddControllersWithViews();

// ------------------------------------------------------
// REGISTRO OBRIGATÓRIO PARA IHttpClientFactory
// ------------------------------------------------------
builder.Services.AddHttpClient(); // <-- ESTA É A LINHA QUE FALTAVA

// ------------------------------------------------------
// Configuração do Banco de Dados (Entity Framework Core)
// ------------------------------------------------------
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ------------------------------------------------------
// Configuração de Sessão
// ------------------------------------------------------
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// ------------------------------------------------------
// Configuração de Autenticação com Cookie
// ------------------------------------------------------
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login/Login";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
        options.SlidingExpiration = true;
    });

// ------------------------------------------------------
// Configuração do Swagger (Documentação da API)
// ------------------------------------------------------
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "MyProject API",
        Version = "v1",
        Description = "Documentação automática da API do projeto"
    });
});

// Criação da aplicação configurada
var app = builder.Build();

// ======================================================
//               CONFIGURAÇÃO DO PIPELINE HTTP
// ======================================================

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();

    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "MyProject API v1");
    });
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// Middlewares principais
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

// ------------------------------------------------------
// Rota padrão da aplicação MVC
// ------------------------------------------------------
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Inicia a aplicação
app.Run();




//using Microsoft.AspNetCore.Authentication.Cookies;
//using Microsoft.EntityFrameworkCore;
//using MyProject.Data;

//var builder = WebApplication.CreateBuilder(args);

//// Configuração de serviços
//builder.Services.AddControllersWithViews();

//// --- Banco de dados ---
//builder.Services.AddDbContext<AppDbContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//// --- Sessão ---
//builder.Services.AddDistributedMemoryCache();
//builder.Services.AddSession(options =>
//{
//    options.IdleTimeout = TimeSpan.FromMinutes(30);
//    options.Cookie.HttpOnly = true;
//    options.Cookie.IsEssential = true;
//});

//// --- Autenticação com Cookies ---
//builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
//    .AddCookie(options =>
//    {
//        options.LoginPath = "/Login/Login";
//        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
//        options.SlidingExpiration = true;
//    });

//var app = builder.Build();

//// Configuração do pipeline
//if (!app.Environment.IsDevelopment())
//{
//    app.UseExceptionHandler("/Home/Error");
//    app.UseHsts();
//}
//else
//{
//    app.UseDeveloperExceptionPage();
//}

//app.UseHttpsRedirection();
//app.UseStaticFiles();

//app.UseRouting();

//app.UseSession();
//app.UseAuthentication();
//app.UseAuthorization();

//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Home}/{action=Index}/{id?}");

//app.Run();
