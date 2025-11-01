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
// Configuração do Banco de Dados (Entity Framework Core)
// ------------------------------------------------------
// Define o AppDbContext e o provedor SQL Server, 
// utilizando a ConnectionString definida no appsettings.json
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ------------------------------------------------------
// Configuração de Sessão
// ------------------------------------------------------
// A sessão é usada para armazenar dados temporários do usuário (como ID logado)
builder.Services.AddDistributedMemoryCache(); // Usa memória para cache da sessão
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Tempo de expiração da sessão
    options.Cookie.HttpOnly = true;                 // Acesso restrito apenas via HTTP
    options.Cookie.IsEssential = true;              // Cookie essencial para funcionamento
});

// ------------------------------------------------------
// Configuração de Autenticação com Cookie
// ------------------------------------------------------
// Define autenticação baseada em cookie (login persistente)
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login/Login";              // Caminho da página de login
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30); // Duração do cookie
        options.SlidingExpiration = true;                // Renova automaticamente antes de expirar
    });

// ------------------------------------------------------
// Configuração do Swagger (Documentação da API)
// ------------------------------------------------------
// Swagger gera uma interface web para testar e documentar a API
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "MyProject API",               // Título exibido no Swagger
        Version = "v1",                        // Versão da documentação
        Description = "Documentação automática da API do projeto"
    });
});

// Criação da aplicação configurada
var app = builder.Build();

// ======================================================
//               CONFIGURAÇÃO DO PIPELINE HTTP
// ======================================================

// Configurações específicas para o ambiente de desenvolvimento
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage(); // Mostra detalhes dos erros (modo Dev)

    // Ativa o Swagger e sua interface no ambiente de desenvolvimento
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "MyProject API v1");
    });
}
else
{
    // Configuração de tratamento de erros no ambiente de produção
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts(); // Ativa HTTP Strict Transport Security
}

// ------------------------------------------------------
// Middlewares principais do ASP.NET Core
// ------------------------------------------------------
app.UseHttpsRedirection(); // Redireciona requisições HTTP para HTTPS
app.UseStaticFiles();      // Habilita o uso de arquivos estáticos (CSS, JS, imagens)

app.UseRouting();          // Ativa o roteamento entre controladores

app.UseSession();          // Habilita o uso de sessão na aplicação
app.UseAuthentication();   // Habilita autenticação via cookie
app.UseAuthorization();    // Habilita autorização (controle de acesso)

// ------------------------------------------------------
// Rota padrão da aplicação MVC
// ------------------------------------------------------
// Define que a rota inicial será HomeController -> Index()
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
