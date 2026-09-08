using ASP_P42.Data;
using ASP_P42.Data.Middleware.AuthSession;
using ASP_P42.Services.Hash;
using ASP_P42.Services.Kdf;
using ASP_P42.Services.Time;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);



// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddHash();
builder.Services.AddKdf();
builder.Services.AddTimeService();
// БД додається як сервіс, але специфічним методом-розширенням
builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("LocalDB")
    )
);

// настройка сессий 
builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(1);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Настройка CORS
// CORS (Cross-Origin Resource Sharing) - «обмен ресурсами между разными источниками»
// это механизм, который управляет тем, может ли один сайт
// обращаться к серверу, который находится на другом адресе(origin)

// POSTMAN игнорирует CORS, потому что CORS - это в первую очередь ограничение 
//                          ограничение браузера, а не запрет самого
//                          HTTP-запроса
// POSTMAN - программа для отправки HTTP-запросов к бэкенд и проверка ответа
builder.Services.AddCors(options => 
    options.AddDefaultPolicy(policy => 
        policy
        .AllowAnyOrigin()    // открытый API - для всех пользователей
        .AllowAnyHeader()    // разрешаем все заголовки 
        .AllowAnyMethod()    // все методы запроса
    )
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors();

app.UseAuthorization();
app.MapStaticAssets();

app.UseSession();  // включаем сессии
// добавляем custom middleware
app.UseAuthSession();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();































